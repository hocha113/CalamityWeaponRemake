using CalamityWeaponRemake.Common.Effects;
using Terraria;
using Terraria.ModLoader;

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

        public override void PostSetupContent()
        {
            //CalamityMod.Items.Weapons.Melee.ArkoftheCosmos arkoftheCosmos = new CalamityMod.Items.Weapons.Melee.ArkoftheCosmos();
            //ModItem _arkoftheCosmos = arkoftheCosmos as ModItem;
            //_arkoftheCosmos = Main.item[ModContent.ItemType<Content.Items.Melee.ArkoftheCosmos>()].ModItem;
        }
    }
}