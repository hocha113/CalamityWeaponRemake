using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Content.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class DivineSourceBlade : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "DivineSourceBlade2";

        public Texture2D Value => DrawUtils.GetT2DValue(Texture);

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
            Item.UseSound = SoundID.Item1;
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

        private int[] projs;
        public override void UseAnimation(Player player)
        {
            if (projs == null)
                projs = new int[6];

            int types = ModContent.ProjectileType<DivineSourceKnifelight>();

            for (int n = 0; n < 6; n++)
            {
                Projectile projectile = AiBehavior.GetProjectileInstance(projs[n]);
                if (projectile != null && projectile.type == types)
                    projectile.Kill();
            }

            for (int i = 0; i < 6; i++)
            {
                int proj = Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(player),
                    player.Center,
                    player.Center.To(Main.MouseWorld).UnitVector() * 8,
                    types,
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI,
                    ai0: 80 + i * 20
                );
                projs[i] = proj;
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
                DrawUtils.GetOrig(Value),
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
