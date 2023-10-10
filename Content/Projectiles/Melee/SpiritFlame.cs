using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class SpiritFlame : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile + "SpiritFlame";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frameCounter = Main.rand.Next(4);
            Projectile.scale = Main.rand.NextFloat(0.2f, 0.8f);
        }

        public override void AI()
        {
            DrawUtils.ClockFrame(ref Projectile.frameCounter, 10, 3);
            if (Projectile.ai[0] == 0)
            {
                Player owner = AiBehavior.GetPlayerInstance(Projectile.owner);
                if (owner != null)
                {
                    Projectile.velocity = owner.velocity * 0.9f + new Vector2(0, -2);
                }
                else
                {
                    Projectile.velocity = new Vector2(0, -2);
                }
            }
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.timeLeft = 120;
                    Projectile.scale = 0.6f;
                    Projectile.ai[1] = 1;
                }
                Projectile.scale *= 1.01f;
                Projectile.velocity = Projectile.velocity.RotatedBy(0.03f);
                Projectile.velocity *= 0.99f;
            }
            if (Projectile.ai[0] == 2)
            {
                if (Projectile.ai[1] == 0)
                {
                    Projectile.timeLeft = 150;
                    Projectile.scale = 0.9f;
                    Projectile.ai[1] = 1;
                }
                Projectile.scale *= 1.015f;
                Projectile.velocity = Projectile.velocity.RotatedBy(0.04f);
                Projectile.velocity *= 0.995f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DrawUtils.GetT2DValue(Texture);
            float alp = Projectile.timeLeft / 30f;
            Main.EntitySpriteDraw(
                texture,
                DrawUtils.WDEpos(Projectile.Center),
                DrawUtils.GetRec(texture, Projectile.frameCounter, 4),
                Color.White * alp,
                Projectile.rotation,
                DrawUtils.GetOrig(texture, 4),
                Projectile.scale,
                SpriteEffects.None,
                0
                );
            return false;
        }
    }
}
