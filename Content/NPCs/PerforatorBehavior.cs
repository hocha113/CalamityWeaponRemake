using CalamityMod.Events;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.NPCs
{
    /// <summary>
    /// 关于血肉宿主的行为定义
    /// </summary>
    internal class PerforatorBehavior
    {
        public static PerforatorBehavior Instance;

        public void Load() => Instance = this;

        /// <summary>
        /// 处理瞬移的视觉效果
        /// </summary>
        /// <param name="npc"></param>
        public void TeleportationEffect(NPC npc) {
            Lighting.AddLight(npc.Center, Color.Red.ToVector3() * 14);
            for (int i = 0; i < 16; i++) {
                int ichorDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Ichor, 0f, 0f, 100, default, 1f);
                Main.dust[ichorDust].velocity *= 2f;
                if (Main.rand.NextBool()) {
                    Main.dust[ichorDust].scale = 0.25f;
                    Main.dust[ichorDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
        }
        /// <summary>
        /// 执行强化行为附加
        /// </summary>
        /// <param name="npc"></param>
        public void Intensive(NPC npc) {
            if (npc.type == CWRIDs.PerforatorHive) {//改动血肉宿主的行为，这会让它在血月更加的暴躁和危险
                PerforatorHive perforatorHive = (PerforatorHive)npc.ModNPC;
                float lifeRatio = npc.life / (float)npc.lifeMax;
                Player player = Main.player[npc.target];
                bool bossRush = BossRushEvent.BossRushActive;
                bool expertMode = Main.expertMode || bossRush;
                bool revenge = CalamityWorld.revenge || bossRush;
                bool death = CalamityWorld.death || bossRush;
                bool phase1 = lifeRatio >= 0.7f;

                if (phase1) {//一阶段
                    if (npc.localAI[0] != 0) {
                        if (npc.localAI[0] % 180 == 0) {//在一阶段，我们需要周期性的发射跨整个屏幕的水平弹幕来提高难度
                            const int maxspanWidth = 9000;
                            const int maxspanProjNum = 60;
                            int step = maxspanWidth / maxspanProjNum;
                            float spanPointXProjs = -(maxspanWidth / 2) + npc.Center.X;
                            float spanPointYProjs = npc.Center.Y - 800;
                            int type = Main.rand.NextBool() ? ModContent.ProjectileType<IchorShot>() : ModContent.ProjectileType<BloodGeyser>();
                            int damage = 56;
                            for (int i = 0; i < 60; i++) {
                                Vector2 spanPos = new Vector2(spanPointXProjs, spanPointYProjs);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), spanPos, new Vector2(0, 6), type, damage, 0f, Main.myPlayer, 0f, player.Center.Y);
                                spanPointXProjs += step;
                            }
                        }
                        if (npc.localAI[0] % 60 == 0) {//并且，我们需要让它频繁的向玩家发射危险性很大的针对性血弹
                            SoundEngine.PlaySound(SoundID.ForceRoar, npc.position);
                            Vector2 toTarget = npc.Center.To(player.Center);
                            Vector2 toTargetUnit = toTarget.UnitVector();
                            Vector2 norlVr = toTargetUnit.GetNormalVector();
                            int damage = 78;
                            float speed = 17;
                            for (int i = 0; i < 13; i++) {
                                int setInYnum = (i < 6 ? i : 12 - i) * 33;
                                Vector2 setInXVr = norlVr * (65 - 10 * i);
                                Vector2 spanPos = npc.Center + setInXVr + toTargetUnit * (setInYnum - 130);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), spanPos, toTargetUnit * speed, ModContent.ProjectileType<BloodGeyser>(), damage, 0f, Main.myPlayer, 0f, player.Center.Y);
                            }
                            for (int i = 0; i < 13; i++) {
                                Vector2 spanPos = npc.Center + toTargetUnit * -i * 33;
                                Projectile.NewProjectile(npc.GetSource_FromAI(), spanPos, toTargetUnit * speed, ModContent.ProjectileType<IchorShot>(), damage, 0f, Main.myPlayer, 0f, player.Center.Y);
                            }
                        }
                    }
                    npc.position.X += npc.velocity.X * 0.5f;
                    npc.position.Y += npc.velocity.Y * 0.2f;
                }
                else {//在二阶段
                    if (npc.localAI[0] % 90 == 0 && npc.localAI[0] != 0) {//周期性发射圆周性的灵液球弹幕
                        for (int i = 0; i < 16; i++) {
                            Vector2 vr = (MathHelper.TwoPi / 16f * i).ToRotationVector2() * 13;
                            int damage = 53;
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + Vector2.UnitY * 50f, vr, ModContent.ProjectileType<IchorBlob>(), damage, 0f, Main.myPlayer, 0f, player.Center.Y);
                        }
                    }
                    if (!NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadMedium>())) {
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PerforatorHeadMedium>(), 1);
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PerforatorHeadMedium>(), 1);
                    }
                    if (npc.localAI[0] % 150 == 0 && npc.localAI[0] != 0) {//我们让它每隔一小段时间就瞬移一次
                        SoundEngine.PlaySound(SoundID.NPCDeath23, npc.position);
                        TeleportationEffect(npc);
                        for (int i = 0; i < 16; i++) {
                            Vector2 pos = npc.Center;
                            Vector2 particleSpeed = Main.rand.NextVector2Unit() * Main.rand.NextFloat(5.5f, 7.7f);
                            CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                                , Main.rand.NextFloat(0.3f, 0.9f), Color.Red, 30, 1, 1.5f, hueShift: 0.0f);
                            CWRParticleHandler.SpawnParticle(energyLeak);
                        }
                        npc.position = player.Center + player.velocity.UnitVector().RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.Next(420, 780);
                        TeleportationEffect(npc);
                        for (int i = 0; i < 16; i++) {
                            int ichorDust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Ichor, 0f, 0f, 100, default, 1f);
                            Main.dust[ichorDust].velocity *= 2f;
                            if (Main.rand.NextBool()) {
                                Main.dust[ichorDust].scale = 0.25f;
                                Main.dust[ichorDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                            }
                        }
                    }
                    if (npc.localAI[0] % 75 == 0 && npc.localAI[0] != 0) {
                        for (int i = 0; i < 4; i++) {
                            float rot = MathHelper.TwoPi / 4f * i;
                            Vector2 spanPos = rot.ToRotationVector2() * 1200 + player.Center;
                            Vector2 vr = (rot + MathHelper.Pi).ToRotationVector2();
                            for (int j = 0; j < 16; j++) {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), spanPos + vr * i * 33, vr * 17, ProjectileID.CursedFlameHostile, 88, 0f, Main.myPlayer);
                            }
                        }
                    }
                    npc.position.X += npc.velocity.X * 0.25f;
                    npc.position.Y += npc.velocity.Y * 0.75f;
                }
            }
        }

        public void BloodMoonDorp(NPC npc) {
            if (npc.type == CWRIDs.PerforatorHive && Main.bloodMoon) {

            }
        }
    }
}
