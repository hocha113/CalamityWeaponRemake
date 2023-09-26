using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using CalamityWeaponRemake.Common.Interfaces;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;

namespace CalamityWeaponRemake.Content.Projectiles
{
    internal class DragonFireRupture : CustomProjectiles
    {
        public override string Texture => CWRConstant.Projectile + "FireCrossburst";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.damage = 100;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

        bool upPos = false;
        public override bool ShouldUpdatePosition()
        {
            return upPos;
        }

        public override void Kill(int timeLeft)
        {

        }

        List<Vector2> randomOffsetVr = new List<Vector2>();
        public override void OnSpawn(IEntitySource source)
        {
            Behavior++;
            Projectile.frameCounter += Main.rand.Next(6);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.localAI[2] = Projectile.velocity.Length();

            for (int i = 0; i < 6; i++)
            {
                randomOffsetVr.Add(HcMath.GetRandomVevtor(0, 360, Main.rand.NextFloat(16, 80)));
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 spanPos = randomOffsetVr[i] + Projectile.Center;
                ncbs.Add(new ncb(spanPos, Main.rand.Next(6)));
            }
        }

        public override void AI()
        {
            ThisTimeValue++;
            NPC target = Projectile.Center.InPosClosestNPC(360);   
            
            if (target != null)
            {
                Vector2 toTarget = Projectile.Center.To(target.Center);
                AiBehavior.EntityToRot(Projectile, toTarget.ToRotation(), 0.1f);
            }

            if (Status == 0)
            {
                if (ThisTimeValue > 5)
                {
                    if (Behavior < 12)
                    {
                        Vector2 spanPos = Projectile.Center + Projectile.rotation.ToRotationVector2() * 160;
                        Projectile.NewProjectile(
                            AiBehavior.GetEntitySource_Parent(Projectile),
                            spanPos,
                            Projectile.velocity,
                            Type,
                            Projectile.damage,
                            Projectile.knockBack,
                            -1,
                            ai1: Behavior
                            );
                    }
                    Status = 1;
                }

            }
            if (Status == 1)
            {
                upPos = true;
                Projectile.velocity = Projectile.rotation.ToRotationVector2() * Projectile.localAI[2];
                Projectile.localAI[2] *= 0.98f;
            }

            Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * 3);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return AiBehavior.CircularHitboxCollision(Projectile.Center, 72, targetHitbox);
        }

        int dorFireType => ModContent.BuffType<Dragonfire>();
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.timeLeft -= 10;
            Projectile.localNPCHitCooldown += 10;
            target.AddBuff(dorFireType, 180);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.timeLeft -= 15;
            Projectile.localNPCHitCooldown += 30;
            target.AddBuff(dorFireType, 60);
            base.OnHitPlayer(target, info);
        }

        struct ncb
        {
            public Vector2 pos;
            public int frame;

            public ncb(Vector2 overPos, int overFrame)
            {
                pos = overPos;
                frame = overFrame;
            }
        }

        List<ncb> ncbs = new List<ncb>();

        public override void PostDraw(Color lightColor)
        {
            DrawUtils.ClockFrame(ref Projectile.frameCounter, 4, 6);
            for (int i = 0; i < 6; i++)
            {
                ncb _ncb = ncbs[i];
                DrawUtils.ClockFrame(ref _ncb.frame, 4, 6);
                ncbs[i] = _ncb;
            }
            for (int i = 0; i < 6; i++)
            {
                ncb _ncb = ncbs[i];
                _ncb.pos = Projectile.Center + randomOffsetVr[i];
                ncbs[i] = _ncb;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = DrawUtils.GetT2DValue(Texture);
            float slp = Projectile.timeLeft / 60f;
            if (slp > 1) slp = 1;

            Main.EntitySpriteDraw(
                mainValue,
                DrawUtils.WDEpos(Projectile.Center),
                DrawUtils.GetRec(mainValue, Projectile.frameCounter, 7),
                Color.White,
                Projectile.rotation,
                DrawUtils.GetOrig(mainValue, Projectile.frameCounter, 7),
                Projectile.scale * slp,
                SpriteEffects.None,
                0
                );

            for (int j = 0; j < 6; j++)
            {
                ncb _ncb = ncbs[j];

                Main.EntitySpriteDraw(
                mainValue,
                DrawUtils.WDEpos(_ncb.pos),
                DrawUtils.GetRec(mainValue, _ncb.frame, 7),
                Color.White,
                Projectile.rotation,
                DrawUtils.GetOrig(mainValue, _ncb.frame, 7),
                Projectile.scale * slp,
                SpriteEffects.None,
                0
                );
            }

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {

        }
    }
}
