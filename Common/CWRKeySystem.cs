using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    internal class CWRKeySystem : ModSystem
    {
        public static ModKeybind HeavenfallLongbowSkillKey { get; private set; }
        public static ModKeybind InfinitePickSkillKey { get; private set; }
        public static ModKeybind TOM_OneClickP { get; private set; }
        public static ModKeybind TOM_QuickFetch { get; private set; }
        public static ModKeybind TOM_GatheringItem { get; private set; }
        public static ModKeybind TOM_GlobalRecall { get; private set; }

        public override void Load() {
            HeavenfallLongbowSkillKey = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("天堂陨落长弓:万象审判", "HeavenfallLongbow Skill Key"), "Q");
            InfinitePickSkillKey = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("无尽之镐:形态切换", "Endless Pick form switch key"), "C");
            TOM_OneClickP = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("欧米茄合成:一键摆放", "Omega synthesis: One-click placement"), "L");
            TOM_QuickFetch = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("欧米茄合成:快速拿取", "Omega synthesis: Quick fetch"), "LeftShift");
            TOM_GatheringItem = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("欧米茄合成:聚拢物品", "Omega synthesis: Gathering item"), "G");
            TOM_GlobalRecall = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("欧米茄合成:全部收回", "Omega synthesis: Global recall"), "K");
        }

        public override void Unload() {
            HeavenfallLongbowSkillKey = null;
            InfinitePickSkillKey = null;
            TOM_OneClickP = null;
            TOM_QuickFetch = null;
            TOM_GlobalRecall = null;
        }
    }
}
