using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalamityWeaponRemake.Common.AuxiliaryMeans
{
    public static class GameUtils
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

        /// <summary>
        /// 检查伤害类型是否与指定类型匹配或继承自指定类型
        /// </summary>
        /// <param name="damageClass">要检查的伤害类型</param>
        /// <param name="intendedClass">目标伤害类型</param>
        /// <returns>如果匹配或继承，则为 true；否则为 false</returns>
        public static bool CountsAsClass(this DamageClass damageClass, DamageClass intendedClass)
        {
            return damageClass == intendedClass || damageClass.GetEffectInheritance(intendedClass);
        }

        #region NetUtils

        /// <summary>
        /// 判断是否处于非服务端状态，如果是在单人或者客户端下将返回true
        /// </summary>
        public static bool isClient => Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.MultiplayerClient;
        /// <summary>
        /// 仅判断是否处于单人状态，在单人模式下返回true
        /// </summary>
        public static bool isSinglePlayer => Main.netMode == NetmodeID.SinglePlayer;


        /// <summary>
        /// 生成Boss级实体，考虑网络状态
        /// </summary>
        /// <param name="player">触发生成的玩家实例</param>
        /// <param name="bossType">要生成的 Boss 的类型</param>
        /// <param name="obeyLocalPlayerCheck">是否要遵循本地玩家检查</param>
        public static void SpawnBossNetcoded(Player player, int bossType, bool obeyLocalPlayerCheck = true)
        {

            if (player.whoAmI == Main.myPlayer || !obeyLocalPlayerCheck)
            {
                // 如果使用物品的玩家是客户端
                // （在此明确排除了服务器端）

                SoundEngine.PlaySound(SoundID.Roar, player.position);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 如果玩家不在多人游戏中，直接生成 Boss
                    NPC.SpawnOnPlayer(player.whoAmI, bossType);
                }
                else
                {
                    // 如果玩家在多人游戏中，请求生成
                    // 仅当 NPCID.Sets.MPAllowedEnemies[type] 为真时才有效，需要在 NPC 代码中设置

                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: bossType);
                }
            }
        }

        /// <summary>
        /// 在易于使用的方式下生成一个新的 NPC，考虑网络状态
        /// </summary>
        /// <param name="source">生成 NPC 的实体源</param>
        /// <param name="spawnPos">生成的位置</param>
        /// <param name="type">NPC 的类型</param>
        /// <param name="start">NPC 的初始状态</param>
        /// <param name="ai0">NPC 的 AI 参数 0</param>
        /// <param name="ai1">NPC 的 AI 参数 1</param>
        /// <param name="ai2">NPC 的 AI 参数 2</param>
        /// <param name="ai3">NPC 的 AI 参数 3</param>
        /// <param name="target">NPC 的目标 ID</param>
        /// <param name="velocity">NPC 的初始速度</param>
        /// <returns>新生成的 NPC 的 ID</returns>
        public static int NewNPCEasy(IEntitySource source, Vector2 spawnPos, int type, int start = 0, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int target = 255, Vector2 velocity = default)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return Main.maxNPCs;

            int n = NPC.NewNPC(source, (int)spawnPos.X, (int)spawnPos.Y, type, start, ai0, ai1, ai2, ai3, target);
            if (n != Main.maxNPCs)
            {
                if (velocity != default)
                {
                    Main.npc[n].velocity = velocity;
                }

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
            }

            return n;
        }

        #endregion
    }
}
