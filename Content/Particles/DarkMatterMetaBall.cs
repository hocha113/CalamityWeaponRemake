﻿using CalamityMod.Graphics.Metaballs;
using CalamityWeaponRemake.Content.Particles.Core;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System.Linq;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Particles
{
    internal class DarkMatterMetaBall : CWRMetaball
    {
        public class CosmicParticle
        {
            public float Size;

            public Vector2 Velocity;

            public Vector2 Center;

            public CosmicParticle(Vector2 center, Vector2 velocity, float size) {
                Center = center;
                Velocity = velocity;
                Size = size;
            }

            public void Update() {
                Center += Velocity;
                Velocity *= 0.96f;
                Size *= 0.91f;
            }
        }

        public static Asset<Texture2D> LayerAsset {
            get;
            private set;
        }

        public static List<CosmicParticle> Particles {
            get;
            private set;
        } = new();

        public override bool AnythingToDraw => Particles.Any();

        public override IEnumerable<Texture2D> Layers {
            get {
                yield return LayerAsset.Value;
            }
        }

        public override MetaballDrawLayer DrawContext => MetaballDrawLayer.AfterProjectiles;

        public override Color EdgeColor => CWRUtils.MultiLerpColor(Main.GameUpdateCount % 30 / 30f, Color.Blue, Color.Black);

        public override void Load() {
            if (Main.netMode == NetmodeID.Server)
                return;

            // Load the layer asset wrapper.
            LayerAsset = ModContent.Request<Texture2D>($"CalamityMod/Graphics/Metaballs/StreamGougeLayer", AssetRequestMode.ImmediateLoad);
        }

        public override void Update() {
            // Update all particle instances.
            // Once sufficiently small, they vanish.
            for (int i = 0; i < Particles.Count; i++)
                Particles[i].Update();
            Particles.RemoveAll(p => p.Size <= 2f);
        }

        public static void SpawnParticle(Vector2 position, Vector2 velocity, float size) =>
            Particles.Add(new(position, velocity, size));

        // Make the texture scroll.
        public override Vector2 CalculateManualOffsetForLayer(int layerIndex) {
            return Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.037f;
        }

        public override void DrawInstances() {
            Texture2D tex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/BasicCircle").Value;

            foreach (CosmicParticle particle in Particles) {
                Vector2 drawPosition = particle.Center - Main.screenPosition;
                Vector2 origin = tex.Size() * 0.5f;
                Vector2 scale = Vector2.One * particle.Size / tex.Size();
                for (int i = 0; i < 15; i++)
                    Main.spriteBatch.Draw(tex, drawPosition, null, Color.White, Main.GameUpdateCount / 10f + i * 0.01f, origin, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
