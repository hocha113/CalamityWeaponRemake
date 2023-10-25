using CalamityMod;
using CalamityMod.Projectiles.Summon;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    internal class CWRItem : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CWRPlayer modPlayer = player.CWR();
            int theReLdamags = damage / 3;
            Vector2 vr = player.Center.To(Main.MouseWorld).UnitVector() * 13;

            if (player.whoAmI == Main.myPlayer)
            {
                if (modPlayer.theRelicLuxor == 1)
                {
                    if (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<TrueMeleeNoSpeedDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI, 1);
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRogue>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<RangedDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRanged>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<MagicDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMagic>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<SummonDamageClass>()
                        && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorSummon>()] < 3)
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorSummon>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                }
                if (modPlayer.theRelicLuxor == 2)
                {
                    theReLdamags += 15;

                    if (item.CountsAsClass<MeleeDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI, 1);
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRogue>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<RangedDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRanged>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<MagicDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMagic>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<SummonDamageClass>()
                        && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorSummon>()] < 6)
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorSummon>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
