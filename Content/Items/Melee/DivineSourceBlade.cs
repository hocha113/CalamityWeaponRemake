using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.noMelee = true;
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
            int types = ModContent.ProjectileType<DivineSourceBeam>();

            Vector2 vector2 = player.Center.To(Main.MouseWorld).UnitVector() * 3;
            Vector2 position = player.Center;
            Projectile.NewProjectile(
                player.parent(), position, vector2
                , types
                , (int)(Item.damage * 1.25f)
                , Item.knockBack
                , player.whoAmI);
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

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AuricBar>(5).
                AddIngredient<CalamityMod.Items.Weapons.Melee.Terratomere>().
                AddIngredient<CalamityMod.Items.Weapons.Melee.Excelsus>().
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
