﻿using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityWeaponRemake.Common
{
    public class CWRnpc : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public byte TerratomereBoltOnHitNum = 0;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(TerratomereBoltOnHitNum);
            base.SendExtraAI(npc, bitWriter, binaryWriter);
        }

        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            try
            {
                TerratomereBoltOnHitNum = binaryReader.ReadByte();
                base.ReceiveExtraAI(npc, bitReader, binaryReader);
            }
            catch (Exception e)
            {
                if (e is EndOfStreamException eose)
                {
                    CalamityWeaponRemake.Instance.Logger.Error((object)"Failed to parse CalamityWeaponRemake packet: Packet was too short, missing data, or otherwise corrupt.", (Exception)eose);
                    return;
                }
                if (e is ObjectDisposedException ode)
                {
                    CalamityWeaponRemake.Instance.Logger.Error((object)"Failed to parse CalamityWeaponRemake packet: Packet reader disposed or destroyed.", (Exception)ode);
                    return;
                }
                if (e is IOException ioe)
                {
                    CalamityWeaponRemake.Instance.Logger.Error((object)"Failed to parse CalamityWeaponRemake packet: An unknown I/O error occurred.", (Exception)ioe);
                    return;
                }
                throw;
            }
        }
    }
}
