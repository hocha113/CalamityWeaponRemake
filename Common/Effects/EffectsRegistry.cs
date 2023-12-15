using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace CalamityWeaponRemake.Common.Effects
{
    public static class EffectsRegistry
    {
        public static Filter BigTentacle => Filters.Scene["CalamityWeaponRemake:BigTentacle"];
        public static MiscShaderData KevinLightningShader => GameShaders.Misc["CalamityWeaponRemake:KevinLightning"];

        public static void LoadEffects()
        {
            var assets = CWRMod.Instance.Assets;
            LoadRegularShaders(assets);
        }

        public static void LoadRegularShaders(AssetRepository assets)
        {
            Ref<Effect> bigTentacle = new(assets.Request<Effect>(CWRConstant.noEffects + "BigTentacle", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CalamityWeaponRemake:BigTentacle"] = new Filter(new(bigTentacle, "Tentacle"), EffectPriority.VeryHigh);

            Ref<Effect> kevinLightningShader = new(assets.Request<Effect>(CWRConstant.noEffects + "KevinLightningShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["CalamityWeaponRemake:KevinLightning"] = new MiscShaderData(kevinLightningShader, "UpdatePass");

            Ref<Effect> shockwave = new(assets.Request<Effect>(CWRConstant.noEffects + "Shockwave", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["Shockwave"] = new Filter(new(shockwave, "Shockwave"), EffectPriority.VeryHigh);
        }
    }
}
