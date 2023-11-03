using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
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
    internal class RemakeBansheeHook : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.Spears[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BansheeHook>()] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BansheeHook>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.BansheeHook>(item))
            {
                item.width = 120;
                item.damage = 250;
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
                item.shoot = ModContent.ProjectileType<RemakeBansheeHookProj>();
                item.shootSpeed = 42f;
                item.value = CalamityGlobalItem.Rarity13BuyPrice;
                item.rare = ModContent.RarityType<PureGreen>();
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.BansheeHook>(item))
            {
                List<TooltipLine> newTooltips = new List<TooltipLine>(tooltips);

                foreach (TooltipLine line in tooltips.ToList()) //复制 tooltips 集合，以便在遍历时修改
                {
                    if (line.Name == "Tooltip0")
                    line.Hide();
                }

                TooltipLine newLine = new TooltipLine(Mod, "CWRText"
                    , Language.GetText("Mods.CalamityWeaponRemake.Items.BansheeHook.Tooltip").Value);
                newTooltips.Add(newLine);

                tooltips.Clear(); // 清空原 tooltips 集合
                tooltips.AddRange(newTooltips); // 添加修改后的 newTooltips 集合
                CWRItems.AppAwakeningLine(tooltips);
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.BansheeHook>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.BansheeHook>(item))
            {
                float num82 = Main.mouseX + Main.screenPosition.X - position.X;
                float num83 = Main.mouseY + Main.screenPosition.Y - position.Y;
                if (player.gravDir == -1f)
                {
                    num83 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - position.Y;
                }
                float num84 = (float)Math.Sqrt(num82 * num82 + num83 * num83);
                if ((float.IsNaN(num82) && float.IsNaN(num83)) || (num82 == 0f && num83 == 0f))
                {
                    num82 = player.direction;
                    num83 = 0f;
                    num84 = item.shootSpeed;
                }
                else
                {
                    num84 = item.shootSpeed / num84;
                }
                num82 *= num84;
                num83 *= num84;
                float ai4 = Main.rand.NextFloat() * item.shootSpeed * 0.75f * player.direction;
                velocity = new Vector2(num82, num83);
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai4);
                if (player.altFunctionUse == 2)
                {
                    SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                    SoundEngine.PlaySound(in BloodflareHeadRanged.ActivationSound, player.Center);
                    item.CWR().MeleeCharge = 0;
                    Main.projectile[proj].ai[1] = 1;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
