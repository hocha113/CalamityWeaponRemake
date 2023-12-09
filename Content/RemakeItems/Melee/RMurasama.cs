using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RMurasama : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Murasama>(item))
            {
                item.height = 128;
                item.width = 56;
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
                item.shoot = ModContent.ProjectileType<MurasamaSlashs>();
                item.shootSpeed = 24f;
                item.rare = ModContent.RarityType<Violet>();
                Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 14));
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Murasama>(item))
            {
                item.damage = Murasama.GetOnDamage;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Murasama>(item))
            {
                return player.ownedProjectileCounts[item.shoot] == 0;
            }
            return base.CanUseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Murasama>(item))
            {
                Projectile.NewProjectile(source, position, velocity, type, Murasama.GetOnDamage, knockback, player.whoAmI, 0f, 0f);
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
