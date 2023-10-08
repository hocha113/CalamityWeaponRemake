using CalamityMod.Items;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class GildedProboscis : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "GildedProboscis";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[base.Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 315;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = 5;
            Item.useTime = 19;
            Item.knockBack = 8.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = CalamityGlobalItem.Rarity11BuyPrice;
            Item.rare = 11;
            Item.shoot = ModContent.ProjectileType<RemakeGildedProboscisProj>();
            Item.shootSpeed = 13f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }
}
