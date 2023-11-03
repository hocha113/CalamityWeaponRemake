using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class CWRNpc : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public byte TerratomereBoltOnHitNum = 0;
        public ushort colldHitTime = 0;
        public byte WhipHitNum = 0;
        public byte WhipHitType = 0;
        public bool SprBoss;

        public override bool CanBeHitByNPC(NPC npc, NPC attacker)
        {
            return base.CanBeHitByNPC(npc, attacker);
        }

        public override void PostAI(NPC npc)
        {
            if (!CWRUtils.isClient)
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

            if (WhipHitNum > 0)
            {
                DrawTameBar(spriteBatch, npc);
            }
        }

        public void DrawTameBar(SpriteBatch spriteBatch, NPC npc)
        {
            Texture2D top = CWRUtils.GetT2DValue(CWRConstant.UI + "TameBarTop");
            Texture2D bar = CWRUtils.GetT2DValue(CWRConstant.UI + "TameBar");
            Texture2D whi = WhipHitDate.Tex((WhipHitTypeEnum)WhipHitType);

            float slp = 0.75f;
            float alp = 1 - (npc.velocity.Length() / 15f);
            if (alp < 0.3f)
            {
                alp = 0.3f;
            }

            int sengs = (int)((1 - (WhipHitNum / 10f)) * bar.Height);
            Rectangle barRec = new(sengs, 0, bar.Width, bar.Height - sengs);
            Color color = Color.White * alp;

            Vector2 drawPos = new Vector2(npc.position.X + (npc.width / 2), npc.Bottom.Y + top.Height) - Main.screenPosition;

            spriteBatch.Draw(
                top,
                drawPos,
                null,
                color,
                0,
                bar.Size() / 2,
                slp,
                SpriteEffects.None,
                0
                );

            spriteBatch.Draw(
                bar,
                drawPos + (new Vector2(14, sengs + 18) * slp),
                barRec,
                color,
                0,
                bar.Size() / 2,
                slp,
                SpriteEffects.None,
                0
                );

            spriteBatch.Draw(
                whi,
                drawPos + (new Vector2(0, whi.Height) * slp),
                null,
                color,
                0,
                bar.Size() / 2,
                slp / 2,
                SpriteEffects.None,
                0
                );
        }
    }
}
