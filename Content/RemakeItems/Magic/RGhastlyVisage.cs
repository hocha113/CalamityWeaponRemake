using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Magic.HeldProjs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Magic
{
    internal class RGhastlyVisage : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.GhastlyVisage>(item))
            {
                item.damage = 69;
                item.DamageType = DamageClass.Magic;
                item.noUseGraphic = true;
                item.channel = true;
                item.mana = 20;
                item.width = 32;
                item.height = 36;
                item.useTime = 27;
                item.useAnimation = 27;
                item.useStyle = ItemUseStyleID.Shoot;
                item.noMelee = true;
                item.knockBack = 5f;
                item.shootSpeed = 9f;
                item.shoot = ModContent.ProjectileType<RemakeGhastlyVisageProj>();
                item.value = CalamityGlobalItem.Rarity13BuyPrice;
                item.rare = ModContent.RarityType<PureGreen>();
            }
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.GhastlyVisage>(item))
            {
                player.statMana += manaConsumed;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Magic.GhastlyVisage>(item))
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<RemakeGhastlyVisageProj>(), damage, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
