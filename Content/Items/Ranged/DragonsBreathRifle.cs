﻿using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.CWRUtils;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    internal class DragonsBreathRifle : ModItem
    {
        public override string Texture => CWRConstant.Item + "Ranged/" + "DragonsBreathRifle";

        public override void SetStaticDefaults() {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults() {
            Item.damage = 328;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 124;
            Item.height = 78;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.reuseDelay = 15;
            Item.useLimitPerAnimation = 2;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6.5f;
            Item.shoot = ModContent.ProjectileType<DragonsBreathRifleHeldProj>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.CWR().remakeItem = true;
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        int dbpType => ModContent.ProjectileType<DragonsBreathRifleHeldProj>();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.ownedProjectileCounts[dbpType] == 0) {
                int proj = Projectile.NewProjectile(CWRUtils.parent(player), position, velocity, dbpType, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2) {
                    Main.projectile[proj].ai[0] = 1;
                }
            }
            return false;
        }

        public override void AddRecipes() {
            CreateRecipe().
                AddIngredient<AuricBar>(5).
                AddIngredient<CalamityMod.Items.Weapons.Ranged.TheSevensStriker>().
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
