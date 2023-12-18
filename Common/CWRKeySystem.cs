using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    internal class CWRKeySystem : ModSystem
    {
        public static ModKeybind HeavenfallLongbowSkillKey { get; private set; }
        public static ModKeybind InfinitePickSkillKey { get; private set; }

        public override void Load()
        {
            HeavenfallLongbowSkillKey = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("天堂陨落长弓技能键", "HeavenfallLongbow Skill Key"), "Q");
            InfinitePickSkillKey = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("无尽之镐形态切换键", "Endless Pick form switch key"), "C");
        }

        public override void Unload()
        {
            HeavenfallLongbowSkillKey = null;
            InfinitePickSkillKey = null;
        }
    }
}
