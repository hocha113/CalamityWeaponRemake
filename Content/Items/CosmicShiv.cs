using CalamityMod.Items;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.Rarities;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Projectiles.Cosmic;

namespace CalamityWeaponRemake.Content.Items
{
    internal class CosmicShiv : CustomItems
    {
        public override string Texture => CWRConstant.Item + "CosmicShiv";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.width = 44;
            Item.height = 44;
            Item.damage = 218;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CosmicShivProjectile>();
            Item.shootSpeed = 2.4f;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.Calamity().donorItem = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            
        }       

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            
        }

        public override void UpdateInventory(Player player)
        {
            
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}
