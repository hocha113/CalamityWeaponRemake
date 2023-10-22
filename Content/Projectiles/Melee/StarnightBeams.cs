using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using CalamityMod.Particles.Metaballs;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class StreamGouges : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "StreamGouge";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.scale = 1f;
            Projectile.alpha = 150;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = 180 * Projectile.MaxUpdates;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.alpha+=5;
            if (Projectile.alpha > 255) 
                Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
            SoundEngine.PlaySound(in SoundID.Item74, target.Center);

            if (Projectile.numHits == 0)
            {
                float randRot = Main.rand.NextFloat(MathHelper.TwoPi);
                for (int i = 0; i < 6; i++)
                {
                    Vector2 vr = (MathHelper.TwoPi / 6 * i + randRot).ToRotationVector2() * 15;
                    Projectile.NewProjectile(
                        AiBehavior.GetEntitySource_Parent(Projectile),
                        target.Center + vr.UnitVector() * 164,
                        vr,
                        ModContent.ProjectileType<GodKillers>(),
                        Projectile.damage / 2,
                        0,
                        Projectile.owner
                        );
                }
            }

            StarRT(Projectile, target);
        }

        public static void StarRT(Projectile projectile, Entity target)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Color color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat(0.3f, 0.64f));
                GeneralParticleHandler.SpawnParticle(new ImpactParticle(Vector2.Lerp(projectile.Center, target.Center, 0.65f), 0.1f, 20, Main.rand.NextFloat(0.4f, 0.5f), color));
                for (int i = 0; i < 20; i++)
                {
                    Vector2 center = target.Center + Main.rand.NextVector2Circular(30f, 30f);
                    FusableParticleManager.GetParticleSetByType<StreamGougeParticleSet>()?.SpawnParticle(center, 60f);
                    float sizeStrength = MathHelper.Lerp(24f, 64f, CalamityUtils.Convert01To010(i / 19f));
                    center = target.Center + projectile.velocity.SafeNormalize(Vector2.UnitY) * MathHelper.Lerp(-40f, 90f, i / 19f);
                    FusableParticleManager.GetParticleSetByType<StreamGougeParticleSet>()?.SpawnParticle(center, sizeStrength);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DrawUtils.GetT2DValue(Texture);
            float alp = Projectile.alpha / 255f;

            if (Projectile.alpha > 225)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Main.EntitySpriteDraw(
                        texture,
                        DrawUtils.WDEpos(Projectile.oldPos[i] + Projectile.Center - Projectile.position),
                        null,
                        Color.White * alp * 0.5f,
                        Projectile.rotation + MathHelper.PiOver4,
                        DrawUtils.GetOrig(texture),
                        Projectile.scale - 0.05f * i,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            Main.EntitySpriteDraw(
                texture,
                DrawUtils.WDEpos(Projectile.Center),
                null,
                Color.White * alp,
                Projectile.rotation + MathHelper.PiOver4,
                DrawUtils.GetOrig(texture),
                Projectile.scale,
                SpriteEffects.None,
                0
                );
            return false;
        }
    }
}
