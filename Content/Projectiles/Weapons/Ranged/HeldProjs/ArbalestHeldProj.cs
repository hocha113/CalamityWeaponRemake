using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.CWRUtils;
using static CalamityWeaponRemake.Common.CWRUtils;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class ArbalestHeldProj : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Arbalest";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.scale = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.hide = true;
        }

        public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int Time { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }
        public int Time2 { get => (int)Projectile.localAI[0]; set => Projectile.localAI[0] = value; }

        private Player Owner => Main.player[Projectile.owner];
        private Vector2 toMou = Vector2.Zero;
        private Item arbalest => Owner.HeldItem;

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            if (!Owner.Alives() || arbalest.type != ModContent.ItemType<Items.Ranged.Arbalest>()
                && arbalest.type != ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Arbalest>())
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Owner.Center + toMou.UnitVector() * 13;
            if (Projectile.IsOwnedByLocalPlayer())
            {
                StickToOwner();
                if (Status == 0)
                    SpanProj();
                if (Status == 1)
                    SpanProj2();
            }
            Time++;
            Time2++;
        }

        public void SpanProj()
        {
            if (Time > 20 && Time < 50 && Owner.PressKey())
            {
                if (Time2 % 10 == 0)
                {
                    SoundEngine.PlaySound(in SoundID.Item5, Owner.Center);
                    int randShootNum = Main.rand.Next(4, 6);
                    Vector2 spanPos = Owner.Center + toMou.UnitVector() * 53;
                    for (int i = 0; i < randShootNum; i++)
                    {
                        Vector2 vr = (toMou.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-5, 5))).ToRotationVector2() * Main.rand.Next(17, 27);
                        int ammo = Projectile.NewProjectile(
                            Owner.parent(),
                            spanPos,
                            vr,
                            (int)Projectile.localAI[1],
                            Projectile.damage,
                            Projectile.knockBack,
                            Projectile.owner
                            );
                        Main.projectile[ammo].MaxUpdates = 2;
                        Main.projectile[ammo].scale = 0.5f + Projectile.localAI[2] / 16;
                    }
                    Projectile.localAI[2]++;
                    if (Projectile.localAI[2] > 16)
                        Projectile.localAI[2] = 0;
                }

            }
            if (Time > 60)
                Time = 0;
        }

        public void SpanProj2()
        {
            if (Time >= 30 && Time <= 60 && Owner.PressKey(false))
            {
                if (Time2 % 5 == 0)
                {
                    Vector2 spanPos = Owner.Center + toMou.UnitVector() * 53;
                    int ammo = Projectile.NewProjectile(
                            Owner.parent(),
                            spanPos,
                            Projectile.rotation.ToRotationVector2() * 18,
                            (int)Projectile.localAI[1],
                            Projectile.damage,
                            Projectile.knockBack,
                            Projectile.owner
                            );
                    Main.projectile[ammo].MaxUpdates = 3;
                    Main.projectile[ammo].penetrate = 1;
                    Main.projectile[ammo].usesLocalNPCImmunity = true;
                    Main.projectile[ammo].localNPCHitCooldown = -1;
                    Main.projectile[ammo].rotation = Projectile.rotation + MathHelper.PiOver2;
                    Main.projectile[ammo].scale = 1.5f;
                }
            }
            if (Time > 60)
                Time = 0;
        }

        public void StickToOwner()
        {
            if (Owner.PressKey() || Owner.PressKey(false))
            {
                toMou = Owner.Center.To(Main.MouseWorld);
                Projectile.rotation = toMou.ToRotation();
                Owner.SetDummyItemTime(2);
                Projectile.timeLeft = 2;
            }
            Owner.direction = toMou.X > 0 ? 1 : -1;
            Owner.heldProj = Projectile.whoAmI;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
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
    }
}
