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

namespace CalamityWeaponRemake.Content
{
    internal class CWRRecipes : ModSystem
    {
        public static int[] RemakItemType => new int[]
        {
            //近战
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
            ItemType<AegisBlade>(),
            ItemType<Murasama>(),
            ItemType<HolyCollider>(),
            //远程
            ItemType<DragonsBreathRifle>(),
            ItemType<Arbalest>(),
            ItemType<Deathwind>(),
            ItemType<Galeforce>(),
            //魔法
            ItemType<DeathhailStaff>(),
            ItemType<FatesReveal>(),
            ItemType<GhastlyVisage>(),
            ItemType<Tradewinds>(),
        };

        public static int[] VrsCalamityItemType => new int[]
        {   
            //近战
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
            ItemType<CalamityMod.Items.Weapons.Melee.AegisBlade>(),
            ItemType<CalamityMod.Items.Weapons.Melee.Murasama>(),
            ItemType<CalamityMod.Items.Weapons.Melee.HolyCollider>(),
            //远程
            ItemType<CalamityMod.Items.Weapons.Ranged.DragonsBreath>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.Arbalest>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>(),
            ItemType<CalamityMod.Items.Weapons.Ranged.Galeforce>(),
            //魔法
            ItemType<CalamityMod.Items.Weapons.Magic.DeathhailStaff>(),
            ItemType<CalamityMod.Items.Weapons.Magic.FatesReveal>(),
            ItemType<CalamityMod.Items.Weapons.Magic.GhastlyVisage>(),
            ItemType<CalamityMod.Items.Weapons.Magic.Tradewinds>(),
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
