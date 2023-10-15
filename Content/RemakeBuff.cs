﻿using CalamityWeaponRemake.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class RemakeBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {
                case BuffID.BeetleMight1:
                    
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.05f;
                    break;
                case BuffID.BeetleMight2:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f;
                    break;
                case BuffID.BeetleMight3:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
                    break;
            }
        }
    }
}
