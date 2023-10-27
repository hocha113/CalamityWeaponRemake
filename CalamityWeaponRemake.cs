using CalamityWeaponRemake.Common.Effects;
using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    public class CalamityWeaponRemake : Mod
    {
        internal static CalamityWeaponRemake Instance;

        public override void Load()
        {
            Instance = this;

            EffectsRegistry.LoadEffects();

            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}