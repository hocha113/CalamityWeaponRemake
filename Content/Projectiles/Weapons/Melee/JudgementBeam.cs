﻿using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SteelSeries.GameSense;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Melee;
using System;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee
{
    internal class JudgementBeam : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "JudgementBeam";

        public Color[] ProjColorDate;

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI() {
            if (ProjColorDate == null) {
                ProjColorDate = CWRUtils.GetColorDate(CWRUtils.GetT2DValue(Texture));
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Color color = CWRUtils.MultiLerpColor(Projectile.timeLeft / 120f, ProjColorDate);
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 5; i++) {
                    Vector2 pos = Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.Next(6);
                    Vector2 particleSpeed = Projectile.velocity * 0.75f;
                    CWRParticle lightdust = new LightParticle(pos, particleSpeed
                        , Main.rand.NextFloat(0.3f, 0.5f), color, 60, 1, 1.5f, hueShift: 0.0f);
                    CWRParticleHandler.SpawnParticle(lightdust);
                }
            }

            if (Projectile.timeLeft % 5 == 0) {
                SpawnGemDust(8, 3);
                SpawnGemDust(8, -3);
            }

            for (int i = 0; i < 10; i++) {
                int shinyDust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y)
                    , Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, default, 1.25f);
                Main.dust[shinyDust].noGravity = true;
                Main.dust[shinyDust].velocity *= 0.5f;
                Main.dust[shinyDust].velocity += Projectile.velocity * 0.1f;
            }

            NPC target = Projectile.Center.InPosClosestNPC(300);
            if (target != null) {
                Projectile.ChasingBehavior2(target.Center, 1, 0.1f);
            }
        }

        // 生成宝石光尘
        public void SpawnGemDust(int count, float velocityMultiplier) {
            for (int i = 0; i < count; i++) {
                int shinyDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, default, 1.25f);
                Main.dust[shinyDust].noGravity = true;
                Main.dust[shinyDust].velocity = Projectile.velocity.GetNormalVector() * velocityMultiplier;
                Main.dust[shinyDust].velocity += Projectile.velocity * 0.1f;
                Main.dust[shinyDust].scale = 2.2f;
            }
        }

        // 生成光尘粒子
        public void SpawnLightParticles(int count, float velocityMultiplier) {
            for (int i = 0; i < count; i++) {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.Next(6);
                Vector2 particleSpeed = Projectile.velocity.GetNormalVector() * velocityMultiplier * Main.rand.NextFloat(0.2f, 1);
                CWRParticle lightdust = new LightParticle(pos, particleSpeed, Main.rand.NextFloat(0.3f, 0.5f), Color.DarkBlue, 60, 1, 1.5f, hueShift: 0.0f);
                CWRParticleHandler.SpawnParticle(lightdust);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.numHits == 0) {
                SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
                float randNum = Main.rand.NextFloat(MathHelper.TwoPi);
                for (int i = 0; i < 6; i++) {
                    Vector2 vr = (MathHelper.TwoPi / 6f * i + randNum).ToRotationVector2() * 3;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vr
                        , ModContent.ProjectileType<OrderbringerWhiteOrb>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                    Main.projectile[proj].penetrate = -1;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, value.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
