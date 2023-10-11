using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles
{
    internal class DoGDeaths : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile + "DoGDeath";

        public new string LocalizationCategory => "Projectiles.Boss";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            //Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 180;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(spanPos);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            spanPos = reader.ReadVector2();
        }

        public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }
        public Vector2 spanPos = Vector2.Zero;

        public override void OnSpawn(IEntitySource source)
        {
            spanPos = Projectile.Center;
            Projectile.netUpdate = true;
        }

        public override void AI()
        {
            if (Status == 0)
            {
                //if (BossRushEvent.BossRushActive)
                //{
                //    Projectile.velocity *= 1.25f;
                //}
                Status = 1;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;


            ThisTimeValue++;
        }

        public override bool CanHitPlayer(Player target)
        {
            if (ThisTimeValue > 210f)
            {
                //return Projectile.Opacity == 1f;
            }
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0 && ThisTimeValue > 210f && Projectile.Opacity == 1f)
            {
                //target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ThisTimeValue >= 210f)
            {
                return true;
            }
            if (Main.zenithWorld)
            {
                return false;
            }
            Texture2D laserTelegraph = DrawUtils.GetT2DValue(CWRConstant.Projectile + "LaserWallTelegraphBeam");
            float yScale = 2f;
            if (ThisTimeValue < 30f)
            {
                yScale = MathHelper.Lerp(0f, 2f, ThisTimeValue / 30f);
            }
            if (ThisTimeValue > 180f)
            {
                yScale = MathHelper.Lerp(2f, 0f, (ThisTimeValue - 180f) / 30f);
            }
            Vector2 scaleInner;
            scaleInner = new Vector2(2400f / laserTelegraph.Width, yScale);
            Vector2 origin = laserTelegraph.Size() * new Vector2(0f, 0.5f);
            Vector2 scaleOuter = scaleInner * new Vector2(1f, 1.6f);
            Color colorOuter = Color.Lerp(Color.Cyan, Color.Purple, ThisTimeValue / 210f * 2f % 1f);
            Color colorInner = Color.Lerp(colorOuter, Color.White, 0.75f);
            colorOuter *= 0.7f;
            colorInner *= 0.7f;
            float drawRot = Projectile.rotation + MathHelper.PiOver2;
            Vector2 drawPos = DrawUtils.WDEpos(Projectile.Center);
            Main.EntitySpriteDraw(laserTelegraph, drawPos, null, colorInner, drawRot, origin, scaleInner, 0);
            Main.EntitySpriteDraw(laserTelegraph, drawPos, null, colorOuter, drawRot, origin, scaleOuter, 0);
            return true;
        }
    }
}
