using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Events;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs;
using CalamityMod.NPCs.Ravager;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Enemy;
using CalamityMod.UI.VanillaBossBars;
using CalamityMod.World;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.CWRUtils;

namespace CalamityWeaponRemake.Content.NPCs.RavagerAs
{
    [AutoloadBossHead]
    internal class RavagerABody : ModNPC
    {
        public override string Texture => CWRConstant.RavagerA + "RavagerABody";
        public override string HeadTexture => CWRConstant.RavagerA + "RavagerABody_Head_Boss";

        private float velocityY = -16f;
        public static readonly SoundStyle JumpSound = new("CalamityMod/Sounds/Custom/Ravager/RavagerJump", 2);
        public static readonly SoundStyle StompSound = new("CalamityMod/Sounds/Custom/Ravager/RavagerStomp", 2);
        public static readonly SoundStyle FistSound = new("CalamityMod/Sounds/Custom/Ravager/RavagerPunch", 2);
        public static readonly SoundStyle LimbLossSound = new("CalamityMod/Sounds/NPCKilled/RavagerLimbLoss", 4);
        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/RavagerHurt", 4);
        public static readonly SoundStyle DeathSound = new("CalamityMod/Sounds/NPCKilled/RavagerDeath", 2);
        public static readonly SoundStyle PillarSound = new("CalamityMod/Sounds/Custom/Ravager/RavagerPillarSummon");
        public static readonly SoundStyle MissileSound = new("CalamityMod/Sounds/Custom/Ravager/RavagerMissileLaunch");

        public int aClawLeft = -1;
        public int aClawRight = -1;
        public int aHead = -1;
        public int aLegLeft = -1;
        public int aLegRight = -1;

        public NPC newAClawLeft => GetNPCInstance(aClawLeft);
        public NPC newAClawRight => GetNPCInstance(aClawRight);
        public NPC newAHead => GetNPCInstance(aHead);
        public NPC newALegLeft => GetNPCInstance(aLegLeft);
        public NPC newALegRight => GetNPCInstance(aLegRight);

        public bool aClawLeftActive => newAClawLeft == null ? false : newAClawLeft.ModNPC.AlivesByNPC<RavagerAClawLeft>();
        public bool aClawRightActive => newAClawRight == null ? false : newAClawRight.ModNPC.AlivesByNPC<RavagerAClawRight>();
        public bool aHeadActive => newAHead == null ? false : newAHead.ModNPC.AlivesByNPC<RavagerAHead>();
        public bool aLegLeftActive => newALegLeft == null ? false : newALegLeft.ModNPC.AlivesByNPC<RavagerALegLeft>();
        public bool aLegRightActive => newALegRight == null ? false : newALegRight.ModNPC.AlivesByNPC<RavagerALegRight>();

        public int Status { get => (int)NPC.ai[0]; set => NPC.ai[0] = value; }
        public int Behavior { get => (int)NPC.ai[1]; set => NPC.ai[1] = value; }
        public int Time { get => (int)NPC.ai[2]; set => NPC.ai[2] = value; }

