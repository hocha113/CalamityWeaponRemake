﻿using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DraedonLabThings;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.Items.Placeable;
using CalamityWeaponRemake.Content.Items.Rogue.Extras;
using CalamityWeaponRemake.Content.Items.Tools;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityWeaponRemake
{
    public static class CWRIDs
    {
        public static bool OnLoadContentBool = true;

        public static int DarkMatterBall;
        public static int BlackMatterStick;
        public static int InfiniteStick;
        public static int EndlessStabilizer;
        public static int DubiousPlating;
        public static int FoodStallChair;
        public static int Gangarus;
        public static int StarMyriadChanges;

        public static int EternitySoul;

        /// <summary>
        /// 无尽锭
        /// </summary>
        public static int[] MaterialsTypes;
        /// <summary>
        /// 湮宇星矢
        /// </summary>
        public static int[] MaterialsTypes2;
        /// <summary>
        /// 天堂陨落
        /// </summary>
        public static int[] MaterialsTypes3;
        /// <summary>
        /// 无尽镐
        /// </summary>
        public static int[] MaterialsTypes4;
        /// <summary>
        /// 物块对应掉落物的词典
        /// </summary>
        public static Dictionary<int, int> TileToItem = new();
        /// <summary>
        /// 墙体对应掉落物的词典
        /// </summary>
        public static Dictionary<int, int> WallToItem = new();
        /// <summary>
        /// 扫地机器人
        /// </summary>
        public static int Androomba;
        /// <summary>
        /// 瘟疫使者
        /// </summary>
        public static int PlaguebringerGoliath;
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
        /// 血肉宿主
        /// </summary>
        public static int PerforatorHive;
        /// <summary>
        /// 腐巢意志
        /// </summary>
        public static int HiveMind;
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
            DubiousPlating = ItemType<DubiousPlating>();
            DarkMatterBall = ItemType<DarkMatterBall>();
            EndlessStabilizer = ItemType<EndlessStabilizer>();
            InfiniteStick = ItemType<InfiniteStick>();
            BlackMatterStick = ItemType<BlackMatterStick>();
            FoodStallChair = ItemType<FoodStallChair>();
            Gangarus = ItemType<Gangarus>();
            StarMyriadChanges = ItemType<StarMyriadChanges>();

            Androomba = NPCType<Androomba>();
            PlaguebringerGoliath = NPCType<PlaguebringerGoliath>();

            PerforatorHive = NPCType<PerforatorHive>();
            HiveMind = NPCType<HiveMind>();

            SepulcherHead = NPCType<SepulcherHead>();
            SepulcherBody = NPCType<SepulcherBody>();
            SepulcherTail = NPCType<SepulcherTail>();

            StormWeaverHead = NPCType<StormWeaverHead>();
            StormWeaverBody = NPCType<StormWeaverBody>();
            StormWeaverTail = NPCType<StormWeaverTail>();

            PrimordialWyrmHead = NPCType<PrimordialWyrmHead>();
            PrimordialWyrmBody = NPCType<PrimordialWyrmBody>();
            PrimordialWyrmTail = NPCType<PrimordialWyrmTail>();

            PerforatorHeadLarge = NPCType<PerforatorHeadLarge>();
            PerforatorBodyLarge = NPCType<PerforatorBodyLarge>();
            PerforatorTailLarge = NPCType<PerforatorTailLarge>();

            PerforatorHeadMedium = NPCType<PerforatorHeadMedium>();
            PerforatorBodyMedium = NPCType<PerforatorBodyMedium>();
            PerforatorTailMedium = NPCType<PerforatorTailMedium>();

            ArmoredDiggerHead = NPCType<CalamityMod.NPCs.NormalNPCs.ArmoredDiggerHead>();
            ArmoredDiggerBody = NPCType<CalamityMod.NPCs.NormalNPCs.ArmoredDiggerBody>();
            ArmoredDiggerTail = NPCType<CalamityMod.NPCs.NormalNPCs.ArmoredDiggerTail>();

            Apollo = NPCType<Apollo>();
            Artemis = NPCType<Artemis>();
            AresBody = NPCType<CalamityMod.NPCs.ExoMechs.Ares.AresBody>();
            ThanatosHead = NPCType<ThanatosHead>();
            ThanatosBody1 = NPCType<ThanatosBody1>();
            ThanatosBody2 = NPCType<ThanatosBody2>();
            ThanatosTail = NPCType<ThanatosTail>();

            DevourerofGodsHead = NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsHead>();
            DevourerofGodsBody = NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();
            DevourerofGodsTail = NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsTail>();
            CosmicGuardianHead = NPCType<CalamityMod.NPCs.DevourerofGods.CosmicGuardianHead>();
            CosmicGuardianBody = NPCType<CalamityMod.NPCs.DevourerofGods.CosmicGuardianBody>();
            CosmicGuardianTail = NPCType<CalamityMod.NPCs.DevourerofGods.CosmicGuardianTail>();

            DesertScourgeHead = NPCType<DesertScourgeHead>();
            DesertScourgeBody = NPCType<DesertScourgeBody>();
            DesertScourgeTail = NPCType<DesertScourgeTail>();
            DesertNuisanceHead = NPCType<DesertNuisanceHead>();
            DesertNuisanceBody = NPCType<DesertNuisanceBody>();
            DesertNuisanceTail = NPCType<DesertNuisanceTail>();

            AstrumDeusHead = NPCType<AstrumDeusHead>();
            AstrumDeusBody = NPCType<AstrumDeusBody>();
            AstrumDeusTail = NPCType<AstrumDeusTail>();

            AquaticScourgeHead = NPCType<AquaticScourgeHead>();
            AquaticScourgeBody = NPCType<AquaticScourgeBody>();
            AquaticScourgeTail = NPCType<AquaticScourgeTail>();

            EidolonWyrmHead = NPCType<EidolonWyrmHead>();
            EidolonWyrmBody = NPCType<EidolonWyrmBody>();
            EidolonWyrmBodyAlt = NPCType<EidolonWyrmBodyAlt>();
            EidolonWyrmTail = NPCType<EidolonWyrmTail>();

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

            MaterialsTypes = new int[]{
                ItemType<AerialiteBar>(),//水华锭
                ItemType<AuricBar>(),//圣金源锭
                ItemType<ShadowspecBar>(),//影魔锭
                ItemType<AstralBar>(),//彗星锭
                ItemType<CosmiliteBar>(),//宇宙锭
                ItemType<CryonicBar>(),//极寒锭
                ItemType<PerennialBar>(),//龙篙锭
                ItemType<ScoriaBar>(),//岩浆锭
                ItemType<MolluskHusk>(),//生物质
                ItemType<MurkyPaste>(),//泥浆杂草混合物质
                ItemType<DepthCells>(),//深渊生物组织
                ItemType<DivineGeode>(),//圣神晶石
                ItemType<DubiousPlating>(),//废弃装甲
                ItemType<EffulgentFeather>(),//闪耀金羽
                ItemType<ExoPrism>(),//星流棱晶
                ItemType<BloodstoneCore>(),//血石核心
                ItemType<CoreofCalamity>(),//灾劫精华
                ItemType<AscendantSpiritEssence>(),//化神精魄
                ItemType<AshesofCalamity>(),//灾厄尘
                ItemType<AshesofAnnihilation>(),//至尊灾厄精华
                ItemType<LifeAlloy>(),//生命合金
                ItemType<LivingShard>(),//生命物质
                ItemType<Lumenyl>(),//流明晶
                ItemType<MeldConstruct>(),//幻塔物质
                ItemType<MiracleMatter>(),//奇迹物质
                ItemType<Polterplasm>(),//幻魂
                ItemType<RuinousSoul>(),//幽花之魂
                ItemType<DarkPlasma>(),//暗物质
                ItemType<UnholyEssence>(),//灼火精华
                ItemType<TwistingNether>(),//扭曲虚空
                ItemType<ArmoredShell>(),//装甲心脏
                ItemType<YharonSoulFragment>(),//龙魂
                ItemType<Rock>()//古恒石
            };
            MaterialsTypes2 = new int[]{
                ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>(),
                ItemType<CalamityMod.Items.Weapons.Ranged.Alluvion>(),
                ItemType<CalamityMod.Items.Weapons.Magic.Apotheosis>(),
                ItemType<Rock>(),
                ItemType<CosmiliteBar>()
            };
            MaterialsTypes3 = new int[]{
                ItemType<CalamityMod.Items.Weapons.Ranged.Drataliornus>(),
                ItemType<CalamityMod.Items.Weapons.Ranged.HeavenlyGale>(),
                ItemType<CalamityMod.Items.Weapons.Magic.Eternity>(),
                ItemType<InfiniteIngot>()
            };
            MaterialsTypes4 = new int[]{
                ItemType<CalamityMod.Items.Tools.CrystylCrusher>(),
                ItemType<CalamityMod.Items.Tools.AbyssalWarhammer>(),
                ItemType<CalamityMod.Items.Tools.AerialHamaxe>(),
                ItemType<CalamityMod.Items.Tools.AstralHamaxe>(),
                ItemType<CalamityMod.Items.Tools.AstralPickaxe>(),
                ItemType<CalamityMod.Items.Tools.AxeofPurity>(),
                ItemType<CalamityMod.Items.Tools.BeastialPickaxe>(),
                ItemType<CalamityMod.Items.Tools.BerserkerWaraxe>(),
                ItemType<CalamityMod.Items.Tools.BlossomPickaxe>(),
                ItemType<CalamityMod.Items.Tools.FellerofEvergreens>(),
                ItemType<CalamityMod.Items.Tools.Gelpick>(),
                ItemType<CalamityMod.Items.Tools.GenesisPickaxe>(),
                ItemType<CalamityMod.Items.Tools.Grax>(),
                ItemType<CalamityMod.Items.Tools.GreatbayPickaxe>(),
                ItemType<CalamityMod.Items.Tools.InfernaCutter>(),
                ItemType<CalamityMod.Items.Tools.ReefclawHamaxe>(),
                ItemType<CalamityMod.Items.Tools.SeismicHampick>(),
                ItemType<CalamityMod.Items.Tools.ShardlightPickaxe>(),
                ItemType<CalamityMod.Items.Tools.SkyfringePickaxe>(),
                ItemType<CalamityMod.Items.Tools.TectonicTruncator>(),
                ItemType<InfiniteIngot>()
            };

            if (CWRMod.Instance.fargowiltasSouls != null) {
                EternitySoul = CWRMod.Instance.fargowiltasSouls.Find<ModItem>("EternitySoul").Type;
            }

            OnLoadContentBool = false;
        }
    }
}
