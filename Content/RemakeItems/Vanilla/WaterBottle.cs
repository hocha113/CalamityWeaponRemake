using CalamityWeaponRemake.Content.RemakeItems.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace CalamityWeaponRemake.Content.RemakeItems.Vanilla
{
    internal class WaterBottle : BaseRItem
    {
        public override int TargetID => ItemID.BottledWater;
        public override bool FormulaSubstitution => false;
        public override void OnConsumeItem(Item item, Player player) {
            //player.QuickSpawnItem(player.parent(), ItemID.Bottle);
        }

        public static void OnRecipeBottle(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack) {
            Main.NewText("你装了一瓶水");
        }
    }
}
