using CalamityMod.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Projectiles.Summon;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Projectiles;

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

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = AiBehavior.GetPlayerInstance(projectile.owner);
            if (player != null)
            {
                bool isPlayer = (source as Player) != null;
                CWRPlayer modPlayer = player.CWR();
                int theReLdamags = projectile.damage / 2;

                if (modPlayer.theRelicLuxor == 1)
                {
                    if (player.whoAmI == Main.myPlayer
                        && projectile.friendly == true
                        && projectile.timeLeft >= 30
                        && projectile.hide == false
                        && isPlayer)
                    {
                        if (projectile.DamageType == ModContent.GetInstance<MeleeDamageClass>()
                            && projectile.type != ModContent.ProjectileType<TheRelicLuxorMelee>()
                            && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorMelee>()] < 35)
                        {
                            int proj = Projectile.NewProjectile(source, projectile.Center, projectile.velocity * 0.75f
                                , ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI);
                            Main.projectile[proj].ai[1] = projectile.whoAmI;
                            Main.projectile[proj].scale = projectile.scale;
                        }
                        else if (projectile.DamageType == ModContent.GetInstance<ThrowingDamageClass>())
                        {

                        }
                        else if (projectile.DamageType == ModContent.GetInstance<RangedDamageClass>())
                        {

                        }
                        else if (projectile.DamageType == ModContent.GetInstance<MagicDamageClass>())
                        {

                        }
                        else if (projectile.DamageType == ModContent.GetInstance<SummonDamageClass>()
                            && player.ownedProjectileCounts[ModContent.ProjectileType<LuxorsGiftSummon>()] < 3)
                        {

                        }
                    }
                }
                if (modPlayer.theRelicLuxor == 2)
                {
                    if (player.whoAmI == Main.myPlayer
                        && projectile.friendly == true
                        && projectile.timeLeft >= 30
                        && projectile.hide == false
                        && isPlayer)
                    {
                        if (projectile.DamageType == ModContent.GetInstance<MeleeDamageClass>()
                            && projectile.type != ModContent.ProjectileType<TheRelicLuxorMelee>()
                            && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorMelee>()] < 35)
                        {
                            int proj = Projectile.NewProjectile(source, projectile.Center, projectile.velocity * 0.75f
                                , ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI);
                            Main.projectile[proj].ai[1] = projectile.whoAmI;
                            Main.projectile[proj].scale = projectile.scale;
                        }
                        else if (projectile.DamageType == ModContent.GetInstance<ThrowingDamageClass>())
                        {

                        }
                        else if (projectile.DamageType == ModContent.GetInstance<RangedDamageClass>())
                        {

                        }
                        else if (projectile.DamageType == ModContent.GetInstance<MagicDamageClass>())
                        {

                        }
                        else if (projectile.DamageType == ModContent.GetInstance<SummonDamageClass>()
                            && player.ownedProjectileCounts[ModContent.ProjectileType<LuxorsGiftSummon>()] < 3)
                        {

                        }
                    }
                }
            }
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
