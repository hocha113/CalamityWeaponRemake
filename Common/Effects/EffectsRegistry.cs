using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common.Effects
{
    public static class EffectsRegistry
    {
        public static MiscShaderData KevinLightningShader => GameShaders.Misc["CalamityWeaponRemake:KevinLightning"];
        public static Filter ColourModulation => Filters.Scene["ColourModulation"];
        public static Texture2D Ticoninfinity;
        public static Effect ColourModulationShader;
        public static Effect MetaballEdgeShader;

        public static void LoadEffects() {
            var assets = CWRMod.Instance.Assets;
            LoadRegularShaders(assets);
        }

        public static void LoadRegularShaders(AssetRepository assets) {
            Ref<Effect> kevinLightningShader = new(assets.Request<Effect>(CWRConstant.noEffects + "KevinLightningShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["CalamityWeaponRemake:KevinLightning"] = new MiscShaderData(kevinLightningShader, "UpdatePass");

            Ref<Effect> shockwave = new(assets.Request<Effect>(CWRConstant.noEffects + "Shockwave", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["Shockwave"] = new Filter(new(shockwave, "Shockwave"), EffectPriority.VeryHigh);

            Ref<Effect> colourModulation = new(assets.Request<Effect>(CWRConstant.noEffects + "ColourModulation", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["ColourModulation"] = new Filter(new(colourModulation, "GoldenPass"), EffectPriority.VeryHigh);

            Ref<Effect> metaballEdgeShader = new(assets.Request<Effect>(CWRConstant.noEffects + "MetaballEdgeShader", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["MetaballEdgeShader"] = new Filter(new(metaballEdgeShader, "GoldenPass"), EffectPriority.VeryHigh);

            Ticoninfinity = ModContent.Request<Texture2D>("CalamityWeaponRemake/Assets/UIs/Ticoninfinity", AssetRequestMode.ImmediateLoad).Value;
            ColourModulationShader = CWRMod.Instance.Assets.Request<Effect>(CWRConstant.noEffects + "ColourModulation", AssetRequestMode.ImmediateLoad).Value;
            MetaballEdgeShader = CWRMod.Instance.Assets.Request<Effect>(CWRConstant.noEffects + "MetaballEdgeShader", AssetRequestMode.ImmediateLoad).Value;
        }
    }
}