        public Player target = null;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.TrailCacheLength[NPC.type] = 13;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.5f,
                PortraitPositionYOverride = -40f,
                PortraitScale = 0.6f,
            };
            value.Position.Y -= 50f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.npcSlots = 20f;
            NPC.aiStyle = -1;
            NPC.damage = 225;
            NPC.width = 332;
            NPC.height = 214;
            NPC.scale = 2;
            NPC.defense = 55;
            NPC.value = Item.buyPrice(0, 75, 0, 0);
            NPC.DR_NERD(0.35f);
            NPC.LifeMaxNERB(1250000, 1854000, 5460000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);//应用血量增幅设置
            NPC.knockBackResist = 0f;
            AIType = -1;//设置为-1，即启用完全的自定义行为
            NPC.boss = true;
            NPC.BossBar = ModContent.GetInstance<NullBossBar>();
            NPC.netAlways = true;
            NPC.alpha = 255;
            NPC.HitSound = HitSound;
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
            Music = MusicLoader.GetMusicSlot(CalamityWeaponRemake.Instance, "Assets/Sounds/Music/ShenA");//这个音乐是占位使用——谁让它这么酷呢？
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.LifeMaxNERB(1250000, 1854000, 5460000);
            NPC.lifeMax += numPlayers * 500000;
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.Ravager")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            writer.Write(velocityY);
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            for (int i = 0; i < 4; i++)
                writer.Write(NPC.Calamity().newAI[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            velocityY = reader.ReadSingle();
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            for (int i = 0; i < 4; i++)
                NPC.Calamity().newAI[i] = reader.ReadSingle();
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
                NPC.Opacity = 1f;

            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        /// <summary>
        /// 添加光量效果
        /// </summary>
        public void BossLight()
        {
            // 大团灯火
            Lighting.AddLight((int)(NPC.Center.X - 110f) / 16, (int)(NPC.Center.Y - 30f) / 16, 0f, 0.5f, 2f);
            Lighting.AddLight((int)(NPC.Center.X + 110f) / 16, (int)(NPC.Center.Y - 30f) / 16, 0f, 0.5f, 2f);

            // 小团灯火
            Lighting.AddLight((int)(NPC.Center.X - 40f) / 16, (int)(NPC.Center.Y - 60f) / 16, 0f, 0.25f, 1f);
            Lighting.AddLight((int)(NPC.Center.X + 40f) / 16, (int)(NPC.Center.Y - 60f) / 16, 0f, 0.25f, 1f);
        }

        public void SpanLightDust()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 10; i++)//生成这种粒子不是好主意
                {
                    Vector2 particleSpeed = GetRandomVevtor(60, 120, -8 * (i / 20f));
                    Vector2 pos = NPC.Center + new Vector2(Main.rand.Next(-16, 6), -66) + new Vector2(Main.rand.Next(-266, 266), 0);
                    Particle energyLeak = new SquishyLightParticle(pos, particleSpeed
                        , Main.rand.NextFloat(0.6f, 1.1f), Color.Purple, 60, 1, 1.5f, hueShift: 0.0f);
                    GeneralParticleHandler.SpawnParticle(energyLeak);
                }
            }
        }

        public override void AI()
        {
            CalamityGlobalNPC modNPC = NPC.Calamity();

            bool bossRush = BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || bossRush;
            bool revenge = CalamityWorld.revenge || bossRush;
            bool death = CalamityWorld.death || bossRush;

            // 获取剩余的生命百分比
            float lifeRatio = NPC.GetLifePercent();

            BossLight();
            SpanLightDust();
            SpanBodyOvers();

            if (!target.Alives())
            {
                
            }
        }

        public void SpanBodyOvers()
        {
            if (NPC.localAI[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient)//生成子实体的行为不能在客户端上运行
            {
                NPC.localAI[0] = 1f;
                aLegLeft = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - 70, (int)NPC.Center.Y + 88, ModContent.NPCType<RavagerALegLeft>(), ai2: NPC.whoAmI);
                aLegRight = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 70, (int)NPC.Center.Y + 88, ModContent.NPCType<RavagerALegRight>(), ai2: NPC.whoAmI);
                aClawLeft = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - 120, (int)NPC.Center.Y + 50, ModContent.NPCType<RavagerAClawLeft>(), ai2: NPC.whoAmI);
                aClawRight = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 120, (int)NPC.Center.Y + 50, ModContent.NPCType<RavagerAClawRight>(), ai2: NPC.whoAmI);
                aHead = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 1, (int)NPC.Center.Y - 20, ModContent.NPCType<RavagerAHead>(), ai2: NPC.whoAmI);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {

        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            Vector2 npcCenter = NPC.Center;

            // NOTE: Right and left hitboxes are interchangeable, each hitbox is the same size and is located to the right or left of the center hitbox.
            // Width = 83, Height = 107
            Rectangle leftHitbox = new Rectangle((int)(npcCenter.X - (NPC.width / 2f) + 8f), (int)(npcCenter.Y - (NPC.height / 4f)), NPC.width / 4, NPC.height / 2);
            // Width = 166, Height = 214
            Rectangle bodyHitbox = new Rectangle((int)(npcCenter.X - (NPC.width / 4f)), (int)(npcCenter.Y - (NPC.height / 2f) + 8f), NPC.width / 2, NPC.height);
            // Width = 83, Height = 107
            Rectangle rightHitbox = new Rectangle((int)(npcCenter.X + (NPC.width / 4f) - 8f), (int)(npcCenter.Y - (NPC.height / 4f)), NPC.width / 4, NPC.height / 2);

            Vector2 leftHitboxCenter = new Vector2(leftHitbox.X + (leftHitbox.Width / 2), leftHitbox.Y + (leftHitbox.Height / 2));
            Vector2 bodyHitboxCenter = new Vector2(bodyHitbox.X + (bodyHitbox.Width / 2), bodyHitbox.Y + (bodyHitbox.Height / 2));
            Vector2 rightHitboxCenter = new Vector2(rightHitbox.X + (rightHitbox.Width / 2), rightHitbox.Y + (rightHitbox.Height / 2));

            Rectangle targetHitbox = target.Hitbox;

            float leftDist1 = Vector2.Distance(leftHitboxCenter, targetHitbox.TopLeft());
            float leftDist2 = Vector2.Distance(leftHitboxCenter, targetHitbox.TopRight());
            float leftDist3 = Vector2.Distance(leftHitboxCenter, targetHitbox.BottomLeft());
            float leftDist4 = Vector2.Distance(leftHitboxCenter, targetHitbox.BottomRight());

            float minLeftDist = leftDist1;
            if (leftDist2 < minLeftDist)
                minLeftDist = leftDist2;
            if (leftDist3 < minLeftDist)
                minLeftDist = leftDist3;
            if (leftDist4 < minLeftDist)
                minLeftDist = leftDist4;

            bool insideLeftHitbox = minLeftDist <= 55f;

            float bodyDist1 = Vector2.Distance(bodyHitboxCenter, targetHitbox.TopLeft());
            float bodyDist2 = Vector2.Distance(bodyHitboxCenter, targetHitbox.TopRight());
            float bodyDist3 = Vector2.Distance(bodyHitboxCenter, targetHitbox.BottomLeft());
            float bodyDist4 = Vector2.Distance(bodyHitboxCenter, targetHitbox.BottomRight());

            float minBodyDist = bodyDist1;
            if (bodyDist2 < minBodyDist)
                minBodyDist = bodyDist2;
            if (bodyDist3 < minBodyDist)
                minBodyDist = bodyDist3;
            if (bodyDist4 < minBodyDist)
                minBodyDist = bodyDist4;

            bool insideBodyHitbox = minBodyDist <= 110f;

            float rightDist1 = Vector2.Distance(rightHitboxCenter, targetHitbox.TopLeft());
            float rightDist2 = Vector2.Distance(rightHitboxCenter, targetHitbox.TopRight());
            float rightDist3 = Vector2.Distance(rightHitboxCenter, targetHitbox.BottomLeft());
            float rightDist4 = Vector2.Distance(rightHitboxCenter, targetHitbox.BottomRight());

            float minRightDist = rightDist1;
            if (rightDist2 < minRightDist)
                minRightDist = rightDist2;
            if (rightDist3 < minRightDist)
                minRightDist = rightDist3;
            if (rightDist4 < minRightDist)
                minRightDist = rightDist4;

            bool insideRightHitbox = minRightDist <= 55f;

            return insideLeftHitbox || insideBodyHitbox || insideRightHitbox;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 480, true);
        }

        public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;

        public override void OnKill()
        {
            CalamityGlobalNPC.SetNewBossJustDowned(NPC);
            DownedBossSystem.downedRavager = true;
            CalamityNetcode.SyncWorld();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawClaw(spriteBatch, drawColor);
            DrawBody(spriteBatch, drawColor);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawLeg(spriteBatch, drawColor);
            DrawHead(spriteBatch, drawColor);
        }

        private void DrawHead(SpriteBatch spriteBatch, Color drawColor)
        {
            if (aHeadActive)
            {
                NPC heads = Main.npc[aHead];
                Texture2D headValue = GetT2DValue(heads.ModNPC.Texture);
                Texture2D headGlow = GetT2DValue(heads.ModNPC.Texture + "Glow");
                
                spriteBatch.Draw(
                    headValue,
                    heads.Center - Main.screenPosition,
                    GetRec(headValue),
                    drawColor,
                    heads.rotation,
                    GetOrig(headValue),
                    heads.scale,
                    SpriteEffects.None,
                    0
                    );

                spriteBatch.Draw(
                    headGlow,
                    heads.Center - Main.screenPosition,
                    GetRec(headGlow, (int)heads.frameCounter, 4),
                    Color.White,
                    heads.rotation,
                    GetOrig(headGlow, 4),
                    heads.scale,
                    SpriteEffects.None,
                    0
                    );

                for (int i = 0; i < heads.oldPos.Length; i++)
                {
                    spriteBatch.Draw(
                        headGlow,
                        heads.oldPos[i] + heads.netOffset - Main.screenPosition,
                        GetRec(headGlow, (int)heads.frameCounter, 4),
                        Color.White,
                        heads.rotation,
                        GetOrig(headGlow, 7),
                        heads.scale,
                        SpriteEffects.None,
                        0
                    );
                }
            }
        }

        private void DrawBody(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D mainValue = GetT2DValue(Texture);
            //Texture2D glow = GetT2DValue(CWRConstant.RavagerA + "");

            spriteBatch.Draw(
                    mainValue,
                    NPC.Center - Main.screenPosition,
                    GetRec(mainValue, (int)NPC.frameCounter, 7),
                    drawColor,
                    NPC.rotation,
                    GetOrig(mainValue, 7),
                    NPC.scale,
                    SpriteEffects.None,
                    0
                    );

            //spriteBatch.Draw(
            //        glow,
            //        NPC.Center - Main.screenPosition,
            //        GetRec(glow, (int)NPC.frameCounter, 7),
            //        Color.White,
            //        NPC.rotation,
            //        GetOrig(glow, 7),
            //        NPC.scale,
            //        SpriteEffects.None,
            //        0
            //        );
        }

        private void DrawLeg(SpriteBatch spriteBatch, Color drawColor)
        {
            if (aLegLeftActive)
            {
                NPC lg = Main.npc[aLegLeft];
                Texture2D value = GetT2DValue(lg.ModNPC.Texture);
                spriteBatch.Draw(
                    value,
                    lg.Center - Main.screenPosition,
                    GetRec(value),
                    drawColor,
                    lg.rotation,
                    GetOrig(value),
                    lg.scale,
                    SpriteEffects.None,
                    0
                    );
            }
            if (aLegRightActive)
            {
                NPC rg = Main.npc[aLegRight];
                Texture2D value = GetT2DValue(rg.ModNPC.Texture);
                spriteBatch.Draw(
                    value,
                    rg.Center - Main.screenPosition,
                    GetRec(value),
                    drawColor,
                    rg.rotation,
                    GetOrig(value),
                    rg.scale,
                    SpriteEffects.None,
                    0
                    );
            }
        }

        private void DrawClaw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (aClawLeftActive)
            {
                NPC lc = Main.npc[aClawLeft];
                RavagerAClawLeft ravagerAClawLeft = lc.ModNPC as RavagerAClawLeft;
                Texture2D value = GetT2DValue(ravagerAClawLeft.Texture);
                DrawChain(spriteBatch, NPC.Center + new Vector2(-120f, 50f), lc.Center);
                spriteBatch.Draw(
                    value,
                    lc.Center - Main.screenPosition,
                    GetRec(value),
                    drawColor,
                    lc.rotation,
                    GetOrig(value),
                    lc.scale,
                    SpriteEffects.None,
                    0
                    );
            }
            if (aClawRightActive)
            {
                NPC rc = Main.npc[aClawRight];
                Texture2D value = GetT2DValue(rc.ModNPC.Texture);
                DrawChain(spriteBatch, NPC.Center + new Vector2(120f, 50f), rc.Center);
                spriteBatch.Draw(
                    value,
                    rc.Center - Main.screenPosition,
                    GetRec(value),
                    drawColor,
                    rc.rotation,
                    GetOrig(value),
                    rc.scale,
                    SpriteEffects.None,
                    0
                    );
            }
        }

        private void DrawChain(SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint)
        {
            Texture2D value = GetT2DValue(CWRConstant.RavagerA + "RavagerAChain");
            Vector2 toEndVr = startPoint.To(endPoint);
            float lengs = toEndVr.Length();
            float rots = toEndVr.ToRotation();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap
                , DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(
                value,
                startPoint - Main.screenPosition,
                new Rectangle(0, 0, value.Width, (int)lengs + 1),
                Color.Blue,
                rots - MathHelper.PiOver2,
                value.Size() / 2,
                1,
                SpriteEffects.None,
                0
                );

            Main.spriteBatch.ResetBlendState();
        }
    }
}
