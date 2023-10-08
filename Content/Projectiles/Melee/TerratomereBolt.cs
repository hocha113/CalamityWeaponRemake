using CalamityMod.Particles;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityMod.Projectiles.Melee;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class TerratomereBolt : ModProjectile
    {
        internal PrimitiveTrail TrailDrawer;

        public NPC target;

        private Particle Head;

        public new string LocalizationCategory => "Projectiles.Melee";

        public override string Texture => CWRConstant.Projectile_Melee + "TerratomereBolt";

        public Player Owner => Main.player[Projectile.owner];

        public ref float Hue => ref Projectile.ai[0];

        public ref float HomingStrenght => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = (Projectile.height = 30);
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 160;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 1.003f;
            NPC target = Projectile.Center.InPosClosestNPC(1600);
            if (target != null && Projectile.timeLeft < 130)
            {
                Vector2 toTarget = Projectile.Center.To(target.Center);
                Projectile.EntityToRot(toTarget.ToRotation(), 0.15f);
                Projectile.velocity = Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.IsOwnedByLocalPlayer())
            {
                int maxNum = Main.rand.Next(3, 5);
                for (int i = 0; i < maxNum; i++)
                {
                    Vector2 offsetVr = HcMath.GetRandomVevtor(0, 360, Main.rand.Next(360, 420));
                    Vector2 spanPos = target.Center + offsetVr;
                    Vector2 vr = offsetVr.UnitVector() * -30;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spanPos, vr,
                    ModContent.ProjectileType<TerratomereBigSlash>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        internal Color ColorFunction(float completionRatio)
        {
            float fadeToEnd = MathHelper.Lerp(0.65f, 1f, (float)Math.Cos((0f - Main.GlobalTimeWrappedHourly) * 3f) * 0.5f + 0.5f);
            float fadeOpacity = Utils.GetLerpValue(1f, 0.64f, completionRatio, clamped: true) * Projectile.Opacity;
            Color endColor = Color.Lerp(Main.hslToRgb(Hue, 1f, 0.8f), Color.PaleTurquoise, (float)Math.Sin(completionRatio * (float)Math.PI * 1.6f - Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
            return Color.Lerp(Color.White, endColor, fadeToEnd) * fadeOpacity;
        }

        internal float WidthFunction(float completionRatio)
        {
            float expansionCompletion = (float)Math.Pow(1f - completionRatio, 3.0);
            return MathHelper.Lerp(0f, 22f * Projectile.scale * Projectile.Opacity, expansionCompletion);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new PrimitiveTrail(WidthFunction, ColorFunction, null, GameShaders.Misc["CalamityMod:TrailStreak"]);
            }
            GameShaders.Misc["CalamityMod:TrailStreak"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ScarletDevilStreak", (AssetRequestMode)2));
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 30);
            Texture2D texture = ModContent.Request<Texture2D>(Texture, (AssetRequestMode)2).Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, 0.5f), Projectile.rotation + (float)Math.PI / 2f, texture.Size() / 2f, Projectile.scale, (SpriteEffects)0);
            return false;
        }
    }
}
