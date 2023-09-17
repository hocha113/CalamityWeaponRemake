using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Cosmic
{
    internal class StarDust : CustomProjectiles
    {
        public override string Texture => CWRConstant.placeholder;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 220;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

        public override void Kill(int timeLeft)
        {
            Lighting.AddLight(Projectile.Center, new Vector3(92, 58, 156));
        }

        public override void OnSpawn(IEntitySource source)
        {
            Lighting.AddLight(Projectile.Center, new Vector3(92, 58, 156));
        }

        public override bool ShouldUpdatePosition()
        {
            return true;
        }

        public override void AI()
        {
            Projectile target = AiBehavior.GetProjectileInstance(Behavior);
            if (AiBehavior.ProjectileAlive(target) == false)
            {
                Projectile.Kill();
                return;
            }
            AiBehavior.ChasingBehavior(Projectile, target.Center, 23, 16);
            if (Projectile.Center.To(target.Center).LengthSquared() < 16 * 16) Projectile.Kill();

            for (int i = 0; i < 3; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, Projectile.direction * 2, 0f, 115, Color.White, 1.3f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 0f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {

        }
    }
}
