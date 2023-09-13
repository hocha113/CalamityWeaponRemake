using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Interfaces;
using CalamityWeaponRemake.Content.Projectiles.Cosmic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items
{
    internal class DimensionalRupture : CustomItems
    {
        public override string Texture => CWRConstant.Item + "DimensionalRupture";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.width = 44;
            Item.height = 44;
            Item.damage = 258;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MightyStar>();
            Item.shootSpeed = 2.4f;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {            
            return false;
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
