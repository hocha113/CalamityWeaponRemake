using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityWeaponRemake.Content
{
    public class CWRNpc : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public byte TerratomereBoltOnHitNum = 0;

        public override bool CanBeHitByNPC(NPC npc, NPC attacker)
        {
            return base.CanBeHitByNPC(npc, attacker);
        }
    }
}
