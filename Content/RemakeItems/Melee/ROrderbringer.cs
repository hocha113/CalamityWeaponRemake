using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class ROrderbringer : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.Orderbringer>();
        public override void SetDefaults(Item item) {
            item.width = item.height = 108;
            item.damage = 228;
            item.DamageType = DamageClass.Melee;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.Swing;
            item.useTime = 18;
            item.useTurn = true;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.value = CalamityGlobalItem.Rarity14BuyPrice;
            item.rare = ModContent.RarityType<DarkBlue>();
            item.shoot = ModContent.ProjectileType<OrderbringerBeams>();
            item.shootSpeed = 6f;
        }
    }
}
