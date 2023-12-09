using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    internal class CWRKeySystem : ModSystem
    {
        public static ModKeybind HeavenfallLongbowSkillKey { get; private set; }

        public override void Load()
        {
            HeavenfallLongbowSkillKey = KeybindLoader.RegisterKeybind(Mod, CWRUtils.Translation("天堂陨落长弓技能键", "HeavenfallLongbow Skill Key"), "Q");
        }

        public override void Unload()
        {
            HeavenfallLongbowSkillKey = null;
        }
    }
}
