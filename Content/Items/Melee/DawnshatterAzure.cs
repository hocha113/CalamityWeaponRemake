using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.DawnshatterAzureProj;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class DawnshatterAzure : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "DawnshatterAzure";
        public override void SetDefaults() {
            Item.height = Item.width = 54;
            Item.damage = 11709;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<DawnshatterAzureProj>();
            Item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanUseItem(Player player) {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }
}
