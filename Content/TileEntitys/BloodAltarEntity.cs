using CalamityWeaponRemake.Content.Tiles;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using System;
using CalamityMod.Items.Materials;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Particles.Core;

namespace CalamityWeaponRemake.Content.TileEntitys
{
    internal class BloodAltarEntity : ModTileEntity
    {
        public Vector2 Center => Position.ToWorldCoordinates(8 * TransmutationOfMatter.Width, 8 * TransmutationOfMatter.Height);
        public long Time = 0;
        public int frameIndex = 1;
        public float rot;
        public float drawGstPos;

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<TransmutationOfMatter>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }

        public override void Update() {
            Main.dayTime = false;
            if (!Main.bloodMoon) {
                CWRUtils.Text("深红的注视正在降临...", Color.DarkRed);
            }
            Main.bloodMoon = true;
            for (int i = 0; i < Main.item.Length; i++) {
                Item orb = Main.item[i];
                if (orb.type == ModContent.ItemType<BloodOrb>()) {
                    Vector2 orbToPos = orb.position.To(Center);
                    if (orbToPos.LengthSquared() > 32 * 32 && Main.rand.NextBool(3)) {
                        Vector2 orbToPosUnit = orbToPos.UnitVector();
                        orb.position += orbToPosUnit * 6;
                        for (int j = 0; j < orbToPos.Length() / 32; j++) {
                            Vector2 spanPos = orb.Center + orbToPosUnit * 32 * j;
                            LightParticle particle = new LightParticle(spanPos, Vector2.Zero, 0.3f, Color.DarkRed, 15);
                            CWRParticleHandler.SpawnParticle(particle);
                        }
                    }
                }
            }
            CWRUtils.ClockFrame(ref frameIndex, 6, 3);
            Lighting.AddLight(Center, Color.DarkRed.ToVector3() * (Math.Abs(MathF.Sin(Time * 0.005f)) * 23 + 2));
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
