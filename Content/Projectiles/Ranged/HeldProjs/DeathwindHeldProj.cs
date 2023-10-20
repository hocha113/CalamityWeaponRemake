using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.AuxiliaryMeans.AiBehavior;
using static CalamityWeaponRemake.Common.DrawTools.DrawUtils;

namespace CalamityWeaponRemake.Content.Projectiles.Ranged.HeldProjs
{
    internal class DeathwindHeldProj : ModProjectile
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Deathwind";

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
        public int useArrow { get => (int)Projectile.localAI[1]; set => Projectile.localAI[1] = value; }

        private Player Owner => Main.player[Projectile.owner];
        private Vector2 toMou = Vector2.Zero;
        private Item deathwind => Owner.HeldItem;

        public override void OnSpawn(IEntitySource source)
        {
            Time2 = 10;
            useArrow = -1000;
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
            if (!PlayerAlive(Owner) || deathwind.type != ModContent.ItemType<Items.Ranged.Deathwind>()
                && deathwind.type != ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>())
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

        public void SpanProj()
        {
            if (Time % deathwind.useTime == 0 && useArrow != -1000)
            {               
                if (CalamityUtils.CheckWoodenAmmo(useArrow, Owner))
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                    for (int i = 0; i < 3; i++)
                    {
                        int ammo = Projectile.NewProjectile(
                                GetEntitySource_Parent(Owner),
                                Projectile.Center,
                                Vector2.Zero,
                                ModContent.ProjectileType<DeathLaser>(),
                                Projectile.damage,
                                Projectile.knockBack,
                                Projectile.owner
                                );
                        Main.projectile[ammo].ai[1] = Projectile.whoAmI;
                        Main.projectile[ammo].rotation = Projectile.rotation + MathHelper.ToRadians(5 - 5 * i);
                    }
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
                    for (int i = 0; i < 3; i++)
                    {
                        int ammo = Projectile.NewProjectile(
                                GetEntitySource_Parent(Owner),
                                Projectile.Center,
                                (Projectile.rotation + MathHelper.ToRadians(5 - 5 * i)).ToRotationVector2() * 17,
                                useArrow,
                                Projectile.damage,
                                Projectile.knockBack,
                                Projectile.owner
                                );
                        Main.projectile[ammo].MaxUpdates = 2;
                    }
                    Time2++;
                }                
            }

            if (Time2 > 3 && Time % 5 == 0 && useArrow != -1000 && !CalamityUtils.CheckWoodenAmmo(useArrow, Owner))
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 vr = (Projectile.rotation + MathHelper.ToRadians(5 - 5 * i)).ToRotationVector2();
                    int ammo = Projectile.NewProjectile(
                            GetEntitySource_Parent(Owner),
                            Projectile.Center + vr * 150,
                            vr * 15,
                            ModContent.ProjectileType<DeadArrow>(),
                            Projectile.damage,
                            Projectile.knockBack,
                            Projectile.owner
                            );
                    Main.projectile[ammo].scale = 1.5f;
                }
                Time2++;
                if (Time2 > 8)
                    Time2 = 0;
            }
        }

        public void StickToOwner()
        {
            if (Owner.PressKey())
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
