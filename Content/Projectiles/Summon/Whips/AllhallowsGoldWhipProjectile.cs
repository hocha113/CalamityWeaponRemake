﻿using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using System;
using Terraria.Graphics.Shaders;
using CalamityMod.Particles.Metaballs;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis;
using CalamityWeaponRemake.Content.Particles;

namespace CalamityWeaponRemake.Content.Projectiles.Summon.Whips
{
    internal class AllhallowsGoldWhipProjectile : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Summon + "AllhallowsGoldWhipProjectile";

        private List<Vector2> whipPoints => Projectile.GetWhipControlPoints();//点集

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 35;
            Projectile.WhipSettings.RangeMultiplier = 1f;
        }

        private float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.NewProjectile(
                AiBehavior.GetEntitySource_Parent(Projectile),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ATrail>(),
                0,
                0,
                Projectile.owner,
                ai0: Projectile.whoAmI
                );
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = Projectile.damage / 2;

            if (Projectile.numHits == 0)
            {
                
            }
        }

        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = new Color(252, 102, 202);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }//绘制连接线

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLine(whipPoints);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = whipPoints[0];

            for (int i = 0; i < whipPoints.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 42, 66);

                Vector2 origin = new Vector2(21, 33);
                float scale = 1;
                float offsetRots = 0;

                if (i == whipPoints.Count - 2)
                {
                    frame.Y = 134;
                    frame.Height = 96;
                    origin = new Vector2(20, 20);
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Time / timeToFlyOut;
                    scale = MathHelper.Lerp(1.05f, 2f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 0)
                {
                    int count = i % 3;
                    if (count == 0)
                    {
                        frame.Y = 102;
                        frame.Height = 30;
                        origin = new Vector2(20, 18);
                    }
                    if (count == 1)
                    {
                        frame.Y = 68;
                        frame.Height = 32;
                        origin = new Vector2(20, 18);
                    }
                    if (count == 2)
                    {
                        frame.Y = 68;
                        frame.Height = 32;
                        origin = new Vector2(20, 18);
                    }
                    scale = 1 + i / 120f;
                }

                Vector2 element = whipPoints[i];
                Vector2 diff = whipPoints[i + 1] - element;

                scale *= 0.7f;
                float rotation = diff.ToRotation() - MathHelper.PiOver2; // 此投射物的精灵图朝下，因此使用PiOver2进行旋转修正
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, Color.White, rotation + offsetRots, origin, scale, flip, 0);
                pos += diff;
            }
            return false;
        }

        private class ATrail : ModProjectile
        {
            internal PrimitiveTrail TrailDrawer;

            public override string Texture => CWRConstant.placeholder;

            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.width = 6;
                Projectile.height = 6;
                Projectile.scale = 1;
                Projectile.alpha = 80;
                Projectile.friendly = true;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 150;
                Projectile.MaxUpdates = 5;
            }

            public int fowerIndex { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }

            public override void AI()
            {
                Projectile ownProj = AiBehavior.GetProjectileInstance(fowerIndex);
                if (ownProj != null)
                {
                    List<Vector2> toPos = AiBehavior.GetWhipControlPoints(ownProj);
                    int index = toPos.Count - 2;
                    if (index < toPos.Count && index >= 0)
                    {
                        float rot = toPos[toPos.Count - 3].To(toPos[toPos.Count - 2]).ToRotation();
                        Projectile.velocity = Projectile.Center.To(toPos[toPos.Count - 2]) + rot.ToRotationVector2() * 62;

                        if (Main.netMode != NetmodeID.Server)
                        {
                            //for (int i = 0; i < 10; i++)
                            //{
                            //    Vector2 center = Projectile.Center + Main.rand.NextVector2Circular(50f, 10f);
                            //    FusableParticleManager.GetParticleSetByType<DivineSourceParticleSet>()?.SpawnParticle(center, 30f);
                            //    float sizeStrength = MathHelper.Lerp(24f, 64f, CalamityUtils.Convert01To010(i / 19f));
                            //    center = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * MathHelper.Lerp(-40f, 90f, i / 19f);
                            //    FusableParticleManager.GetParticleSetByType<DivineSourceParticleSet>()?.SpawnParticle(center, sizeStrength);
                            //}
                        }
                    }

                    Projectile.timeLeft = 2;
                }
                else Projectile.Kill();
            }

            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                return false;
            }

            internal Color ColorFunction(float completionRatio)
            {
                float amount = MathHelper.Lerp(0.65f, 1f, (float)Math.Cos((0f - Main.GlobalTimeWrappedHourly) * 3f) * 0.5f + 0.5f);
                float num = Utils.GetLerpValue(1f, 0.64f, completionRatio, clamped: true) * Projectile.Opacity;

                Color value = Color.Lerp(new Color(255, 223, 186), new Color(255, 218, 185), (float)Math.Sin(completionRatio * MathF.PI * 1.6f - Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);

                return Color.Lerp(new Color(255, 248, 220), value, amount) * num;
            }

            internal float WidthFunction(float completionRatio)
            {
                float amount = (float)Math.Pow(1f - completionRatio, 3.0);
                return MathHelper.Lerp(0f, 62f * Projectile.scale * Projectile.Opacity, amount);
            }

            public override bool PreDraw(ref Color lightColor)
            {
                if (TrailDrawer == null)
                {
                    TrailDrawer = new PrimitiveTrail(WidthFunction, ColorFunction, null, GameShaders.Misc["CalamityMod:TrailStreak"]);
                }

                GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
                TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 30);
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Main.EntitySpriteDraw(
                    value,
                    Projectile.Center - Main.screenPosition,
                    null,
                    Color.Lerp(lightColor, Color.White, 0.5f),
                    Projectile.rotation + MathHelper.PiOver2,
                    value.Size() / 2f,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
                return false;
            }
        }
    }
}
