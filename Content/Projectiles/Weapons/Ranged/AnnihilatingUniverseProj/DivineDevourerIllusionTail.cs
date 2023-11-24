using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.AnnihilatingUniverseProj
{
    internal class DivineDevourerIllusionTail : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Ranged + "AnnihilatingUniverseProj/" + "DivineDevourerIllusionTail";

        public override void SetDefaults()
        {
            Projectile.height = 34;
            Projectile.width = 34;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 260;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Projectile aheadSegment = Main.projectile[(int)Projectile.ai[1]];
            if (!aheadSegment.Alives())
            {
                Projectile.Kill();
                return;
            }
            Vector2 directionToNextSegment = aheadSegment.Center - Projectile.Center;
            if (aheadSegment.rotation != Projectile.rotation)
            {
                directionToNextSegment = directionToNextSegment.RotatedBy(MathHelper.WrapAngle(aheadSegment.rotation - Projectile.rotation) * 0.08f);
                directionToNextSegment = directionToNextSegment.MoveTowards((aheadSegment.rotation - Projectile.rotation).ToRotationVector2(), 1f);
            }

            Projectile.rotation = directionToNextSegment.ToRotation() + MathHelper.PiOver2;
            Projectile.Center = aheadSegment.Center - directionToNextSegment.SafeNormalize(Vector2.Zero) * Projectile.scale * Projectile.width;
            Projectile.spriteDirection = (directionToNextSegment.X > 0).ToDirectionInt();
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
