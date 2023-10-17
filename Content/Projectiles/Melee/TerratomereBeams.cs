using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class TerratomereBeams : ModProjectile
    {
        public Vector2[] ControlPoints;

        public PrimitiveTrail SlashDrawer;

        public new string LocalizationCategory => "Projectiles.Melee";

        public bool Flipped => Projectile.ai[0] == 1f;

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 144;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player owner = AiBehavior.GetPlayerInstance(Projectile.owner);
            if (owner != null) Projectile.Center = owner.Center;
            Projectile.Opacity = Utils.GetLerpValue(0f, 26f, Projectile.timeLeft, clamped: true);
            Projectile.velocity *= 0.91f;
            Projectile.scale *= 1.03f;
        }

        public float SlashWidthFunction(float completionRatio)
        {
            return Projectile.scale * 50f;
        }

        public Color SlashColorFunction(float completionRatio)
        {
            return Color.Lime * Utils.GetLerpValue(0.07f, 0.57f, completionRatio, clamped: true) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (SlashDrawer == null)
            {
                SlashDrawer = new PrimitiveTrail(SlashWidthFunction, SlashColorFunction, null, GameShaders.Misc["CalamityMod:ExobladeSlash"]);
            }

            Main.spriteBatch.EnterShaderRegion();
            TerratomereHoldoutProj.PrepareSlashShader(Flipped);
            List<Vector2> list = new List<Vector2>();

            if (ControlPoints == null) 
                return false;

            for (int i = 0; i < ControlPoints.Length; i++)
            {
                list.Add(ControlPoints[i] + ControlPoints[i].SafeNormalize(Vector2.Zero) * (Projectile.scale - 1f) * 70f);
            }

            for (int j = 0; j < 3; j++)
            {
                SlashDrawer.Draw(list, Projectile.Center - Main.screenPosition, 65);
            }

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool collBool = false;
            Vector2 starPos = Projectile.Center;
            for (int i = 0; i < 20; i++)
            {
                collBool = Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                starPos,
                Projectile.Center +
                (Projectile.velocity.UnitVector() * Projectile.scale * 130)
                .RotatedBy(MathHelper.ToRadians(-90 + i * 9))
                );
                if (collBool) break;
            }

            return collBool;
        }
    }
}
