using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Melee;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class DivineSourceBlade : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "DivineSourceBlade2";

        public Texture2D Value => CWRUtils.GetT2DValue(Texture);

        public override void SetDefaults()
        {
            Item.height = 154;
            Item.width = 154;
            Item.damage = 332;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 16;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;            
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.shoot = ModContent.ProjectileType<DivineSourceBladeProjectile>();
            Item.shootSpeed = 17f;
            Item.CWR().remakeItem = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            float itmeRots = player.itemRotation + (MathHelper.ToRadians(60)) * player.direction;
            //+player.Center.To(Main.MouseWorld).ToRotation() + MathHelper.Pi * (player.direction > 0 ? 0 : 1);
            player.itemRotation = itmeRots;

            player.itemLocation = player.Center + player.itemRotation.ToRotationVector2() * -15 * player.direction;
            float rots = (player.itemRotation - MathHelper.PiOver2 + (player.direction < 0 ? MathHelper.Pi : 0)) - player.direction * MathHelper.PiOver2;
            player.SetCompositeArmFront(Math.Abs(rots) > 0.01f, Player.CompositeArmStretchAmount.Full, rots);
        }

        public override void UseAnimation(Player player)
        {
            Item.initialize();

            if (Item.CWR().ai[1] == 0)
                Item.CWR().ai[1] = 1;

            int types = ModContent.ProjectileType<DivineSourceBeam>();

            Vector2 vector2 = player.Center.To(Main.MouseWorld).UnitVector() * 3;
            Vector2 position = player.Center;
            int proj = Projectile.NewProjectile(
                player.parent(), position, vector2
                , types
                , (int)(Item.damage * 0.75f)
                , Item.knockBack
                , player.whoAmI);
           
            //if (Main.projectile.IndexInRange(proj))
            //{
            //    TerratomereBeams terratomereBeams = Main.projectile[proj].ModProjectile as TerratomereBeams;
            //    if (terratomereBeams != null)
            //    {
            //        terratomereBeams.Projectile.ai[0] = (player.direction == 1f).ToInt();
            //        terratomereBeams.Projectile.ai[1] = 1;
            //        terratomereBeams.ControlPoints = GenerateSlashPoints(vector2.X < 0).ToArray();
            //    }
            //}

            Item.CWR().ai[0] = proj;
            Item.CWR().ai[1] *= -1;
        }

        public IEnumerable<Vector2> GenerateSlashPoints(bool dir)
        {
            float starRot = MathHelper.ToRadians(-170);
            float endRot = MathHelper.ToRadians(60);
            if (dir)
            {
                starRot = MathHelper.ToRadians(-10);
                endRot = MathHelper.ToRadians(-240);
            }

            for (int i = 0; i < 30; i++)
            {
                float completion = MathHelper.Lerp(endRot, starRot, i / 30f);
                yield return completion.ToRotationVector2() * 84f;
            }            
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(
                Value,
                position,
                frame,
                drawColor,
                MathHelper.PiOver4,
                origin,
                scale * 1.5f,
                SpriteEffects.None,
                0
                );
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            spriteBatch.Draw(
                Value,
                Item.Center - Main.screenPosition,
                null,
                lightColor,
                MathHelper.PiOver4,
                CWRUtils.GetOrig(Value),
                scale,
                SpriteEffects.None,
                0
                );
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return base.HoldoutOffset();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
