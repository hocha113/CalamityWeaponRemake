using CalamityMod.Particles;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Effects;
using CalamityWeaponRemake.Content.Particles.Core;
using System.Reflection.Metadata;
using Terraria;
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
            LoadClient();
            CWRParticleHandler.Load();
            EffectsRegistry.LoadEffects();
            On_Main.DrawInfernoRings += PeSystem.CWRDrawForegroundParticles;
            base.Load();
        }

        public override void Unload()
        {
            CWRParticleHandler.Unload();
            On_Main.DrawInfernoRings -= PeSystem.CWRDrawForegroundParticles;
            base.Unload();
        }

        public void FindMod()
        {
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
        }

        public void LoadClient()
        {
            if (Main.dedServ)
                return;

            MusicLoader.AddMusicBox(Instance, MusicLoader.GetMusicSlot("CalamityWeaponRemake/Assets/Sounds/Music/Funkytown"), Find<ModItem>("FoodStallChair").Type, Find<ModTile>("FoodStallChair").Type, 0);
        }
    }
}