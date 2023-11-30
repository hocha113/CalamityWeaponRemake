using CalamityMod.Particles.Metaballs;
using CalamityMod.Particles;
using CalamityMod;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using CalamityMod.Sounds;
using Terraria.Audio;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.AnnihilatingUniverseProj
{
    internal class CelestialObliterationArrow : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Item + "Ammo/VanquisherArrow";

        public PrimitiveTrail PierceAfterimageDrawer = null;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.height = 54;
            Projectile.width = 54;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 13;
            Projectile.timeLeft = 100;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Color color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat(0.3f, 0.64f));
            Lighting.AddLight(Projectile.Center, color.ToVector3());
            Projectile.velocity += (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 0.1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.numHits == 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 19; i++)
                    {
                        Vector2 center = Projectile.Center + Main.rand.NextVector2Circular(50f, 15f).RotatedBy(Projectile.rotation);
                        FusableParticleManager.GetParticleSetByType<StreamGougeParticleSet>()?.SpawnParticle(center, 30f);
                        float sizeStrength = MathHelper.Lerp(24f, 64f, CalamityUtils.Convert01To010(i / 19f));
                        center = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * MathHelper.Lerp(-40f, 90f, i / 19f);
                        FusableParticleManager.GetParticleSetByType<StreamGougeParticleSet>()?.SpawnParticle(center, sizeStrength);
                    }
                }
                if (Main.rand.NextBool(3))
                {
                    int proj = Projectile.NewProjectile(Projectile.parent(), Projectile.Center + Projectile.Center.To(target.Center) / 2, Vector2.Zero
                    , ModContent.ProjectileType<CelestialDevourer>(), Projectile.damage / 2, 0, Projectile.owner);
                    Main.projectile[proj].scale = 0.3f;
                }
            }
            if (target.type == ModContent.NPCType<SepulcherHead>() || target.type == ModContent.NPCType<SepulcherBody>() || target.type == ModContent.NPCType<SepulcherTail>())
            {
                ModNPC modNPC = target.ModNPC;
                modNPC.NPC.life = 0;
                modNPC.NPC.checkDead();
                modNPC.OnKill();
                modNPC.HitEffect(hit);
                modNPC.NPC.active = false;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 16; i++)
                {
                    Vector2 particleSpeed = Projectile.velocity * Main.rand.NextFloat(0.5f, 0.7f);
                    Vector2 pos = Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height));
                    CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                        , Main.rand.NextFloat(0.3f, 0.7f), Color.Purple, 30, 1, 1.5f, hueShift: 0.0f);
                    CWRParticleHandler.SpawnParticle(energyLeak);
                }
            }
        }

        public float PrimitiveWidthFunction(float completionRatio) => Projectile.scale * 30f;

        public Color PrimitiveColorFunction(float _) => Color.AliceBlue * Projectile.Opacity;
        
        public override bool PreDraw(ref Color lightColor)
        {
            PierceAfterimageDrawer ??= new(PrimitiveWidthFunction, PrimitiveColorFunction, null, GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"]);

            float localIdentityOffset = Projectile.identity * 0.1372f;
            Color mainColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset) % 1f, Color.Blue, Color.White, Color.BlueViolet, Color.CadetBlue, Color.DarkBlue);
            Color secondaryColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset + 0.2f) % 1f, Color.Blue, Color.White, Color.BlueViolet, Color.CadetBlue, Color.DarkBlue);

            mainColor = Color.Lerp(Color.White, mainColor, 0.85f);
            secondaryColor = Color.Lerp(Color.White, secondaryColor, 0.85f);

            Vector2 trailOffset = Projectile.Size * 0.5f - Main.screenPosition;
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/EternityStreak"));
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseImage2("Images/Extra_189");
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseColor(mainColor);
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseSecondaryColor(secondaryColor);
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].Apply();
            PierceAfterimageDrawer.Draw(Projectile.oldPos, trailOffset, 53);

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = tex.Size() * 0.5f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(tex, drawPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, origin, Projectile.scale, 0, 0f);
            return false;
        }
    }
}
