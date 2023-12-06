﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Placeables.DraedonStructures;
using CalamityMod.TileEntities;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Enums;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Tiles
{
    internal class TransmutationOfMatter : ModTile
    {
        public override string Texture => CWRConstant.Asset + "Tiles/" + "TransmutationOfMatter";
        public const int Width = 3;
        public const int Height = 3;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;

        // 动画帧数为45。单元格在动画帧42上创建
        public const int TotalFrames = 45;
        private const int FramesPerColumn = 15;
        public const int AnimationFramerate = 5;

        public const int BetweenCellDowntime = 675;
        public const int CellCreateFrame = 42;
        public const int MagicFrameDelay = AnimationFramerate - 1;

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = Width;
            TileObjectData.newTile.Height = Height;
            TileObjectData.newTile.Origin = new Point16(OriginOffsetX, OriginOffsetY);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.LavaDeath = false;

            ModTileEntity te = ModContent.GetInstance<TransmutationOfMatterEntity>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);

            TileObjectData.addTile(Type);
            AddMapEntry(new Color(67, 72, 81), CalamityUtils.GetItemName<PowerCellFactoryItem>());
            AnimationFrameHeight = 68;
        }

        public override bool CanExplode(int i, int j) => false;

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.Electric);
            return false;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Tile t = Main.tile[i, j];
            int left = i - t.TileFrameX % (Width * SheetSquare) / SheetSquare;
            int top = j - t.TileFrameY % (Height * SheetSquare) / SheetSquare;

            TransmutationOfMatterEntity factory = CalamityUtils.FindTileEntity<TransmutationOfMatterEntity>(i, j, Width, Height, SheetSquare);
            int numCells = factory?.CellStack ?? 0;
            if (numCells > 0)
                Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16f, ModContent.ItemType<DraedonPowerCell>(), numCells);

            factory?.Kill(left, top);
        }

        public override bool RightClick(int i, int j)
        {
            TransmutationOfMatterEntity thisFactory = CalamityUtils.FindTileEntity<TransmutationOfMatterEntity>(i, j, Width, Height, SheetSquare);
            Player player = Main.LocalPlayer;
            player.CancelSignsAndChests();
            CalamityPlayer mp = player.Calamity();
            if (thisFactory is null || thisFactory.ID == mp.CurrentlyViewedFactoryID)
            {
                mp.CurrentlyViewedFactoryID = -1;
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            else if (thisFactory != null)
            {
                SoundEngine.PlaySound(mp.CurrentlyViewedFactoryID == -1 ? SoundID.MenuOpen : SoundID.MenuTick);
                mp.CurrentlyViewedFactoryID = thisFactory.ID;
                Main.playerInventory = true;
                Main.recBigList = false;
            }

            Recipe.FindRecipes();
            return true;
        }

        int frameIndex = 1;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Main.tile[i, j];
            int frameXPos = t.TileFrameX;
            int frameYPos = t.TileFrameY;
            CWRUtils.ClockFrame(ref frameIndex, 6, 3);
            frameYPos += frameIndex % 4 * (Height * SheetSquare);

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 offset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + offset;
            Color drawColor = Lighting.GetColor(i, j);

            if (!t.IsHalfBlock && t.Slope == 0)
                spriteBatch.Draw(tex, drawOffset, new Rectangle(frameXPos, frameYPos, 16, 16)
                    , drawColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            else if (t.IsHalfBlock)
                spriteBatch.Draw(tex, drawOffset + Vector2.UnitY * 8f, new Rectangle(frameXPos, frameYPos, 16, 16)
                    , drawColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            return false;
        }
    }
}
