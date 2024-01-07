using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Effects;
using CalamityWeaponRemake.Content.NPCs;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using CalamityWeaponRemake.Content.UIs;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    public class CWRMod : Mod
    {
        internal static CWRMod Instance;
        internal Mod musicMod = null;
        internal Mod betterWaveSkipper = null;
        internal Mod fargowiltasSouls = null;
        internal List<Mod> LoadMods;
        internal static List<BaseRItem> RItemInstances;

        public override void PostSetupContent() {
            LoadMods = ModLoader.Mods.ToList();
            List<Type> rItemIndsTypes = CWRUtils.GetSubclasses(typeof(BaseRItem));
            foreach (Type type in rItemIndsTypes) {
                if (type != typeof(BaseRItem)) {
                    object obj = Activator.CreateInstance(type);
                    if (obj is BaseRItem inds) {
                        inds.Load();
                        inds.SetStaticDefaults();
                        //最后再判断一下TargetID是否为0，因为如果这是一个有效的Ritem实例，那么它的TargetID就不可能为0，否则将其添加进去会导致LoadRecipe部分报错
                        if (inds.TargetID != 0)
                            RItemInstances.Add(inds);
                    }
                }
            }
            //加载一次ID列表，从这里加载可以保障所有内容已经添加好了
            CWRIDs.Load();
            //加载生物定义
            new PerforatorBehavior().Load();
            //将自定义的UI放到最后加载，在这之前是确保物品、ID、生物等其他内容都加载完成后
            new CompressorUI().Load();
            new SupertableUI().Load();
            new RecipeUI().Load();
            new DragButton().Load();
        }

        public override void Load() {
            Instance = this;
            RItemInstances = new List<BaseRItem>();
            FindMod();
            LoadClient();
            
            CWRParticleHandler.Load();
            EffectsRegistry.LoadEffects();
            On_Main.DrawInfernoRings += PeSystem.CWRDrawForegroundParticles;
            base.Load();
        }

        public override void Unload() {
            CWRParticleHandler.Unload();
            On_Main.DrawInfernoRings -= PeSystem.CWRDrawForegroundParticles;
            base.Unload();
        }

        public void FindMod() {
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
            ModLoader.TryGetMod("BetterWaveSkipper", out betterWaveSkipper);
            ModLoader.TryGetMod("FargowiltasSouls", out fargowiltasSouls);
        }

        public void LoadClient() {
            if (Main.dedServ)
                return;

            MusicLoader.AddMusicBox(Instance, MusicLoader.GetMusicSlot("CalamityWeaponRemake/Assets/Sounds/Music/BuryTheLight"), Find<ModItem>("FoodStallChair").Type, Find<ModTile>("FoodStallChair").Type, 0);
        }
    }
}