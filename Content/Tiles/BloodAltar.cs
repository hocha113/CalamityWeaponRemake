﻿using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Placeable;
using CalamityWeaponRemake.Content.TileEntitys;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
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
using CalamityMod.TileEntities;

namespace CalamityWeaponRemake.Content.Tiles
{
    internal class BloodAltar : ModTile
    {
        public override string Texture => CWRConstant.Asset + "Tiles/" + "BloodAltar";
        public const int Width = 4;
        public const int Height = 3;
        public const int OriginOffsetX = 1;
        public const int OriginOffsetY = 1;
        public const int SheetSquare = 18;

        public override void SetStaticDefaults() {
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
            ModTileEntity te = ModContent.GetInstance<BloodAltarEntity>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(te.Hook_AfterPlacement, -1, 0, true);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
            AddMapEntry(Color.Red, CalamityUtils.GetItemName<Items.Placeable.BloodAltar>());
            AnimationFrameHeight = 54;

            AdjTiles = new int[] {
                TileID.DemonAltar
            };
        }

        public override bool CanExplode(int i, int j) => false;

        public override bool CreateDust(int i, int j, ref int type) {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.Electric);
            return false;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            ModContent.GetInstance<BloodAltarEntity>().Kill(i, j);
            Main.dayTime = true;
            if (Main.bloodMoon) {
                Main.bloodMoon = false;
            }
        }

        public override bool RightClick(int i, int j) {
            TileEntity.InitializeAll();
            SoundEngine.PlaySound(SoundID.Chat);
            Recipe.FindRecipes();
            return true;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
            Tile t = Main.tile[i, j];
            int frameXPos = t.TileFrameX;
            int frameYPos = t.TileFrameY;
            BloodAltarEntity bloodAltarEntity = CalamityUtils.FindTileEntity<BloodAltarEntity>(i, j, Width, Height, SheetSquare);
            if (bloodAltarEntity != null)
                frameYPos += bloodAltarEntity.frameIndex % 4 * (Height * SheetSquare);

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
