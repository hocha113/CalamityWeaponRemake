﻿using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles.Weapons.Melee
{
    internal class CosmicRay : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 15000;
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
            Projectile.timeLeft = 90;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override string Texture => CWRConstant.placeholder;

        public int Status
        {
            set => Projectile.ai[0] = value;
            get => (int)Projectile.ai[0];
        }

        public int ThisTimeValue
        {
            set => Projectile.ai[1] = value;
            get => (int)Projectile.ai[1];
        }

        public int Leng { set; get; } = 5000;

        public bool SterDust { set; get; } = true;

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.rotation);
            writer.Write(ThisTimeValue);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.rotation = reader.ReadSingle();
            ThisTimeValue = reader.ReadInt32();
        }

        public override void AI()
        {
            ThisTimeValue++;
            if (ThisTimeValue > 30) SterDust = true;

            if (ThisTimeValue % 10 == 0 && Projectile.timeLeft <= 60 && Projectile.IsOwnedByLocalPlayer())
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 17, ModContent.ProjectileType<GalaxyStar>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner);
            }
        }

        public void NewSterDust(Vector2 center)
        {
            float angle = Main.rand.NextFloat(MathF.PI * 2f);
            int numSpikes = 5;
            float spikeAmplitude = 22f;
            float scale = Main.rand.NextFloat(1f, 1.35f);

            for (float spikeAngle = 0f; spikeAngle < MathF.PI * 2f; spikeAngle += 0.1f)
            {
                Vector2 offset = spikeAngle.ToRotationVector2() * (2f + (float)(Math.Sin(angle + spikeAngle * numSpikes) + 1.0) * spikeAmplitude)
                                 * Main.rand.NextFloat(0.95f, 1.05f);

                Dust.NewDustPerfect(center, 173, offset, 0, default, scale).customData = 0.025f;
            }
        }

        Vector2 OnHitPos = Vector2.Zero;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool collBool = false;
            float point = 0f;

            Vector2 startPos = Projectile.Center;

            Vector2 endPos = Projectile.rotation.ToRotationVector2() * Leng + startPos;

            collBool = Collision.CheckAABBvLineCollision(
                            targetHitbox.TopLeft(),
                            targetHitbox.Size(),
                            startPos,
                            endPos,
                            8,
                            ref point
                            );

            return collBool;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCHitCooldown += 2;
            Projectile.damage -= 15;

            if (SterDust)
            {
                NewSterDust(target.Center);
                SterDust = false;
                ThisTimeValue = 0;
            }
            Projectile.netUpdate = true;
        }

        int Rot = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainValue = CWRUtils.GetT2DValue(CWRConstant.Masking + "Streak3");
            Texture2D startValue = CWRUtils.GetT2DValue(CWRConstant.Projectile + "TornadoProj");

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.EntitySpriteDraw(
                mainValue,
                CWRUtils.WDEpos(Projectile.Center),
                CWRUtils.GetRec(mainValue, 0, 0, Leng, mainValue.Height),
                new Color(92, 58, 156),
                Projectile.rotation,
                new Vector2(0, mainValue.Height * 0.5f),
                new Vector2(1, 0.2f),
                SpriteEffects.None,
                0
            );

            Rot++;

            int vortexLayers = Projectile.timeLeft;
            if (Projectile.timeLeft >= 77) vortexLayers = 90 - Projectile.timeLeft;
            if (vortexLayers > 13 && Projectile.timeLeft < 77) vortexLayers = 13;

            for (int i = 0; i < vortexLayers; i++)
            {
                Main.EntitySpriteDraw(
                startValue,
                CWRUtils.WDEpos(Projectile.Center),
                CWRUtils.GetRec(startValue),
                new Color(92, 58, 156),
                Projectile.rotation + MathHelper.ToRadians(Rot * (i / 5f) + 30 * i),
                CWRUtils.GetOrig(startValue),
                (1 + i * 0.5f) * 0.5f,
                SpriteEffects.None,
                0
                );
            }

            for (int i = 0; i < 13; i++)
            {
                Main.EntitySpriteDraw(
                startValue,
                CWRUtils.WDEpos(Projectile.Center),
                CWRUtils.GetRec(startValue),
                new Color(92, 58, 156),
                Projectile.rotation + MathHelper.ToRadians(Rot * (i / 5f) + 30 * i),
                CWRUtils.GetOrig(startValue),
                1,
                SpriteEffects.None,
                0
                );
            }



            Main.spriteBatch.ResetBlendState();

            return false;
        }
    }
}