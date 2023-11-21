﻿using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RGoldplumeSpear : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GoldplumeSpear>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GoldplumeSpear>(item))
            {
                item.width = 54;
                item.damage = 24;
                item.DamageType = DamageClass.Melee;
                item.noMelee = true;
                item.useTurn = true;
                item.noUseGraphic = true;
                item.useAnimation = 23;
                item.useStyle = ItemUseStyleID.Shoot;
                item.useTime = 23;
                item.knockBack = 5.75f;
                item.UseSound = SoundID.Item1;
                item.autoReuse = true;
                item.height = 54;
                item.value = CalamityGlobalItem.Rarity3BuyPrice;
                item.rare = ItemRarityID.Orange;
                item.shoot = ModContent.ProjectileType<RemakeGoldplumeSpearProjectile>();
                item.shootSpeed = 8f;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GoldplumeSpear>(item))
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                {
                    Main.projectile[proj].ai[1] = 1;
                    Main.projectile[proj].rotation = player.Center.To(Main.MouseWorld).ToRotation();
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GoldplumeSpear>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GoldplumeSpear>(item))
            {
                if (player.ownedProjectileCounts[item.shoot] > 0)
                    return false;
            }
            return base.UseItem(item, player);
        }
    }
}
