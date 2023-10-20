using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Common;
using System.Collections.Generic;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Common.AuxiliaryMeans;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class SpatialSpears : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Melee";

        public override string Texture => CWRConstant.placeholder;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public int Status { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
        public int Behavior { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public int Time { get => (int)Projectile.ai[2]; set => Projectile.ai[2] = value; }

        public override void AI()
        {            
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            if (Status == 0)
            {
                Lighting.AddLight(Projectile.Center, 0.05f, 1f, 0.05f);

                Projectile.velocity *= 1.02f;
                if (Projectile.timeLeft == 80 && Projectile.IsOwnedByLocalPlayer())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vr = Projectile.velocity.RotatedBy(MathHelper.ToRadians(10 - 10 * i)) * 0.75f;
                        Projectile.NewProjectile(
                            AiBehavior.GetEntitySource_Parent(Projectile),
                            Projectile.Center,
                            vr,
                            Type,
                            Projectile.damage / 2,
                            0,
                            Projectile.owner,
                            1
                            );
                    }
                }
            }
            if (Status == 1)
            {
                Lighting.AddLight(Projectile.Center, 0.85f, 0.05f, 0.05f);
                if (Projectile.timeLeft == 60 && Projectile.IsOwnedByLocalPlayer())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vr = Projectile.velocity.RotatedBy(MathHelper.ToRadians(10 - 10 * i)) * 0.75f;
                        Projectile.NewProjectile(
                            AiBehavior.GetEntitySource_Parent(Projectile),
                            Projectile.Center,
                            vr,
                            Type,
                            Projectile.damage / 2,
                            0,
                            Projectile.owner,
                            2
                            );
                    }
                }
                if (Projectile.timeLeft < 60)
                {
                    Projectile.velocity *= 0.99f;
                }
            }
            if (Status == 2)
            {
                Lighting.AddLight(Projectile.Center, 0.05f, 1f, 0.75f);

                
                if (Projectile.timeLeft == 60 && Projectile.IsOwnedByLocalPlayer())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vr = Projectile.velocity.RotatedBy(MathHelper.ToRadians(10 - 10 * i)) * 0.75f;
                        Projectile.NewProjectile(
                            AiBehavior.GetEntitySource_Parent(Projectile),
                            Projectile.Center,
                            vr,
                            Type,
                            Projectile.damage / 2,
                            0,
                            Projectile.owner,
                            3
                            );
                    }
                }
                if (Projectile.timeLeft < 60)
                {
                    Projectile.velocity *= 0.99f;
                }
            }
            if (Status == 3)
            {
                Lighting.AddLight(Projectile.Center, 0.55f, 1f, 0.05f);

                Projectile.velocity *= 0.98f;
            }

            if (Main.rand.NextBool(8))
            {
                switch (Projectile.ai[0])
                {
                    case 0:
                        Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height
                            , DustID.TerraBlade, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
                        break;
                    case 1:
                        Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height
                            , DustID.BlueFairy, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
                        break;
                    case 2:
                        Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height
                            , DustID.PinkFairy, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
                        break;
                    case 3:
                        Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height
                            , DustID.CopperCoin, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
                        break;
                }
            }
        }

        private static List<Texture2D> textures => new List<Texture2D>()
        {
            DrawUtils.GetT2DValue(CWRConstant.Projectile_Melee + "SpatialSpear0"),
            DrawUtils.GetT2DValue(CWRConstant.Projectile_Melee + "SpatialSpear1"),
            DrawUtils.GetT2DValue(CWRConstant.Projectile_Melee + "SpatialSpear2"),
            DrawUtils.GetT2DValue(CWRConstant.Projectile_Melee + "SpatialSpear3")
        };

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value = textures[(int)Projectile.ai[0]];
            Color color = lightColor;
            switch (Projectile.ai[0])
            {
                case 0:
                    color = Color.Gold;
                    break;
                case 1:
                    color = Color.Red;
                    break;
                case 2:
                    color = Color.Blue;
                    break;
                case 3:
                    color = Color.Purple;
                    break;
            }
            color = HcMath.RecombinationColor((color, 0.3f), (lightColor, 0.7f));
            Main.EntitySpriteDraw(
                value, 
                Projectile.Center - Main.screenPosition, 
                null,
                color, 
                Projectile.rotation + MathHelper.PiOver4, 
                DrawUtils.GetOrig(value), 
                Projectile.scale, 
                SpriteEffects.None
                );
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in SoundID.Item10, Projectile.position);
            for (int i = 4; i < 12; i++)
            {
                float num = Projectile.oldVelocity.X * (30f / i);
                float num2 = Projectile.oldVelocity.Y * (30f / i);
                int num3 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num, Projectile.oldPosition.Y - num2), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default(Color), 1.8f);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].velocity *= 0.5f;
                num3 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num, Projectile.oldPosition.Y - num2), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default(Color), 1.4f);
                Main.dust[num3].velocity *= 0.05f;
            }
        }
    }
}
