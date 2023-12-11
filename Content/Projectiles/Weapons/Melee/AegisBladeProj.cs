﻿using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Particles.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee
{
    internal class AegisBladeProj : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "AegisBlade";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (Projectile.ai[1] != 1)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity = new Vector2(0, -12);
            }
            if (Projectile.ai[1] == 0)
            {
                Projectile.scale += 0.01f;
                Projectile.velocity *= 0.97f;
                Projectile.position += Main.player[Projectile.owner].velocity;
                if (Projectile.velocity.LengthSquared() < 9)
                {
                    Projectile.ai[1] = 1;
                    Projectile.ai[0] = 1;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (Projectile.scale < 3)
                {
                    Projectile.scale += 0.02f;
                    if (!CWRUtils.isServer)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 pos = Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.Next(143, 150) * Projectile.scale;
                            Vector2 particleSpeed = pos.To(Projectile.Center).UnitVector() * 16;
                            CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                                , 0.3f, Color.Gold, 16, 1, 1.5f, hueShift: 0.0f, _entity: Projectile);
                            CWRParticleHandler.SpawnParticle(energyLeak);
                        }
                    }
                }
                
                Projectile.velocity = Vector2.Zero;
                Projectile.damage += 25;
                Projectile.rotation += 0.2f;
                Projectile.position += Main.player[Projectile.owner].velocity;
                if (Projectile.ai[0] > 60)
                {
                    Projectile.ai[1] = 2;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.ai[1] == 2)
            {
                NPC npc = Projectile.ProjFindingNPCTarget(16000);
                if (npc != null)
                {
                    Projectile.ChasingBehavior(npc.Center, 56);
                    Projectile.penetrate = 1;
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.Kill();
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
            Projectile.ai[0]++;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.damage = Projectile.originalDamage;
            Projectile.Explode(1200);
            if (!CWRUtils.isServer)
            {
                for (int i = 0; i < 156; i++)
                {
                    Vector2 pos = Projectile.Center;
                    Vector2 particleSpeed = Main.rand.NextVector2Unit() * Main.rand.Next(13, 34);
                    CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                        , Main.rand.NextFloat(0.5f, 1.3f), Color.Gold, 30, 1, 1.5f, hueShift: 0.0f);
                    CWRParticleHandler.SpawnParticle(energyLeak);
                }
            }
            if (Projectile.IsOwnedByLocalPlayer())
            {
                for (int i = 0; i < 32; i++)
                {
                    Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + velocity.UnitVector() * 13, velocity, ModContent.ProjectileType<AegisFlame>(), (int)(Projectile.damage * 0.75), 0f, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, Color.White
                , Projectile.rotation, value.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            if (Projectile.ai[1] == 2)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Main.EntitySpriteDraw(value, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.White * (1 - i * 0.1f)
                    , Projectile.rotation, value.Size() / 2, Projectile.scale - i * 0.1f, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}