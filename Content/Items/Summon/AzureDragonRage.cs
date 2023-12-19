using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Summon.Whips;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Summon
{
    internal class AzureDragonRage : ModItem
    {
        public override string Texture => CWRConstant.Item_Summon + "AzureDragonRage";

        public override void SetDefaults() {
            Item.DefaultToWhip(ModContent.ProjectileType<AzureDragonRageProjectile>(), 272, 2.5f, 13, 35);
            Item.rare = ItemRarityID.Purple;
        }

        public override bool MeleePrefix() {
            return true;
        }
    }
}
