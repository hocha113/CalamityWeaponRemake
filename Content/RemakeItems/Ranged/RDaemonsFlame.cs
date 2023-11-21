﻿using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalamityMod;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Content.RemakeItems.Ranged
{
    internal class RDaemonsFlame : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.DaemonsFlame>(item))
            {
                item.damage = 150;
                item.width = 62;
                item.height = 128;
                item.useTime = 12;
                item.useAnimation = 12;
                item.useStyle = ItemUseStyleID.Shoot;
                item.knockBack = 4f;
                item.UseSound = SoundID.Item5;
                item.noMelee = true;
                item.noUseGraphic = true;
                item.DamageType = DamageClass.Ranged;
                item.channel = true;
                item.autoReuse = true;
                item.shoot = ModContent.ProjectileType<DaemonsFlameHeldProj>();
                item.shootSpeed = 20f;
                item.useAmmo = AmmoID.Arrow;
                item.value = CalamityGlobalItem.Rarity13BuyPrice;
                item.rare = ModContent.RarityType<PureGreen>();
                item.CWR().remakeItem = true;
                item.Calamity().canFirePointBlankShots = true;
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.DaemonsFlame>(item))
            {
                if (player.ownedProjectileCounts[item.shoot] > 0)
                    return false;
            }
            return base.UseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.DaemonsFlame>(item))
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DaemonsFlameHeldProj>(), damage, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
