﻿using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityWeaponRemake.Content.Items.Melee;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles
{
    internal class RElementalShivProj : BaseShortswordProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<ElementalShiv>();

        public override string Texture => "CalamityMod/Items/Weapons/Melee/ElementalShiv";

        public override Action<Projectile> EffectBeforePullback => delegate
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 14f, ModContent.ProjectileType<ElementBallShivs>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);
        };

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(22f);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override void SetVisualOffsets()
        {
            int ofsX = Projectile.width / 2;
            int ofsY = Projectile.height / 2;
            DrawOriginOffsetX = 0f;
            DrawOffsetX = -(22 - ofsX);
            DrawOriginOffsetY = -(22 - ofsY);
        }

        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
            {
                int num = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 66, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[num].velocity *= 0.2f;
                Main.dust[num].noGravity = true;
            }
        }
    }
}
