using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Ranged;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Particles.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
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

        public const int MaxVientNum = 13;

        Color chromaColor => CWRUtils.MultiLerpColor(Projectile.ai[0] % 15 / 15f, HeavenfallLongbow.rainbowColors);
        private Player Owners => CWRUtils.GetPlayerInstance(Projectile.owner);
        private Item heavenfall => Owners.HeldItem;
        private Vector2 toMou = Vector2.Zero;
        private int ChargeValue;
        private int oldChargeValue;
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
            if (Projectile.IsOwnedByLocalPlayer() && Owners.ownedProjectileCounts[ProjectileType<VientianePunishment>()] == 0)
                SpanProj();
            StickToOwner();

            if (ChargeValue > 200)
                ChargeValue = 200;

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
                    oldChargeValue = ChargeValue;
                    ChargeValue += 5;
                    SpanInfiniteRune();
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
                    oldChargeValue = ChargeValue;
                    ChargeValue += 3;
                    SpanInfiniteRune();
                    Time = 0;
                }
            }

            if (ChargeValue >= 200 && CWRKeySystem.HeavenfallLongbowSkillKey.JustPressed)
            {
                int types = ProjectileType<VientianePunishment>();
                if (Owners.ownedProjectileCounts[types] < MaxVientNum)
                {
                    int randomOffset = Main.rand.Next(MaxVientNum);
                    int frmer = 0;
                    for (int i = 0; i < MaxVientNum; i++)
                    {
                        int proj = Projectile.NewProjectile(Projectile.parent(), Owners.Center, Vector2.Zero, types, (int)(weaponDamage2 * 0.5f), weaponKnockback2, Owners.whoAmI, i + randomOffset);
                        if (i == 0)
                            frmer = proj;
                        VientianePunishment vientianePunishment = Main.projectile[proj].ModProjectile as VientianePunishment;
                        if (vientianePunishment != null)
                        {
                            vientianePunishment.Index = i;
                            vientianePunishment.FemerProjIndex = frmer;
                            vientianePunishment.Projectile.netUpdate = true;
                            vientianePunishment.Projectile.netUpdate2 = true;
                        }
                    }
                }
                ChargeValue = 0;
            }
        }

        public void SpanInfiniteRune()
        {
            if (oldChargeValue < 200 && ChargeValue >= 200)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.PlasmaBoltSound, Projectile.Center);
                float rot = 0;
                for (int j = 0; j < 500; j++)
                {
                    rot += MathHelper.TwoPi / 500f;
                    float scale = 2f / (3f - (float)Math.Cos(2 * rot)) * 25;
                    float outwardMultiplier = MathHelper.Lerp(4f, 220f, Utils.GetLerpValue(0f, 120f, Time, true));
                    Vector2 lemniscateOffset = scale * new Vector2((float)Math.Cos(rot), (float)Math.Sin(2f * rot) / 2f);
                    Vector2 pos = Owners.Center + lemniscateOffset * outwardMultiplier;
                    Vector2 particleSpeed = Vector2.Zero;
                    Color color = CWRUtils.MultiLerpColor(j / 500f, HeavenfallLongbow.rainbowColors);
                    CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                        , 1.5f, color, 120, 1, 1.5f, hueShift: 0.0f, _entity: Owners, _followingRateRatio: 1);
                    CWRParticleHandler.SpawnParticle(energyLeak);
                }
                Projectile.NewProjectile(Projectile.parent(), Owners.Center, Vector2.Zero, ProjectileType<InfiniteRune>(), 99999, 0, Owners.whoAmI);
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
            Color drawColor2 = CWRUtils.MultiLerpColor(Projectile.ai[0] % 15 / 15f, HeavenfallLongbow.rainbowColors);
            if (ChargeValue < 200)
                drawColor2 = CWRUtils.MultiLerpColor(ChargeValue / 200f, HeavenfallLongbow.rainbowColors);
            float slp2 = ChargeValue / 300f;
            if (slp2 > 1)
                slp2 = 1;
            if (slp2 < 0.1f)
                slp2 = 0;

            Main.spriteBatch.SetAdditiveState();
            for(int i = 0; i < 8; i++)
            Main.EntitySpriteDraw(
                mainValue,
                Projectile.Center - Main.screenPosition,
                CWRUtils.GetRec(mainValue),
                drawColor2,
                Projectile.rotation,
                CWRUtils.GetOrig(mainValue),
                Projectile.scale * (1 + slp2 * 0.08f),
                Owners.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically
                );
            Main.spriteBatch.ResetBlendState();

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
