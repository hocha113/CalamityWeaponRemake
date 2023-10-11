﻿using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Content.Items.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.IsOwnedByLocalPlayer())
                Main.LocalPlayer.CWR().KevinCharge = 0;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projIndex);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projIndex = reader.ReadInt32();
        }

        Player Owner => AiBehavior.GetPlayerInstance(Projectile.owner);
        int projIndex = -1;
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
            }//如果是左键弹幕，执行原有的基类行为
            if (Projectile.ai[1] == 1)
            {
                drawUIalp += 5;
                if (drawUIalp > 255) drawUIalp = 255;//在此处控制充能UI的透明度参数

                if (Projectile.IsOwnedByLocalPlayer())//当主人按住右键时锁定弹幕的存在时间
                {
                    if (PlayerInput.Triggers.Current.MouseRight)
                        Projectile.timeLeft = 2;
                }

                Projectile.velocity = Vector2.Zero;
                if (Owner == null)//防御性代码，任何时候都不希望后续代码访问null值，或者对无效对象进行操作
                {
                    Projectile.Kill();
                    return;
                }
                Projectile.ai[0]++;
                if (Projectile.IsOwnedByLocalPlayer())//让玩家朝向正确的方向
                    Owner.direction = Owner.Center.To(Main.MouseWorld).X > 0 ? 1 : -1;

                if (Projectile.ai[2] == 0)
                {
                    Projectile.Center = Owner.Center;
                    Projectile.rotation += MathHelper.ToRadians(25);//旋转长矛

                    float frontArmRotation = (MathHelper.PiOver2 - 0.31f) * -Owner.direction;
                    Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmRotation);
                    //Main.NewText(Projectile.ai[0]);
                    if (Projectile.IsOwnedByLocalPlayer())
                    {
                        if (Projectile.ai[0] % 20 == 0)//周期性发射弹幕
                        {
                            SoundEngine.PlaySound(SoundID.Item102, Projectile.Center);
                            for (int i = 0; i < 6; i++)
                            {
                                Vector2 vr = HcMath.GetRandomVevtor(0, 360, 15);
                                Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(Owner),
                                    Owner.Center,
                                    vr,
                                    ModContent.ProjectileType<RedLightningFeather>(),
                                    Projectile.damage,
                                    0,
                                    Owner.whoAmI
                                    );
                            }
                            Main.LocalPlayer.CWR().KevinCharge += 500 / 3;
                        }
                        if (Projectile.ai[0] > 60)//当旋转时间超过60tike时切换下一个状态
                        {
                            Main.LocalPlayer.CWR().KevinCharge = 500;
                            Projectile.ai[2] = 1;
                            Projectile.ai[0] = 0;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                if (Projectile.ai[2] == 1)
                {
                    if (Projectile.IsOwnedByLocalPlayer())
                    {
                        Vector2 toMous = Owner.Center.To(Main.MouseWorld).UnitVector();
                        Vector2 topos = toMous * 56 + Owner.Center;
                        Projectile.Center = Vector2.Lerp(topos, Projectile.Center, 0.01f);
                        Projectile.rotation = toMous.ToRotation() + MathHelper.PiOver4;

                        Main.LocalPlayer.CWR().KevinCharge = 500 - (int)Projectile.ai[0];//同步主人玩家的特斯拉充能值，后续将应用于UI绘制

                        if (Projectile.ai[0] > 10 && Owner.ownedProjectileCounts[ModContent.ProjectileType<GildedProboscisKevinLightning>()] == 0)
                        {
                            projIndex = Projectile.NewProjectile(
                                    AiBehavior.GetEntitySource_Parent(Owner),
                                    Owner.Center,
                                    Owner.Center.To(Main.MouseWorld).UnitVector() * 15f,
                                    ModContent.ProjectileType<GildedProboscisKevinLightning>(),
                                    Projectile.damage / 3,
                                    0,
                                    Owner.whoAmI
                                    );
                            Projectile.netUpdate = true;
                        }
                        Projectile kevin = AiBehavior.GetProjectileInstance(projIndex);
                        if (kevin != null)
                        {
                            Vector2 pos = Projectile.Center + toMous.UnitVector() * 85;
                            kevin.Center = pos;

                            if (Projectile.ai[0] > 500)
                            {
                                kevin.Kill();
                                kevin.netUpdate = true;
                                Main.LocalPlayer.CWR().KevinCharge = 0;
                                Projectile.ai[2] = 0;
                                Projectile.ai[0] = 0;
                                Projectile.netUpdate = true;
                                SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, Projectile.Center);
                            }//时长够了后切换回旋转阶段
                        }
                    }//以下行为只能由主人来运行
                }//在这个状态下将发射特斯拉闪电
            }//如果是右键弹幕，执行特定行为
        }

        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(4))
            {
                int num = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.RedTorch, Projectile.direction * 2, 0f, 150);
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

        public override void PostDraw(Color lightColor)
        {
            DrawKevinChargeBar();
        }

        public void DrawKevinChargeBar()
        {
            if (Owner == null || Projectile.ai[1] != 1) return;

            Texture2D textureFront = DrawUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/GenericBarFront");
            Texture2D textureBack = DrawUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/GenericBarBack");
            Vector2 drawPos = DrawUtils.WDEpos(Owner.Center + new Vector2(textureFront.Width / -2, 135));
            float alp = (drawUIalp / 255f);
            Rectangle backRec = new Rectangle(0, 0, (int)(textureBack.Width * (Owner.CWR().KevinCharge / 500f)), textureBack.Height);

            Main.EntitySpriteDraw(
                textureFront,
                drawPos,
                null,
                Color.Gold * alp,
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
    }
}
