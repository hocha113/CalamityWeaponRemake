using CalamityMod;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityWeaponRemake.Common
{
    public class CWRItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

        /// <summary>
        /// 用于存储物品的状态值，对这个数组的使用避免了额外类成员的创建
        /// (自建类成员数据对于修改物品而言总是令人困惑)。
        /// 这个数组不会自动的网络同步，需要在合适的时机下调用同步指令
        /// </summary>
        public float[] ai = new float[] {0, 0, 0};
        /// <summary>
        /// 是否是一个重制物品
        /// </summary>
        public bool remakeItem;
        /// <summary>
        /// 是否正在真近战
        /// </summary>
        public bool closeCombat;
        /// <summary>
        /// 正在手持这个物品的玩家实例
        /// </summary>
        public Player HoldOwner = null;
        /// <summary>
        /// 一般用于近战类武器的充能值
        /// </summary>
        public float MeleeCharge;

        public override void NetSend(Item item, BinaryWriter writer)
        {
            base.NetSend(item, writer);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            base.NetReceive(item, reader);
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("_MeleeCharge", MeleeCharge);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            MeleeCharge = tag.GetFloat("_MeleeCharge");
        }

        public override void HoldItem(Item item, Player player)
        {
            OwnerByDir(item, player);
        }

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

        private void OwnerByDir(Item item, Player player)
        {
            if (item.useStyle == ItemUseStyleID.Swing && player.whoAmI == Main.myPlayer && player.PressKey())
            {
                player.direction = Math.Sign(player.position.To(Main.MouseWorld).X);
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
