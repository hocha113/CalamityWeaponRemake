using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityWeaponRemake.Content
{
    internal class UIDates : ModSystem
    {
        public override void UpdateUI(GameTime gameTime) {
            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseIndex = layers.FindIndex((GameInterfaceLayer layer) => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex != -1) {

            }
        }
    }
}
