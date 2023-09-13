using CalamityWeaponRemake.Common.AuxiliaryMeans;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CalamityWeaponRemake.Common
{
    [BackgroundColor(49, 32, 36, 216)]
    public class ContentConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public ContentConfig()
        {
            ForceReplaceResetContent = true;
            WeaponEnhancementSystem = false;
        }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool ForceReplaceResetContent { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool WeaponEnhancementSystem { get; set; }

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }
    }
}
