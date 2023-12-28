using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    /// <summary>
    /// 天蓝悠悠球
    /// </summary>
    internal class AirSpinner : ModItem
    {
        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override string Texture => CWRConstant.Cay_Wap_Melee + "AirSpinner";

        public override void SetStaticDefaults() {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 28;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 26;
            Item.knockBack = 4f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<RAirSpinnerYoyo>();
            Item.shootSpeed = 14f;
            Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.CWR().remakeItem = true;
        }
    }
}
