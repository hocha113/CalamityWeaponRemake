using CalamityMod.Items.Materials;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Magic;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Items.Ranged;
using CalamityWeaponRemake.Content.Tiles;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using CalamityWeaponRemake.Content.RemakeItems.Core;

namespace CalamityWeaponRemake.Content
{
    internal class CWRRecipes : ModSystem
    {
        public override void PostAddRecipes(){
            if (CWRConstant.ForceReplaceResetContent){
                foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                    baseRItem.UnLoadItemRecipe();
                }
            }
            else{
                foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                    baseRItem.LoadItemRecipe();
                }
            }

            Recipe.Create(ItemType<CalamityMod.Items.Weapons.Melee.HolyCollider>())
                .AddIngredient(ItemType<CalamityMod.Items.Weapons.Melee.CelestialClaymore>())
                .AddIngredient(ItemType<DivineGeode>(), 16)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public static string Any => Language.GetTextValue("LegacyMisc.37");
        public static RecipeGroup ARGroup;
        public static RecipeGroup GodDWGroup;

        public override void AddRecipeGroups()
        {
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
