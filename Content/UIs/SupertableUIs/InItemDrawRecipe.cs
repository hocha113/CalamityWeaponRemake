using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityWeaponRemake.Content.UIs.SupertableUIs
{
    //我现在还没想好该如何实现这些，所以先这样吧
    public class InItemDrawRecipe
    {
        public Texture2D mainBookPValue => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/BookPans");
        public Texture2D mainCellValue => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/MainValue3");

        public void Draw(SpriteBatch spriteBatch, Vector2 drawPos, Item[] items) {

        }
    }
}
