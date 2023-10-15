using CalamityMod;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace CalamityWeaponRemake.Content.Projectiles
{
    public abstract class BaseGlaiveProjectile : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Melee";

        public float FlappingVelocity = 3;

        public virtual float StartFlappingAngle => -70f;

        public virtual float EndFlappingAngle => 70f;

        public float FlappingAngle = 0f;

        public Player Owner => AiBehavior.GetPlayerInstance(Projectile.owner);

        public virtual float AngularMirrorImage(float arg)
        {
            arg = MathHelper.ToRadians(arg);
            Vector2 v = arg.ToRotationVector2();
            v = new Vector2(-v.X, v.Y);
            return v.ToRotation();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.direction = Math.Sign(Projectile.velocity.X);
            FlappingAngle = StartFlappingAngle;
        }

        public virtual void Behavior()
        {
            if (Owner == null)
            {
                Projectile.Kill();
                return;
            }

            Owner.heldProj = Projectile.whoAmI;
            //Owner.itemTime = Owner.itemAnimation;
            Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
            Projectile.direction = Owner.direction;
            FlappingVelocity = MathF.Abs(StartFlappingAngle - EndFlappingAngle) / Owner.itemAnimationMax;
            FlappingAngle += FlappingVelocity;
            Projectile.rotation = Projectile.direction > 0 ? MathHelper.ToRadians(FlappingAngle) : AngularMirrorImage(FlappingAngle);
            if (FlappingAngle > EndFlappingAngle)
                Projectile.Kill();
        }

        public override void AI()
        {
            Behavior();
        }

        public virtual Vector2 DrawOrig => 
            Projectile.direction > 0 ? new Vector2(0, DrawUtils.GetT2DValue(Texture).Height)
            : new Vector2(DrawUtils.GetT2DValue(Texture).Width, DrawUtils.GetT2DValue(Texture).Height);

        public virtual Rectangle? DrawRec => null;

        public virtual SpriteEffects DrawSE => 
            Projectile.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;  

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DrawUtils.GetT2DValue(Texture);
            float offsetRot = Projectile.direction > 0 ? MathHelper.PiOver4 : -MathHelper.PiOver4;
            Main.EntitySpriteDraw(
                texture,
                DrawUtils.WDEpos(Projectile.Center),
                DrawRec,
                Color.White,
                Projectile.rotation + offsetRot,
                DrawOrig,
                Projectile.scale,
                DrawSE,
                0
                );
            return false;
        }
    }
}
