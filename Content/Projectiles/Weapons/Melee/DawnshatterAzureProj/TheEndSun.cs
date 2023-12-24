using CalamityMod.NPCs.Yharon;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.DawnshatterAzureProj
{
    internal class TheEndSun : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "TheEndSun";
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 6;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;           
        }

        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            SpanDust();
        }

        public void SpanDust() {
            for (int i = 0; i < 1; i++) {
                if (Main.rand.NextBool()) {
                    Vector2 vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center - vector3 * 30f, 0, 0, DustID.InfernoFork)];
                    dust.noGravity = true;
                    dust.position = Projectile.Center - vector3 * Main.rand.Next(10, 21);
                    dust.velocity = vector3.RotatedBy(1.5707963705062866) * 6f;
                    dust.scale = 0.9f + Main.rand.NextFloat();
                    dust.fadeIn = 0.5f;
                    dust.customData = Projectile;
                    vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    dust.noGravity = true;
                    dust.position = Projectile.Center - vector3 * Main.rand.Next(10, 21);
                    dust.velocity = vector3.RotatedBy(1.5707963705062866) * 6f;
                    dust.scale = 0.9f + Main.rand.NextFloat();
                    dust.fadeIn = 0.5f;
                    dust.customData = Projectile;
                    dust.color = Color.Crimson;
                }
                else {
                    Vector2 vector4 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center - vector4 * 30f, 0, 0, DustID.Flare)];
                    dust.noGravity = true;
                    dust.position = Projectile.Center - vector4 * Main.rand.Next(20, 31);
                    dust.velocity = vector4.RotatedBy(-1.5707963705062866) * 5f;
                    dust.scale = 0.9f + Main.rand.NextFloat();
                    dust.fadeIn = 0.5f;
                    dust.customData = Projectile;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.numHits == 0) {
                target.CWR().TheEndSunOnHitNum = 1;
                SoundEngine.PlaySound(Yharon.ShortRoarSound, target.position);
            }
            if (Projectile.numHits < 3) {
                for (int i = 0; i < 3; i++) {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(Main.rand.Next(-260, 260), -520 + Main.rand.Next(-160, 160)), new Vector2(0, 26)
                    , ModContent.ProjectileType<TheDaybreak>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override void OnKill(int timeLeft) {
            base.OnKill(timeLeft);
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = TextureAssets.Projectile[Type].Value;
            Vector2 orig = value.Size() / 2;
            Vector2 offset = Projectile.Size / 2f - Main.screenPosition;
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                Main.EntitySpriteDraw(value, Projectile.oldPos[i] + offset, null, Color.White * (i / (float)Projectile.oldPos.Length), Projectile.rotation, orig, Projectile.scale, 0, 0);
            }
            return false;
        }
    }
}
