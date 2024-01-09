using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Rogue.Extras;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityWeaponRemake.Content
{
    internal class CWRWorld : ModSystem
    {
        public static bool TitleMusicBoxEasterEgg = true;

        public override void ClearWorld() {
            TitleMusicBoxEasterEgg = true;
        }

        public override void NetReceive(BinaryReader reader) {
            TitleMusicBoxEasterEgg = reader.ReadBoolean();
        }

        public override void NetSend(BinaryWriter writer) {
            writer.Write(TitleMusicBoxEasterEgg);
        }

        public override void OnWorldLoad() {
            if (SupertableUI.Instance != null) {
                SupertableUI.Instance.loadOrUnLoadZenithWorldAsset = true;
                SupertableUI.Instance.Active = false;
            }
            if (RecipeUI.Instance != null) {
                RecipeUI.Instance.index = 0;
                RecipeUI.Instance.LoadPsreviewItems();
            }
            Gangarus.ZenithWorldAsset();
        }

        public override void SaveWorldData(TagCompound tag) {
            tag.Add("_TitleMusicBoxEasterEgg", TitleMusicBoxEasterEgg);
        }

        public override void LoadWorldData(TagCompound tag) {
            TitleMusicBoxEasterEgg = tag.GetBool("_TitleMusicBoxEasterEgg");
        }
    }
}
