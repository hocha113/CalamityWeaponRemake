using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Magic.HeldProjs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RAirSpinner : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.AirSpinner>(item))
            {
                item.width = 28;
                item.height = 28;
                item.DamageType = DamageClass.MeleeNoSpeed;
                item.damage = 26;
                item.knockBack = 4f;
                item.useTime = 22;
                item.useAnimation = 22;
                item.autoReuse = true;
                item.useStyle = ItemUseStyleID.Shoot;
                item.UseSound = SoundID.Item1;
                item.channel = true;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.shoot = ModContent.ProjectileType<RemakeAirSpinnerYoyo>();
                item.shootSpeed = 14f;
                item.rare = ItemRarityID.Orange;
                item.value = CalamityGlobalItem.Rarity3BuyPrice;
            }
        }
    }
}
