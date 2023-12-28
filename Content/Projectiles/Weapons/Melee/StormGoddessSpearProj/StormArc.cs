﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using CalamityWeaponRemake.Common;
using CalamityMod;
using Terraria.Graphics.Shaders;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using SteelSeries.GameSense;
using Terraria.Audio;
using ReLogic.Utilities;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.StormGoddessSpearProj
{
    internal class StormArc : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        internal PrimitiveTrail LightningDrawer;
        private Color light => Lighting.GetColor((int)(Projectile.position.X + (Projectile.width * 0.5)) / 16, (int)((Projectile.position.Y + (Projectile.height * 0.5)) / 16.0));
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }

        public override void SetDefaults() {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 13;
            Projectile.MaxUpdates = 3;
            Projectile.tileCollide = true;
        }


        HashSet<NPC> shockedbefore = new HashSet<NPC>();
        int prevX = 0;
        public override void AI() {
            if (Projectile.localAI[0] == 0f) {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }

            Vector2 move = Vector2.Zero;
            float distance = 160f;
            bool target = false;
            NPC npc = null;
            bool pastNPC = false;
            if (Projectile.timeLeft < 118) {
                for (int k = 0; k < Main.maxNPCs; k++) {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !shockedbefore.Contains(Main.npc[k])) {
                        Vector2 newMove = Main.npc[k].Center - (Projectile.velocity + Projectile.Center);
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance) {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                            npc = Main.npc[k];

                        }
                    }
                }
            }

            if (!target) {
                foreach (NPC pastnpc in shockedbefore) {
                    Vector2 newMove = pastnpc.Center - (Projectile.velocity + Projectile.Center);
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance) {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                        npc = pastnpc;
                        pastNPC = true;
                    }
                }
            }

            Vector2 current = Projectile.Center;
            if (target) {
                shockedbefore.Add(npc);
                npc.HitEffect(0, Projectile.damage);
                move += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * distance / 30;
                if (pastNPC) {
                    prevX++;
                    move += new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) * prevX;
                }
            }
            else {
                move = (Projectile.velocity + new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6))) * 5;
            }

            for (int i = 0; i < 20; i++) {
                current += move / 20f;
            }

            Projectile.position = current;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.velocity = oldVelocity;
            Projectile.timeLeft -= 32;
            return false;
        }

        private void AdjustMagnitude(ref Vector2 vector) {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f) {
                vector *= 6f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Electrified, 120);
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < Main.rand.Next(3, 16); i++) {
                    Vector2 pos = target.Center + Main.rand.NextVector2Unit() * Main.rand.Next(target.width);
                    Vector2 particleSpeed = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(15.5f, 37.7f);
                    CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                        , 0.3f, light, 6 + Main.rand.Next(5), 1, 1.5f, hueShift: 0.0f);
                    CWRParticleHandler.SpawnParticle(energyLeak);
                }
                SoundEngine.PlaySound(SoundID.Item94, target.position);
            }
        }

        public float PrimitiveWidthFunction(float completionRatio) => CalamityUtils.Convert01To010(completionRatio) * Projectile.scale * Projectile.width * 0.6f;

        public Color PrimitiveColorFunction(float completionRatio) {
            float colorInterpolant = (float)Math.Sin(Projectile.identity / 3f + completionRatio * 20f + Main.GlobalTimeWrappedHourly * 1.1f) * 0.5f + 0.5f;
            Color color = CalamityUtils.MulticolorLerp(colorInterpolant, light);
            return color;
        }

        public override bool PreDraw(ref Color lightColor) {
            if (LightningDrawer is null)
                LightningDrawer = new PrimitiveTrail(PrimitiveWidthFunction, PrimitiveColorFunction, PrimitiveTrail.RigidPointRetreivalFunction, GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]);

            GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].UseImage1("Images/Misc/Perlin");
            GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].Apply();

            LightningDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 80);
            return false;
        }
    }
}
