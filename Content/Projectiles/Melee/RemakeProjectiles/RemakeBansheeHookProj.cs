using CalamityMod.Projectiles.Melee;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using static CalamityMod.Projectiles.BaseProjectiles.BaseSpearProjectile;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Common;
using System.Collections.Generic;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Terraria.GameInput;
using CalamityWeaponRemake.Common.DrawTools;

namespace CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles
{
    internal class RemakeBansheeHookProj : BaseSpearProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<BansheeHook>();

        public override SpearType SpearAiType => SpearType.GhastlyGlaiveSpear;

        public override float TravelSpeed => 22f;

        public override Action<Projectile> EffectBeforeReelback => delegate
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.8f, ModContent.ProjectileType<RemakeBansheeHookScythe>(), Projectile.damage, Projectile.knockBack * 0.85f, Projectile.owner);
        };

        public override string Texture => CWRConstant.Item_Melee + "BansheeHook";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.alpha = 255;
        }

        Player owner => AiBehavior.GetPlayerInstance(Projectile.owner);
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                base.AI();

                if (owner != null)
                {
                    if (owner.itemAnimation == owner.itemAnimationMax / 2 && Projectile.IsOwnedByLocalPlayer())
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 vr = Projectile.velocity.UnitVector().RotatedBy(MathHelper.ToRadians(-20 + 10 * i)) * 10f;
                            Projectile.NewProjectile(
                                AiBehavior.GetEntitySource_Parent(Projectile),
                                Projectile.Center,
                                vr,
                                ModContent.ProjectileType<BansheeHookScythe>(),
                                Projectile.damage,
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
                    if (Projectile.localAI[1] % 10 == 0)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 vr = HcMath.GetRandomVevtor(0, 360, 25);
                            Projectile.NewProjectile(
                                AiBehavior.GetEntitySource_Parent(owner),
                                owner.Center,
                                vr,
                                ModContent.ProjectileType<BansheeHookScythe>(),
                                Projectile.damage / 2,
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
                        
                        if (Projectile.localAI[1] > 10 && Projectile.localAI[1] % 20 == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 spanPos = Main.MouseWorld + HcMath.GetRandomVevtor(0, 360, 160);
                                Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(owner),
                                    spanPos,
                                    spanPos.To(Main.MouseWorld).UnitVector() * 15f,
                                    ModContent.ProjectileType<BansheeHookScythe>(),
                                    Projectile.damage / 2,
                                    0,
                                    owner.whoAmI
                                    );
                            }
                        }
                    }
                }
            }
        }

        public override void ExtraBehavior()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            float num = player.itemAnimation / (float)player.itemAnimationMax;
            float num2 = (1f - num) * (MathF.PI * 2f);
            float num3 = Projectile.velocity.ToRotation();
            float num4 = Projectile.velocity.Length();
            Vector2 spinningpoint = Vector2.UnitX.RotatedBy(MathF.PI + num2) * new Vector2(num4, Projectile.ai[0]);
            Vector2 destination = vector + spinningpoint.RotatedBy(num3) + new Vector2(num4 + TravelSpeed + 40f, 0f).RotatedBy(num3);
            Vector2 vector2 = player.SafeDirectionTo(destination, Vector2.UnitX * player.direction);
            Vector2 vector3 = Projectile.velocity.SafeNormalize(Vector2.UnitY);
            float num5 = 2f;
            for (int i = 0; i < num5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, 60, 0f, 0f, 110);
                dust.velocity = player.SafeDirectionTo(dust.position) * 2f;
                dust.position = Projectile.Center + vector3.RotatedBy(num2 * 2f + i / num5 * (MathF.PI * 2f)) * 10f;
                dust.scale = 1f + Main.rand.NextFloat(0.6f);
                dust.velocity += vector3 * 3f;
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust2 = Dust.NewDustDirect(Projectile.Center, 20, 20, 60, 0f, 0f, 110);
                dust2.velocity = player.SafeDirectionTo(dust2.position) * 2f;
                dust2.position = Projectile.Center + vector2 * -110f;
                dust2.scale = 0.45f + Main.rand.NextFloat(0.4f);
                dust2.fadeIn = 0.7f + Main.rand.NextFloat(0.4f);
                dust2.noGravity = true;
                dust2.noLight = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = Projectile.spriteDirection == -1 ? ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/Spears/BansheeHookAlt").Value
                : ModContent.Request<Texture2D>(Texture).Value;
            if (Projectile.ai[1] == 0)
            {
                Vector2 position = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Vector2 origin = new Vector2(Projectile.spriteDirection == 1 ? texture2D.Width + 8f : -8f, -8f);
                Main.EntitySpriteDraw(texture2D, position, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            }
            if (Projectile.ai[1] == 1)
            {
                Main.EntitySpriteDraw(
                    texture2D, DrawUtils.WDEpos(Projectile.Center), null, lightColor, 
                    Projectile.rotation, DrawUtils.GetOrig(texture2D), 
                    Projectile.scale, SpriteEffects.None);
            }
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D = Projectile.spriteDirection == -1 ? ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/Spears/BansheeHookAltGlow").Value
                : ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/Spears/BansheeHookGlow").Value;
            if (Projectile.ai[1] == 0)
            {
                Vector2 position = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Vector2 origin = new Vector2(Projectile.spriteDirection == 1 ? texture2D.Width - -8f : -8f, -8f);
                Main.EntitySpriteDraw(texture2D, position, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None);
            }            
            if (Projectile.ai[1] == 1)
            {
                Main.EntitySpriteDraw(
                    texture2D, DrawUtils.WDEpos(Projectile.Center), null, lightColor,
                    Projectile.rotation, DrawUtils.GetOrig(texture2D),
                    Projectile.scale, SpriteEffects.None);
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float f = Projectile.rotation - MathF.PI / 4f * Math.Sign(Projectile.velocity.X) + (Projectile.spriteDirection == -1).ToInt() * MathF.PI;
            float num = -95f;
            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + f.ToRotationVector2() * num, (TravelSpeed + 1f) * Projectile.scale, ref collisionPoint))
            {
                return true;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<BansheeHookBoom>(), (int)(hit.Damage * 0.25), 10f, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            }
        }
    }
}
