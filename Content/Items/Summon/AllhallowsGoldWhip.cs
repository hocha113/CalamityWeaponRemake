using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Summon.Whips;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Summon
{
    internal class AllhallowsGoldWhip : ModItem
    {
        public override string Texture => CWRConstant.Item_Summon + "AllhallowsGoldWhip";

        public override void SetDefaults() {
            Item.DefaultToWhip(ModContent.ProjectileType<AllhallowsGoldWhipProjectile>(), 902, 0, 12, 45);
            Item.rare = ItemRarityID.Green;
            Item.channel = true;
        }

        public override bool MeleePrefix() {
            return true;
        }
    }
}
