using CalamityMod;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.CWRUtils;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    public class BaseHeldBow : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;

        public Player Owner => Main.player[Projectile.owner];

        public Vector2 toMou = Vector2.Zero;

        public Vector2 oldToMou = Vector2.Zero;

        public virtual float HandDistance => 5;

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.hide = true;
        }

        public override bool ShouldUpdatePosition() {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.WriteVector2(toMou);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            toMou = reader.ReadVector2();
        }

        public override void PostAI() {
            Projectile.position = Owner.RotatedRelativePoint(Owner.MountedCenter, true) - Projectile.Size / 2f + toMou.UnitVector() * HandDistance;
            Projectile.rotation = toMou.ToRotation();
            Projectile.spriteDirection = Projectile.direction = Math.Sign(toMou.X);
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
        }

        public override bool PreAI() {
            if (Projectile.IsOwnedByLocalPlayer()) {
                Vector2 oldToMou = toMou;
                toMou = Owner.Center.To(Main.MouseWorld);
                if (oldToMou != toMou) {
                    Projectile.netUpdate = true;
                }
            }
            return true;
        }

        public override void AI() {
            if (Projectile.IsOwnedByLocalPlayer()) {
                StickToOwner();
                SpanProj();
            }
        }

        public virtual void StickToOwner() {

        }

        public virtual void SpanProj() {

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            return false;
        }

        public override bool PreDraw(ref Color lightColor) {
            if (Owner == null) return false;

            SpriteEffects spriteEffects = toMou.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            float drawRot = Projectile.rotation;

            Texture2D mainValue = GetT2DValue(Texture);
            Main.EntitySpriteDraw(mainValue, WDEpos(Projectile.Center), null, Color.White, drawRot
                , new Vector2(13, mainValue.Height * 0.5f), Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}
