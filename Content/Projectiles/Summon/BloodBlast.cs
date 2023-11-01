using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Summon
{
    internal class BloodBlast : ModProjectile
    {
        public override string Texture => CWRConstant.placeholder;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 660;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.MaxUpdates = 13;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            for (int i = 0; i < 6; i++)
            {
                float randNum = Main.rand.NextFloat();
                Vector2 vr = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * (6 + 12 * randNum);
                Dust.NewDust(Projectile.Center, 32, 32, DustID.Blood, vr.X, vr.Y, (int)(200 + randNum * 55), Scale: 0.8f + 2 * randNum);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return AiBehavior.CircularHitboxCollision(Projectile.Center, 100, targetHitbox);
        }
    }
}
