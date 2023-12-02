using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RStreamGouge : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.StreamGouge>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.StreamGouge>(item))
            {
                item.width = 100;
                item.height = 100;
                item.damage = 440;
                item.DamageType = DamageClass.Melee;
                item.noMelee = true;
                item.useTurn = true;
                item.noUseGraphic = true;
                item.channel = true;
                item.useAnimation = 19;
                item.useTime = 19;
                item.useStyle = 5;
                item.knockBack = 9.75f;
                item.UseSound = SoundID.Item1;
                item.autoReuse = true;
                item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
                item.shoot = ModContent.ProjectileType<RemakeStreamGougeProj>();
                item.shootSpeed = 15f;
                item.rare = ModContent.RarityType<DarkBlue>();
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.StreamGouge>(item))
            {
                CWRUtils.OnModifyTooltips(Mod, item, tooltips, "StreamGouge", 3);
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.StreamGouge>(item))
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                    Main.projectile[proj].localAI[1] = 1;
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.StreamGouge>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.StreamGouge>(item))
            {
                if (player.ownedProjectileCounts[item.shoot] > 0)
                    return false;
            }
            return base.UseItem(item, player);
        }
    }
}
