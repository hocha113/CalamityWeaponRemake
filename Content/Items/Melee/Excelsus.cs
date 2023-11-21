﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    /// <summary>
    /// 宙宇波能刃
    /// </summary>
    internal class Excelsus : ModItem
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override string Texture => CWRConstant.Cay_Wap_Melee + "Excelsus";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 78;
            Item.damage = 250;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 94;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.shoot = ModContent.ProjectileType<ExcelsusMain>();
            Item.shootSpeed = 12f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation
                , ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/ExcelsusGlow").Value);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ExcelsusBomb>(), damage * 3, knockback, player.whoAmI);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    float speedX = velocity.X + Main.rand.NextFloat(-1.5f, 1.5f);
                    float speedY = velocity.Y + Main.rand.NextFloat(-1.5f, 1.5f);
                    switch (i)
                    {
                        case 0:
                            type = ModContent.ProjectileType<ExcelsusMain>();
                            break;
                        case 1:
                            type = ModContent.ProjectileType<ExcelsusBlue>();
                            break;
                        case 2:
                            type = ModContent.ProjectileType<ExcelsusPink>();
                            break;
                    }

                    Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, type, damage, knockback, player.whoAmI);
                }
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<LaserFountains>(), Item.damage, 0f, player.whoAmI, target.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<LaserFountains>(), Item.damage, 0f, player.whoAmI, target.whoAmI);
        }
    }
}
