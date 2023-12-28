﻿using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    /// <summary>
    /// 净神者之焰
    /// </summary>
    internal class CleansingBlaze : ModItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "CleansingBlaze";
        public new string LocalizationCategory => "Items.Weapons.Ranged";
        public override void SetDefaults() {
            Item.damage = 130;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 32;
            Item.useTime = 3;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item34;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EssenceFire>();
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Gel;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.CWR().remakeItem = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/CleansingBlazeGlow").Value);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int num6 = Main.rand.Next(2, 4);
            for (int index = 0; index < num6; ++index) {
                float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override Vector2? HoldoutOffset() {
            return new Vector2(-5, 0);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) {
            if (Main.rand.Next(0, 100) < 90)
                return false;
            return true;
        }
    }
}
