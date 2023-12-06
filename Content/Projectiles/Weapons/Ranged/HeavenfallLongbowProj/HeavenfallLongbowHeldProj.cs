using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Ranged;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.AnnihilatingUniverseProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeavenfallLongbowProj
{
    internal class HeavenfallLongbowHeldProj : ModProjectile
    {
        public override string Texture => CWRConstant.Item_Ranged + "HeavenfallLongbow";
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<HeavenfallLongbow>();

        private Player Owners => CWRUtils.GetPlayerInstance(Projectile.owner);
        private Item heavenfall => Owners.HeldItem;
        private Vector2 toMou = Vector2.Zero;
        private ref float Time => ref Projectile.ai[0];
        private ref float Time2 => ref Projectile.ai[1];

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 116;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(toMou);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            toMou = reader.ReadVector2();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.7f, 0.5f);

            if (Owners == null)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.IsOwnedByLocalPlayer())
                SpanProj();
            StickToOwner();
            Time++;
            Time2++;
        }

        public void SpanProj()
        {
            Vector2 vr = Projectile.rotation.ToRotationVector2();
            int weaponDamage2 = Owners.GetWeaponDamage(Owners.ActiveItem());
            float weaponKnockback2 = Owners.GetWeaponKnockback(Owners.ActiveItem(), Owners.ActiveItem().knockBack);
            if (Projectile.ai[2] == 0)
            {
                if (Time > 10)
                {
                    SoundEngine.PlaySound(HeavenlyGale.FireSound, Projectile.Center);
                    Owners.PickAmmo(Owners.ActiveItem(), out _, out _, out weaponDamage2, out weaponKnockback2, out _);
                    Projectile.NewProjectile(Projectile.parent(), Projectile.Center, vr * 20, ProjectileType<InfiniteArrow>(), weaponDamage2, weaponKnockback2, Owners.whoAmI);
                    Time = 0;
                }
            }
            else
            {
                if (Time > 15)
                {
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
                    for (int i = 0; i < 5; i++)
                    {
                        Owners.PickAmmo(Owners.ActiveItem(), out _, out _, out weaponDamage2, out weaponKnockback2, out _);
                        Vector2 spanPos = Projectile.Center + new Vector2(0, -633) + new Vector2(Main.MouseWorld.X - Owners.position.X, 0) * Main.rand.NextFloat(0.3f, 0.45f);
                        Vector2 vr3 = spanPos.To(Main.MouseWorld).UnitVector().RotateRandom(12 * CWRUtils.atoR) * 23;
                        Projectile.NewProjectile(Projectile.parent(), spanPos, vr3, ProjectileType<ParadiseArrow>(), (int)(weaponDamage2 * 0.5f), weaponKnockback2, Owners.whoAmI);
                    }
                    
                    //for (int i = 0; i < 55; i++)
                    //{
                    //    Vector2 vr2 = vr.RotateRandom(15 * CWRUtils.atoR) * Main.rand.Next(21, 58);
                    //    CWRParticleHandler.SpawnParticle(new HeavenHeavySmoke(Projectile.Center, vr2, Color.White, 30
                    //        , Main.rand.NextFloat(0.6f, 1.2f) * Projectile.scale, 0.28f, 0f, glowing: false, 0f, required: true, Owners));
                    //}
                    Time = 0;
                }
            }
        }

        public void StickToOwner()
        {
            if (heavenfall.ModItem == null)
            {
                Projectile.Kill();
                return;
            }
            HeavenfallLongbow bow = (HeavenfallLongbow)heavenfall.ModItem;
            if (bow == null)
            {
                Projectile.Kill();
                return;
            }
            bow.Item.damage = 9999;

            if (Projectile.IsOwnedByLocalPlayer())
            {
                Vector2 oldToMou = toMou;
                toMou = Owners.Center.To(Main.MouseWorld);
                if (oldToMou != toMou)
                {
                    Projectile.netUpdate = true;
                }
            }
            if ((Projectile.ai[2] == 0 && Owners.PressKey()) || (Projectile.ai[2] == 1 && Owners.PressKey(false)))
            {
                Projectile.timeLeft = 2;
                Owners.itemTime = 2;
                Owners.itemAnimation = 2;
                float frontArmRotation = (MathHelper.PiOver2 - 0.31f) * -Owners.direction;
                Owners.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmRotation);
            }
            Projectile.position = Owners.RotatedRelativePoint(Owners.MountedCenter, true) - Projectile.Size / 2f + toMou.UnitVector() * 15;
            Projectile.rotation = toMou.ToRotation();
            Projectile.spriteDirection = Projectile.direction = Math.Sign(toMou.X);
            Owners.ChangeDir(Projectile.direction);
            Owners.heldProj = Projectile.whoAmI;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture + "Glow");
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue),
                Color.White,
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue),
                Projectile.scale,
                Owners.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically
                );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(Texture);
            
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue),
                lightColor,
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue),
                Projectile.scale,
                Owners.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically
                );
            return false;
        }

        public override bool? CanDamage() => false;
    }
}
