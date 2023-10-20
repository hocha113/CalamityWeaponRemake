using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Projectiles.Ranged.HeldProjs;

namespace CalamityWeaponRemake.Content.Projectiles.Ranged
{
    internal class DeathLaser : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile + "RayBeam";
        private Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 1;
            Projectile.alpha = 80;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public int HeldProj { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int Time { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }
        private ref float wit => ref Projectile.localAI[0];

        public override void AI()
        {
            Projectile heldBow = AiBehavior.GetProjectileInstance(HeldProj);
            if (!AiBehavior.PlayerAlive(Owner) || !(heldBow != null && heldBow.type == ModContent.ProjectileType<DeathwindHeldProj>()))
            { 
                Projectile.Kill();
                return;
            }
            Projectile.Center = heldBow.Center;
            Projectile.alpha += 15;

            Vector2 toRot = Projectile.rotation.ToRotationVector2();
            Vector2 ordPos = Projectile.Center;

            wit = (10 - Projectile.timeLeft) / 15f;

            for (int i = 0; i < 100; i++)
            {
                Vector2 offsetPos = toRot * i * 16;
                Lighting.AddLight(ordPos + offsetPos, 0.4f, 0.2f, 0.4f);
                Dust obj = Main.dust[Dust.NewDust(Projectile.position + offsetPos, 26, 26
                        , Main.rand.NextBool(3) ? 56 : 242, Projectile.velocity.X, Projectile.velocity.Y, 100)];
                obj.velocity = Vector2.Zero;
                obj.position -= Projectile.velocity / 5f * i;
                obj.noGravity = true;
                obj.scale = 0.8f;
                obj.noLight = true;
            }

            Time++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(
                        targetHitbox.TopLeft(),
                        targetHitbox.Size(),
                        Projectile.Center,
                        Projectile.rotation.ToRotationVector2() * 5000 + Projectile.Center,
                        8,
                        ref point
                    );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D body = DrawUtils.GetT2DValue(Texture + "Body");
            Texture2D head = DrawUtils.GetT2DValue(Texture + "Head");
            Texture2D dons = DrawUtils.GetT2DValue(Texture + "Don");
            Color color = new Color(92, 58, 156, Projectile.alpha);

            float rots = Projectile.rotation - MathHelper.PiOver2;
            Vector2 slp = new Vector2(wit, 1);
            Vector2 dir = Projectile.rotation.ToRotationVector2();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap
                , DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.EntitySpriteDraw(
                dons,
                Projectile.Center - Main.screenPosition,
                null,
                color,
                rots,
                new Vector2(dons.Width * 0.5f, 0),
                slp,
                SpriteEffects.None,
                0
                );

            Main.EntitySpriteDraw(
                body,
                Projectile.Center - Main.screenPosition + dir * dons.Height,
                new Rectangle(0, Time * -5, body.Width, 5000 + 1),
                color,
                rots,
                new Vector2(body.Width * 0.5f, 0),
                slp,
                SpriteEffects.None,
                0
                );

            Main.EntitySpriteDraw(
                head,
                Projectile.Center + dir * 5000 - Main.screenPosition + dir * dons.Height,
                null,
                color,
                rots,
                new Vector2(body.Width * 0.5f, 0),
                slp,
                SpriteEffects.None,
                0
                );
            Main.spriteBatch.ResetBlendState();
            return false;
        }
    }
}
