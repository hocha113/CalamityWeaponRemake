﻿using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.AuxiliaryMeans.AiBehavior;
using static CalamityWeaponRemake.Common.DrawTools.DrawUtils;
using CalamityMod.Projectiles.Ranged;
using Mono.Cecil;
using System;
using CalamityWeaponRemake.Content.Projectiles.Melee;

namespace CalamityWeaponRemake.Content.Projectiles.Ranged.HeldProjs
{
    internal class GaleforceHeldProj : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Galeforce";

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
        public int useArrow { get => (int)Projectile.localAI[1]; set => Projectile.localAI[1] = value; }

        private Player Owner => Main.player[Projectile.owner];
        private Vector2 toMou = Vector2.Zero;
        private Item galeforce => Owner.HeldItem;

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            if (!Owner.Alives() || galeforce.type != ModContent.ItemType<Items.Ranged.Galeforce>()
                && galeforce.type != ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Galeforce>())
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Owner.Center + toMou.UnitVector() * 13;
            if (Projectile.IsOwnedByLocalPlayer())
            {
                StickToOwner();                
                SpanProj();
            }
            Time++;
        }

        public void StickToOwner()
        {
            if (Owner.PressKey() || Owner.PressKey(false))
            {
                toMou = Owner.Center.To(Main.MouseWorld);
                Projectile.rotation = toMou.ToRotation();
                Owner.SetDummyItemTime(2);
                float armRot = Projectile.rotation - Projectile.direction * MathHelper.PiOver2;
                Owner.SetCompositeArmFront(Math.Abs(armRot) > 0.01f, Player.CompositeArmStretchAmount.Full, armRot);
                Projectile.timeLeft = 2;
            }
            Owner.direction = toMou.X > 0 ? 1 : -1;
            Owner.heldProj = Projectile.whoAmI;
        }

        public void SpanProj()
        {
            Vector2 vr = (Projectile.rotation).ToRotationVector2() * galeforce.shootSpeed;
            Projectile arrow = GetProjectileInstance(useArrow);
            int offsetDamages = arrow == null ? 0 : arrow.damage;

            if (Owner.altFunctionUse == 2)
            {
                if (Time % 5 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
                    vr *= 0.5f;
                    int ammo = Projectile.NewProjectile(
                                GetEntitySource_Parent(Owner),
                                Projectile.Center,
                                vr,
                                ModContent.ProjectileType<FeatherLarge>(),
                                Projectile.damage / 2,
                                Projectile.knockBack,
                                Projectile.owner
                                );
                }
            }
            else
            {
                if (Time % galeforce.useTime == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
                    int ammo = Projectile.NewProjectile(
                                GetEntitySource_Parent(Owner),
                                Projectile.Center,
                                vr,
                                useArrow,
                                galeforce.damage + offsetDamages,
                                Projectile.knockBack,
                                Projectile.owner
                                );

                    for (int i = -8; i <= 8; i += 8)
                    {
                        Vector2 velocity2 = vr.RotatedBy(MathHelper.ToRadians(i));
                        Projectile.NewProjectile(GetEntitySource_Parent(Owner)
                            , Projectile.Center, velocity2, ModContent.ProjectileType<FeatherLarge>()
                            , (galeforce.damage + offsetDamages) / 4, 0f, Owner.whoAmI);
                    }                   
                }
            }
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
