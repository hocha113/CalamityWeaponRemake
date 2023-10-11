using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using static CalamityWeaponRemake.Common.AuxiliaryMeans.AiBehavior;
using static CalamityWeaponRemake.Common.DrawTools.DrawUtils;

namespace CalamityWeaponRemake.Content.Projectiles
{
    public class HeldProjectiles : CustomProjectiles
    {
        public override string Texture => CWRConstant.placeholder;

        public override void SetStaticDefaults()
        {

        }

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
        }

        public override int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

        Player Owner => GetPlayerInstance(Projectile.owner);
        Vector2 toMou => Owner.Center.To(Main.MouseWorld);

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SpanOnRotProj();
        }

        public override void OnSpawn(IEntitySource source)
        {
            SpanOnRotProj();
        }

        public virtual void SpanOnRotProj()
        {
            if (Owner != null) Projectile.rotation = toMou.ToRotation();
        }

        public virtual void RotProj()
        {
            Projectile.EntityToRot(toMou.ToRotation(), 0.2f);
        }

        public virtual void FlyProjPos(float dis = 0, Vector2 offset = default)
        {
            Vector2 rotOffset = Projectile.rotation.ToRotationVector2() * dis;
            Projectile.Center = offset == default ? Owner.Center + rotOffset : Owner.Center + rotOffset + offset;
        }

        public virtual void HoldingJudgment()
        {
            if (PlayerInput.Triggers.Current.MouseLeft) Projectile.timeLeft = 2;
            else Projectile.Kill();
        }

        public virtual void OwnerDirection()
        {
            Owner.direction = toMou.X > 0 ? 1 : -1;
        }

        public virtual void OwnerArm()
        {
            //TODO
        }

        public override void PostAI()
        {
            if (Owner == null)
            {
                Projectile.Kill();
                return;
            }

            HoldingJudgment();
            OwnerDirection();
            OwnerArm();

            RotProj();
            FlyProjPos();
        }

        public override void AI()
        {

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public virtual Vector2 drawOrig => Vector2.Zero;
        public virtual float drawRot => Projectile.rotation;
        public virtual SpriteEffects drawSpriteEffects => toMou.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

        public override bool PreDraw(ref Color lightColor)
        {
            if (Owner == null) return false;

            Texture2D mainValue = GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                WDEpos(Projectile.Center),
                null,
                Color.White,
                drawRot,
                drawOrig,
                Projectile.scale,
                drawSpriteEffects,
                0
                );
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
