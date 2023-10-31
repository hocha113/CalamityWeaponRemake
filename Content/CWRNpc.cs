using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityWeaponRemake.Content
{
    public enum WhipHitType : byte
    {
        ElementWhip =1,
        BleedingScourge,
        AzureDragonRage,
        GhostFireWhip,
        WhiplashGalactica,
        AllhallowsGoldWhip
    }

    public class CWRNpc : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public byte TerratomereBoltOnHitNum = 0;
        public ushort colldHitTime = 0;
        public byte WhipHitNum = 0;
        public byte WhipHitType = 0;

        public override bool CanBeHitByNPC(NPC npc, NPC attacker)
        {
            return base.CanBeHitByNPC(npc, attacker);
        }

        public override void PostAI(NPC npc)
        {
            if (!GameUtils.isClient)
            {
                if (WhipHitNum > 10)
                {
                    WhipHitNum = 10;
                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}
