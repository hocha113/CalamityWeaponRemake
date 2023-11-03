﻿using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class TerrorBlasts : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Melee";

        public override string Texture => CWRConstant.placeholder;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override bool? CanDamage()
        {
            return true;
        }

        public override void AI()
        {
            base.AI();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CWRUtils.CircularHitboxCollision(Projectile.Center, 150, targetHitbox);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in SoundID.Item60, Projectile.position);
            Projectile.width = 400;
            Projectile.height = 400;
            Vector2 projCenter = Projectile.position;

            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(projCenter, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 100, default, 1.5f);
            }

            for (int i = 0; i < 66; i++)
            {
                Vector2 pos = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(-200, 200) + projCenter;
                int num = Dust.NewDust(pos, 1, 1, DustID.RedTorch, 0f, 0f, 0, default, 2.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 3f;
                num = Dust.NewDust(pos, 2, 2, DustID.RedTorch, 0f, 0f, 100, default, 1.5f);
                Main.dust[num].velocity *= 2f;
                Main.dust[num].noGravity = true;
            }

            Projectile.localAI[0] = 1f;
            Projectile.Damage();
        }
    }
}
