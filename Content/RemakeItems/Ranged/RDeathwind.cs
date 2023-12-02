using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Ranged
{
    internal class RDeathwind : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Deathwind>(item))
            {
                item.damage = 248;
                item.DamageType = DamageClass.Ranged;
                item.width = 40;
                item.height = 82;
                item.useTime = 14;
                item.useAnimation = 14;
                item.useStyle = ItemUseStyleID.Shoot;
                item.noMelee = true;
                item.noUseGraphic = true;
                item.knockBack = 5f;
                item.value = CalamityGlobalItem.Rarity14BuyPrice;
                item.rare = ModContent.RarityType<DarkBlue>();
                item.autoReuse = true;
                item.shoot = ModContent.ProjectileType<DeathwindHeldProj>();
                item.shootSpeed = 20f;
                item.useAmmo = AmmoID.Arrow;
                item.Calamity().canFirePointBlankShots = true;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Deathwind>(item))
            {
                CWRUtils.OnModifyTooltips(Mod, item, tooltips, "Deathwind", 2);
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Deathwind>(item))
            {
                item.initialize();
                item.CWR().ai[1] = type;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<DeathwindHeldProj>()] <= 0)
                {
                    item.CWR().ai[0] = Projectile.NewProjectile(source, position, Vector2.Zero
                    , ModContent.ProjectileType<DeathwindHeldProj>()
                    , damage, knockback, player.whoAmI);
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
