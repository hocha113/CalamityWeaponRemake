﻿using CalamityWeaponRemake.Content.Particles.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static CalamityMod.CalamityUtils;
using System;
using Terraria.ModLoader;
using Terraria;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Particles
{
    internal class GangarusWave : CWRParticle
    {
        public override string Texture => "CalamityMod/Particles/HollowCircleHardEdge";
        public override bool UseAdditiveBlend => true;
        public override bool SetLifetime => true;
        public override bool UseCustomDraw => true;

        private float OriginalScale;
        private float FinalScale;
        private float opacity;
        private Vector2 Squish;
        private Color BaseColor;
        private Entity Entity;
        private Vector2 EntityPos;
        private Vector2 OldEntityPos;
        private Vector2 EntityVariation;

        public GangarusWave(Vector2 position, Vector2 velocity, Color color, Vector2 squish, float rotation, float originalScale, float finalScale, int lifeTime, Entity entity) {
            Position = position;
            Velocity = velocity;
            BaseColor = color;
            OriginalScale = originalScale;
            FinalScale = finalScale;
            Scale = originalScale;
            Lifetime = lifeTime;
            Squish = squish;
            Rotation = rotation;
            Entity = entity;
        }

        public override void Update() {
            float pulseProgress = PiecewiseAnimation(LifetimeCompletion, new CurveSegment[] { new CurveSegment(EasingType.PolyOut, 0f, 0f, 1f, 4) });
            Scale = MathHelper.Lerp(OriginalScale, FinalScale, pulseProgress);

            opacity = (float)Math.Sin(MathHelper.PiOver2 + LifetimeCompletion * MathHelper.PiOver2);

            Color = BaseColor * opacity;
            Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);
            Velocity *= 0.95f;

            if (Entity != null) {
                OldEntityPos = EntityPos;
                EntityPos = Entity.Center;
                if (OldEntityPos != Vector2.Zero) {
                    EntityVariation = OldEntityPos.To(EntityPos);
                    Position += EntityVariation;
                }
                //Projectile projectile = ((Projectile)Entity);
                //if (projectile != null)
                //    Rotation = projectile.rotation;
            }
        }

        public override void CustomDraw(SpriteBatch spriteBatch) {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, Color * opacity, Rotation, tex.Size() / 2f, Scale * Squish, SpriteEffects.None, 0);
        }
    }
}
