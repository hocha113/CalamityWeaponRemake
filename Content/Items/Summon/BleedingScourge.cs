using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Summon.Whips;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Summon
{
    internal class BleedingScourge : ModItem
    {
        public override string Texture => CWRConstant.Item_Summon + "BleedingScourge";

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<BleedingScourgeProjectile>(), 591, 3, 13, 40);
            Item.rare = ItemRarityID.Purple;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
