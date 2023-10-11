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
        public static string Item_Magic = "CalamityWeaponRemake/Assets/Items/Magic/";
        public static string Item_Melee = "CalamityWeaponRemake/Assets/Items/Melee/";
        public static string Item_Ranged = "CalamityWeaponRemake/Assets/Items/Ranged/";
        public static string Item_Summon = "CalamityWeaponRemake/Assets/Items/Summon/";
        public static string Projectile = "CalamityWeaponRemake/Assets/Projectiles/";
        public static string Projectile_Magic = "CalamityWeaponRemake/Assets/Projectiles/Magic/";
        public static string Projectile_Melee = "CalamityWeaponRemake/Assets/Projectiles/Melee/";
        public static string Projectile_Ranged = "CalamityWeaponRemake/Assets/Projectiles/Ranged/";
        public static string Projectile_Summon = "CalamityWeaponRemake/Assets/Projectiles/Summon/";
        public static string UI = Asset + "UIs/";
        public static string Buff = Asset + "Buffs/";

        public static string noEffects = "Assets/Effects/";
        public static string noItem = "Assets/Items/";
        public static string noProjectile = "Assets/Projectiles/";

        public static bool ForceReplaceResetContent => ModContent.GetInstance<ContentConfig>().ForceReplaceResetContent;
        public static bool WeaponEnhancementSystem => ModContent.GetInstance<ContentConfig>().WeaponEnhancementSystem;
    }
}
