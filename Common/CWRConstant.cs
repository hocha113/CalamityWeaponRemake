using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    public static class CWRConstant
    {
        public const string Asset = "CalamityWeaponRemake/Assets/";
        public const string Effects = Asset + "Effects/";
        public const string placeholder = Asset + "placeholder";
        public const string Masking = "CalamityWeaponRemake/Assets/Masking/";
        public const string Item = "CalamityWeaponRemake/Assets/Items/";
        public const string Item_Magic = "CalamityWeaponRemake/Assets/Items/Magic/";
        public const string Item_Melee = "CalamityWeaponRemake/Assets/Items/Melee/";
        public const string Item_Ranged = "CalamityWeaponRemake/Assets/Items/Ranged/";
        public const string Item_Summon = "CalamityWeaponRemake/Assets/Items/Summon/";
        public const string Projectile = "CalamityWeaponRemake/Assets/Projectiles/";
        public const string Projectile_Magic = "CalamityWeaponRemake/Assets/Projectiles/Magic/";
        public const string Projectile_Melee = "CalamityWeaponRemake/Assets/Projectiles/Melee/";
        public const string Projectile_Ranged = "CalamityWeaponRemake/Assets/Projectiles/Ranged/";
        public const string Projectile_Summon = "CalamityWeaponRemake/Assets/Projectiles/Summon/";
        public const string UI = Asset + "UIs/";
        public const string Buff = Asset + "Buffs/";
        public const string Dust = Asset + "Dusts/";

        public const string noEffects = "Assets/Effects/";
        public const string noItem = "Assets/Items/";
        public const string noProjectile = "Assets/Projectiles/";

        public static bool ForceReplaceResetContent => ModContent.GetInstance<ContentConfig>().ForceReplaceResetContent;
        public static bool WeaponEnhancementSystem => ModContent.GetInstance<ContentConfig>().WeaponEnhancementSystem;
    }
}
