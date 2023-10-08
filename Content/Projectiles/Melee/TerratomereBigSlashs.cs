﻿using CalamityMod.Projectiles.Melee;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Common.AuxiliaryMeans;

namespace CalamityWeaponRemake.Content.Projectiles.Melee
{
    internal class TerratomereBigSlashs : ModProjectile
    {
        public int TargetIndex = -1;

        public PrimitiveTrail SlashDrawer;

        public new string LocalizationCategory => "Projectiles.Melee";

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 28;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 27;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.scale = Utils.GetLerpValue(0f, 8f, Projectile.timeLeft, clamped: true);
        }

        public float SlashWidthFunction(float _)
        {
            return (float)Projectile.width * Projectile.scale * Utils.GetLerpValue(0f, 0.1f, _, clamped: true);
        }

        public Color SlashColorFunction(float _)
        {
            return Color.Lime * Projectile.Opacity;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TargetIndex = target.whoAmI;
            target.CWR().TerratomereBoltOnHitNum++;
            if (target.CWR().TerratomereBoltOnHitNum > 6) 
                target.CWR().TerratomereBoltOnHitNum = 0;
            target.netUpdate = true;
            if (target.CWR().TerratomereBoltOnHitNum > 5)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<TerratomereExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (Projectile.timeLeft > 30)
                {
                    Projectile.timeLeft = 30;
                }
                Projectile.velocity *= 0.2f;
                Projectile.damage = 0;
                Projectile.netUpdate = true;
            }           
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner && TargetIndex >= 0)
            {
                if (Main.npc[TargetIndex].CWR().TerratomereBoltOnHitNum > 5)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.npc[TargetIndex].Center, Vector2.Zero, ModContent.ProjectileType<TerratomereSlashCreator>(), Projectile.damage, Projectile.knockBack, Projectile.owner, TargetIndex, Main.rand.NextFloat(MathF.PI * 2f));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (SlashDrawer == null)
            {
                SlashDrawer = new PrimitiveTrail(SlashWidthFunction, SlashColorFunction, null, GameShaders.Misc["CalamityMod:ExobladePierce"]);
            }

            GameShaders.Misc["CalamityMod:ExobladePierce"].SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/BlobbyNoise"));
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseImage2("Images/Extra_189");
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseColor(Terratomere.TerraColor1);
            GameShaders.Misc["CalamityMod:ExobladePierce"].UseSecondaryColor(Terratomere.TerraColor2);
            for (int i = 0; i < 4; i++)
            {
                SlashDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 30);
            }

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.oldPos[0] == Vector2.Zero)
            {
                return false;
            }

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.oldPos[0] + Projectile.Size * 0.5f, Projectile.Center);
        }
    }
}
