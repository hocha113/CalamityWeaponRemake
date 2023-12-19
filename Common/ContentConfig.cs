using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CalamityWeaponRemake.Common
{
    [BackgroundColor(49, 32, 36, 216)]
    public class ContentConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [BackgroundColor(192, 54, 64, 192)]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool ForceReplaceResetContent { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool WeaponEnhancementSystem { get; set; }

        [System.Obsolete]
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {
            return true;
        }
    }
}
