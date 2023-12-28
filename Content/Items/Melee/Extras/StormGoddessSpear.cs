using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.StormGoddessSpearProj;

namespace CalamityWeaponRemake.Content.Items.Melee.Extras
{
    internal class StormGoddessSpear : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "StormGoddessSpear";
        public override void SetDefaults() {
            Item.width = 100;
            Item.height = 100;
            Item.damage = 440;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 19;
            Item.useTime = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.shoot = ModContent.ProjectileType<StormGoddessSpearProj>();
            Item.shootSpeed = 15f;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.CWR().remakeItem = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return true;
        }

        public override bool CanUseItem(Player player) {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StormGoddessSpearProj>()] <= 0;
        }
    }
}
