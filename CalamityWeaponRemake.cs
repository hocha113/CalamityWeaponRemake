using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Effects;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Projectiles;

namespace CalamityWeaponRemake
{
	public class CalamityWeaponRemake : Mod
	{
        internal static CalamityWeaponRemake Instance;

        public override void Load()
        {
            Instance = this;

            EffectsRegistry.LoadEffects();

            base.Load();
        }

        

        public override void Unload()
        {
            base.Unload();
        }
    }
}