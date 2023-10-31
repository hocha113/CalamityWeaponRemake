using CalamityMod.Particles.Metaballs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria;

namespace CalamityWeaponRemake.Content.Particles
{
    internal class DivineSourceParticleSet : BaseFusableParticleSet
    {
        public override float BorderSize => 3f;

        public override bool BorderShouldBeSolid => false;

        public override Color BorderColor => Color.Lerp(Color.Gold, Color.White, 0.75f) * 0.85f;

        public override List<Effect> BackgroundShaders => new List<Effect> { GameShaders.Misc["CalamityMod:BaseFusableParticleEdge"].Shader };

        public override List<Texture2D> BackgroundTextures => new List<Texture2D> { ModContent.Request<Texture2D>("CalamityMod/Particles/Metaballs/StreamGougeLayer").Value };

        public override FusableParticle SpawnParticle(Vector2 center, float sizeStrength)
        {
            Particles.Add(new FusableParticle(center, sizeStrength));
            return Particles.Last();
        }

        public override void UpdateBehavior(FusableParticle particle)
        {
            particle.Size = MathHelper.Clamp(particle.Size - 1f, 0f, 200f) * 0.98f;
        }

        public override void PrepareOptionalShaderData(Effect effect, int index)
        {
            if (index == 0)
            {
                Vector2 value = Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.045f;
                effect.Parameters["generalBackgroundOffset"].SetValue(value);
                effect.Parameters["upscaleFactor"].SetValue(Vector2.One * -0.3f);
            }
        }

        public override void DrawParticles()
        {
            Texture2D value = ModContent.Request<Texture2D>("CalamityMod/Particles/Metaballs/FusableParticleBase").Value;
            foreach (FusableParticle particle in Particles)
            {
                Vector2 position = particle.Center - Main.screenPosition;
                Vector2 origin = value.Size() * 0.5f;
                Vector2 scale = Vector2.One * particle.Size / value.Size();
                Main.spriteBatch.Draw(value, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
