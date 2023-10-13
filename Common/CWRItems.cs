using CalamityMod;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    public class CWRItems : GlobalItem
    {
        //这个的创建是一种妥协，因为替换原模组内容的手段无非是Global类与反射等机制代码，
        //但这些方法都无法很好且高效的解决物品类中的特有成员值的适配问题，
        //比如，重制物品A类的相对于原物品类新增加了成员a，但如何让原模组的物品A在应用重制修改后也能正常使用成员a?
        //我无法找到更好的解决方法，于是只能创建并即将大量应用这个CWRItems类
        public override bool InstancePerEntity => true;

        public bool remakeItem;

        public float BansheeHookCharge = 500;
        public int KevinCharge = 500;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (remakeItem)
            {
                TooltipLine nameLine = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "ItemName" && x.Mod == "Terraria");
                ApplyNameLineColor(
                    new Color(1f, 0.72f + 0.2f * Main.DiscoG / 255f, 0.45f + 0.5f * Main.DiscoG / 255f)
                    , nameLine
                    );

                AppAwakeningLine(tooltips);
            }
        }

        private void ApplyNameLineColor(Color color, TooltipLine nameLine) => nameLine.OverrideColor = color;

        public static void AppAwakeningLine(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(CalamityWeaponRemake.Instance, "CalamityWeaponRemake",
                    CalamityUtils.ColorMessage(
                        GameUtils.Translation("- 觉醒 -", "- Awakening -")
                        , new Color(196, 35, 44))
                    );
            tooltips.Add(line);
        }
    }
}
