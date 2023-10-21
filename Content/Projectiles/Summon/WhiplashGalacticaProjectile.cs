using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Melee;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityMod;
using System;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;

namespace CalamityWeaponRemake.Content.Projectiles.Summon
{
    internal class WhiplashGalacticaProjectile : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Summon + "WhiplashGalacticaProjectile";

        private List<Vector2> whipPoints => Projectile.GetWhipControlPoints();
        private Vector2 whipTopPos => whipPoints[whipPoints.Count - 2];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 1f;
        }

        private float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.NewProjectile(
                AiBehavior.GetEntitySource_Parent(Projectile),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<Trail>(),
                0,
                0,
                ai2:Projectile.whoAmI
                );
        }

        public override bool PreAI()
        {
            Vector2 topPos = whipPoints[whipPoints.Count - 2];
            float lengs = Main.player[Projectile.owner].Center.To(topPos).Length() / 14f;
            if (lengs > 120)
                lengs = 120;
            if (lengs < 30)
                lengs = 30;
            Projectile.WhipSettings.Segments = (int)lengs;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage -= 15;

            if (Projectile.numHits == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(
                        AiBehavior.GetEntitySource_Parent(Projectile),

                        target.Center + 
                        Main.player[Projectile.owner].Center.To(target.Center).UnitVector()
                        .RotatedBy(MathHelper.ToRadians(Main.rand.Next(-75, 75))) * 300,

                        Vector2.Zero,
                        ModContent.ProjectileType<CosmicFire>(),
                        Projectile.damage / 2,
                        0,
                        ai2: Projectile.whoAmI
                    );
                }
            }
            

            //StarnightBeams.StarRT(Projectile, target);
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
                Color color = new Color(252, 102, 202);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }//绘制连接线

        public override bool PreDraw(ref Color lightColor)
        {
            DrawLine(whipPoints);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D _men = DrawUtils.GetT2DValue(CWRConstant.Projectile_Summon + "WhiplashGalacticaProjectileGlows");

            Vector2 pos = whipPoints[0];

            for (int i = 0; i < whipPoints.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 34); // 手柄的大小（以像素为单位）

                Vector2 origin = new Vector2(15, 20); // 从图像左上角开始测量玩家手部起始位置的偏移量
                float scale = 1;

                // 这些语句确定当前段要绘制精灵图表中的哪个部分。
                if (i == whipPoints.Count - 2)
                {
                    frame.Y = 65; // 距离帧开始处到精灵图顶部的距离

                    frame.Height = 30; // 帧图高度

                    // 为了获得更具影响力的外观，当完全伸展时，它会将鞭尖缩放，并在卷曲时向下缩放
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Time / timeToFlyOut;
                    scale = MathHelper.Lerp(1.05f, 2f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 0)
                {
                    frame.Y = 43;
                    frame.Height = 14;
                    scale = 1 + i / 120f;
                }

                Vector2 element = whipPoints[i];
                Vector2 diff = whipPoints[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // 此投射物的精灵图朝下，因此使用PiOver2进行旋转修正
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);
                Main.EntitySpriteDraw(_men, pos - Main.screenPosition, frame, Color.White, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }

        private class Trail : ModProjectile
        {
            internal PrimitiveTrail TrailDrawer;

            public override string Texture => CWRConstant.placeholder;

            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.width = 6;
                Projectile.height = 6;
                Projectile.scale = 1;
                Projectile.alpha = 80;
                Projectile.friendly = true;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.penetrate = -1;
                Projectile.MaxUpdates = 5;
                Projectile.timeLeft = 150;
            }

            public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
            public int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
            public int Time { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

            public override void AI()
            {
                Projectile ownProj = AiBehavior.GetProjectileInstance(Behavior);
                if (ownProj != null)
                {
                    List<Vector2> toPos = AiBehavior.GetWhipControlPoints(ownProj);
                    int index = toPos.Count - 2;
                    if (index < toPos.Count && index >= 0)
                        Projectile.velocity = Projectile.Center.To(toPos[toPos.Count - 2]);
                    Projectile.timeLeft = 2;
                }
                else Projectile.Kill();
            }

            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                return false;
            }

            internal Color ColorFunction(float completionRatio)
            {
                float amount = MathHelper.Lerp(0.65f, 1f, (float)Math.Cos((0f - Main.GlobalTimeWrappedHourly) * 3f) * 0.5f + 0.5f);
                float num = Utils.GetLerpValue(1f, 0.64f, completionRatio, clamped: true) * Projectile.Opacity;
                Color value = Color.Lerp(new Color(192, 192, 192), new Color(211, 211, 211), (float)Math.Sin(completionRatio * MathF.PI * 1.6f - Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
                return Color.Lerp(new Color(255, 255, 255), value, amount) * num;
            }

            internal float WidthFunction(float completionRatio)
            {
                float amount = (float)Math.Pow(1f - completionRatio, 3.0);
                return MathHelper.Lerp(0f, 32f * Projectile.scale * Projectile.Opacity, amount);
            }

            public override bool PreDraw(ref Color lightColor)
            {
                if (TrailDrawer == null)
                {
                    TrailDrawer = new PrimitiveTrail(WidthFunction, ColorFunction, null, GameShaders.Misc["CalamityMod:TrailStreak"]);
                }

                GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak"));
                TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 30);
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Main.EntitySpriteDraw(
                    value,
                    Projectile.Center - Main.screenPosition,
                    null,
                    Color.Lerp(lightColor, Color.White, 0.5f),
                    Projectile.rotation + MathHelper.PiOver2,
                    value.Size() / 2f,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
                return false;
            }
        }
    }
}
