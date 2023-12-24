﻿using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityWeaponRemake.Content.Tiles;
using System.Buffers;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using static CalamityWeaponRemake.CWRMod;
using static Terraria.ModLoader.ModContent;
using CalamityWeaponRemake.Common;
using CalamityMod.Rarities;
using Terraria.DataStructures;
using Terraria.ID;
using CalamityWeaponRemake.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Items.Materials
{
    internal class InfinityCatalyst : ModItem
    {
        public override string Texture => CWRConstant.Item + "Materials/InfinityCatalyst";
        public float QFH {
            get {
                const float baseBonus = 1.0f;
                var modBonuses = new Dictionary<string, float>{
                    {"LightAndDarknessMod", 0.1f},
                    {"DDmod", 0.1f},
                    {"MaxStackExtra", 0.1f},
                    {"Wild", 0.1f},
                    {"Coralite", 0.1f},
                    {"AncientsAwakened", 0.1f},
                    {"NoxusBoss", 0.25f},
                    {"FargowiltasSouls", 0.25f},
                    {"MagicBuilder", 0.25f},
                    {"CalamityPostMLBoots", 0.25f},
                    {"仆从暴击", 0.25f}
                };
                float overMdgs = Instance.LoadMods.Count / 10f;
                overMdgs = overMdgs < 0.5f ? 0 : overMdgs;
                float totalBonus = modBonuses.Sum(pair => hasMod(pair.Key) ? pair.Value : 0);
                return baseBonus + overMdgs + totalBonus;
            }
        }
        private bool hasMod(string name) {
            return Instance.LoadMods.Any(mod => mod.Name == name);
        }

        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 9999;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 16));
        }

        public override void SetDefaults() {
            Item.width = Item.height = 32;
            Item.maxStack = 99;
            Item.rare = RarityType<HotPink>();
            Item.value = Item.sellPrice(gold: 999);
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.CWR().isInfiniteItem = true;
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                string text = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.InfinityCatalyst.DisplayName");
                InfiniteIngot.drawColorText(Main.spriteBatch, line, text, basePosition);
                return false;
            }
            return true;
        }

        public override void AddRecipes() {
            int QFD(int num) => (int)(num * QFH);

            CreateRecipe()
                .AddIngredient<AerialiteBar>(QFD(15))//水华锭
                .AddIngredient<AuricBar>(QFD(5))//圣金源锭
                .AddIngredient<ShadowspecBar>(QFD(5))//影魔锭
                .AddIngredient<AstralBar>(QFD(15))//彗星锭
                .AddIngredient<CosmiliteBar>(QFD(15))//宇宙锭
                .AddIngredient<CryonicBar>(QFD(15))//极寒锭
                .AddIngredient<PerennialBar>(QFD(15))//龙篙锭
                .AddIngredient<ScoriaBar>(QFD(15))//岩浆锭
                .AddIngredient<MolluskHusk>(QFD(200))//生物质
                .AddIngredient<MurkyPaste>(QFD(10))//泥浆杂草混合物质
                .AddIngredient<DepthCells>(QFD(300))//深渊生物组织
                .AddIngredient<DivineGeode>(QFD(120))//圣神晶石
                .AddIngredient<DubiousPlating>(QFD(120))//废弃装甲
                .AddIngredient<EffulgentFeather>(QFD(15))//闪耀金羽
                .AddIngredient<ExoPrism>(QFD(10))//星流棱晶
                .AddIngredient<BloodstoneCore>(QFD(300))//血石核心
                .AddIngredient<CoreofCalamity>(QFD(300))//灾劫精华
                .AddIngredient<AscendantSpiritEssence>(QFD(300))//化神精魄
                .AddIngredient<AshesofCalamity>(QFD(150))//灾厄尘
                .AddIngredient<AshesofAnnihilation>(QFD(10))//至尊灾厄精华
                .AddIngredient<LifeAlloy>(QFD(300))//生命合金
                .AddIngredient<LivingShard>(QFD(300))//生命物质
                .AddIngredient<Lumenyl>(QFD(300))//流明晶
                .AddIngredient<MeldConstruct>(QFD(300))//幻塔物质
                .AddIngredient<MiracleMatter>(QFD(50))//奇迹物质
                .AddIngredient<Polterplasm>(QFD(200))//幻魂
                .AddIngredient<RuinousSoul>(QFD(100))//幽花之魂
                .AddIngredient<DarkPlasma>(QFD(10))//暗物质
                .AddIngredient<UnholyEssence>(QFD(300))//灼火精华
                .AddIngredient<TwistingNether>(QFD(25))//扭曲虚空
                .AddIngredient<ArmoredShell>(QFD(25))//装甲心脏
                .AddIngredient<YharonSoulFragment>(QFD(75))//龙魂
                .AddIngredient<Rock>(1)//古恒石
                .AddOnCraftCallback(SpawnAction)
                .AddTile(TileType<DarkMatterCompressor>())
                .Register();
        }

        public static void SpawnAction(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack) {
            item.CWR().noDestruct = true;
            SoundEngine.PlaySound(new SoundStyle(CWRConstant.Sound + "Pewatermagic"));
        }
    }
}
