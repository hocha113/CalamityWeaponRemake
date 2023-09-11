using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Common.AuxiliaryMeans
{
    public class GameUtils
    {
        /// <summary>
        /// 在游戏中发送文本消息
        /// </summary>
        /// <param name="message">要发送的消息文本</param>
        /// <param name="colour">（可选）消息的颜色,默认为 null</param>
        public static void Text(string message, Color? colour = null)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(message, colour);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), (Color)(colour == null ? Color.White : colour));
        }
    }
}
