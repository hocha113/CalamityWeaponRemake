using CalamityWeaponRemake.Common.Effects;
using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    public class CalamityWeaponRemake : Mod
    {
        internal static CalamityWeaponRemake Instance;

        internal Mod musicMod = null;

        public override void Load()
        {
            Instance = this;

            FindMod();
            EffectsRegistry.LoadEffects();

            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public void FindMod()
        {
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
        }
    }
}