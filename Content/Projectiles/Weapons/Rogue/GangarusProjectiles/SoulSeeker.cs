using CalamityMod.Dusts;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue.GangarusProjectiles
{
    internal class SoulSeeker : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/SupremeCalamitas/SoulSeekerSupreme";
        private NPC Target => Main.npc[(int)Projectile.ai[0]];

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults() {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 380;
        }

        public override void AI() {
            CWRUtils.ClockFrame(ref Projectile.frame, 5, 5);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.timeLeft > 250) {
                Projectile.velocity *= 0.98f;
            }
            else {
                if (!Target.Alives()) {
                    Projectile.Kill();
                    return;
                }
                if (Projectile.timeLeft == 110) {
                    Projectile.velocity = Projectile.velocity.UnitVector() * 23;
                }
                //Projectile.ChasingBehavior2(Target.Center, 1.001f, 0.15f);
                if (Projectile.Center.To(Target.Center).LengthSquared() > 80000) {
                    Projectile.ChasingBehavior(Target.Center, 23);
                }
                else {
                    Projectile.ChasingBehavior2(Target.Center, 1.001f, 0.15f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft) {
            for (int i = 0; i < 5; i++) {
                int brimDust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                Main.dust[brimDust].velocity *= 3f;
                if (Main.rand.NextBool()) {
                    Main.dust[brimDust].scale = 0.5f;
                    Main.dust[brimDust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 10; j++) {
                int brimDust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                Main.dust[brimDust2].noGravity = true;
                Main.dust[brimDust2].velocity *= 5f;
                brimDust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                Main.dust[brimDust2].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, CWRUtils.GetRec(value, Projectile.frame, 6)
                , Color.White, Projectile.rotation, CWRUtils.GetOrig(value, 6), Projectile.scale, Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                Main.EntitySpriteDraw(value, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, CWRUtils.GetRec(value, Projectile.frame, 6)
                , Color.White * ((6 - i) / 16f), Projectile.rotation, CWRUtils.GetOrig(value, 6), Projectile.scale, Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }
            return false;
        }
    }
}
