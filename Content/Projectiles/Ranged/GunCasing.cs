using CalamityWeaponRemake.Common;
using static CalamityWeaponRemake.Common.DrawTools.DrawUtils;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace CalamityWeaponRemake.Content.Projectiles.Ranged
{
    internal class GunCasing : CustomProjectiles
    {
        public override string Texture => CWRConstant.Projectile + "GunCasing";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.damage = 10;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
            Projectile.scale = 1.2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
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
            ThisTimeValue++;
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 13);
            Projectile.velocity += new Vector2(0, 0.1f);

            if (ThisTimeValue % 13 == 0) Dust.NewDust(Projectile.Center, 3, 3, DustID.Smoke, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = GetT2DValue(Texture);

            Main.EntitySpriteDraw(
                mainValue,
                WDEpos(Projectile.Center),
                null,
                Color.White,
                Projectile.rotation,
                GetOrig(mainValue),
                Projectile.scale,
                SpriteEffects.None,
                0
                );

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {

        }
    }
}
