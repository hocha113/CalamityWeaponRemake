﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Magic;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Magic
{
    internal class RTradewinds : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Magic.Tradewinds>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.Tradewinds>(item))
            {
                item.damage = 25;
                item.DamageType = DamageClass.Magic;
                item.mana = 5;
                item.width = 28;
                item.height = 30;
                item.useTime = 12;
                item.useAnimation = 12;
                item.useStyle = ItemUseStyleID.Shoot;
                item.noMelee = true;
                item.knockBack = 5f;
                item.value = CalamityGlobalItem.Rarity3BuyPrice;
                item.rare = ItemRarityID.Orange;
                item.UseSound = SoundID.Item7;
                item.autoReuse = true;
                item.shoot = ModContent.ProjectileType<TradewindsProjectile>();
                item.shootSpeed = 20f;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.Tradewinds>(item))
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                {
                    Main.projectile[proj].ai[0] = 2;
                    for (int i = 0; i <= 360; i += 3)
                    {
                        Vector2 vr = new Vector2(3f, 3f).RotatedBy(MathHelper.ToRadians(i));
                        int num = Dust.NewDust(player.Center, player.width, player.height, DustID.Smoke, vr.X, vr.Y, 200, new Color(232, 251, 250, 200), 1.4f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position = player.Center + velocity.UnitVector() * 23;
                        Main.dust[num].velocity = vr;
                    }
                }
                else
                {
                    Main.projectile[proj].penetrate = 13;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.Tradewinds>(item))
            {
                item.useTime = 12;
                item.useAnimation = 12;
                if (player.altFunctionUse == 2)
                {
                    item.useTime = 18;
                    item.useAnimation = 18;
                    type = ModContent.ProjectileType<Feathers>();
                }
            }
            base.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.Tradewinds>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }
    }
}
