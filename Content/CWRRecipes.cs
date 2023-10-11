using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Items.Ranged;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityWeaponRemake.Content
{
    internal class CWRRecipes : ModSystem
    {
        public static int[] RemakItemType => new int[]
        {
            ItemType<BrinyBaron>(),
            ItemType<CosmicShiv>(),
            ItemType<DragonsBreath>(),
        };

        public static int[] VrsCalamityItemType => new int[]
        {
            ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>(),
            ItemType<CalamityMod.Items.Weapons.Melee.CosmicShiv>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.DragonsBreath>(),
        };

        public override void PostAddRecipes()
        {
            if (CWRConstant.ForceReplaceResetContent)
            {
                for (int i = 0; i < Recipe.numRecipes; i++)
                {
                    Recipe recipe = Main.recipe[i];

                    for (int j = 0; j < RemakItemType.Length; j++)
                    {
                        int type = RemakItemType[j];

                        if (recipe.HasResult(type))
                        {
                            recipe.DisableRecipe();
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < RemakItemType.Length; j++)
                {
                    int type = RemakItemType[j];
                    int vrsType = VrsCalamityItemType[j];
                    Recipe.Create(type).AddIngredient(vrsType).Register();
                }
            }
        }
    }
}
