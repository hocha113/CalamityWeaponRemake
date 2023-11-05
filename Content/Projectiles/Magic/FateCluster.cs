﻿using CalamityWeaponRemake.Common;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace CalamityWeaponRemake.Content.Projectiles.Magic
{
    internal class FateCluster : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Proj_Magic + "FatesRevealFlame";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frameCounter = Main.rand.Next(4);
        }

        public override void AI()
        {
            CWRUtils.ClockFrame(ref Projectile.frameCounter, 5, 3);
            Projectile.rotation = Projectile.velocity.ToRotation();

            NPC target = Projectile.Center.InPosClosestNPC(300);

            if (target != null)
            {
                Projectile.ChasingBehavior2(target.Center, 1.01f, 0.25f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.Explode(64);

            for (int i = 0; i <= 360; i += 3)
            {
                Vector2 vr = new Vector2(3f, 3f).RotatedBy(MathHelper.ToRadians(i));
                int num = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height
                    , DustID.RedTorch, vr.X, vr.Y, 200, new Color(232, 251, 250, 200), 1.4f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position = Projectile.Center;
                Main.dust[num].velocity = vr;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue, Projectile.frameCounter, 4),
                Color.White * (Projectile.alpha / 255f),
                Projectile.rotation - MathHelper.PiOver2,
                CWRUtils.GetOrig(mainValue, 4),
                Projectile.scale,
                SpriteEffects.None
                );
            return false;
        }
    }
}
