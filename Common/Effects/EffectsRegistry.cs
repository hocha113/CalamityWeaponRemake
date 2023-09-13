using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection.Metadata;
using Terraria;
using Terraria.Graphics.Effects;

namespace CalamityWeaponRemake.Common.Effects
{
    public static class EffectsRegistry
    {
        public static Filter BigTentacle => Filters.Scene["CalamityWeaponRemake:BigTentacle"];

        public static void LoadEffects()
        {
            var assets = CalamityWeaponRemake.Instance.Assets;
            LoadRegularShaders(assets);
        }

        public static void LoadRegularShaders(AssetRepository assets)
        {
            Ref<Effect> bigTentacle = new(assets.Request<Effect>(CWRConstant.noEffects + "BigTentacle", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CalamityWeaponRemake:BigTentacle"] = new Filter(new(bigTentacle, "Tentacle"), EffectPriority.VeryHigh);
        }
    }
}
