using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Terraria.Graphics.Shaders;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.AnnihilatingUniverseProj
{
    internal class CosmicEddies : ModProjectile
    {
        public override string Texture => CWRConstant.placeholder;

        public override void SetDefaults()
        {
            Projectile.height = 24;
            Projectile.width = 24;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 260;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 60)
            {
                Projectile.scale = Projectile.timeLeft / 60f;
            }
            if (Projectile.timeLeft == 130 && Projectile.IsOwnedByLocalPlayer())
            {
                Projectile.NewProjectile(Projectile.parent(), Projectile.Center, Projectile.Center.To(Main.MouseWorld).UnitVector() * 23
                    , ModContent.ProjectileType<DivineDevourerIllusionHead>(), 355, 3, Projectile.owner);
            }
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion();

            Texture2D noiseTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/VoronoiShapes").Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = noiseTexture.Size() * 0.5f;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(Projectile.scale);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(Color.DarkBlue);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(Color.Fuchsia);
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            Main.EntitySpriteDraw(noiseTexture, drawPosition, null, Color.White, 0f, origin, 0.5f, SpriteEffects.None, 0);
            Main.spriteBatch.ExitShaderRegion();

            return false;
        }
    }
}
