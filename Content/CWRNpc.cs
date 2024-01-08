﻿using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class CWRNpc : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public byte TerratomereBoltOnHitNum = 0;
        public byte OrderbringerOnHitNum = 0;
        public bool TheEndSunOnHitNum;
        public ushort colldHitTime = 0;
        public byte WhipHitNum = 0;
        public byte WhipHitType = 0;
        public bool SprBoss;
        public bool ObliterateBool;
        public bool GangarusSign;

        public override bool CanBeHitByNPC(NPC npc, NPC attacker) {
            return base.CanBeHitByNPC(npc, attacker);
        }

        public override bool CheckDead(NPC npc) {
            if (ObliterateBool) {
                return true;
            }
            return base.CheckDead(npc);
        }

        public override void PostAI(NPC npc) {
            if (!CWRUtils.isClient) {
                if (WhipHitNum > 10) {
                    WhipHitNum = 10;
                }
            }
            if (Main.bloodMoon) {//在血月的情况下让一些生物执行特殊的行为，将这段代码写在PostAI中是防止被覆盖
                if (npc.type == CWRIDs.PerforatorHive)//改动血肉宿主的行为，这会让它在血月更加的暴躁和危险
                    PerforatorBehavior.Instance.Intensive(npc);
                if (npc.type == CWRIDs.HiveMind)//改动腐巢意志的行为，这会让它在血月更加的恐怖和强大
                    HiveMindBehavior.Instance.Intensive(npc);
            }
        }

        public override bool PreKill(NPC npc) {
            return base.PreKill(npc);
        }

        public override void OnKill(NPC npc) {
            if (npc.boss) {
                if (CWRIDs.targetNpcTypes7.Contains(npc.type) || npc.type == CWRIDs.PlaguebringerGoliath) {
                    for (int i = 0; i < Main.rand.Next(3, 6); i++) {
                        int type = Item.NewItem(npc.parent(), npc.Hitbox, CWRIDs.DubiousPlating, Main.rand.Next(7, 13));
                        if (CWRUtils.isClient) {
                            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, type, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
            }
            PerforatorBehavior.Instance.BloodMoonDorp(npc);
            HiveMindBehavior.Instance.BloodMoonDorp(npc);
            base.OnKill(npc);
        }

        public override void HitEffect(NPC npc, NPC.HitInfo hit) {
            if (npc.life <= 0) {
                if (TheEndSunOnHitNum) {
                    for (int i = 0; i < Main.rand.Next(16, 33); i++) {
                        npc.NPCLoot();
                    }
                }
            }
            base.HitEffect(npc, hit);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
            if (WhipHitNum > 0) {
                DrawTameBar(spriteBatch, npc);
            }
            if (Main.bloodMoon) {
                if (npc.type == CWRIDs.PerforatorHive) {
                    PerforatorBehavior.Instance.Draw(spriteBatch, npc);
                }
                if (npc.type == CWRIDs.HiveMind) {
                    HiveMindBehavior.Instance.Draw(spriteBatch, npc);
                }
            }
        }

        public void DrawTameBar(SpriteBatch spriteBatch, NPC npc) {
            Texture2D top = CWRUtils.GetT2DValue(CWRConstant.UI + "TameBarTop");
            Texture2D bar = CWRUtils.GetT2DValue(CWRConstant.UI + "TameBar");
            Texture2D whi = WhipHitDate.Tex((WhipHitTypeEnum)WhipHitType);

            float slp = 0.75f;
            float alp = 1 - (npc.velocity.Length() / 15f);
            if (alp < 0.3f) {
                alp = 0.3f;
            }

            int sengs = (int)((1 - (WhipHitNum / 10f)) * bar.Height);
            Rectangle barRec = new(sengs, 0, bar.Width, bar.Height - sengs);
            Color color = Color.White * alp;

            Vector2 drawPos = new Vector2(npc.position.X + (npc.width / 2), npc.Bottom.Y + top.Height) - Main.screenPosition;

            spriteBatch.Draw(top, drawPos, null, color, 0, bar.Size() / 2, slp, SpriteEffects.None, 0);

            spriteBatch.Draw(bar, drawPos + (new Vector2(14, sengs + 18) * slp), barRec, color, 0, bar.Size() / 2, slp, SpriteEffects.None, 0);

            spriteBatch.Draw(whi, drawPos + (new Vector2(0, whi.Height) * slp), null, color, 0, bar.Size() / 2, slp / 2, SpriteEffects.None, 0);
        }
    }
}
