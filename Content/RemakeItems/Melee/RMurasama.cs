using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
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
    internal class RMurasama : BaseRItem
    {
        public override void Load() {
            SetReadonlyTargetID = ModContent.ItemType<CalamityMod.Items.Weapons.Melee.Murasama>();
        }
        public override void SetDefaults(Item item)
        {
            item.height = 134;
            item.width = 90;
            item.damage = 2222;
            item.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = true;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTime = 5;
            item.knockBack = 6.5f;
            item.autoReuse = false;
            item.value = CalamityGlobalItem.Rarity15BuyPrice;
            item.shoot = ModContent.ProjectileType<MurasamaSlash>();
            item.shootSpeed = 24f;
            item.rare = ModContent.RarityType<Violet>();
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 14));
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "Murasama", 3);
        }

        public override void HoldItem(Item item, Player player)
        {
            item.damage = Murasama.GetOnDamage;
        }

        public override bool? CanUseItem(Item item, Player player)
        {
            return player.ownedProjectileCounts[item.shoot] == 0;
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, Murasama.GetOnDamage, knockback, player.whoAmI, 0f, 0f);
            Main.projectile[proj].scale = Murasama.GetOnScale;
            return false;
        }
    }
}
