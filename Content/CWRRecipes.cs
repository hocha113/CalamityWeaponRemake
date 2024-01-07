using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using CalamityWeaponRemake.Content.RemakeItems.Vanilla;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityWeaponRemake.Content
{
    internal class CWRRecipes : ModSystem
    {
        public static void SpawnAction(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack) {
            item.TurnToAir();
            CombatText.NewText(Main.LocalPlayer.Hitbox, Main.DiscoColor, Language.GetTextValue($"Mods.CalamityWeaponRemake.Tools.RecipesLoseText"));
        }

        public override void PostAddRecipes() {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                    if (baseRItem.FormulaSubstitution)
                        baseRItem.UnLoadItemRecipe();
                }
            }
            else {
                foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                    if (baseRItem.FormulaSubstitution)
                        baseRItem.LoadItemRecipe();
                }
            }
            //添加无尽锭的额外联动合成
            {
                if (CWRIDs.EternitySoul > ItemID.None) {
                    for (int i = 0; i < Recipe.numRecipes; i++) {
                        Recipe recipe = Main.recipe[i];
                        if (recipe.HasResult(ItemType<InfiniteIngot>())) {
                            recipe.AddIngredient(CWRIDs.EternitySoul);
                        }
                    }
                }
            }
            //修改暴政的合成
            {
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
            }
            //添加圣火之刃的合成
            {
                Recipe.Create(ItemType<CalamityMod.Items.Weapons.Melee.HolyCollider>())
                .AddIngredient(ItemType<CalamityMod.Items.Weapons.Melee.CelestialClaymore>())
                .AddIngredient(ItemType<DivineGeode>(), 16)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }
            //添加风暴长矛的合成
            {
                Recipe.Create(ItemID.ThunderSpear)
                .AddIngredient(ItemID.Spear)
                .AddIngredient<StormlionMandible>(5)
                .AddTile(TileID.Anvils)
                .Register();
                Recipe.Create(ItemID.ThunderSpear)
                    .AddIngredient(ItemID.Trident)
                    .AddIngredient<StormlionMandible>(5)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            //添加水瓶的合成事件
            {
                for (int i = 0; i < Recipe.numRecipes; i++) {
                    Recipe recipe = Main.recipe[i];
                    if (recipe.HasResult(ItemID.BottledWater)) {
                        recipe.AddOnCraftCallback(WaterBottle.OnRecipeBottle);
                    }
                }
            }
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
