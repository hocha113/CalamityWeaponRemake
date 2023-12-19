using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs.Ravager;
using CalamityMod.UI.VanillaBossBars;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.NPCs.RavagerAs
{
    internal class RavagerCover : GlobalNPC
    {
        public override void SetDefaults(NPC npc) {
            base.SetDefaults(npc);
            if (npc.type == ModContent.NPCType<RavagerBody>()) {
                npc.Calamity().canBreakPlayerDefense = true;
                npc.lavaImmune = true;
                npc.noGravity = true;
                npc.npcSlots = 20f;
                npc.aiStyle = -1;
                npc.GetNPCDamage();
                npc.width = 332;
                npc.height = 214;
                npc.defense = 55;
                npc.value = Item.buyPrice(0, 75, 0, 0);
                npc.DR_NERD(0.35f);
                npc.LifeMaxNERB(750000, 854000, 1460000);
                if (DownedBossSystem.downedProvidence && !BossRushEvent.BossRushActive)//如果普罗米修斯已经被击败过一次，同时不处于BossRush挑战期间，进行属性增强
                {
                    npc.damage = (int)(npc.damage * 1.5);
                    npc.defense += 15;
                    npc.lifeMax *= (int)(npc.lifeMax * 1.25);
                    npc.value *= 1.5f;
                }
                double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
                npc.lifeMax += (int)(npc.lifeMax * HPBoost);//应用血量增幅设置
                npc.knockBackResist = 0f;
                npc.boss = true;
                npc.BossBar = ModContent.GetInstance<RavagerBossBar>();
                npc.netAlways = true;
                npc.alpha = 255;
                npc.HitSound = RavagerBody.HitSound;
                npc.DeathSound = RavagerBody.DeathSound;
                npc.Calamity().VulnerableToSickness = false;
                npc.Calamity().VulnerableToWater = true;
            }
        }

        public override void OnKill(NPC npc) {
            if (npc.type == ModContent.NPCType<RavagerBody>()) {
                int types = ModContent.NPCType<EvilSoul>();
                int types2 = ModContent.NPCType<RavagerABody>();

                if (DownedBossSystem.downedProvidence && !NPC.AnyNPCs(types) && !NPC.AnyNPCs(types2)) {
                    SoundEngine.PlaySound(SoundID.ScaryScream, npc.Center);

                    if (Main.netMode != NetmodeID.MultiplayerClient) {
                        int soud = NPC.NewNPC(new EntitySource_BossSpawn(npc), (int)npc.position.X, (int)npc.position.Y, types, 1);
                        Main.npc[soud].timeLeft *= 20;
                        CalamityUtils.BossAwakenMessage(soud);
                    }
                    else
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, npc.whoAmI, types);
                }
            }
        }
    }
}
