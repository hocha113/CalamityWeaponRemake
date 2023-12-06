﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.TileEntities;
using CalamityMod.Tiles.DraedonStructures;
using CalamityMod;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Content.Tiles
{
    internal class TransmutationOfMatterEntity : ModTileEntity
    {
        public Vector2 Center => Position.ToWorldCoordinates(8f * TransmutationOfMatter.Width, 8f * TransmutationOfMatter.Height);
        public long Time = 0;
        private short _stack = 0;

        public short CellStack
        {
            get => _stack;
            set
            {
                _stack = value;
                SendSyncPacket();
            }
        }

        private long CycleFrameCounter
        {
            get
            {
                long totalCycleTime = TransmutationOfMatter.BetweenCellDowntime + TransmutationOfMatter.TotalFrames * TransmutationOfMatter.AnimationFramerate;
                return Time % totalCycleTime;
            }
        }

        private bool IsCellFrame
        {
            get
            {
                long magicFrame = TransmutationOfMatter.BetweenCellDowntime + TransmutationOfMatter.CellCreateFrame * TransmutationOfMatter.AnimationFramerate + TransmutationOfMatter.MagicFrameDelay;
                return CycleFrameCounter == magicFrame;
            }
        }

        // Property which allows anyone to get the current animation frame of this specific factory.
        public int AnimationFrame
        {
            get
            {
                int f = (int)CycleFrameCounter;

                // The animation sticks on the last frame throughout the entire downtime period.
                if (f < TransmutationOfMatter.BetweenCellDowntime)
                    return TransmutationOfMatter.TotalFrames - 1;

                // Remove the starting downtime period for the framerate divisor calculation.
                return (f - TransmutationOfMatter.BetweenCellDowntime) / TransmutationOfMatter.AnimationFramerate;
            }
        }

        // This guarantees that this tile entity will not persist if not placed directly on the top left corner of a Power Cell Factory tile.
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<TransmutationOfMatter>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }

        public override void Update()
        {
            ++Time;
            int maxCellStack = ModContent.GetModItem(ModContent.ItemType<DraedonPowerCell>()).Item.maxStack;
            if (IsCellFrame && CellStack < maxCellStack)
                // The property setter will automatically send the necessary packet.
                CellStack++;
        }

        // This code is called as a hook when the player places the Power Cell Factory tile so that the tile entity may be placed.
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            // If in multiplayer, tell the server to place the tile entity and DO NOT place it yourself. That would mismatch IDs.
            // Also tell the server that you placed the 4x4 tiles that make up the Power Cell Factory.
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, TransmutationOfMatter.Width, TransmutationOfMatter.Height);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }

            // If in single player, just place the tile entity, no problems.
            int id = Place(i, j);
            return id;
        }

        // This code is called on dedicated servers only. It is the server-side response to MessageID.TileEntityPlacement.
        // When the server receives such a message from a client, it sends a MessageID.TileEntitySharing to all clients.
        // This will cause them to Place the tile entity locally at that position, all with exactly the same ID.
        public override void OnNetPlace() => NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);

        // If this factory breaks, anyone who's viewing it is no longer viewing it.
        public override void OnKill()
        {
            for (int i = 0; i < Main.maxPlayers; ++i)
            {
                Player p = Main.player[i];
                if (!p.active)
                    continue;

                // Use reflection to stop TML from spitting an error here.
                // Try-catching will not stop this error, TML will print it to console anyway. The error is harmless.
                ModPlayer[] mpStorageArray = (ModPlayer[])typeof(Player).GetField("modPlayers", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(p);
                if (mpStorageArray.Length == 0)
                    continue;

                CalamityPlayer mp = p.Calamity();
                if (mp.CurrentlyViewedFactoryID == ID)
                    mp.CurrentlyViewedFactoryID = -1;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["time"] = Time;
            tag["cells"] = _stack;
        }

        public override void LoadData(TagCompound tag)
        {
            Time = tag.GetLong("time");
            _stack = tag.GetShort("cells");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Time);
            writer.Write(_stack);
        }

        public override void NetReceive(BinaryReader reader)
        {
            Time = reader.ReadInt64();
            _stack = reader.ReadInt16();
        }

        private void SendSyncPacket()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            ModPacket packet = Mod.GetPacket();
            //packet.Write((byte)CalamityModMessageType.TransmutationOfMatter);
            packet.Write(ID);
            packet.Write(Time);
            packet.Write(_stack);
            packet.Send(-1, -1);
        }

        internal static bool ReadSyncPacket(Mod mod, BinaryReader reader)
        {
            int teID = reader.ReadInt32();
            bool exists = ByID.TryGetValue(teID, out TileEntity te);

            // The rest of the packet must be read even if it turns out the factory doesn't exist for whatever reason.
            long time = reader.ReadInt64();
            short cellStack = reader.ReadInt16();

            // When a server gets this packet, it immediately sends an equivalent packet to all clients.
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = mod.GetPacket();
                //packet.Write((byte)CalamityModMessageType.TransmutationOfMatter);
                packet.Write(teID);
                packet.Write(time);
                packet.Write(cellStack);
                packet.Send(-1, -1);
            }

            //if (exists && te is TETransmutationOfMatter factory)
            //{
            //    // Only clients update their timer from this packet. When a server receives this packet it ignores the time variable.
            //    if (Main.netMode == NetmodeID.MultiplayerClient)
            //        factory.Time = time;
            //    //factory._stack = cellStack;
            //    return true;
            //}
            return false;
        }
    }
}
