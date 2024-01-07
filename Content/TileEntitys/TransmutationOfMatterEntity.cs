using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Tiles;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;

namespace CalamityWeaponRemake.Content.TileEntitys
{
    internal class TransmutationOfMatterEntity : ModTileEntity
    {
        public int frameIndex = 1;
        public Vector2 Center => Position.ToWorldCoordinates(8 * TransmutationOfMatter.Width, 8 * TransmutationOfMatter.Height);
        public long Time = 0;
        public float rot;
        public float drawGstPos;

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<TransmutationOfMatter>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }

        public override void Update() {
            if (SupertableUI.instance.Active) {
                float leng = Main.LocalPlayer.Center.To(Center).LengthSquared();
                if (leng >= 100 * 100 && leng < 1000 * 1000) {
                    SupertableUI.instance.Active = false;
                }
            }
            CWRUtils.ClockFrame(ref frameIndex, 6, 3);
            Time++;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, TransmutationOfMatter.Width, TransmutationOfMatter.Height);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }

            int id = Place(i, j);
            return id;
        }

        public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
    }
}
