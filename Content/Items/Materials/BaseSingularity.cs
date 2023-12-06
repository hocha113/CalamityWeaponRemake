using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Content.Items.Materials.BaseSingularity;

namespace CalamityWeaponRemake.Content.Items.Materials
{
    internal class BaseSingularity : ModItem
    {
        public override string Texture => CWRConstant.Item + "Materials/SingularityLayer_0";
        public virtual string TopTex => CWRConstant.Item + "Materials/SingularityLayer_1";
        public new string LocalizationCategory => "Items.Materials";
        public  Color[] DrawColors = new Color[] { new Color(255, 255, 255), new Color(153, 84, 176), new Color(153, 84, 176), Color.Blue };

        public int frameIndex;
        public int frameIndex2;
        public int time;
        public int MaxLeryFrameNum;
        public int MaxLeryFrameNumTop;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 999;
            ItemID.Sets.SortingPriorityMaterials[Type] = 114;
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 6));
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 25;
            Item.rare = 13;
            Item.value = Item.sellPrice(gold: 999);
            MaxLeryFrameNum = 3;
            MaxLeryFrameNumTop = 6;
            DrawColors = new Color[] {};
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.5f * brightness, 0f, 0.5f * brightness);
        }

        public virtual Color ColorFunOne(int time)
        {
            return Color.White;
        }

        public virtual Color ColorFunTwo(int time)
        {
            return Color.White;
        }

        public virtual void DrawLyerOne(SpriteBatch spriteBatch, Vector2 position)
        {
            Texture2D val0 = CWRUtils.GetT2DValue(Texture);
            Color color = ColorFunOne(time);
            spriteBatch.Draw(val0, position, CWRUtils.GetRec(val0, frameIndex, MaxLeryFrameNum), ColorFunOne(time), 0, CWRUtils.GetOrig(val0, MaxLeryFrameNum), 1, SpriteEffects.None, 0);
        }

        public virtual void DrawLyerTwo(SpriteBatch spriteBatch, Vector2 position)
        {
            Texture2D val0 = CWRUtils.GetT2DValue(TopTex);
            Color color = ColorFunTwo(time);
            spriteBatch.Draw(val0, position, CWRUtils.GetRec(val0, frameIndex, MaxLeryFrameNumTop), ColorFunTwo(time), 0, CWRUtils.GetOrig(val0, MaxLeryFrameNumTop), 1, SpriteEffects.None, 0);
        }

        public virtual void DrawItem(SpriteBatch spriteBatch, Vector2 position)
        {
            CWRUtils.ClockFrame(ref frameIndex, 6, MaxLeryFrameNum - 1);
            CWRUtils.ClockFrame(ref frameIndex2, 6, MaxLeryFrameNumTop - 1);
            time++;
            DrawLyerOne(spriteBatch, position);
            DrawLyerTwo(spriteBatch, position);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position
            , Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawItem(spriteBatch, position);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            DrawItem(spriteBatch, Item.Center - Main.screenPosition);
            return false;
        }

        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            return base.PreDrawTooltip(lines, ref x, ref y);
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }
}
