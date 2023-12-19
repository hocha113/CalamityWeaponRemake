using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Effects;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using CalamityWeaponRemake.Content.UIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    public class CWRMod : Mod
    {
        internal static CWRMod Instance;
        internal Mod musicMod = null;
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
        }

        public override void Load(){
            Instance = this;
            RItemInstances = new List<BaseRItem>();
            FindMod();
            LoadClient();
            new CompressorUI().Load();
            new SupertableUI().Load();
            CWRParticleHandler.Load();
            EffectsRegistry.LoadEffects();
            On_Main.DrawInfernoRings += PeSystem.CWRDrawForegroundParticles;
            base.Load();
        }

        public override void Unload(){
            CWRParticleHandler.Unload();
            On_Main.DrawInfernoRings -= PeSystem.CWRDrawForegroundParticles;
            base.Unload();
        }

        public void FindMod(){
            musicMod = null;
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
        }

        public void LoadClient(){
            if (Main.dedServ)
                return;

            MusicLoader.AddMusicBox(Instance, MusicLoader.GetMusicSlot("CalamityWeaponRemake/Assets/Sounds/Music/BuryTheLight"), Find<ModItem>("FoodStallChair").Type, Find<ModTile>("FoodStallChair").Type, 0);
        }
    }
}