using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Content.Items.Summon;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public enum WhipHitTypeEnum : byte
    {
        ElementWhip = 1,
        BleedingScourge,
        AzureDragonRage,
        GhostFireWhip,
        WhiplashGalactica,
        AllhallowsGoldWhip
    }

    public static class WhipHitDate
    {
        public static Texture2D Tex(WhipHitTypeEnum hitType)
        {
            switch (hitType)
            {
                case WhipHitTypeEnum.ElementWhip:
                    return DrawUtils.GetT2DValue(ModContent.GetInstance<ElementWhip>().Texture);
                case WhipHitTypeEnum.BleedingScourge:
                    return DrawUtils.GetT2DValue(ModContent.GetInstance<BleedingScourge>().Texture);
                case WhipHitTypeEnum.AzureDragonRage:
                    return DrawUtils.GetT2DValue(ModContent.GetInstance<AzureDragonRage>().Texture);
                case WhipHitTypeEnum.GhostFireWhip:
                    return DrawUtils.GetT2DValue(ModContent.GetInstance<GhostFireWhip>().Texture);
                case WhipHitTypeEnum.WhiplashGalactica:
                    return DrawUtils.GetT2DValue(ModContent.GetInstance<WhiplashGalactica>().Texture);
                case WhipHitTypeEnum.AllhallowsGoldWhip:
                    return DrawUtils.GetT2DValue(ModContent.GetInstance<AllhallowsGoldWhip>().Texture);
                default:
                    return DrawUtils.GetT2DValue(CWRConstant.placeholder);
            }
        }
    }
}
