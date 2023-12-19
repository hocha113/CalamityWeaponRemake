using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityWeaponRemake.Content
{
    internal class CWRRecipes : ModSystem
    {
        public override void PostAddRecipes() {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                    baseRItem.UnLoadItemRecipe();
                }
            }
            else {
                foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                    baseRItem.LoadItemRecipe();
                }
            }
            //修改暴政的合成
            for (int i = 0; i < Recipe.numRecipes; i++) {
                Recipe recipe = Main.recipe[i];
                if (recipe.HasResult(ItemType<CalamityMod.Items.Weapons.Melee.TheEnforcer>())) {
                    recipe.DisableRecipe();
                }
            }
            Recipe.Create(ItemType<CalamityMod.Items.Weapons.Melee.TheEnforcer>())
                .AddIngredient(ItemType<CalamityMod.Items.Weapons.Melee.HolyCollider>())
                .AddIngredient(ItemType<CosmiliteBar>(), 5)
                .AddTile(TileType<CosmicAnvil>())
                .Register();
            //添加圣火之刃的合成
            Recipe.Create(ItemType<CalamityMod.Items.Weapons.Melee.HolyCollider>())
                .AddIngredient(ItemType<CalamityMod.Items.Weapons.Melee.CelestialClaymore>())
                .AddIngredient(ItemType<DivineGeode>(), 16)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public static string Any => Language.GetTextValue("LegacyMisc.37");
        public static RecipeGroup ARGroup;
        public static RecipeGroup GodDWGroup;

        public override void AddRecipeGroups() {
            string apostolicRelics = Language.GetTextValue($"Mods.CalamityWeaponRemake.CWRRecipes.ApostolicRelics");
            ARGroup = new RecipeGroup(() => $"{Any} {apostolicRelics}", ItemType<ArmoredShell>(), ItemType<DarkPlasma>(), ItemType<TwistingNether>());
            RecipeGroup.RegisterGroup(apostolicRelics, ARGroup);

            string godEaterWeapon = Language.GetTextValue($"Mods.CalamityWeaponRemake.CWRRecipes.GodEaterWeapon");
            GodDWGroup = new RecipeGroup(() => $"{Any} {apostolicRelics}"
                , ItemType<CalamityMod.Items.Weapons.Melee.Excelsus>()
                , ItemType<CalamityMod.Items.Weapons.Melee.TheObliterator>()
                , ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>()
                , ItemType<CalamityMod.Items.Weapons.Magic.DeathhailStaff>()
                , ItemType<CalamityMod.Items.Weapons.Summon.StaffoftheMechworm>()
                , ItemType<CalamityMod.Items.Weapons.Rogue.Eradicator>()
                , ItemType<CalamityMod.Items.Weapons.Melee.CosmicDischarge>()
                , ItemType<CalamityMod.Items.Weapons.Ranged.Norfleet>()
                );
            RecipeGroup.RegisterGroup(godEaterWeapon, GodDWGroup);
        }
    }
}
