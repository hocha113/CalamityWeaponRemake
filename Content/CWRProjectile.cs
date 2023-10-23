using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    /// <summary>
    /// 用于分组弹幕的发射源，这决定了一些弹幕的特殊行为
    /// </summary>
    public enum SpanTypesEnum : byte
    {
        DeadWing = 1
    }

    public class CWRProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public byte SpanTypes;

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

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(projectile, target, info);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (SpanTypes == (byte)SpanTypesEnum.DeadWing)
            {
                int types = ModContent.ProjectileType<DeadWave>();
                Player player = Main.player[projectile.owner];
                if (player.Center.To(target.Center).LengthSquared() < 600 * 600
                    && projectile.type != types
                    && projectile.numHits == 0)
                {
                    Vector2 vr = player.Center.To(Main.MouseWorld)
                        .RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))).UnitVector() * Main.rand.Next(7, 9);
                    Vector2 pos = player.Center + vr * 10;
                    Projectile.NewProjectileDirect(
                        AiBehavior.GetEntitySource_Parent(player),
                        pos,
                        vr,
                        ModContent.ProjectileType<DeadWave>(),
                        projectile.damage,
                        projectile.knockBack,
                        projectile.owner
                        ).rotation = vr.ToRotation(); ;
                }
            }
        }        
    }
}
