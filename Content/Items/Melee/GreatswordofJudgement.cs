﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    /// <summary>
    /// 制裁大剑
    /// </summary>
    internal class GreatswordofJudgement : ModItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "GreatswordofJudgement";
        public override void SetDefaults() {
            Item.width = 78;
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 78;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<JudgementBeam>();
            Item.shootSpeed = 15f;
            Item.CWR().remakeItem = true;
        }
    }
}
