using CalamityMod.Particles;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace CalamityWeaponRemake.Content.Particles.Core
{
    internal class CWRParticleHandler
    {
        private static List<CWRParticle> particles;
        //List containing the particles to delete
        private static List<CWRParticle> particlesToKill;
        //Static list for details concerning every particle type
        internal static Dictionary<Type, int> particleTypes;
        internal static Dictionary<int, Texture2D> particleTextures;
        private static List<CWRParticle> particleInstances;
        //Lists used when drawing particles batched
        private static List<CWRParticle> batchedAlphaBlendParticles;
        private static List<CWRParticle> batchedNonPremultipliedParticles;
        private static List<CWRParticle> batchedAdditiveBlendParticles;

        public static void LoadModParticleInstances(Mod mod)
        {
            Type baseParticleType = typeof(CWRParticle);
            foreach (Type type in AssemblyManager.GetLoadableTypes(mod.Code))
            {
                if (type.IsSubclassOf(baseParticleType) && !type.IsAbstract && type != baseParticleType)
                {
                    int ID = particleTypes.Count; //Get the ID of the particle
                    particleTypes[type] = ID;

                    CWRParticle instance = (CWRParticle)FormatterServices.GetUninitializedObject(type);
                    particleInstances.Add(instance);

                    string texturePath = type.Namespace.Replace('.', '/') + "/" + type.Name;
                    if (instance.Texture != "")
                        texturePath = instance.Texture;
                    particleTextures[ID] = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad).Value;
                }
            }
        }

        internal static void Load()
        {
            particles = new List<CWRParticle>();
            particlesToKill = new List<CWRParticle>();
            particleTypes = new Dictionary<Type, int>();
            particleTextures = new Dictionary<int, Texture2D>();
            particleInstances = new List<CWRParticle>();

            batchedAlphaBlendParticles = new List<CWRParticle>();
            batchedNonPremultipliedParticles = new List<CWRParticle>();
            batchedAdditiveBlendParticles = new List<CWRParticle>();

            LoadModParticleInstances(CalamityWeaponRemake.Instance);
        }

        internal static void Unload()
        {
            particles = null;
            particlesToKill = null;
            particleTypes = null;
            particleTextures = null;
            particleInstances = null;
            batchedAlphaBlendParticles = null;
            batchedNonPremultipliedParticles = null;
            batchedAdditiveBlendParticles = null;
        }

        /// <summary>
        /// Spawns the particle instance provided into the world. If the particle limit is reached but the particle is marked as important, it will try to replace a non important particle.
        /// </summary>
        public static void SpawnParticle(CWRParticle particle)
        {
            // Don't queue particles if the game is paused.
            // This precedent is established with how Dust instances are created.
            //Don't spawn particles if on the server side either, or if the particles dict is somehow null
            if (Main.gamePaused || Main.dedServ || particles == null)
                return;

            //if (particles.Count >= CalamityConfig.Instance.ParticleLimit && !particle.Important)
            //    return;

            particles.Add(particle);
            particle.Type = particleTypes[particle.GetType()];
        }

        public static void Update()
        {
            foreach (CWRParticle particle in particles)
            {
                if (particle == null)
                    continue;
                particle.Position += particle.Velocity;
                particle.Time++;
                particle.Update();
            }
            //Clear out particles whose time is up
            particles.RemoveAll(particle => particle.Time >= particle.Lifetime && particle.SetLifetime || particlesToKill.Contains(particle));
            particlesToKill.Clear();
        }

        public static void RemoveParticle(CWRParticle particle)
        {
            particlesToKill.Add(particle);
        }

        public static void DrawAllParticles(SpriteBatch sb)
        {
            if (particles.Count == 0)
                return;

            sb.End();
            var rasterizer = Main.Rasterizer;
            rasterizer.ScissorTestEnable = true;
            Main.instance.GraphicsDevice.RasterizerState.ScissorTestEnable = true;
            Main.instance.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);

            //Batch the particles to avoid constant restarting of the spritebatch
            foreach (CWRParticle particle in particles)
            {
                if (particle == null)
                    continue;

                if (particle.UseAdditiveBlend)
                    batchedAdditiveBlendParticles.Add(particle);
                else if (particle.UseHalfTransparency)
                    batchedNonPremultipliedParticles.Add(particle);
                else
                    batchedAlphaBlendParticles.Add(particle);
            }
            if (batchedAlphaBlendParticles.Count > 0)
            {
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                foreach (CWRParticle particle in batchedAlphaBlendParticles)
                {
                    if (particle.UseCustomDraw)
                        particle.CustomDraw(sb);
                    else
                    {
                        Rectangle frame = particleTextures[particle.Type].Frame(1, particle.FrameVariants, 0, particle.Variant);
                        sb.Draw(particleTextures[particle.Type], particle.Position - Main.screenPosition, frame, particle.Color, particle.Rotation, frame.Size() * 0.5f,
                            particle.Scale, SpriteEffects.None, 0f);
                    }
                }
                sb.End();
            }


            if (batchedNonPremultipliedParticles.Count > 0)
            {
                rasterizer = Main.Rasterizer;
                rasterizer.ScissorTestEnable = true;
                Main.instance.GraphicsDevice.RasterizerState.ScissorTestEnable = true;
                Main.instance.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
                sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                foreach (CWRParticle particle in batchedNonPremultipliedParticles)
                {
                    if (particle.UseCustomDraw)
                        particle.CustomDraw(sb);
                    else
                    {
                        Rectangle frame = particleTextures[particle.Type].Frame(1, particle.FrameVariants, 0, particle.Variant);
                        sb.Draw(particleTextures[particle.Type], particle.Position - Main.screenPosition, frame, particle.Color, particle.Rotation, frame.Size() * 0.5f, particle.Scale, SpriteEffects.None, 0f);
                    }
                }
                sb.End();
            }

            if (batchedAdditiveBlendParticles.Count > 0)
            {
                rasterizer = Main.Rasterizer;
                rasterizer.ScissorTestEnable = true;
                Main.instance.GraphicsDevice.RasterizerState.ScissorTestEnable = true;
                Main.instance.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
                sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                foreach (CWRParticle particle in batchedAdditiveBlendParticles)
                {
                    if (particle.UseCustomDraw)
                        particle.CustomDraw(sb);
                    else
                    {
                        Rectangle frame = particleTextures[particle.Type].Frame(1, particle.FrameVariants, 0, particle.Variant);
                        sb.Draw(particleTextures[particle.Type], particle.Position - Main.screenPosition, frame, particle.Color, particle.Rotation, frame.Size() * 0.5f, particle.Scale, SpriteEffects.None, 0f);
                    }
                }
                sb.End();
            }

            batchedAlphaBlendParticles.Clear();
            batchedNonPremultipliedParticles.Clear();
            batchedAdditiveBlendParticles.Clear();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        }

        /// <summary>
        /// Gives you the amount of particle slots that are available. Useful when you need multiple particles at once to make an effect and dont want it to be only halfway drawn due to a lack of particle slots
        /// </summary>
        /// <returns></returns>
        public static int FreeSpacesAvailable()
        {
            //Safety check
            if (Main.dedServ || particles == null)
                return 0;

            return CalamityConfig.Instance.ParticleLimit - particles.Count();
        }

        /// <summary>
        /// Gives you the texture of the particle type. Useful for custom drawing
        /// </summary>
        public static Texture2D GetTexture(int type) => particleTextures[type];

#pragma warning disable CS0414
        private static string noteToEveryone = "This particle system was inspired by spirit mod's own particle system, with permission granted by Yuyutsu. Love you spirit mod! -Iban";
#pragma warning restore CS0414
    }
}
