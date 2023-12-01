using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RAegisBlade : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.AegisBlade>(item))
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
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.AegisBlade>(item))
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
            return base.CanUseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.AegisBlade>(item))
            {
                int damages = damage;
                if (player.altFunctionUse == 2)
                {
                    damages = (int)(damage * 3.3f);
                }
                else
                {
                    damages = (int)(damage * 0.3f);
                }
                _ = Projectile.NewProjectile(source, position, velocity, item.shoot, damages, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.AegisBlade>(item))
            {
                List<TooltipLine> newTooltips = new List<TooltipLine>(tooltips);

                foreach (TooltipLine line in tooltips.ToList()) //复制 tooltips 集合，以便在遍历时修改
                {
                    if (line.Name == "Tooltip0" || line.Name == "Tooltip1")
                        line.Hide();
                }

                TooltipLine newLine = new TooltipLine(Mod, "CWRText"
                    , Language.GetText("Mods.CalamityWeaponRemake.Items.AegisBlade.Tooltip").Value);
                newTooltips.Add(newLine);

                tooltips.Clear(); // 清空原 tooltips 集合
                tooltips.AddRange(newTooltips); // 添加修改后的 newTooltips 集合
                CWRItems.AppAwakeningLine(tooltips);
            }
        }
    }
}
