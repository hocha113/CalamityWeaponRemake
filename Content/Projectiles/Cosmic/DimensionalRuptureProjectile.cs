using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace CalamityWeaponRemake.Content.Projectiles.Cosmic
{
    internal class DimensionalRuptureProjectile : CustomProjectiles
    {
        public override string Texture => CWRConstant.Item + "DimensionalRupture";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {

        }

        public override int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

        public override void Kill(int timeLeft)
        {

        }

        public override void OnSpawn(IEntitySource source)
        {

        }

        public override bool ShouldUpdatePosition()
        {
            return true;
        }

        public override void AI()
        {

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
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
