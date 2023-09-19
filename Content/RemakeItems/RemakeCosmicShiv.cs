using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles.Cosmic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems
{
    public class RemakeCosmicShiv : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.CosmicShiv>())
            {
                item.useStyle = ItemUseStyleID.Rapier;
                item.DamageType = DamageClass.Melee;
                item.useAnimation = 15;
                item.useTime = 15;
                item.width = 44;
                item.height = 44;
                item.damage = 218;
                item.knockBack = 9f;
                item.UseSound = SoundID.Item1;
                item.autoReuse = true;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.shoot = ModContent.ProjectileType<CosmicShivProjectile>();
                item.shootSpeed = 2.4f;
                item.value = CalamityGlobalItem.Rarity14BuyPrice;
                item.rare = ModContent.RarityType<DarkBlue>();
                item.Calamity().donorItem = true;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
