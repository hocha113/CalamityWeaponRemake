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
            ItemType<AirSpinner>(),
            ItemType<BansheeHook>(),
            ItemType<BlightedCleaver>(),
            ItemType<BurntSienna>(),
            ItemType<CometQuasher>(),
            ItemType<DefiledGreatsword>(),
            ItemType<DragonRage>(),
            ItemType<ElementalLance>(),
            ItemType<ElementalShiv>(),
            ItemType<EntropicClaymore>(),
            ItemType<Excelsus>(),
            ItemType<GildedProboscis>(),
            ItemType<GoldplumeSpear>(),
            ItemType<StreamGouge>(),
            ItemType<Terratomere>(),
            ItemType<TerrorBlade>(),
            ItemType<WindBlade>(),

            ItemType<DragonsBreath>(),
            ItemType<Arbalest>(),
            ItemType<Deathwind>(),
            ItemType<Galeforce>(),
        };

        public static int[] VrsCalamityItemType => new int[]
        {
            ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>(),
            ItemType<CalamityMod.Items.Weapons.Melee.CosmicShiv>(),           
            ItemType<CalamityMod.Items.Weapons.Melee.AirSpinner>(),
            ItemType<CalamityMod.Items.Weapons.Melee.BansheeHook>(),
            ItemType<CalamityMod.Items.Weapons.Melee.BlightedCleaver>(),
            ItemType<CalamityMod.Items.Weapons.Melee.BurntSienna>(),
            ItemType<CalamityMod.Items.Weapons.Melee.CometQuasher>(),
            ItemType<CalamityMod.Items.Weapons.Melee.DefiledGreatsword>(),
            ItemType<CalamityMod.Items.Weapons.Melee.DragonRage>(),
            ItemType<CalamityMod.Items.Weapons.Melee.ElementalLance>(),
            ItemType<CalamityMod.Items.Weapons.Melee.ElementalShiv>(),
            ItemType<CalamityMod.Items.Weapons.Melee.EntropicClaymore>(),
            ItemType<CalamityMod.Items.Weapons.Melee.Excelsus>(),
            ItemType<CalamityMod.Items.Weapons.Melee.GildedProboscis>(),
            ItemType<CalamityMod.Items.Weapons.Melee.GoldplumeSpear>(),
            ItemType<CalamityMod.Items.Weapons.Melee.StreamGouge>(),
            ItemType<CalamityMod.Items.Weapons.Melee.Terratomere>(),
            ItemType<CalamityMod.Items.Weapons.Melee.TerrorBlade>(),
            ItemType<CalamityMod.Items.Weapons.Melee.WindBlade>(),

            ItemType<CalamityMod.Items.Weapons.Ranged.DragonsBreath>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.Arbalest>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.Galeforce>(),
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
