using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeavenfallLongbowProj
{
    internal class HeavenfallLongbowHeldProj : ModProjectile
    {
        public override string Texture => CWRConstant.Item_Ranged + "HeavenfallLongbow";
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<HeavenfallLongbow>();

        private Player Owners => CWRUtils.GetPlayerInstance(Projectile.owner);
        private Item heavenfall => Owners.HeldItem;
        private Vector2 toMou = Vector2.Zero;
        private ref float Time => ref Projectile.ai[0];
        private ref float Time2 => ref Projectile.ai[1];

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 116;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(toMou);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            toMou = reader.ReadVector2();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.7f, 0.5f);

            if (Owners == null)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.IsOwnedByLocalPlayer())
                SpanProj();
            StickToOwner();
            Time++;
            Time2++;
        }

        public void SpanProj()
        {
            
        }

        public void StickToOwner()
        {
            if (heavenfall.ModItem == null)
            {
                Projectile.Kill();
                return;
            }
            HeavenfallLongbow bow = (HeavenfallLongbow)heavenfall.ModItem;
            if (bow == null)
            {
                Projectile.Kill();
                return;
            }
            bow.Item.damage = 9999;

            if (Projectile.IsOwnedByLocalPlayer())
            {
                Vector2 oldToMou = toMou;
                toMou = Owners.Center.To(Main.MouseWorld);
                if (oldToMou != toMou)
                {
                    Projectile.netUpdate = true;
                }
            }
            if (Owners.PressKey() || Owners.PressKey(false))
            {
                Projectile.timeLeft = 2;
                Owners.itemTime = 2;
                Owners.itemAnimation = 2;
                float frontArmRotation = (MathHelper.PiOver2 - 0.31f) * -Owners.direction;
                Owners.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmRotation);
            }
            Projectile.position = Owners.RotatedRelativePoint(Owners.MountedCenter, true) - Projectile.Size / 2f + toMou.UnitVector() * 15;
            Projectile.rotation = toMou.ToRotation();
            Projectile.spriteDirection = Projectile.direction = Math.Sign(toMou.X);
            Owners.ChangeDir(Projectile.direction);
            Owners.heldProj = Projectile.whoAmI;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture + "Glow");
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue),
                Color.White,
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue),
                Projectile.scale,
                Owners.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically
                );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue),
                lightColor,
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue),
                Projectile.scale,
                Owners.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically
                );
            return false;
        }

        public override bool? CanDamage() => false;
    }
}
