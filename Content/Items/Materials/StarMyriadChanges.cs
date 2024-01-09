using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.DataStructures;

namespace CalamityWeaponRemake.Content.Items.Materials
{
    internal class StarMyriadChanges : ModItem
    {
        public override string Texture => "CalamityWeaponRemake/StarMyriadChanges";
        public new string LocalizationCategory => "Items.Materials";

        public override void SetStaticDefaults() {
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Gel;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 30));
        }

        public override void SetDefaults() {
            Item.width = Item.height = 25;
            Item.rare = ItemRarityID.Lime;
            Item.value = Terraria.Item.sellPrice(gold: 99999);
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.CWR().isInfiniteItem = true;
            Item.CWR().OmigaSnyContent = SupertableRecipeDate.FullItems17;
        }

        public static void DrawItemIcon(SpriteBatch spriteBatch, Vector2 position, Color color, int Type, float alp = 1, float slp = 0) {
            Texture2D value = TextureAssets.Item[Type].Value;
            if (slp == 0) {
                slp = 32 / (float)value.Width;
            }
            spriteBatch.Draw(value, position, null, color * alp, 1, value.Size() / 2, slp, SpriteEffects.None, 0);
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                string text = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.StarMyriadChanges.DisplayName");
                InfiniteIngot.drawColorText(Main.spriteBatch, line, text, basePosition);
                return false;
            }
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            return true;
        }
    }
}
