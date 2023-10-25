using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityMod.Rarities;
using System.Linq;
using System;
using CalamityMod;

namespace CalamityWeaponRemake.Content.Items
{
    internal class TheLuxirSoulArtifact : ModItem
    {
        public new string LocalizationCategory => "Items.Accessories";

        public override string Texture => CWRConstant.Item + "TheLuxirSoulArtifact";

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 48;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.accessory = true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            if (tooltips == null || player == null) return;

            Item item = player.HeldItem;
            if (item == null) return;

            TooltipLine cumstops = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[tips]") && x.Mod == "Terraria");
            if (cumstops == null) return;

            if (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<TrueMeleeNoSpeedDamageClass>())
            {
                cumstops.Text = GameUtils.Translation(
                    "刀刃的挥舞将发射出炽热的灵魂\n"
                    + "忠！诚！",
                    "The wave of the blade will emit a fiery soul\n"
                    + "Loyal! !"
                    );
                cumstops.OverrideColor = Color.Lerp(Color.Red, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else if (item.CountsAsClass<ThrowingDamageClass>())
            {
                cumstops.Text = GameUtils.Translation(
                    "投掷出高速弹跳的耀界之灵",
                    "Hurl the Spirit of Glory with a high speed bounce"
                    );
                cumstops.OverrideColor = Color.Lerp(Color.BlueViolet, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else if (item.CountsAsClass<RangedDamageClass>())
            {
                cumstops.Text = GameUtils.Translation(
                    "枪口将迸发出耀界闪电",
                    "The muzzle of the gun will burst forth lightning"
                    );
                cumstops.OverrideColor = Color.Lerp(Color.AliceBlue, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else if (item.CountsAsClass<MagicDamageClass>())
            {
                cumstops.Text = GameUtils.Translation(
                    "散落的魔力将凝聚为金源炸弹",
                    "The scattered magic will condense into the gold source bomb"
                    );
                cumstops.OverrideColor = Color.Lerp(Color.Gold, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else if (item.CountsAsClass<SummonDamageClass>())
            {
                cumstops.Text = GameUtils.Translation(
                    "召唤泛金能量体为你而战",
                    "Summon Pangold Energies to fight for you"
                    );
                cumstops.OverrideColor = Color.Lerp(Color.LightGoldenrodYellow, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else
            {
                cumstops.Text = GameUtils.Translation(
                    "耀界之神将对你提供多种援助",
                    "The Gods of Glory will offer you many kinds of assistance"
                    );
                cumstops.OverrideColor = Color.Lerp(Color.LavenderBlush, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CWR().theRelicLuxor = 2;
        }
    }
}
