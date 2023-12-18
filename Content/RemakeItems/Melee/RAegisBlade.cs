﻿using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RAegisBlade : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.AegisBlade>();
        public override int ProtogenesisID => ModContent.ItemType<AegisBlade>();
        public override void Load() {
            SetReadonlyTargetID = TargetID;
        }
        public override void SetDefaults(Item item)
        {
            item.width = 72;
            item.height = 72;
            item.scale = 0.9f;
            item.damage = 108;
            item.DamageType = DamageClass.Melee;
            item.useAnimation = item.useTime = 13;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.Swing;
            item.knockBack = 2.25f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 15f;
            item.shoot = ProjectileID.PurificationPowder;
            item.value = CalamityGlobalItem.Rarity8BuyPrice;
            item.rare = ItemRarityID.Yellow;
        }

        public override bool? CanUseItem(Item item, Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
                item.noMelee = true;
                item.UseSound = SoundID.Item73;
                item.shoot = ModContent.ProjectileType<AegisBladeProj>();
            }
            else
            {
                item.noUseGraphic = false;
                item.noMelee = false;
                item.UseSound = SoundID.Item73;
                item.shoot = ModContent.ProjectileType<AegisBeams>();
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<AegisBladeProj>()] == 0;
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int damages = damage;
            if (player.altFunctionUse == 2)
            {
                damages = (int)(damage * 1.3f);
            }
            else
            {
                damages = (int)(damage * 0.3f);
            }
            _ = Projectile.NewProjectile(source, position, velocity, item.shoot, damages, knockback, player.whoAmI);
            return false;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "AegisBlade", 2);
        }
    }
}
