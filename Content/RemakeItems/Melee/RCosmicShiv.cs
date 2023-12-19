using CalamityMod;
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
    internal class RCosmicShiv : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.CosmicShiv>();
        public override int ProtogenesisID => ModContent.ItemType<CosmicShiv>();
        public override void Load() {
            SetReadonlyTargetID = TargetID;
        }
        public override void SetDefaults(Item item) {
            item.useStyle = ItemUseStyleID.Rapier;
            item.DamageType = DamageClass.Melee;
            item.useAnimation = 15;
            item.useTime = 15;
            item.width = 44;
            item.height = 44;
            item.damage = 218;
            item.knockBack = 9f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<RCosmicShivProjectile>();
            item.shootSpeed = 2.4f;
            item.value = CalamityGlobalItem.Rarity14BuyPrice;
            item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "CosmicShiv", 2);
        }
    }
}
