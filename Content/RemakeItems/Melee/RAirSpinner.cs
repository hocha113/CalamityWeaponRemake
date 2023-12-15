﻿using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RAirSpinner : BaseRItem
    {
        public override void Load() {
            SetReadonlyTargetID = ModContent.ItemType<CalamityMod.Items.Weapons.Melee.AirSpinner>();
        }
        public override void SetDefaults(Item item){
            item.width = 28;
            item.height = 28;
            item.DamageType = DamageClass.MeleeNoSpeed;
            item.damage = 26;
            item.knockBack = 4f;
            item.useTime = 22;
            item.useAnimation = 22;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.Shoot;
            item.UseSound = SoundID.Item1;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<RAirSpinnerYoyo>();
            item.shootSpeed = 14f;
            item.rare = ItemRarityID.Orange;
            item.value = CalamityGlobalItem.Rarity3BuyPrice;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips){
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "AirSpinner", 2);
        }
    }
}
