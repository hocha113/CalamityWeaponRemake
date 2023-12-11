﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles
{
    internal class RCosmicShivProjectile : BaseShortswordProjectile
    {
        public override string Texture => CWRConstant.Item_Melee + "CosmicShiv";

        public override Action<Projectile> EffectBeforePullback => delegate
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 14f, ModContent.ProjectileType<RCosmicShivBall>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);
        };

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(24f);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }

        public override void SetVisualOffsets()
        {
            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;
            DrawOriginOffsetX = 0f;
            DrawOffsetX = -(24 - HalfProjWidth);
            DrawOriginOffsetY = -(24 - HalfProjHeight);
        }

        public override void ExtraBehavior()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ShadowbeamStaff);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 36; i++)
            {
                int dustID = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height - 16, 173);
                Dust obj = Main.dust[dustID];
                obj.velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 36; i++)
            {
                int dustID = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height - 16, 173);
                Dust obj = Main.dust[dustID];
                obj.velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);
        }
    }
}
