using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Particles;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Items.Ranged;
using System;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeavenfallLongbowProj
{
    internal class ParadiseArrow : ModProjectile
    {
        public override string Texture => CWRConstant.placeholder;

        public PrimitiveTrail PierceDrawer = null;

        Color chromaColor => CWRUtils.MultiLerpColor(Projectile.ai[0] % 15 / 15f, HeavenfallLongbow.rainbowColors);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.height = 24;
            Projectile.width = 24;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7 * Projectile.MaxUpdates;
        }

        public override void AI()
        {
            if (!CWRUtils.isServer)
            {
                Color outerSparkColor = chromaColor;
                float scaleBoost = MathHelper.Clamp(Projectile.ai[1] * 0.005f, 0f, 2f);
                float outerSparkScale = 1.3f + scaleBoost;
                HeavenfallStarParticle spark = new HeavenfallStarParticle(Projectile.Center, Projectile.velocity, false, 7, outerSparkScale, outerSparkColor);
                CWRParticleHandler.SpawnParticle(spark);

                Color innerSparkColor = CWRUtils.MultiLerpColor(Projectile.ai[1] % 30 / 30f, HeavenfallLongbow.rainbowColors);
                float innerSparkScale = 0.6f + scaleBoost;
                HeavenfallStarParticle spark2 = new HeavenfallStarParticle(Projectile.Center, Projectile.velocity, false, 7, innerSparkScale, innerSparkColor);
                CWRParticleHandler.SpawnParticle(spark2);
            }
            NPC target = Projectile.Center.InPosClosestNPC(1300);
            if (target != null && Projectile.ai[0] > 30)
            {
                Projectile.ChasingBehavior2(target.Center, 1, 0.3f);
            }

            Projectile.ai[0]++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                float slp = Main.rand.NextFloat(0.5f, 1.2f);
                GeneralParticleHandler.SpawnParticle(new PulseRing(target.Center + Main.rand.NextVector2Unit() * Main.rand.Next(13, 330), Vector2.Zero, CWRUtils.MultiLerpColor(Main.rand.NextFloat(1), HeavenfallLongbow.rainbowColors), 0.05f * slp, 0.8f * slp, 8));
            }
            Projectile.timeLeft -= 15;
            if (Projectile.timeLeft <= 0)
                Projectile.timeLeft = 0;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.Explode(300);
        }

        public float PrimitiveWidthFunction(float completionRatio) => Projectile.scale * 15f;

        public Color PrimitiveColorFunction(float _) => CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + Projectile.identity * 0.1372f) % 1f, HeavenfallLongbow.rainbowColors) * Projectile.Opacity * (Projectile.timeLeft / 30f);

        public override bool PreDraw(ref Color lightColor)
        {
            PierceDrawer ??= new(PrimitiveWidthFunction, PrimitiveColorFunction, null, GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"]);

            float localIdentityOffset = Projectile.identity * 0.1372f;
            Color mainColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset) % 1f, HeavenfallLongbow.rainbowColors);
            Color secondaryColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset + 0.2f) % 1f, HeavenfallLongbow.rainbowColors);

            mainColor = Color.Lerp(Color.White, mainColor, 0.85f);
            secondaryColor = Color.Lerp(Color.White, secondaryColor, 0.85f);

            Vector2 trailOffset = Projectile.Size * 0.5f - Main.screenPosition;
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/EternityStreak"));
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseImage2("Images/Extra_189");
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseColor(mainColor);
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseSecondaryColor(secondaryColor);
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].Apply();
            PierceDrawer.Draw(Projectile.oldPos, trailOffset, 53);
            return false;
        }
    }
}
