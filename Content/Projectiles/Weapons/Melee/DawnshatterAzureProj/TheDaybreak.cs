using CalamityWeaponRemake.Common;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityMod.NPCs.Yharon;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.DawnshatterAzureProj
{
    internal class TheDaybreak : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "Daybreak";
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 32;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.MaxUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.ai[0] % 100 == 0 && Projectile.ai[0] > 0) {
                Projectile.velocity *= -1;
            }
            Projectile.ai[0]++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.numHits == 0) {
                target.CWR().TheEndSunOnHitNum = 1;
                SoundEngine.PlaySound(Yharon.ShortRoarSound, target.position);
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
                Main.EntitySpriteDraw(value, Projectile.oldPos[i] + offset, null, Color.White * (i / (float)Projectile.oldPos.Length), Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
