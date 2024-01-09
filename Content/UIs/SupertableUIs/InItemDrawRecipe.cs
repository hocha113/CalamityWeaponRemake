using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography;
using Terraria;
using Terraria.GameContent;

namespace CalamityWeaponRemake.Content.UIs.SupertableUIs
{
    public class InItemDrawRecipe
    {
        public static InItemDrawRecipe Instance;//这个类并不需要一个Load方法，它会在 RecipeUI 类中被初始化加载

        public Texture2D mainBookPValue => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/BookPans");
        public Texture2D mainCellValue => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/MainValue3");
        public Texture2D TOMTex => CWRUtils.GetT2DValue(CWRConstant.Asset + "Items/Placeable/" + "TransmutationOfMatterItem");

        public bool OnSupTale => SupertableUI.Instance.onMainP || SupertableUI.Instance.onMainP2 || SupertableUI.Instance.onInputP || SupertableUI.Instance.onCloseP;

        /// <summary>
        /// 在只利用一个数字索引的情况下反向计算出对应的格坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 ArcCellPos(int index, Vector2 pos) {
            int y = index / 9;
            int x = index - (y * 9);
            return (new Vector2(x, y) * new Vector2(48, 46)) + pos;
        }

        public Vector2 Prevention(Vector2 pos) {
            float maxW = mainBookPValue.Width * 2.2f;
            float maxH = mainBookPValue.Height * 2.5f;
            if (pos.X < 0) {
                pos.X = 0;
            }
            if (pos.X + maxW > Main.screenWidth) {
                pos.X = Main.screenWidth - maxW;
            }
            if (pos.Y < 0) {
                pos.Y = 0;
            }
            if (pos.Y + maxH > Main.screenHeight) {
                pos.Y = Main.screenHeight - maxH;
            }
            return pos;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawPos, string[] names) {
            drawPos = Prevention(drawPos);
            Item[] items = new Item[names.Length];
            Item targetItem = SupertableUI.InStrGetItem(names[names.Length - 1], true);
            for(int i = 0; i < names.Length - 1; i++) {
                string name = names[i];
                Item item = SupertableUI.InStrGetItem(name, true);
                items[i] = item;                
            }
            spriteBatch.Draw(mainBookPValue, drawPos + new Vector2(-100, -100), null, Color.DarkGoldenrod, 0, Vector2.Zero, new Vector2(2.2f, 2.5f), SpriteEffects.None, 0);//绘制出UI主体
            spriteBatch.Draw(mainCellValue, drawPos + new Vector2(-25, -25), null, Color.DarkGoldenrod, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出UI主体
            spriteBatch.Draw(TOMTex, drawPos + new Vector2(-70, -80), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出UI主体
            Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, 
                $"{CWRUtils.Translation("在", "In") + CalamityUtils.GetItemName<TransmutationOfMatterItem>() + CWRUtils.Translation("进行终焉合成", "Perform final synthesis")}："
                , drawPos.X - 20, drawPos.Y - 60, Color.White, Color.Black, new Vector2(0.3f), 1f);

            if (targetItem != null) {
                string text = $"{CWRUtils.Translation("合成获得：", "Synthetic acquisition：") + CalamityUtils.GetItemName(targetItem.type)}";
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, drawPos.X - 20, drawPos.Y + 410, Color.White, Color.Black, new Vector2(0.3f), 1f);

                SupertableUI.DrawItemIcons(spriteBatch, targetItem, drawPos + new Vector2(text.Length * 20, 410), new Vector2(0.0001f, 0.0001f), Color.White, 1, 1.5f) ;
            }

            for (int i = 0; i < items.Length - 1; i++) {//遍历绘制出UI格中的所有预览物品
                if (items[i] != null) {
                    Item item = items[i];
                    if (item != null) {
                        SupertableUI.DrawItemIcons(spriteBatch, item, ArcCellPos(i, drawPos), new Vector2(0.0001f, 0.0001f));
                    }
                }
            }
        }
    }
}
