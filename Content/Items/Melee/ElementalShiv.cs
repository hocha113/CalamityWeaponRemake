﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityWeaponRemake.Common;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class ElementalShiv : ModItem
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override string Texture => CWRConstant.Cay_Wap_Melee + "ElementalShiv";

        public override void SetDefaults() {
            Item.width = 44;
            Item.height = 44;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = (Item.useTime = 13);
            Item.shoot = ModContent.ProjectileType<ElementalShivProj>();
            Item.shootSpeed = 2.4f;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.Rarity11BuyPrice;
            Item.rare = ItemRarityID.Purple;
        }

        public override bool MeleePrefix() {
            return true;
        }
    }
}
