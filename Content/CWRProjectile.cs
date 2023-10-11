using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    internal class CWRProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            base.SetDefaults(projectile);
        }

        public override void AI(Projectile projectile)
        {
            base.AI(projectile);
        }

        public override bool PreAI(Projectile projectile)
        {
            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
        }

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            return base.CanHitPlayer(projectile, target);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(projectile, target, info);
        }

        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            return base.GetAlpha(projectile, lightColor);
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            return base.PreDraw(projectile, ref lightColor);
        }
    }
}
