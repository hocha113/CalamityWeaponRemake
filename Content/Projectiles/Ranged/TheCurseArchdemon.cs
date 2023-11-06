using CalamityMod.Particles;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Ranged
{
    internal class TheCurseArchdemon : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Proj_Melee + "GaelSkull";

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.alpha += 5;
            CWRUtils.ClockFrame(ref Projectile.frameCounter, 10, 4);
            Projectile.rotation = Projectile.velocity.ToRotation();
            SpanDust();

            //for (int i = 0; i < 10; i++)//生成这种粒子不是好主意
            //{
            //    Vector2 particleSpeed = Projectile.rotation.ToRotationVector2() * 38 * (i / 20f);
            //    Particle energyLeak = new SquishyLightParticle(Projectile.Center, particleSpeed
            //        , Main.rand.NextFloat(1.6f, 1.8f), Color.Blue, 60, 1, 1.5f, hueShift: 0.01f);
            //    GeneralParticleHandler.SpawnParticle(energyLeak);
            //}
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.Explode();

            for (int i = 0; i < 30; i++)
            {
                Vector2 particleSpeed = Main.rand.NextVector2Unit() * Main.rand.Next(3, 7);
                Particle energyLeak = new SquishyLightParticle(Projectile.Center, particleSpeed
                    , Main.rand.NextFloat(0.6f, 1.3f), Color.DarkRed, 60, 1, 1.5f, hueShift: 0.01f);
                GeneralParticleHandler.SpawnParticle(energyLeak);
            }
        }

        public void SpanDust()
        {
            for (int i = 0; i < 1; i++)
            {
                if (Main.rand.NextBool())
                {
                    Vector2 vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center - vector3 * 30f, 0, 0, DustID.RedTorch)];
                    dust.noGravity = true;
                    dust.position = Projectile.Center - vector3 * Main.rand.Next(10, 21);
                    dust.velocity = vector3.RotatedBy(1.5707963705062866) * 6f;
                    dust.scale = 0.9f + Main.rand.NextFloat();
                    dust.fadeIn = 0.5f;
                    dust.customData = Projectile;
                    vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    dust.noGravity = true;
                    dust.position = Projectile.Center - vector3 * Main.rand.Next(10, 21);
                    dust.velocity = vector3.RotatedBy(1.5707963705062866) * 6f;
                    dust.scale = 0.9f + Main.rand.NextFloat();
                    dust.fadeIn = 0.5f;
                    dust.customData = Projectile;
                    dust.color = Color.Crimson;
                }
                else
                {
                    Vector2 vector4 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center - vector4 * 30f, 0, 0, DustID.RedTorch)];
                    dust.noGravity = true;
                    dust.position = Projectile.Center - vector4 * Main.rand.Next(20, 31);
                    dust.velocity = vector4.RotatedBy(-1.5707963705062866) * 5f;
                    dust.scale = 0.9f + Main.rand.NextFloat();
                    dust.fadeIn = 0.5f;
                    dust.customData = Projectile;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue, Projectile.frameCounter, 5),
                lightColor * (Projectile.alpha / 255f),
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue, 4),
                Projectile.scale,
                Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically
                );
            return false;
        }
    }
}
