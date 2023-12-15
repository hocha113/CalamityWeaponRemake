﻿using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RGildedProboscis : BaseRItem
    {
        public override void Load() {
            SetReadonlyTargetID = ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GildedProboscis>();
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GildedProboscis>()] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GildedProboscis>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            item.width = 66;
            item.damage = 315;
            item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            item.noMelee = true;
            item.useTurn = true;
            item.noUseGraphic = true;
            item.useAnimation = 19;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTime = 19;
            item.knockBack = 8.75f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 66;
            item.value = CalamityGlobalItem.Rarity11BuyPrice;
            item.rare = ItemRarityID.Purple;
            item.shoot = ModContent.ProjectileType<RGildedProboscisProj>();
            item.shootSpeed = 13f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GildedProboscis>(item))
            {
                CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "GildedProboscis");
            }
        }

        public override bool? AltFunctionUse(Item item, Player player)
        {
            return true;
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.altFunctionUse == 2) {
                SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                Main.projectile[proj].ai[1] = 1;
            }
            return false;
        }
    }
}
