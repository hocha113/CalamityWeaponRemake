using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.NPCs
{
    internal class Creeper : ModNPC
    {
        public override string Texture => CWRConstant.Asset + "Creeper";

        public override void SetDefaults()
        {
            NPC.lifeMax = 300;
            NPC.height = 60;
            NPC.width = 30;
            NPC.friendly = false;
            NPC.damage = 322;
        }

        public override void AI()
        {
            Player target = Common.CWRUtils.NPCFindingPlayerTarget(NPC, 600);
            if (target.Alives())
            {
                NPC.ai[0] = 1;
            }
            else
            {
                NPC.ai[0] = 0;
            }

            if (NPC.localAI[0] > 5)
            {
                NPC.frameCounter++;
                NPC.localAI[0] = 0;
            }

            if (NPC.ai[0] == 0)
            {
                if (NPC.frameCounter > 4)
                {
                    NPC.frameCounter = 0;
                }
            }
            if (NPC.ai[0] == 1)
            {
                if (NPC.frameCounter > 19)
                {
                    NPC.frameCounter = 5;
                }

                Vector2 toTarget = NPC.Center.To(target.Center);
                NPC.direction = Math.Sign(toTarget.X);
                NPC.velocity.X = NPC.direction * 3;
            }
            NPC.localAI[0]++;
        }

        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.Item14, NPC.position);
            NPC.width = 600;
            NPC.height = 600;
            NPC.Center = NPC.position;
            for (int j = 0; j < 3; j++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                int numSpikes = 5;
                float spikeAmplitude = 22f;
                float scale = Main.rand.NextFloat(1f, 1.35f);

                for (float spikeAngle = 0f; spikeAngle < MathHelper.TwoPi; spikeAngle += 0.15f)
                {
                    Vector2 offset = spikeAngle.ToRotationVector2() * (2f + (MathF.Sin(angle + spikeAngle * numSpikes) + 1) * spikeAmplitude)
                                     * Main.rand.NextFloat(0.95f, 1.05f);

                    Dust.NewDustPerfect(NPC.Center + Common.CWRUtils.GetRandomVevtor(0, 360, Main.rand.Next(16, 220)), 262, offset, 0, default, scale).customData = 0.025f;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                NPC.Center - Main.screenPosition + new Vector2(0, 6),
                CWRUtils.GetRec(mainValue, (int)NPC.frameCounter, 20),
                Color.White,
                NPC.rotation,
                CWRUtils.GetOrig(mainValue, 20),
                NPC.scale,
                NPC.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally
                );
            return false;
        }
    }
}
