﻿using CalamityMod;
using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class BrimstoneSword : ModItem
    {
        public override string Texture => "CalamityMod/Items/Weapons/Melee/BrimstoneSword";
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.damage = 90;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.useAnimation = Item.useTime = 10;
            Item.shoot = ModContent.ProjectileType<BrimstoneSwordHeldProj>();
            Item.shootSpeed = 2f;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.CWR().remakeItem = true;
        }

        public override bool MeleePrefix() => true;
    }
}
