using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using System;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class ExcelsusBomb : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "StreamGouge";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 5;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height
                    , Main.rand.NextBool(3) ? 56 : 242, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height
                    , DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.width = 600;
            Projectile.height = 600;
            Projectile.Center = Projectile.position;
            Projectile.Damage();
            for (int j = 0; j < 3; j++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                int numSpikes = 5;
                float spikeAmplitude = 22f;
                float scale = Main.rand.NextFloat(1f, 1.35f);

                for (float spikeAngle = 0f; spikeAngle < MathHelper.TwoPi; spikeAngle += 0.15f)
                {
                    Vector2 offset = spikeAngle.ToRotationVector2() * (2f + (MathF.Sin(angle + spikeAngle * numSpikes) + 1) * spikeAmplitude)
                                     * Main.rand.NextFloat(0.95f, 1.05f);

                    Dust.NewDustPerfect(Projectile.Center + HcMath.GetRandomVevtor(0, 360, Main.rand.Next(16, 220)), 173, offset, 0, default, scale).customData = 0.025f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = DrawUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation + MathHelper.PiOver4,
                DrawUtils.GetOrig(mainValue),
                Projectile.scale,
                SpriteEffects.None,
                0
                );
            return false;
        }
    }
}
