using CalamityMod;
using Terraria.Localization;
using Terraria;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityWeaponRemake.Content.Items.Melee;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using static Humanizer.In;
using Terraria.GameInput;
using CalamityWeaponRemake.Common.DrawTools;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Sounds;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles
{
    internal class RemakeGildedProboscisProj : BaseSpearProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<GildedProboscis>();

        public override float InitialSpeed => 3f;

        public override float ReelbackSpeed => 2.4f;

        public override float ForwardSpeed => 0.95f;

        public override string Texture => CWRConstant.Projectile_Melee + "GildedProboscisProj";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
        }

        Player owner => AiBehavior.GetPlayerInstance(Projectile.owner);
        int projIndex = -1;
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                base.AI();
                if (owner != null)
                {
                    if (owner.itemAnimation == owner.itemAnimationMax / 2 && Projectile.IsOwnedByLocalPlayer())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 vr = Projectile.velocity.UnitVector().RotatedBy(MathHelper.ToRadians(-10 + 10 * i)) * 25f;
                            Projectile.NewProjectile(
                                AiBehavior.GetEntitySource_Parent(Projectile),
                                Projectile.Center,
                                vr,
                                ModContent.ProjectileType<RedLightningFeather>(),
                                Projectile.damage / 2,
                                Projectile.knockBack,
                                Main.myPlayer
                                );
                        }
                    }
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (PlayerInput.Triggers.Current.MouseRight) Projectile.timeLeft = 2;
                Projectile.velocity = Vector2.Zero;
                if (owner == null)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.localAI[1]++;
                if (Projectile.IsOwnedByLocalPlayer())
                    owner.direction = owner.Center.To(Main.MouseWorld).X > 0 ? 1 : -1;

                if (Projectile.ai[2] == 0)
                {
                    Projectile.Center = owner.Center;
                    Projectile.rotation += MathHelper.ToRadians(25);
                        
                    if (Projectile.localAI[1] % 20 == 0)
                    {
                        SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, Projectile.Center);
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 vr = HcMath.GetRandomVevtor(0, 360, 15);
                            Projectile.NewProjectile(
                                AiBehavior.GetEntitySource_Parent(owner),
                                owner.Center,
                                vr,
                                ModContent.ProjectileType<RedLightningFeather>(),
                                Projectile.damage,
                                0,
                                owner.whoAmI
                                );
                        }
                    }
                    if (Projectile.localAI[1] > 60)
                    {
                        Projectile.ai[2] = 1;
                        Projectile.localAI[1] = 0;
                    }
                }
                if (Projectile.ai[2] == 1)
                {
                    if (Projectile.IsOwnedByLocalPlayer())
                    {
                        Vector2 toMous = owner.Center.To(Main.MouseWorld).UnitVector();
                        Vector2 topos = toMous * 56 + owner.Center;
                        Projectile.Center = Vector2.Lerp(topos, Projectile.Center, 0.01f);
                        Projectile.rotation = toMous.ToRotation() + MathHelper.PiOver4;

                        if (Projectile.localAI[1] > 10 && owner.ownedProjectileCounts[ModContent.ProjectileType<GildedProboscisKevinLightning>()] == 0)
                        {
                            projIndex = Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(owner),
                                    owner.Center,
                                    owner.Center.To(Main.MouseWorld).UnitVector() * 15f,
                                    ModContent.ProjectileType<GildedProboscisKevinLightning>(),
                                    Projectile.damage / 3,
                                    0,
                                    owner.whoAmI
                                    );
                        }
                        Projectile kevin = AiBehavior.GetProjectileInstance(projIndex);
                        if (kevin != null)
                        {
                            Vector2 pos = Projectile.Center + toMous.UnitVector() * 85;
                            kevin.Center = pos;

                            if (Projectile.localAI[1] > 500)
                            {
                                kevin.Kill();
                                Projectile.ai[2] = 0;
                                Projectile.localAI[1] = 0;
                            }
                        }                        
                    }
                }
            }
        }

        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(4))
            {
                int num = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 60, Projectile.direction * 2, 0f, 150);
                Main.dust[num].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.canGhostHeal && !Main.player[Projectile.owner].moonLeech)
            {
                Player obj = Main.player[Projectile.owner];
                obj.statLife++;
                obj.HealEffect(1);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = DrawUtils.GetT2DValue(Texture);

            if (Projectile.ai[1] == 0)
            {
                return base.PreDraw(ref lightColor);
            }
            else
            {
                Main.EntitySpriteDraw(
                    texture2D, DrawUtils.WDEpos(Projectile.Center), null, lightColor,
                    Projectile.rotation + MathHelper.PiOver2, DrawUtils.GetOrig(texture2D),
                    Projectile.scale, SpriteEffects.None);
                return false;
            }         
        }
    }
}
