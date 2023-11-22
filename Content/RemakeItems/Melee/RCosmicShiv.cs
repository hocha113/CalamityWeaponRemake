using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    public class RCosmicShiv : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.CosmicShiv>() && CWRConstant.ForceReplaceResetContent)
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
                item.shoot = ModContent.ProjectileType<RemakeCosmicShivProjectile>();
                item.shootSpeed = 2.4f;
                item.value = CalamityGlobalItem.Rarity14BuyPrice;
                item.rare = ModContent.RarityType<DarkBlue>();
                item.Calamity().donorItem = true;
            }
        }
    }
}
