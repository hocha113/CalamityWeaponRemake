using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Events;
using CalamityMod.NPCs.Ravager;
using CalamityMod.World;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.NPCs.RavagerAs
{
    internal class RavagerAClawRight : ModNPC
    {
        public override string Texture => CWRConstant.RavagerA + "RavagerAClawRight";

        NPC body => CWRUtils.GetNPCInstance((int)NPC.ai[2]);

        public override void SetStaticDefaults() {
            this.HideFromBestiary();
        }

        public override void SetDefaults() {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.damage = 125;
            NPC.width = 80;
            NPC.height = 40;
            NPC.scale = 2;
            NPC.defense = 40;
            NPC.DR_NERD(0.15f);
            NPC.lifeMax = 12500;
            NPC.knockBackResist = 0f;
            AIType = -1;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.alpha = 255;
            NPC.HitSound = RavagerBody.HitSound;
            NPC.DeathSound = RavagerBody.LimbLossSound;
            if (DownedBossSystem.downedProvidence && !BossRushEvent.BossRushActive) {
                NPC.damage = (int)(NPC.damage * 1.5);
                NPC.defense *= 2;
                NPC.lifeMax *= 4;
            }
            if (BossRushEvent.BossRushActive) {
                NPC.lifeMax = 26000;
            }
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
        }

        public override void AI() {
            if (body == null) {
                NPC.active = false;
                NPC.life = 0;
                NPC.checkDead();
                return;
            }

            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;


            NPC.velocity = NPC.Center.To(body.Center).UnitVector() * -3;
            NPC.Center = body.Center + new Vector2(220f, 80f);
            NPC.rotation = NPC.velocity.ToRotation();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            return false;
        }

        public override bool CheckActive() {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
            if (hurtInfo.Damage > 0)
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 240, true);
        }

        public override void HitEffect(NPC.HitInfo hit) {
            if (NPC.life > 0) {
                int num285 = 0;
                while (num285 < hit.Damage / NPC.lifeMax * 100.0) {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                    num285++;
                }
            }
            else {
                if (Main.netMode != NetmodeID.Server) {
                    //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerClawRight").Type, 1f);
                    //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ScavengerClawRight2").Type, 1f);
                }
            }
        }
    }
}
