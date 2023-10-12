using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Melee.ArkoftheCosmosProj
{
    internal class SlaughterExplosion : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "SlaughterExplosion";

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 14;
            Projectile.scale = 0.7f;
        }

        public override void AI()
        {
            DrawUtils.ClockFrame(ref Projectile.frameCounter, 10, 5);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = DrawUtils.GetT2DValue(Texture);
            Color color = Color.White;
            if (Projectile.ai[0] == 1) color = Color.Red;
            Main.EntitySpriteDraw(
                mainValue,
                DrawUtils.WDEpos(Projectile.Center),
                DrawUtils.GetRec(mainValue, Projectile.frameCounter, 6),
                color,
                Projectile.rotation,
                DrawUtils.GetOrig(mainValue, 6),
                Projectile.scale,
                SpriteEffects.None,
                0
                );
            return false;
        }
    }
}
