using CalamityMod;
using CalamityWeaponRemake.Common;
using System;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.DawnshatterAzureProj
{
    internal class DawnshatterAzureProj : BaseSpearProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<Items.Melee.DawnshatterAzure>();

        public override string Texture => CWRConstant.Projectile_Melee + "DawnshatterAzureProj";

        public Player Owner => Main.player[Projectile.owner];

        public Item elementalLance => Owner.HeldItem;

        protected float HoldoutRangeMin => -24f;
        protected float HoldoutRangeMax => 96f;

        public override float InitialSpeed => 3f;

        public override float ReelbackSpeed => 2.5f;

        public override float ForwardSpeed => 0.9f;

        public override Action<Projectile> EffectBeforeReelback => delegate {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity, Projectile.velocity
                , ModContent.ProjectileType<TheEndSun>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        };

        public override void SetDefaults() {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
            Projectile.hide = true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            int duration = player.itemAnimationMax;
            if (Projectile.timeLeft > duration) {
                Projectile.timeLeft = duration;
            }
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            float halfDuration = duration * 0.5f;
            float progress;
            if (Projectile.timeLeft < halfDuration) {
                progress = Projectile.timeLeft / halfDuration;
            }
            else {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }
            if (Projectile.timeLeft == duration / 2) {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity, Projectile.velocity * 15
                , ModContent.ProjectileType<TheEndSun>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            Projectile.rotation = Projectile.velocity.ToRotation();
            player.direction = Math.Sign(player.position.To(Main.MouseWorld).X);
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            float rot = Projectile.rotation + MathHelper.PiOver4;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2;
            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), rot, origin, Projectile.scale, 0, 0);
            return false;
        }
    }
}
