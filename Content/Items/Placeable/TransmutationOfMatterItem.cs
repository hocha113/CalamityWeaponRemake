﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Rarities;
using CalamityMod.Tiles.DraedonStructures;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Placeable
{
    internal class TransmutationOfMatterItem : ModItem
    {
        public override string Texture => CWRConstant.Asset + "Items/Placeable/" + "TransmutationOfMatterItem";
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults() {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 1;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<TransmutationOfMatter>();

            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.value = Terraria.Item.buyPrice(gold: 16);
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<ShadowspecBar>(25)
                .AddIngredient<DraedonsForge>()
                .AddIngredient<AltarOfTheAccursedItem>()
                .AddTile(ModContent.TileType<DarkMatterCompressor>())
                .Register();
        }
    }
}
