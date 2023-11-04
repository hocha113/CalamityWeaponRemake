using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    internal class CWRTiles : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            base.RightClick(i, j, type);

            Mod musicMod = CalamityWeaponRemake.Instance.musicMod;
            if (musicMod is not null)
            {
                if (type == musicMod.Find<ModTile>("CalamityTitleMusicBox").Type 
                    && !NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitas>()))
                {
                    CalamityPlayer modPlayer = Main.LocalPlayer.Calamity();

                    if (!CWRWorld.TitleMusicBoxEasterEgg)
                    {
                        if (modPlayer.sCalKillCount <= 0)
                        {
                            return;
                        }
                    }

                    CalamityUtils.DisplayLocalizedText(
                        CWRUtils.Translation(
                            "要来一场音乐的狂欢吗？",
                            "Want to have a musical orgy?")
                        , Color.Pink);

                    if (!CWRUtils.isClient)
                    {
                        int npc = CWRUtils.NewNPCEasy(null, new Vector2(i, j) * 16 + new Vector2(0, -32)
                        , ModContent.NPCType<SupremeCalamitas>());

                        Main.npc[npc].CWR().SprBoss = true;
                        Main.npc[npc].life = Main.npc[npc].lifeMax = 66666666;
                        Main.npc[npc].damage *= 2;
                        Main.npc[npc].netUpdate = true;
                        Main.npc[npc].netUpdate2 = true;

                        CWRWorld.TitleMusicBoxEasterEgg = false;
                        CalamityNetcode.SyncWorld();
                    }
                }
            }  
        }
    }
}
