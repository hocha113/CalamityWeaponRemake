using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityWeaponRemake.Content.Tiles
{
    internal class FoodStallChair : ModTile
    {
        public override string Texture => CWRConstant.Asset + "Tiles/" + "FoodStallChair";

        private int MaxRigDims = 280;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileID.Sets.CanBeSatOnForNPCs[Type] = true; // 方便为NPC调用ModifySittingTargetInfo
            TileID.Sets.CanBeSatOnForPlayers[Type] = true; // 方便为玩家调用ModifySittingTargetInfo

            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            AdjTiles = new int[] { TileID.Chairs };

            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Chair"));

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

            // 如果决定添加更多样式的垂直堆叠，则需要设置这三行代码以下3行代码
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1); // 面向右将使用第二个纹理样式
            TileObjectData.addTile(Type);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {//如果返回了true，那么这个物块项目就能够被玩家交互，这里判定距离来防止发生无限距离交互的情况
            return settings.player.IsWithinSnappngRangeToTile(i, j, 180);                                                                                                        // 避免能够从远处触发它
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            info.AnchorTilePosition.X = i;
            info.AnchorTilePosition.Y = j;
        }

        public override void RandomUpdate(int i, int j)
        {
            base.RandomUpdate(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = CWRUtils.GetTile(i, j);
            short changeFrames = 60;

            if (player.IsWithinSnappngRangeToTile(i, j, 180))
            { // 避免能够从远处触发它
                player.GamepadEnableGrappleCooldown();
                player.sitting.SitDown(player, i, j);

                if (tile != null)
                {
                    int mouse2TopLeftX = tile.TileFrameX / 36 * -1;
                    int mouse2TopLeftY = tile.TileFrameY / 36 * -1;

                    if (mouse2TopLeftX < -1)
                    {
                        mouse2TopLeftX += 2;
                        changeFrames = -36;
                    }
                }
            }
            
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, 180))
            { // 匹配RightClick中条件。仅当单击时执行某些操作时才应显示交互
                return;
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.FoodStallChair>();//当玩家鼠标悬停在物块之上时，显示该物品的材质

            if (Main.tile[i, j].TileFrameX / 18 < 1)
            {
                player.cursorItemIconReversed = true;
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return base.PreDraw(i, j, spriteBatch);
        }
    }
}
