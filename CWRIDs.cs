using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SupremeCalamitas;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    public static class CWRIDs
    {
        public static bool OnLoadContentBool = true;

        public static Dictionary<int, int> TileToItem = new Dictionary<int, int>();
        public static Dictionary<int, int> WallToItem = new Dictionary<int, int>();

        /// <summary>
        /// 坟墓灾虫
        /// </summary>
        public static List<int> targetNpcTypes;
        public static int SepulcherHead;
        public static int SepulcherBody;
        public static int SepulcherTail;
        /// <summary>
        /// 风暴吞噬者
        /// </summary>
        public static List<int> targetNpcTypes2;
        public static int StormWeaverHead;
        public static int StormWeaverBody;
        public static int StormWeaverTail;
        /// <summary>
        /// 幻海妖龙 
        /// </summary>
        public static List<int> targetNpcTypes3;
        public static int PrimordialWyrmHead;
        public static int PrimordialWyrmBody;
        public static int PrimordialWyrmTail;
        /// <summary>
        /// 血肉蠕虫 
        /// </summary>
        public static List<int> targetNpcTypes4;
        public static int PerforatorHeadLarge;
        public static int PerforatorBodyLarge;
        public static int PerforatorTailLarge;
        /// <summary>
        /// 血肉蠕虫2 
        /// </summary>
        public static List<int> targetNpcTypes5;
        public static int PerforatorHeadMedium;
        public static int PerforatorBodyMedium;
        public static int PerforatorTailMedium;
        /// <summary>
        /// 装甲掘地虫 
        /// </summary>
        public static List<int> targetNpcTypes6;
        public static int ArmoredDiggerHead;
        public static int ArmoredDiggerBody;
        public static int ArmoredDiggerTail;
        /// <summary>
        /// 星流巨械
        /// </summary>
        public static List<int> targetNpcTypes7;
        public static int Apollo;
        public static int Artemis;
        public static int AresBody;
        public static int ThanatosHead;
        public static int ThanatosBody1;
        public static int ThanatosBody2;
        public static int ThanatosTail;
        /// <summary>
        /// 神明吞噬者
        /// </summary>
        public static List<int> targetNpcTypes8;
        public static int DevourerofGodsHead;
        public static int DevourerofGodsBody;
        public static int DevourerofGodsTail;
        public static int CosmicGuardianHead;
        public static int CosmicGuardianBody;
        public static int CosmicGuardianTail;
        /// <summary>
        /// 荒漠灾虫
        /// </summary>
        public static List<int> targetNpcTypes9;
        public static int DesertScourgeHead;
        public static int DesertScourgeBody;
        public static int DesertScourgeTail;
        public static int DesertNuisanceHead;
        public static int DesertNuisanceBody;
        public static int DesertNuisanceTail;
        /// <summary>
        /// 星神游龙
        /// </summary>
        public static List<int> targetNpcTypes10;
        public static int AstrumDeusHead;
        public static int AstrumDeusBody;
        public static int AstrumDeusTail;
        /// <summary>
        /// 沙海狂龙
        /// </summary>
        public static List<int> targetNpcTypes11;
        public static int AquaticScourgeHead;
        public static int AquaticScourgeBody;
        public static int AquaticScourgeTail;
        /// <summary>
        /// 幻海妖龙幼年体
        /// </summary>
        public static List<int> targetNpcTypes12;
        public static int EidolonWyrmHead;
        public static int EidolonWyrmBody;
        public static int EidolonWyrmBodyAlt;
        public static int EidolonWyrmTail;
        /// <summary>
        /// 月球领主
        /// </summary>
        public static List<int> targetNpcTypes13;
        /// <summary>
        /// 世界吞噬者
        /// </summary>
        public static List<int> targetNpcTypes14;
        /// <summary>
        /// 毁灭者
        /// </summary>
        public static List<int> targetNpcTypes15;

        public static void Load() {

            SepulcherHead = ModContent.NPCType<SepulcherHead>();
            SepulcherBody = ModContent.NPCType<SepulcherBody>();
            SepulcherTail = ModContent.NPCType<SepulcherTail>();

            StormWeaverHead = ModContent.NPCType<StormWeaverHead>();
            StormWeaverBody = ModContent.NPCType<StormWeaverBody>();
            StormWeaverTail = ModContent.NPCType<StormWeaverTail>();

            PrimordialWyrmHead = ModContent.NPCType<PrimordialWyrmHead>();
            PrimordialWyrmBody = ModContent.NPCType<PrimordialWyrmBody>();
            PrimordialWyrmTail = ModContent.NPCType<PrimordialWyrmTail>();

            PerforatorHeadLarge = ModContent.NPCType<PerforatorHeadLarge>();
            PerforatorBodyLarge = ModContent.NPCType<PerforatorBodyLarge>();
            PerforatorTailLarge = ModContent.NPCType<PerforatorTailLarge>();

            PerforatorHeadMedium = ModContent.NPCType<PerforatorHeadMedium>();
            PerforatorBodyMedium = ModContent.NPCType<PerforatorBodyMedium>();
            PerforatorTailMedium = ModContent.NPCType<PerforatorTailMedium>();

            ArmoredDiggerHead = ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.ArmoredDiggerHead>();
            ArmoredDiggerBody = ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.ArmoredDiggerBody>();
            ArmoredDiggerTail = ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.ArmoredDiggerTail>();

            Apollo = ModContent.NPCType<Apollo>();
            Artemis = ModContent.NPCType<Artemis>();
            AresBody = ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Ares.AresBody>();
            ThanatosHead = ModContent.NPCType<ThanatosHead>();
            ThanatosBody1 = ModContent.NPCType<ThanatosBody1>();
            ThanatosBody2 = ModContent.NPCType<ThanatosBody2>();
            ThanatosTail = ModContent.NPCType<ThanatosTail>();

            DevourerofGodsHead = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsHead>();
            DevourerofGodsBody = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();
            DevourerofGodsTail = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsTail>();
            CosmicGuardianHead = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.CosmicGuardianHead>();
            CosmicGuardianBody = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.CosmicGuardianBody>();
            CosmicGuardianTail = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.CosmicGuardianTail>();

            DesertScourgeHead = ModContent.NPCType<DesertScourgeHead>();
            DesertScourgeBody = ModContent.NPCType<DesertScourgeBody>();
            DesertScourgeTail = ModContent.NPCType<DesertScourgeTail>();
            DesertNuisanceHead = ModContent.NPCType<DesertNuisanceHead>();
            DesertNuisanceBody = ModContent.NPCType<DesertNuisanceBody>();
            DesertNuisanceTail = ModContent.NPCType<DesertNuisanceTail>();

            AstrumDeusHead = ModContent.NPCType<AstrumDeusHead>();
            AstrumDeusBody = ModContent.NPCType<AstrumDeusBody>();
            AstrumDeusTail = ModContent.NPCType<AstrumDeusTail>();

            AquaticScourgeHead = ModContent.NPCType<AquaticScourgeHead>();
            AquaticScourgeBody = ModContent.NPCType<AquaticScourgeBody>();
            AquaticScourgeTail = ModContent.NPCType<AquaticScourgeTail>();

            EidolonWyrmHead = ModContent.NPCType<EidolonWyrmHead>();
            EidolonWyrmBody = ModContent.NPCType<EidolonWyrmBody>();
            EidolonWyrmBodyAlt = ModContent.NPCType<EidolonWyrmBodyAlt>();
            EidolonWyrmTail = ModContent.NPCType<EidolonWyrmTail>();

            targetNpcTypes = new List<int> { SepulcherHead, SepulcherBody, SepulcherTail };
            targetNpcTypes2 = new List<int> { StormWeaverHead, StormWeaverBody, StormWeaverTail };
            targetNpcTypes3 = new List<int> { PrimordialWyrmHead, PrimordialWyrmBody, PrimordialWyrmTail };
            targetNpcTypes4 = new List<int> { PerforatorHeadLarge, PerforatorBodyLarge, PerforatorTailLarge };
            targetNpcTypes5 = new List<int> { PerforatorHeadMedium, PerforatorBodyMedium, PerforatorTailMedium };
            targetNpcTypes6 = new List<int> { ArmoredDiggerHead, ArmoredDiggerBody, ArmoredDiggerTail };
            targetNpcTypes7 = new List<int> { Apollo, Artemis, AresBody, ThanatosHead, ThanatosBody1, ThanatosBody2, ThanatosTail };
            targetNpcTypes8 = new List<int> { DevourerofGodsHead, DevourerofGodsBody, DevourerofGodsTail, CosmicGuardianHead, CosmicGuardianBody, CosmicGuardianTail };
            targetNpcTypes9 = new List<int> { DesertScourgeHead, DesertScourgeBody, DesertScourgeTail, DesertNuisanceHead, DesertNuisanceBody, DesertNuisanceTail };
            targetNpcTypes10 = new List<int> { AstrumDeusHead, AstrumDeusBody, AstrumDeusTail };
            targetNpcTypes11 = new List<int> { AquaticScourgeHead, AquaticScourgeBody, AquaticScourgeTail };
            targetNpcTypes12 = new List<int> { EidolonWyrmHead, EidolonWyrmBody, EidolonWyrmBodyAlt, EidolonWyrmTail };
            targetNpcTypes13 = new List<int> { NPCID.MoonLordFreeEye, NPCID.MoonLordCore, NPCID.MoonLordHand, NPCID.MoonLordHead, NPCID.MoonLordLeechBlob };
            targetNpcTypes14 = new List<int> { NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail };
            targetNpcTypes15 = new List<int> { NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail };

            OnLoadContentBool = false;
        }
    }
}
