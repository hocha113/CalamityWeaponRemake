﻿using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RElementalLance : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.ElementalLance>();
        public override int ProtogenesisID => ModContent.ItemType<ElementalLance>();
        public override void Load() {
            SetReadonlyTargetID = TargetID;
        }
        public override void SetStaticDefaults() {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.ElementalLance>()] = true;
        }

        public override void SetDefaults(Item item) {
            item.width = 88;
            item.damage = 160;
            item.DamageType = DamageClass.Melee;
            item.noMelee = true;
            item.useTurn = true;
            item.noUseGraphic = true;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.useTime = 20;
            item.knockBack = 9.5f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 88;
            item.value = CalamityGlobalItem.Rarity11BuyPrice;
            item.rare = ItemRarityID.Purple;
            item.shoot = ModContent.ProjectileType<RElementalLanceProjectile>();
            item.shootSpeed = 12f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "ElementalLance", 2);
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.altFunctionUse == 2)
                Main.projectile[proj].ai[1] = 1;
            return false;
        }

        public override bool? AltFunctionUse(Item item, Player player) {
            return true;
        }

        public override bool? UseItem(Item item, Player player) {
            return player.ownedProjectileCounts[item.shoot] == 0;
        }
    }
}
