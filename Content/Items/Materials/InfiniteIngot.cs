using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI.Chat;
using static CalamityWeaponRemake.CalamityWeaponRemake;

namespace CalamityWeaponRemake.Content.Items.Materials
{
    internal class InfiniteIngot : ModItem
    {
        public override string Texture => CWRConstant.Item + "Materials/InfiniteIngot";
        public new string LocalizationCategory => "Items.Materials";
        public float QFH
        {
            get
            {
                bool hasMod(string name)
                {
                    return Instance.LoadMods.Any((Mod mod) => mod.Name == name);
                }
                float sengs = 1;
                float overMdgs = Instance.LoadMods.Count / 10f;
                if (overMdgs < 0.5f)
                    overMdgs = 0;
                sengs += overMdgs;
                if (hasMod("LightAndDarknessMod"))
                    sengs += 0.1f;
                if (hasMod("DDmod"))
                    sengs += 0.1f;
                if (hasMod("MaxStackExtra"))
                    sengs += 0.1f;
                if (hasMod("Wild"))
                    sengs += 0.1f;
                if (hasMod("Coralite"))
                    sengs += 0.1f;
                if (hasMod("AncientsAwakened"))
                    sengs += 0.1f;
                if (hasMod("NoxusBoss"))
                    sengs += 0.25f;
                if (hasMod("FargowiltasSouls"))
                    sengs += 0.25f;
                if (hasMod("MagicBuilder"))
                    sengs += 0.25f;
                if (hasMod("CalamityPostMLBoots"))
                    sengs += 0.25f;
                if (hasMod("仆从暴击"))
                    sengs += 0.25f;
                return sengs;
            }
        }
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 9999;
            ItemID.Sets.SortingPriorityMaterials[Type] = 114;
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 12));
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 25;
            Item.maxStack = 99;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.value = Item.sellPrice(gold: 999);
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName" && line.Mod == "Terraria")
            { 
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                string Txet = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.InfiniteIngot.DisplayName");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.Font, Txet, basePosition, Main.DiscoColor, line.Rotation, line.Origin, line.BaseScale * 1.06f, line.MaxWidth, line.Spread);
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            int QFD(int num) => (int)(num * QFH);
            CreateRecipe()
                .AddIngredient<AerialiteBar>(QFD(150))//水华锭
                .AddIngredient<AuricBar>(QFD(5))//圣金源锭
                .AddIngredient<ShadowspecBar>(QFD(5))//影魔锭
                .AddIngredient<AstralBar>(QFD(50))//彗星锭
                .AddIngredient<CosmiliteBar>(QFD(150))//宇宙锭
                .AddIngredient<CryonicBar>(QFD(50))//极寒锭
                .AddIngredient<PerennialBar>(QFD(150))//龙篙锭
                .AddIngredient<ScoriaBar>(QFD(75))//岩浆锭
                .AddIngredient<MolluskHusk>(QFD(1200))//生物质
                .AddIngredient<MurkyPaste>(QFD(150))//泥浆杂草混合物质
                .AddIngredient<DepthCells>(QFD(1200))//深渊生物组织
                .AddIngredient<DivineGeode>(QFD(1200))//圣神晶石
                .AddIngredient<DubiousPlating>(QFD(1200))//废弃装甲
                .AddIngredient<EffulgentFeather>(QFD(150))//闪耀金羽
                .AddIngredient<ExoPrism>(QFD(50))//星流棱晶
                .AddIngredient<BloodstoneCore>(QFD(1200))//血石核心
                .AddIngredient<CoreofCalamity>(QFD(1200))//灾劫精华
                .AddIngredient<AscendantSpiritEssence>(QFD(1800))//化神精魄
                .AddIngredient<AshesofCalamity>(QFD(150))//灾厄尘
                .AddIngredient<AshesofAnnihilation>(QFD(150))//至尊灾厄精华
                .AddIngredient<LifeAlloy>(QFD(600))//生命合金
                .AddIngredient<LivingShard>(QFD(600))//生命物质
                .AddIngredient<Lumenyl>(QFD(600))//流明晶
                .AddIngredient<MeldConstruct>(QFD(1800))//幻塔物质
                .AddIngredient<MiracleMatter>(QFD(50))//奇迹物质
                .AddIngredient<Polterplasm>(QFD(1200))//幻魂
                .AddIngredient<RuinousSoul>(QFD(600))//幽花之魂
                .AddIngredient<DarkPlasma>(QFD(50))//暗物质
                .AddIngredient<UnholyEssence>(QFD(1800))//灼火精华
                .AddIngredient<TwistingNether>(QFD(75))//扭曲虚空
                .AddIngredient<ArmoredShell>(QFD(75))//装甲心脏
                .AddIngredient<YharonSoulFragment>(QFD(75))//龙魂
                .AddIngredient<Rock>(1)//古恒石
                .AddTile(ModContent.TileType<DarkMatterCompressor>())
                .Register();
        }
    }
}
