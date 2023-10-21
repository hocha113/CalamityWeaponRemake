using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Projectiles.Summon;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Items.Summon
{
    internal class WhiplashGalactica : ModItem
    {
        public override string Texture => CWRConstant.Item_Summon + "WhiplashGalactica";

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<WhiplashGalacticaProjectile>(), 202, 2, 12, 45);
            Item.rare = ItemRarityID.Green;
            Item.channel = true;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
