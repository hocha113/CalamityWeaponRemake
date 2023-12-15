using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    internal class CWRIDs
    {
        public static int BlightedCleaver;

        public void Load() {
            BlightedCleaver = ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BlightedCleaver>();
        }
    }
}
