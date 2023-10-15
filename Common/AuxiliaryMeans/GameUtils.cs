using CalamityWeaponRemake.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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
        /// 一个根据语言选项返回字符的方法
        /// </summary>
        public static string Translation(string Chinese = null, string English = null, string Japanese = null, string Russian = null)
        {
            string text;
            if (Language.ActiveCulture.Name == "zh-Hans") text = Chinese;
            else if (Language.ActiveCulture.Name == "ja-Hans") text = Japanese;
            else if (Language.ActiveCulture.Name == "ru-Hans") text = Russian;
            else text = English;
            if (text == null || text == default) text = "Invalid Character";
            return text;
        }

        /// <summary>
        /// 判断是否重写该物品
        /// </summary>
        public static bool RemakeByItem<T>(Item item) where T : ModItem
        {
            return item.type == ModContent.ItemType<T>() && CWRConstant.ForceReplaceResetContent;
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

        /// <summary>
        /// 通过玩家ID、弹幕标识和可选的弹幕类型来获取对应的弹幕索引
        /// </summary>
        /// <param name="player">玩家的ID</param>
        /// <param name="projectileIdentity">弹幕的标识</param>
        /// <param name="projectileType">可选的弹幕类型</param>
        /// <returns>找到的弹幕索引，如果未找到则返回 -1</returns>
        public static int GetProjectileByIdentity(int player, int projectileIdentity, params int[] projectileType)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].identity == projectileIdentity && Main.projectile[i].owner == player
                    && (projectileType.Length == 0 || projectileType.Contains(Main.projectile[i].type)))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 创建一个新的召唤物投射物，并返回其索引
        /// </summary>
        /// <param name="source">实体来源</param>
        /// <param name="spawn">生成位置</param>
        /// <param name="velocity">初始速度</param>
        /// <param name="type">投射物类型</param>
        /// <param name="rawBaseDamage">原始基础伤害</param>
        /// <param name="knockback">击退力度</param>
        /// <param name="owner">所有者ID（默认为255）</param>
        /// <param name="ai0">自定义AI参数0（默认为0）</param>
        /// <param name="ai1">自定义AI参数1（默认为0）</param>
        /// <returns>新投射物的索引</returns>
        public static int NewSummonProjectile(IEntitySource source, Vector2 spawn, Vector2 velocity, int type, int rawBaseDamage, float knockback, int owner = 255, float ai0 = 0, float ai1 = 0)
        {
            int projectileIndex = Projectile.NewProjectile(source, spawn, velocity, type, rawBaseDamage, knockback, owner, ai0, ai1);
            if (projectileIndex != Main.maxProjectiles)
            {
                Main.projectile[projectileIndex].originalDamage = rawBaseDamage;
                Main.projectile[projectileIndex].ContinuouslyUpdateDamageStats = true;
            }
            return projectileIndex;
        }

        public static CWRNpc CWR(this NPC npc)
            => npc.GetGlobalNPC<CWRNpc>();

        public static CWRPlayer CWR(this Player player)
            => player.GetModPlayer<CWRPlayer>();

        public static CWRItems CWR(this Item item)
            => item.GetGlobalItem<CWRItems>();

        #region NetUtils

        /// <summary>
        /// 判断是否处于客户端状态，如果是在单人或者服务端下将返回false
        /// </summary>
        public static bool isClient => Main.netMode == NetmodeID.MultiplayerClient;
        /// <summary>
        /// 仅判断是否处于单人状态，在单人模式下返回true
        /// </summary>
        public static bool isSinglePlayer => Main.netMode == NetmodeID.SinglePlayer;
        /// <summary>
        /// 发收统一端口的实例
        /// </summary>
        public static ModPacket Packet => CalamityWeaponRemake.Instance.GetPacket();
        /// <summary>
        /// 检查一个 Projectile 对象是否属于当前客户端玩家拥有的，如果是，返回true
        /// </summary>
        public static bool IsOwnedByLocalPlayer(this Projectile projectile) => projectile.owner == Main.myPlayer;

        /// <summary>
        /// 同步整个世界状态
        /// </summary>
        public static void SyncWorld()
        {
            if (Main.dedServ)
            {
                NetMessage.SendData(7);
            }
        }

        /// <summary>
        /// 将指定数量的元素从二进制读取器读取到列表中，使用提供的读取函数
        /// </summary>
        /// <typeparam name="T">列表中的元素类型</typeparam>
        /// <param name="reader">要读取的二进制读取器</param>
        /// <param name="list">要填充读取元素的列表</param>
        /// <param name="count">要读取的元素数量</param>
        /// <param name="readFunction">用于从二进制读取器中读取类型 T 的元素的函数</param>
        public static void ReadToList<T>(this BinaryReader reader, List<T> list, int count, Func<BinaryReader, T> readFunction)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                T value = readFunction(reader);
                list.Add(value);
            }
        }

        /// <summary>
        /// 从二进制读取器中读取指定数量的整数到列表中
        /// </summary>
        /// <param name="reader">要读取的二进制读取器</param>
        /// <param name="list">要填充读取整数的列表</param>
        /// <param name="count">要读取的整数数量</param>
        public static void ReadToList(this BinaryReader reader, List<int> list, int count)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                int value = reader.ReadInt32();
                list.Add(value);
            }
        }

        /// <summary>
        /// 从二进制读取器中读取指定数量的字节到列表中
        /// </summary>
        /// <param name="reader">要读取的二进制读取器</param>
        /// <param name="list">要填充读取字节的列表</param>
        /// <param name="count">要读取的字节数量</param>
        public static void ReadToList(this BinaryReader reader, List<byte> list, int count)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                byte value = reader.ReadByte();
                list.Add(value);
            }
        }

        /// <summary>
        /// 从二进制读取器中读取指定数量的单精度浮点数到列表中
        /// </summary>
        /// <param name="reader">要读取的二进制读取器</param>
        /// <param name="list">要填充读取单精度浮点数的列表</param>
        /// <param name="count">要读取的单精度浮点数数量</param>
        public static void ReadToList(this BinaryReader reader, List<float> list, int count)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                float value = reader.ReadSingle();
                list.Add(value);
            }
        }

        /// <summary>
        /// 从二进制读取器中读取指定数量的二维向量到列表中
        /// </summary>
        /// <param name="reader">要读取的二进制读取器</param>
        /// <param name="list">要填充读取二维向量的列表</param>
        /// <param name="count">要读取的二维向量数量</param>
        public static void ReadToList(this BinaryReader reader, List<Vector2> list, int count)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                float valueX = reader.ReadSingle();
                float valueY = reader.ReadSingle();
                list.Add(new Vector2(valueX, valueY));
            }
        }

        /// <summary>
        /// 将列表中的元素数量和元素内容写入二进制写入器，使用提供的写入函数
        /// </summary>
        /// <typeparam name="T">列表中的元素类型</typeparam>
        /// <param name="writer">要写入的二进制写入器</param>
        /// <param name="list">要写入的列表</param>
        /// <param name="writeFunction">用于将类型 T 的元素写入二进制写入器的函数</param>
        public static void WriteList<T>(this BinaryWriter writer, List<T> list, Action<BinaryWriter, T> writeFunction)
        {
            writer.Write(list.Count);
            foreach (T value in list)
            {
                writeFunction(writer, value);
            }
        }

        /// <summary>
        /// 将整数列表写入二进制写入器
        /// </summary>
        /// <param name="writer">要写入的二进制写入器</param>
        /// <param name="list">要写入的整数列表</param>
        public static void WriteList(this BinaryWriter writer, List<int> list)
        {
            writer.Write(list.Count);
            foreach (int value in list)
            {
                writer.Write(value);
            }
        }

        /// <summary>
        /// 将字节列表写入二进制写入器
        /// </summary>
        /// <param name="writer">要写入的二进制写入器</param>
        /// <param name="list">要写入的字节列表</param>
        public static void WriteList(this BinaryWriter writer, List<byte> list)
        {
            writer.Write(list.Count);
            foreach (byte value in list)
            {
                writer.Write(value);
            }
        }

        /// <summary>
        /// 将单精度浮点数列表写入二进制写入器
        /// </summary>
        /// <param name="writer">要写入的二进制写入器</param>
        /// <param name="list">要写入的单精度浮点数列表</param>
        public static void WriteList(this BinaryWriter writer, List<float> list)
        {
            writer.Write(list.Count);
            foreach (float value in list)
            {
                writer.Write(value);
            }
        }

        /// <summary>
        /// 将二维向量列表写入二进制写入器
        /// </summary>
        /// <param name="writer">要写入的二进制写入器</param>
        /// <param name="list">要写入的二维向量列表</param>
        public static void WriteList(this BinaryWriter writer, List<Vector2> list)
        {
            writer.Write(list.Count);
            foreach (Vector2 value in list)
            {
                writer.Write(value.X);
                writer.Write(value.Y);
            }
        }


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
