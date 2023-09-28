using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    public static class CWRConstant
    {
        public static string Asset = "CalamityWeaponRemake/Assets/";
        public static string Effects = Asset + "Effects/";
        public static string placeholder = Asset + "placeholder";
        public static string Masking = "CalamityWeaponRemake/Assets/Masking/";
        public static string Item = "CalamityWeaponRemake/Assets/Items/";
        public static string Projectile = "CalamityWeaponRemake/Assets/Projectiles/";

        public static string noEffects = "Assets/Effects/";
        public static string noItem = "Assets/Items/";
        public static string noProjectile = "Assets/Projectiles/";

        public static bool ForceReplaceResetContent => ModContent.GetInstance<ContentConfig>().ForceReplaceResetContent;
        public static bool WeaponEnhancementSystem => ModContent.GetInstance<ContentConfig>().WeaponEnhancementSystem;
    }
}
