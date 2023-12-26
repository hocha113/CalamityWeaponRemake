﻿using CalamityWeaponRemake.Common;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Placeable
{
    internal class FoodStallChair : ModItem
    {
        public override string Texture => CWRConstant.Item + "Placeable/" + "FoodStallChair";

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FoodStallChair>());
            Item.width = 32;
            Item.height = 32;
            Item.value = 1150;
        }

        public override bool? UseItem(Player player) {
            player.direction = -1;
            return base.UseItem(player);
        }
    }
}
