using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RTerratomere : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Terratomere>(item))
            {
                item.width = 60;
                item.height = 66;
                item.damage = 303;
                item.DamageType = DamageClass.Melee;
                item.useAnimation = 21;
                item.useTime = 21;
                item.useStyle = ItemUseStyleID.Swing;
                item.useTurn = true;
                item.knockBack = 7f;
                item.autoReuse = true;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.value = CalamityGlobalItem.Rarity12BuyPrice;
                item.rare = ModContent.RarityType<Turquoise>();
                item.shoot = ModContent.ProjectileType<RemakeTerratomereHoldoutProj>();
                item.shootSpeed = 60f;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Terratomere>(item))
            {
                CWRUtils.OnModifyTooltips(Mod, item, tooltips, "Terratomere", 3);
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Terratomere>(item))
            {
                if (player.ownedProjectileCounts[item.shoot] > 0)
                {
                    return false;
                }
            }
            return base.UseItem(item, player);
        }
    }
}
