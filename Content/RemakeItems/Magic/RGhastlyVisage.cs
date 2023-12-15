﻿using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Magic.HeldProjs;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Magic
{
    internal class RGhastlyVisage : BaseRItem
    {
        public override void Load() {
            SetReadonlyTargetID = ModContent.ItemType<CalamityMod.Items.Weapons.Magic.GhastlyVisage>();
        }
        public override void SetDefaults(Item item)
        {
            item.damage = 69;
            item.DamageType = DamageClass.Magic;
            item.noUseGraphic = true;
            item.channel = true;
            item.mana = 20;
            item.width = 32;
            item.height = 36;
            item.useTime = 27;
            item.useAnimation = 27;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noMelee = true;
            item.knockBack = 5f;
            item.shootSpeed = 9f;
            item.shoot = ModContent.ProjectileType<RemakeGhastlyVisageProj>();
            item.value = CalamityGlobalItem.Rarity13BuyPrice;
            item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "GhastlyVisage");
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {
            player.statMana += manaConsumed;
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<RemakeGhastlyVisageProj>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}
