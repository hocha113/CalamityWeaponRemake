using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.Interfaces;
using CalamityWeaponRemake.Common.SoundEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.AuxiliaryMeans.AiBehavior;
using static CalamityWeaponRemake.Common.DrawTools.DrawUtils;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace CalamityWeaponRemake.Content.Projectiles
{
    internal class DragonsBreathProjectiles : CustomProjectiles
    {
        public override string Texture => CWRConstant.Item + "DragonsBreath";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 1;
            Projectile.damage = 588;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
        }

        public override int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public override int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public override int ThisTimeValue { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

        public override void Kill(int timeLeft)
        {
            if (Owner != null) Projectile.rotation = toMou.ToRotation();
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Owner != null) Projectile.rotation = toMou.ToRotation();           
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        Player Owner => GetPlayerInstance(Projectile.owner);
        Vector2 toMou => Owner.Center.To(Main.MouseWorld);
        public override void AI()
        {
            ThisTimeValue++;
            Projectile.localAI[0]++;
            if (Owner == null)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                if (Status == 0)
                {
                    if (PlayerInput.Triggers.Current.MouseLeft) Projectile.timeLeft = 2;
                    else Projectile.Kill();
                }
                if (Status == 1)
                {
                    if (PlayerInput.Triggers.Current.MouseRight) Projectile.timeLeft = 2;
                    else Projectile.Kill();
                }
            }

            Owner.direction = toMou.X > 0 ? 1 : -1;
            Projectile.EntityToRot(toMou.ToRotation(), 0.2f);
            Vector2 rotOffset = Projectile.rotation.ToRotationVector2() * 6f;
            Projectile.Center = Owner.Center + rotOffset;

            Vector2 speed = Projectile.rotation.ToRotationVector2() * 12f;
            Vector2 offset = rotOffset;
            Vector2 shootPos = Owner.Center + offset * 13 + offset.GetNormalVector() * 16 * Owner.direction;

            if (Status == 0)
            {
                if (ThisTimeValue % 60 == 0)
                {
                    Projectile.localAI[2] = 0;                    
                }

                if (ThisTimeValue % 30 == 0 && ThisTimeValue % 60 != 0)
                {
                    SoundEngine.PlaySound(ModSound.loadTheRounds, Projectile.Center);
                }

                if (Projectile.localAI[0] % 6 == 0 && Projectile.localAI[2] < 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 vr = (Projectile.rotation - MathHelper.ToRadians(Main.rand.NextFloat(80, 100)) * Owner.direction).ToRotationVector2() * Main.rand.NextFloat(3, 7) + Owner.velocity;
                        Projectile.NewProjectile(GetEntitySource_Parent(Projectile), Projectile.Center, vr, ModContent.ProjectileType<GunCasing>(), 10, Projectile.knockBack, Owner.whoAmI);
                    }

                    ShootFire(shootPos);
                    SpawnDragonsBreathDust(shootPos, speed);
                    Projectile.rotation += MathHelper.ToRadians(-15) * Owner.direction;
                    Owner.velocity += speed * -0.2f;
                    SoundEngine.PlaySound(in SoundID.Item38, shootPos);
                    Projectile.localAI[2]++;
                }
            }
            if (Status == 1)
            {
                if (ThisTimeValue % 30 == 0)
                {
                    SpawnDragonsBreathDust(shootPos, speed);                   
                    shootPos = Owner.Center + offset * 33 + offset.GetNormalVector() * 16 * Owner.direction;
                    SpawnSomgDust(shootPos, speed);
                    ShootFire2(shootPos);                   
                    SoundEngine.PlaySound(in SoundID.Item74, shootPos);
                }
            }
            
        }

        int fireType => ModContent.ProjectileType<DragonsBreathRound>();
        int fireCross => ModContent.ProjectileType<DragonFireRupture>();
        public void ShootFire(Vector2 shootPos)
        {
            if (Main.myPlayer != Projectile.owner) return;

            for (int i = 0; i < 6; i++)
            {
                float angleOffset = MathHelper.ToRadians(-3 + i);
                Vector2 rotatedVel = (Projectile.rotation + angleOffset).ToRotationVector2() * 13f;
                Projectile.NewProjectile(GetEntitySource_Parent(Owner), shootPos, rotatedVel, (int)Projectile.localAI[1], Projectile.damage, Projectile.knockBack, Owner.whoAmI);
                Projectile.NewProjectile(GetEntitySource_Parent(Projectile), shootPos, rotatedVel, fireType, (int)(Projectile.damage * 1.2f), Projectile.knockBack, Owner.whoAmI);               
            }
        }

        public void ShootFire2(Vector2 shootPos)
        {
            if (Main.myPlayer != Projectile.owner) return;

            for (int i = 0; i < 3; i++)
            {
                float angleOffset = MathHelper.ToRadians(-6 + i * 6);
                Vector2 rotatedVel = (Projectile.rotation + angleOffset).ToRotationVector2() * 13f;
                Projectile.NewProjectile(GetEntitySource_Parent(Projectile), shootPos, rotatedVel, fireCross, (int)(Projectile.damage * 0.62f), Projectile.knockBack, Owner.whoAmI);
            }
        }

        private void SpawnDragonsBreathDust(Vector2 pos, Vector2 velocity, int splNum = 1)
        {
            if (Main.myPlayer != Projectile.owner) return;

            pos += velocity.SafeNormalize(Vector2.Zero) * Projectile.width * Projectile.scale * 0.71f;
            for (int i = 0; i < 30 * splNum; i++)
            {
                int dustID;
                switch (Main.rand.Next(6))
                {
                    case 0:
                        dustID = 262;
                        break;
                    case 1:
                    case 2:
                        dustID = 54;
                        break;
                    default:
                        dustID = 53;
                        break;
                }
                float num = Main.rand.NextFloat(3f, 13f) * splNum;
                float angleRandom = 0.06f;
                Vector2 dustVel = Utils.RotatedBy(new Vector2(num, 0f), (double)velocity.ToRotation(), default(Vector2));
                dustVel = dustVel.RotatedBy(0f - angleRandom);
                dustVel = dustVel.RotatedByRandom(2f * angleRandom);
                if (Main.rand.NextBool(4))
                {
                    dustVel = Vector2.Lerp(dustVel, -Vector2.UnitY * dustVel.Length(), Main.rand.NextFloat(0.6f, 0.85f)) * 0.9f;
                }
                float scale = Main.rand.NextFloat(0.5f, 1.5f);
                int idx = Dust.NewDust(pos, 1, 1, dustID, dustVel.X, dustVel.Y, 0, default, scale);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].position = pos;
            }
        }

        private void SpawnSomgDust(Vector2 pos, Vector2 velocity)
        {
            if (Main.myPlayer != Projectile.owner) return;

            Dust.NewDust(pos, 16, 16, DustID.Smoke);

            for (int i = 0; i < 66; i++)
            {
                Vector2 vr = (velocity.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))).ToRotationVector2() * Main.rand.Next(3, 16);
                Dust.NewDust(pos, 3, 3, DustID.Smoke, vr.X, vr.Y, 15);
                int dust = Dust.NewDust(pos, 3, 3, DustID.AmberBolt, vr.X, vr.Y, 15);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Owner == null) return false;

            SpriteEffects spriteEffects = toMou.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            float drawRot = Projectile.rotation;

            Texture2D mainValue = GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                mainValue,
                WDEpos(Projectile.Center),
                null,
                Color.White,
                drawRot,
                new Vector2(13, mainValue.Height * 0.5f),
                Projectile.scale,
                spriteEffects,
                0
                );
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
