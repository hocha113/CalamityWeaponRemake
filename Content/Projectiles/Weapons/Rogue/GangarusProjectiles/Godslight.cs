﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using System;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue.GangarusProjectiles
{
    internal class Godslight : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        internal PrimitiveTrail LightningDrawer;
        internal Vector2[] RayPoint;
        internal int pointNum => 100;
        internal Color[] colors;
        public override bool ShouldUpdatePosition() => false;

        public override void SetDefaults() {
            Projectile.width = Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 190;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.scale = 0.1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 11;
        }

        public override bool PreAI() {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[0] == 0) {
                RayPoint = new Vector2[pointNum];
                for (int i = 0; i < pointNum; i++) {
                    RayPoint[i] = Projectile.velocity.ToRotation().ToRotationVector2() * (-pointNum * 30 + 60 * i) + Projectile.Center;
                }
                foreach (Vector2 pos in RayPoint) {
                    Vector2 spanPos = pos + Main.rand.NextVector2Unit() * Main.rand.Next(56);
                    Vector2 vr = new Vector2((Main.rand.NextBool() ? -1 : 1) * Main.rand.Next(7, 51), 0);
                    LightParticle light = new LightParticle(spanPos
                        , vr, 0.3f, CWRUtils.MultiLerpColor(Main.rand.NextFloat(), colors), 30);
                    CWRParticleHandler.SpawnParticle(light);
                }
                Projectile.ai[0] = 1;
            }
            if (Projectile.timeLeft > 60) {
                Projectile.scale += 0.5f;
                if (Projectile.scale > 6)
                    Projectile.scale = 6;
            }
            if (Projectile.timeLeft < 20) {
                Projectile.scale -= 1f;
                if (Projectile.scale < 0)
                    Projectile.scale = 0;
            }
            if (Projectile.timeLeft == 20) {
                SpanDeadLightPenms();
            }
            return true;
        }

        public void SpanDeadLightPenms() {
            if (Projectile.IsOwnedByLocalPlayer()) {
                foreach (Vector2 pos in RayPoint) {
                    LightParticle light = new LightParticle(pos + Main.rand.NextVector2Unit() * Main.rand.Next(56)
                        , new Vector2(0, Main.rand.Next(7, 51)), 0.3f, CWRUtils.MultiLerpColor(Main.rand.NextFloat(), colors), 30);
                    CWRParticleHandler.SpawnParticle(light);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size()
                , Projectile.rotation.ToRotationVector2() * -3000 + Projectile.Center
                , Projectile.rotation.ToRotationVector2() * 3000 + Projectile.Center, 32, ref point);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            Projectile.damage = (int)(Projectile.damage * 0.98f);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }

        public override void OnKill(int timeLeft) {
            SpanDeadLightPenms();
        }

        public float PrimitiveWidthFunction(float completionRatio) => CalamityUtils.Convert01To010(completionRatio) * Projectile.scale * Projectile.width * Projectile.ai[1];

        public Color PrimitiveColorFunction(float completionRatio) {
            float colorInterpolant = (float)Math.Sin(Projectile.identity / 3f + completionRatio * 20f + Main.GlobalTimeWrappedHourly * 1.1f) * 0.5f + 0.5f;
            Color color = CalamityUtils.MulticolorLerp(colorInterpolant, colors);
            return color;
        }

        public override bool PreDraw(ref Color lightColor) {
            if (RayPoint != null) {
                if (LightningDrawer is null)
                    LightningDrawer = new PrimitiveTrail(PrimitiveWidthFunction, PrimitiveColorFunction, PrimitiveTrail.RigidPointRetreivalFunction, GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]);

                GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].UseImage1("Images/Misc/Perlin");
                GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].Apply();
                LightningDrawer.Draw(RayPoint, Projectile.Size * 0.5f - Main.screenPosition, 50);
            }
            return false;
        }
    }
}
