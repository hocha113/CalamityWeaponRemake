﻿using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class RedLightningFeather : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "RedLightningFeather";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }
        public Vector2 DashVr
        {
            set
            {
                Projectile.localAI[0] = value.X;
                Projectile.localAI[1] = value.Y;
            }
            get
            {
                return new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            }
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override void OnSpawn(IEntitySource source)
        {

        }

        public override bool ShouldUpdatePosition()
        {
            return true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            CWRUtils.ClockFrame(ref Projectile.frameCounter, 5, 3);

            if (Status == 0)
            {
                if (Projectile.timeLeft < 60)
                {
                    NPC target = Projectile.Center.InPosClosestNPC(900, false);
                    if (target != null && Behavior == 0)
                    {
                        Behavior = 1;
                    }
                    if (Behavior == 1)
                    {
                        DashVr = Projectile.Center.To(target.Center).UnitVector() * 37;
                        Behavior = 2;
                    }
                    if (Behavior == 2)
                    {
                        Projectile.velocity = DashVr;
                    }
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                CWRUtils.WDEpos(Projectile.Center),
                CWRUtils.GetRec(mainValue, Projectile.frameCounter, 4),
                Color.White,
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue, 4),
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
