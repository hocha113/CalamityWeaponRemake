using Terraria.UI;

namespace CalamityWeaponRemake.Common.UITools
{
    public static class Layout
    {
        /// <summary>
        /// 对UI版面的布局设置
        /// </summary>
        public static void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
    }
}
