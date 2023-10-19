using CalamityWeaponRemake.Common;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using static CalamityWeaponRemake.Common.AuxiliaryMeans.AiBehavior;
using static CalamityWeaponRemake.Common.DrawTools.DrawUtils;
using Microsoft.Xna.Framework.Graphics;

namespace CalamityWeaponRemake.Content.Projectiles.Ranged
{
    internal class DeathwindHeldProj : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Deathwind";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.hide = true;
        }

        public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int Time { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }
        private Player Owner => Main.player[Projectile.owner];
        private Vector2 toMou = Vector2.Zero;
        private Item deathwind => Owner.HeldItem;

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            if (!PlayerAlive(Owner) || (deathwind.type != ModContent.ItemType<Items.Ranged.Deathwind>()
                && deathwind.type != ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>()))
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Owner.Center + toMou.UnitVector() * 13;
            if (Projectile.IsOwnedByLocalPlayer())
            {
                StickToOwner();
                SpanProj();
            }
            Time++;
        }

        public void SpanProj()
        {
            if (Time % 10 == 0)
            {
                int ammo = Projectile.NewProjectile(
                            GetEntitySource_Parent(Owner),
                            Projectile.Center,
                            toMou.UnitVector() * 15,
                            (int)Projectile.localAI[1],
                            Projectile.damage,
                            Projectile.knockBack,
                            Projectile.owner
                            );
                Main.projectile[ammo].extraUpdates = 2;
            }
        }

        public void StickToOwner()
        {
            if (Owner.PressKey())
            {
                toMou = Owner.Center.To(Main.MouseWorld);
                Projectile.rotation = toMou.ToRotation();
                Owner.SetDummyItemTime(2);
                Projectile.timeLeft = 2;
            }
            Owner.direction = toMou.X > 0 ? 1 : -1;
            Owner.heldProj = Projectile.whoAmI;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Owner == null) return false;

            SpriteEffects spriteEffects = toMou.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            float drawRot = Projectile.rotation;

            Texture2D mainValue = GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                WDEpos(Projectile.Center),
                null,
                Color.White,
                drawRot,
                new Vector2(13, mainValue.Height * 0.5f),
                Projectile.scale,
                spriteEffects,
                0
                );
            return false;
        }
    }
}
