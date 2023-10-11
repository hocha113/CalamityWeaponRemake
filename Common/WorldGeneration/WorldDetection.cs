using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace CalamityWeaponRemake.Common.WorldGeneration
{
    public static class WorldDetection
    {
        /// <summary>
        /// 在世界中生成矿石
        /// </summary>
        /// <param name="tileID">矿石的瓦块ID</param>
        /// <param name="veinSize">矿脉的大小</param>
        /// <param name="chanceDenominator">生成机会的分母，值越小生成机会越大</param>
        public static void CreateOre(int tileID, int veinSize, int chanceDenominator)
        {
            // 根据机会分母循环尝试生成矿脉
            for (int i = 0; i < Main.maxTilesX * Main.maxTilesY / chanceDenominator; i++)
            {
                // 随机选择一个位置
                int x = Main.rand.Next(1, Main.maxTilesX - 1);
                int y = Main.rand.Next((int)GenVars.rockLayerLow, Main.maxTilesY - 1);

                // 检查位置的瓦块类型是否是可以生成矿脉的类型
                if (Main.tile[x, y].TileType == TileID.Stone ||
                    Main.tile[x, y].TileType == TileID.Dirt ||
                    Main.tile[x, y].TileType == TileID.Ebonstone ||
                    Main.tile[x, y].TileType == TileID.Crimstone ||
                    Main.tile[x, y].TileType == TileID.Pearlstone ||
                    Main.tile[x, y].TileType == TileID.Sand ||
                    Main.tile[x, y].TileType == TileID.Mud ||
                    Main.tile[x, y].TileType == TileID.SnowBlock ||
                    Main.tile[x, y].TileType == TileID.IceBlock)
                {
                    // 在符合条件的位置生成矿脉
                    WorldGen.TileRunner(x, y, veinSize, 15, tileID);
                }
            }
        }
    }
}
