using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles
{
    public class SeaBlueBrinySpout : CustomProjectiles
    {
        public override string Texture => CWRConstant.Projectile + "BrinySpout";

        public int MaxTierLimit
        {
            get => (int)Projectile.localAI[1]; set => Projectile.localAI[1] = value;
        }
        public float Magnifying
        {
            get => Projectile.localAI[2]; set => Projectile.localAI[2] = value;
        }


        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 16;
        }

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 40;
            Projectile.damage = 100;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
        }

        public override int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }
        public int OwnerProJindex
        {
            get => (int)Projectile.localAI[0]; set => Projectile.localAI[0] = value;
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override void OnSpawn(IEntitySource source)
        {

        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
            writer.Write(Projectile.alpha);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadInt32();
            Projectile.localAI[1] = reader.ReadInt32();
            Projectile.localAI[2] = reader.ReadInt32();
            Projectile.alpha = reader.ReadInt32();
        }

        public override void AI()
        {
            ThisTimeValue++;

            if (Status == 0)
            {
                if (Magnifying == 0)
                    Magnifying = 0.12f;
                Behavior++;
                Status = 1;
            }

            if (Status == 1)
            {
                if (Behavior == 1)
                {
                    OwnerProJindex = Projectile.whoAmI;
                    if (Projectile.IsOwnedByLocalPlayer())
                        Projectile.alpha = HcMath.HcRandom.Next(0, 10000);
                    Projectile.netUpdate = true;
                }

                if (Behavior <= MaxTierLimit && ThisTimeValue > 5 && Projectile.IsOwnedByLocalPlayer())
                {
                    int proj = Projectile.NewProjectile(
                        AiBehavior.GetEntitySource_Parent(Projectile),
                        Projectile.Center + new Vector2(0, -Projectile.height * Projectile.scale),
                        Vector2.Zero,
                        Projectile.type,
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                        );

                    Projectile newProj = AiBehavior.GetProjectileInstance(proj);
                    if (newProj != null)
                    {
                        newProj.ai[1] = Behavior;
                        newProj.localAI[0] = OwnerProJindex;
                        newProj.localAI[1] = MaxTierLimit;
                        newProj.scale *= 1 + Behavior * Magnifying;
                        newProj.alpha = Projectile.alpha;
                        newProj.netUpdate = true;
                    }
                    else
                    {
                        Projectile.Kill();
                    }

                    Status = 2;
                    Projectile.netUpdate = true;
                }
            }
            if (Status == 2)
            {
                if (Behavior == 1)
                {
                    Player target = AiBehavior.NPCFindingPlayerTarget(Projectile, -1);
                    if (target != null)
                    {
                        //Vector2 toTarget = Projectile.Center.To(target.Center).SafeNormalize(Vector2.Zero);
                        Projectile.Center += Projectile.velocity;
                    }
                }
            }
            if (Status == 3)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.Center += new Vector2((float)Math.Sin(MathHelper.ToRadians(ThisTimeValue * 5)) * 3 * Projectile.scale, 0);
            }

            if (Behavior != 1 && Status != 3)
            {
                Projectile OwnerProj = AiBehavior.GetProjectileInstance(OwnerProJindex);
                if (OwnerProj != null && Projectile.alpha == OwnerProj.alpha)
                {
                    Projectile.timeLeft = Behavior * 6;
                    float offsetY = 0;
                    for (int i = 1; i < Behavior; i++)
                    {
                        offsetY += -Projectile.height * (1 + i * Magnifying);
                    }
                    Projectile.velocity = Vector2.Zero;
                    Projectile.Center = new Vector2(OwnerProj.Center.X + (float)Math.Sin(MathHelper.ToRadians(ThisTimeValue * 5)) * 60 * Projectile.scale, OwnerProj.Center.Y + offsetY);
                }
                else
                {
                    Status = 3;
                }
            }
            else
            {

            }

            if (PlayerInput.Triggers.Current.MouseRight) Projectile.Kill();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = DrawUtils.GetT2DValue(Texture);
            DrawUtils.ClockFrame(ref Projectile.frameCounter, 3, 5);

            Main.EntitySpriteDraw(
                mainValue,
                DrawUtils.WDEpos(Projectile.Center),
                DrawUtils.GetRec(mainValue, Projectile.frameCounter, 6),
                Color.White,
                Projectile.rotation,
                DrawUtils.GetOrig(mainValue, 6),
                Projectile.scale,
                SpriteEffects.None,
                0
                );
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {

        }
    }
}
