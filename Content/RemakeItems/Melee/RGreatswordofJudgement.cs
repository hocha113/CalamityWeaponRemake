using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RGreatswordofJudgement : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GreatswordofJudgement>();
        public override void SetDefaults(Item item) {
            item.width = 78;
            item.damage = 40;
            item.DamageType = DamageClass.Melee;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.Swing;
            item.useTime = 18;
            item.useTurn = true;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 78;
            item.value = CalamityGlobalItem.Rarity10BuyPrice;
            item.rare = ItemRarityID.Red;
            item.shoot = ModContent.ProjectileType<JudgementBeam>();
            item.shootSpeed = 15f;
        }
    }
}
