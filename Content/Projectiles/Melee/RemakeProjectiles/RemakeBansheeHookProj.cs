using CalamityMod;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Content.Items.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles
{
    internal class RemakeBansheeHookProj : BaseSpearProjectile
    {
        private bool remakeMode => ModContent.GetInstance<ContentConfig>().ForceReplaceResetContent;

        public override LocalizedText DisplayName => CalamityUtils.GetItemName<BansheeHook>();

        public override SpearType SpearAiType => SpearType.GhastlyGlaiveSpear;

        public override float TravelSpeed => 22f;

        public override Action<Projectile> EffectBeforeReelback => delegate
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.8f, ModContent.ProjectileType<RemakeBansheeHookScythe>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack * 0.85f, Projectile.owner);
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

        Player Owner => AiBehavior.GetPlayerInstance(Projectile.owner);
        Item bansheeHook => Owner.HeldItem;//oh，这个物品实例的转化访问语法是从鸿蒙方舟的项目中学习到的，
                                           //这提供了不同实例之间互相访问的手段，尤其是当下的使用情景中尤为有用
        int drawUIalp = 0;
        public override void AI()
        {            
            if (Projectile.ai[1] == 0)
            {
                base.AI();

                if (Owner != null)
                {
                    if (Owner.itemAnimation == Owner.itemAnimationMax / 2 && Projectile.IsOwnedByLocalPlayer())
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
                Projectile.velocity = Vector2.Zero;
                if (Owner == null)
                {
                    Projectile.Kill();
                    return;
                }
                if (bansheeHook == null || bansheeHook.type != ModContent.ItemType<BansheeHook>() 
                    && bansheeHook.type != ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BansheeHook>())
                {
                    Projectile.Kill();
                    return;
                }//因为需要替换原模组的内容，所以这里放弃了直接访问类型来获取属性，作为补救，禁止其余物品发射该弹幕，即使这种情况不应该出现
                Projectile.localAI[1]++;

                if (Projectile.IsOwnedByLocalPlayer())
                {
                    float frontArmRotation = (MathHelper.PiOver2 - 0.31f) * -Owner.direction;
                    Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmRotation);
                    if (PlayerInput.Triggers.Current.MouseRight) Projectile.timeLeft = 2;
                    Owner.direction = Owner.Center.To(Main.MouseWorld).X > 0 ? 1 : -1;
                }

                if (Projectile.ai[2] == 0)
                {
                    Projectile.Center = Owner.Center;
                    Projectile.rotation += MathHelper.ToRadians(25);

                    drawUIalp += 5;
                    if (drawUIalp > 255) drawUIalp = 255;

                    if (Projectile.IsOwnedByLocalPlayer())
                    {
                        bansheeHook.CWR().BansheeHookCharge += 8.333f;
                        if (Projectile.localAI[1] % 20 == 0)
                        {
                            SoundEngine.PlaySound(
                                SoundID.DD2_GhastlyGlaivePierce,
                                Projectile.Center
                                );
                            for (int i = 0; i < 9; i++)
                            {
                                Vector2 vr = HcMath.GetRandomVevtor(0, 360, 25);
                                Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(Owner),
                                    Owner.Center,
                                    vr,
                                    ModContent.ProjectileType<BansheeHookScythe>(),
                                    Projectile.damage / 2,
                                    0,
                                    Owner.whoAmI
                                    );
                            }
                        }
                        if (Projectile.localAI[1] % 10 == 0)
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                Vector2 vr = (MathHelper.TwoPi / 9 * i).ToRotationVector2() * 10;
                                Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(Owner),
                                    Owner.Center,
                                    vr,
                                    ModContent.ProjectileType<SpiritFlame>(),
                                    Projectile.damage / 3,
                                    0,
                                    Owner.whoAmI,
                                    1
                                    );
                            }
                            for (int i = 0; i < 9; i++)
                            {
                                Vector2 vr = (MathHelper.TwoPi / 9 * i).ToRotationVector2() * 20;
                                Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(Owner),
                                    Owner.Center,
                                    vr,
                                    ModContent.ProjectileType<SpiritFlame>(),
                                    Projectile.damage / 4,
                                    0,
                                    Owner.whoAmI,
                                    1
                                    );
                            }
                        }
                    }
                    if (Projectile.localAI[1] > 60)
                    {
                        bansheeHook.CWR().BansheeHookCharge = 500;
                        Projectile.ai[2] = 1;
                        Projectile.localAI[1] = 0;
                    }
                }
                if (Projectile.ai[2] == 1)
                {
                    if (Projectile.IsOwnedByLocalPlayer())
                    {
                        Vector2 toMous = Owner.Center.To(Main.MouseWorld).UnitVector();
                        Vector2 topos = toMous * 56 + Owner.Center;
                        Projectile.Center = Vector2.Lerp(topos, Projectile.Center, 0.01f);
                        Projectile.rotation = toMous.ToRotation();
                        Projectile.localAI[2]++;

                        bansheeHook.CWR().BansheeHookCharge--;

                        if (Projectile.localAI[1] > 10 && Projectile.localAI[1] % 20 == 0)
                        {
                            int damages = (int)(Projectile.damage * 0.75f);
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 spanPos = Main.MouseWorld + HcMath.GetRandomVevtor(0, 360, 160);
                                Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(Owner),
                                    spanPos,
                                    spanPos.To(Main.MouseWorld).UnitVector() * 15f,
                                    ModContent.ProjectileType<AbominateHookScythe>(),
                                    damages,
                                    0,
                                    Owner.whoAmI
                                    );
                            }
                        }
                        if (Projectile.localAI[1] > 10 && Projectile.localAI[1] % 15 == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 pos = Projectile.Center + Projectile.rotation.ToRotationVector2() * 45 * Projectile.scale + HcMath.GetRandomVevtor(0, 360, Main.rand.Next(2, 16));
                                Projectile.NewProjectile(
                                        AiBehavior.GetEntitySource_Parent(Owner),
                                        pos,
                                        Vector2.Zero,
                                        ModContent.ProjectileType<SpiritFlame>(),
                                        Projectile.damage / 2,
                                        0,
                                        Owner.whoAmI
                                        );
                            }
                        }

                        if (bansheeHook.CWR().BansheeHookCharge <= 0)
                        {
                            Projectile.ai[2] = 0;
                            Projectile.localAI[1] = 0;
                            Projectile.netUpdate = true;
                            bansheeHook.CWR().BansheeHookCharge = 0;
                            SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, Projectile.Center);
                            SoundEngine.PlaySound(in BloodflareHeadRanged.ActivationSound, Projectile.Center);
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
                    Projectile.rotation + MathHelper.PiOver4, DrawUtils.GetOrig(texture2D),
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
                    Projectile.rotation + MathHelper.PiOver4, DrawUtils.GetOrig(texture2D),
                    Projectile.scale, SpriteEffects.None);
            }
            DrawKevinChargeBar();
            DrawStar();

        }

        public void DrawStar()
        {
            if (Projectile.localAI[2] != 0)
            {
                Texture2D mainValue = DrawUtils.GetT2DValue(CWRConstant.Masking + "StarTexture_White");
                Vector2 pos = DrawUtils.WDEpos(Projectile.Center + Projectile.rotation.ToRotationVector2() * 45 * Projectile.scale);
                int Time = (int)Projectile.localAI[2];
                int slp = Time * 5;
                if (slp > 255) { slp = 255; }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                for (int i = 0; i < 5; i++)
                {
                    Main.spriteBatch.Draw(
                        mainValue,
                        pos,
                        null,
                        Color.Red,
                        MathHelper.ToRadians(Time * 5 + i * 17),
                        DrawUtils.GetOrig(mainValue),
                        (slp / 1755f),
                        SpriteEffects.None,
                        0
                        );
                }
                for (int i = 0; i < 5; i++)
                {
                    Main.spriteBatch.Draw(
                        mainValue,
                        pos,
                        null,
                        Color.White,
                        MathHelper.ToRadians(Time * 6 + i * 17),
                        DrawUtils.GetOrig(mainValue),
                        (slp / 2055f),
                        SpriteEffects.None,
                        0
                        );
                }
                for (int i = 0; i < 5; i++)
                {
                    Main.spriteBatch.Draw(
                        mainValue,
                        pos,
                        null,
                        Color.Gold,
                        MathHelper.ToRadians(Time * 9 + i * 17),
                        DrawUtils.GetOrig(mainValue),
                        (slp / 2355f),
                        SpriteEffects.None,
                        0
                        );
                }
                Main.spriteBatch.ResetBlendState();
            }
        }

        public void DrawKevinChargeBar()
        {
            if (Owner == null || Projectile.ai[1] != 1) return;

            Texture2D textureFront = DrawUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/GenericBarFront");
            Texture2D textureBack = DrawUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/GenericBarBack");
            Vector2 drawPos = DrawUtils.WDEpos(Owner.Center + new Vector2(textureFront.Width / -2, 135));
            float alp = (drawUIalp / 255f);
            Rectangle backRec = new Rectangle(0, 0, (int)(textureBack.Width * (bansheeHook.CWR().BansheeHookCharge / 500f)), textureBack.Height);

            Main.EntitySpriteDraw(
                textureFront,
                drawPos,
                null,
                Color.Red * alp,
                0,
                new Vector2(3, 1),
                1.2f,
                SpriteEffects.None,
                0
                );

            Main.EntitySpriteDraw(
                textureBack,
                drawPos,
                backRec,
                Color.DarkGoldenrod * alp,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0
                );
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
