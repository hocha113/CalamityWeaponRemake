using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Materials
{
    internal class CosmicMetalSingularity : BaseSingularity
    {
        public override string TopTex => "CalamityMod/Items/Materials/CosmiliteBar";

        public override void SetDefaults()
        {
            base.SetDefaults();
            MaxLeryFrameNumTop = 10;
            DrawColors = new Color[] { new Color(255, 255, 255), new Color(153, 84, 176), new Color(153, 84, 176), Color.Blue };
        }

        public override Color ColorFunOne(int time)
        {
            return CWRUtils.MultiLerpColor(time % 200 / 200f, DrawColors[0], DrawColors[0], DrawColors[1], DrawColors[1], DrawColors[2]
                , DrawColors[3], DrawColors[2], DrawColors[1], DrawColors[1], DrawColors[0], DrawColors[0]);
        }

        public override Color ColorFunTwo(int time)
        {
            return Color.White * (0.3f + Math.Abs(MathF.Sin(time * CWRUtils.atoR * 0.3f)) * 0.7f);
        }

        public override void DrawLyerTwo(SpriteBatch spriteBatch, Vector2 position)
        {
            Texture2D val0 = CWRUtils.GetT2DValue(TopTex);
            spriteBatch.Draw(val0, position, CWRUtils.GetRec(val0, frameIndex, 10)
                , ColorFunTwo(time), time / 4f, CWRUtils.GetOrig(val0, 10), 0.6f, SpriteEffects.None, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<CosmiliteBar>(125).Register();
        }
    }
}
