﻿using CalamityMod.Items;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RBansheeHook : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BansheeHook>();
        public override int ProtogenesisID => ModContent.ItemType<BansheeHook>();
        public override void Load() {
            SetReadonlyTargetID = TargetID;
        }
        public override void SetStaticDefaults() {
            ItemID.Sets.Spears[TargetID] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[TargetID] = true;
        }

        public override void SetDefaults(Item item) {
            item.width = 120;
            item.damage = 220;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = true;
            item.DamageType = DamageClass.Melee;
            item.useAnimation = 21;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTime = 21;
            item.knockBack = 8.5f;
            item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
            item.autoReuse = true;
            item.height = 108;
            item.shoot = ModContent.ProjectileType<RBansheeHookProj>();
            item.shootSpeed = 42f;
            item.value = CalamityGlobalItem.Rarity13BuyPrice;
            item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "BansheeHook");
        }

        public override bool? AltFunctionUse(Item item, Player player) {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.BansheeHook>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            float num82 = Main.mouseX + Main.screenPosition.X - position.X;
            float num83 = Main.mouseY + Main.screenPosition.Y - position.Y;
            if (player.gravDir == -1f) {
                num83 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - position.Y;
            }
            float num84 = (float)Math.Sqrt(num82 * num82 + num83 * num83);
            if ((float.IsNaN(num82) && float.IsNaN(num83)) || (num82 == 0f && num83 == 0f)) {
                num82 = player.direction;
                num83 = 0f;
                num84 = item.shootSpeed;
            }
            else {
                num84 = item.shootSpeed / num84;
            }
            num82 *= num84;
            num83 *= num84;
            float ai4 = Main.rand.NextFloat() * item.shootSpeed * 0.75f * player.direction;
            velocity = new Vector2(num82, num83);
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai4);
            if (player.altFunctionUse == 2) {
                SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                SoundEngine.PlaySound(in BloodflareHeadRanged.ActivationSound, player.Center);
                item.CWR().MeleeCharge = 0;
                Main.projectile[proj].ai[1] = 1;
            }
            return false;
        }
    }
}
