﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Placeable
{
    internal class DarkMatterCompressorItem : ModItem
    {
        public override string Texture => CWRConstant.Asset + "Items/Placeable/" + "DarkMatterCompressorItem";
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<DarkMatterCompressor>();

            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.value = Terraria.Item.buyPrice(gold: 16);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<DarkPlasma>(25)//暗物质
                .AddIngredient<StaticRefiner>()
                .AddIngredient<ProfanedCrucible>()
                .AddIngredient<PlagueInfuser>()
                .AddIngredient<MonolithAmalgam>()
                .AddIngredient<VoidCondenser>()
                .Register();
        }
    }
}
