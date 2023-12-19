using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Summon.Whips;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Summon
{
    internal class ElementWhip : ModItem
    {
        public override string Texture => CWRConstant.Item_Summon + "ElementWhip";

        public override void SetDefaults() {
            Item.DefaultToWhip(ModContent.ProjectileType<ElementWhipProjectile>(), 192, 2, 12, 30);
            Item.rare = ItemRarityID.Purple;
        }

        public override bool MeleePrefix() {
            return true;
        }
    }
}
