using CalamityMod.Items.Placeables.FurnitureExo;
using CalamityMod.Tiles.FurnitureExo;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Placeable
{
    internal class InfiniteToiletItem : ModItem
    {
        public override string Texture => CWRConstant.Item + "Placeable/" + "InfiniteToiletItem";
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<InfiniteToilet>();
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                string text = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.InfiniteToiletItem.DisplayName");
                InfiniteIngot.drawColorText(Main.spriteBatch, line, text, basePosition);
                return false;
            }
            return true;
        }

        public static void DrawItemIcon(SpriteBatch spriteBatch, Vector2 position, int Type, float alp = 1) {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix);
            for (int i = 0; i < 13; i++)
                spriteBatch.Draw(TextureAssets.Item[Type].Value, position, null, Main.DiscoColor * alp, Main.GameUpdateCount * 0.1f, TextureAssets.Item[Type].Value.Size() / 2, 1, SpriteEffects.None, 0);
            spriteBatch.ResetUICanvasState();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            DrawItemIcon(spriteBatch, position, Type);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, Item.Center - Main.screenPosition, null, Main.DiscoColor, Main.GameUpdateCount * 0.1f, TextureAssets.Item[Type].Value.Size() / 2, 1, SpriteEffects.None, 0);
            return false;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<InfiniteIngot>(15)
                .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => {
                    if (type == ModContent.ItemType<InfiniteIngot>()) {
                        amount = 0;
                    }
                })
                .AddOnCraftCallback(CWRRecipes.SpawnAction)
                .Register();
        }
    }
}
