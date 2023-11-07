﻿using CalamityMod.Events;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs;
using CalamityMod;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.NPCs.RavagerAs
{
    internal class RavagerALegRight : ModNPC
    {
        public override string Texture => CWRConstant.RavagerA + "RavagerALegRight";

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 55;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 40;
            NPC.DR_NERD(0.15f);
            NPC.lifeMax = 62500;
            NPC.knockBackResist = 0f;
            AIType = -1;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.noTileCollide = true;
            NPC.alpha = 255;
            NPC.HitSound = RavagerBody.HitSound;
            NPC.DeathSound = RavagerBody.LimbLossSound;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 140000;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
        }

        public override void AI()
        {
            NPC.damage = 0;

            if (NPC.alpha > 0)
            {
                NPC.alpha -= 10;
                if (NPC.alpha < 0)
                    NPC.alpha = 0;

                NPC.ai[1] = 0f;
            }

            NPC body = CWRUtils.GetNPCInstance((int)NPC.ai[2]);
            if (body == null)
            {
                NPC.active = false;
                NPC.life = 0;
                NPC.checkDead();
                return;
            }

            NPC.Center = body.Center + new Vector2(70f, 88f);
        }

        public override bool CheckActive() => false;

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerLegRight").Type, 1f);
                    //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerLegRight2").Type, 1f);
                }
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
