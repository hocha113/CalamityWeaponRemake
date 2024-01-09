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
        public Vector2 Center => Position.ToWorldCoordinates(8 * BloodAltar.Width, 8 * BloodAltar.Height);
        public long Time = 0;
        public int frameIndex = 1;
        public float rot;
        public float drawGstPos;

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<BloodAltar>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
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
                    if (orbToPos.LengthSquared() > 62 * 62) {
                        Vector2 orbToPosUnit = orbToPos.UnitVector();
                        float leng = orbToPos.Length() / 62f;
                        for (int j = 0; j < 62; j++) {
                            Vector2 spanPos = orb.Center + orbToPosUnit * leng * j;
                            LightParticle particle = new LightParticle(spanPos, Vector2.Zero, 0.3f, Color.DarkRed, 15);
                            CWRParticleHandler.SpawnParticle(particle);
                        }
                        orb.position = Center;
                    }
                    else {
                        orb.position = Center;
                        Chest chest = CWRUtils.FindNearestChest(Position.X, Position.Y);
                        if (chest != null) {
                            Vector2 chestPos = new Vector2(chest.x, chest.y) * 16;
                            Vector2 PosToChest = Center.To(chestPos);
                            Vector2 PosToChestUnit = PosToChest.UnitVector();
                            float leng = PosToChest.Length() / 32f;
                            for (int j = 0; j < 32; j++) {
                                Vector2 spanPos = Center + PosToChestUnit * leng * j;
                                LightParticle particle = new LightParticle(spanPos, Vector2.Zero, 0.3f, Color.DarkGreen, 15);
                                CWRParticleHandler.SpawnParticle(particle);
                            }
                            chest.AddItem(orb);
                            orb.TurnToAir();
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
                NetMessage.SendTileSquare(Main.myPlayer, i, j, BloodAltar.Width, BloodAltar.Height);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }

            int id = Place(i, j);
            return id;
        }

        public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
    }
}
