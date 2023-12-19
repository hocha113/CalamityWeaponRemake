using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RTerratomere : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.Terratomere>();
        public override int ProtogenesisID => ModContent.ItemType<Terratomere>();
        public override void Load() {
            SetReadonlyTargetID = TargetID;
        }
        public override void SetDefaults(Item item) {
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
            item.shoot = ModContent.ProjectileType<RTerratomereHoldoutProj>();
            item.shootSpeed = 60f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "Terratomere", 3);
        }

        public override bool? UseItem(Item item, Player player) {
            return player.ownedProjectileCounts[item.shoot] == 0;
        }
    }
}
