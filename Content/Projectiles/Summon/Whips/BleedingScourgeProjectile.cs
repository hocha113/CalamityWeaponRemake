﻿using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Ranged;

namespace CalamityWeaponRemake.Content.Projectiles.Summon.Whips
{
    internal class BleedingScourgeProjectile : ModProjectile
    {
        int Time = 0;

        public override string Texture => CWRConstant.Projectile_Summon + "BleedingScourgeProjectile";

        private List<Vector2> whipPoints => Projectile.GetWhipControlPoints();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 0.8f;
        }

        public override bool PreAI()
        {
            if (whipPoints.Count - 2 >= 0 && whipPoints.Count - 2 < whipPoints.Count)
            {
                Vector2 pos = whipPoints[whipPoints.Count - 2];
                int num = Dust.NewDust(pos, Projectile.width, Projectile.height, DustID.Blood, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity = Vector2.Zero;

                //Player owners = AiBehavior.GetPlayerInstance(Projectile.owner);
                //if (owners != null)
                //{
                //    float lengs = owners.Center.To(pos).Length();
                //    if (lengs > 160 && Time % 5 == 0)
                //    {
                //        Vector2 vr = owners.Center.To(pos).UnitVector() * 13;
                //        Projectile.NewProjectileDirect(
                //            AiBehavior.GetEntitySource_Parent(Projectile),
                //            pos,
                //            vr,
                //            ModContent.ProjectileType<BloodflareSouls>(),
                //            Projectile.damage / 2,
                //            2,
                //            owners.whoAmI
                //            );
                //    }
                //}
            }
            Time++;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage -= 35;
            if (Projectile.numHits == 0)
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = target.Center + HcMath.GetRandomVevtor(-120, -60, Main.rand.Next(760, 820));
                Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(Projectile),
                    pos,
                    pos.To(target.Center).UnitVector() * 13,
                    ModContent.ProjectileType<BloodBall>(),
                    Projectile.damage / 2,
                    0,
                    Projectile.owner
                    );
            }            
        }

        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 2; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = new Color(222, 10, 112);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }//绘制连接线

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLine(whipPoints);
            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = DrawUtils.GetT2DValue(Texture);

            Vector2 pos = whipPoints[0];

            for (int i = 1; i < whipPoints.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 32, 38);

                Vector2 origin = new Vector2(17, 23);
                float scale = 1 + i / 30f;

                if (i != 1)
                {
                    frame.Y = 42;
                    frame.Height = 20;
                    origin = new Vector2(16, 10);
                }

                if (i == whipPoints.Count - 2)
                {
                    frame.Y = 66;
                    frame.Height = 24;
                    origin = new Vector2(16, 10);
                }

                if (i == 1)
                {
                    frame = new Rectangle(0, 0, 32, 38);
                    origin = new Vector2(17, 23);
                    scale = 1.5f;
                }

                Vector2 element = whipPoints[i];
                Vector2 diff = whipPoints[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);
                //Main.EntitySpriteDraw(_men, pos - Main.screenPosition, frame, Color.White, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}