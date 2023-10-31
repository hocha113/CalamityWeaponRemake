using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Summon.Whips
{
    internal class AllhallowsGoldWhipProjectile : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Summon + "AllhallowsGoldWhipProjectile";

        private List<Vector2> whipPoints => Projectile.GetWhipControlPoints();//点集

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 35;
            Projectile.WhipSettings.RangeMultiplier = 1f;
        }

        private float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnSpawn(IEntitySource source)
        {
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodKillsFire>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = Projectile.damage / 2;

            if (Projectile.numHits == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(
                        AiBehavior.GetEntitySource_Parent(Projectile),

                        target.Center -
                        Main.player[Projectile.owner].Center.To(target.Center).UnitVector()
                        .RotatedBy(MathHelper.ToRadians(Main.rand.Next(-75, 75))) * 300,

                        Vector2.Zero,
                        ModContent.ProjectileType<CosmicFire>(),
                        Projectile.damage + 500,
                        0,
                        Projectile.owner,
                        Projectile.whoAmI
                    );
                }
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

            Vector2 pos = whipPoints[0];

            for (int i = 0; i < whipPoints.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 42, 66);

                Vector2 origin = new Vector2(21, 33);
                float scale = 1;
                float offsetRots = 0;

                if (i == whipPoints.Count - 2)
                {
                    frame.Y = 134;
                    frame.Height = 96;
                    origin = new Vector2(20, 20);
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Time / timeToFlyOut;
                    scale = MathHelper.Lerp(1.05f, 2f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 0)
                {
                    int count = i % 3;
                    if (count == 0)
                    {
                        frame.Y = 102;
                        frame.Height = 30;
                        origin = new Vector2(20, 18);
                    }
                    if (count == 1)
                    {
                        frame.Y = 68;
                        frame.Height = 32;
                        origin = new Vector2(20, 18);
                    }
                    if (count == 2)
                    {
                        frame.Y = 102;
                        frame.Height = 30;
                        origin = new Vector2(22, 18);
                        offsetRots = MathHelper.Pi;
                    }
                    scale = 1 + i / 120f;
                }

                Vector2 element = whipPoints[i];
                Vector2 diff = whipPoints[i + 1] - element;

                scale *= 0.7f;
                float rotation = diff.ToRotation() - MathHelper.PiOver2; // 此投射物的精灵图朝下，因此使用PiOver2进行旋转修正
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, Color.White, rotation + offsetRots, origin, scale, flip, 0);
                pos += diff;
            }
            return false;
        }
    }
}
