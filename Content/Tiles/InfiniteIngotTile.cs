using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.Items.Ranged;
using CalamityWeaponRemake.Content.Items.Tools;
using CalamityWeaponRemake.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityWeaponRemake.Content.Tiles
{
    internal class InfiniteIngotTile : ModTile
    {
        public override string Texture => CWRConstant.Asset + "Tiles/" + "InfiniteIngotTile";
        public override void SetStaticDefaults() {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(121, 89, 9), CalamityUtils.GetItemName<InfiniteIngot>());

        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            Player closestPlayer = null;
            Vector2 tilePosition = new Vector2(i, j) * 16;
            closestPlayer = CWRUtils.TileFindPlayer(i, j);
            if (closestPlayer == null || closestPlayer?.HeldItem.type != ModContent.ItemType<InfinitePick>()) {
                Projectile.NewProjectile(new EntitySource_WorldEvent(), tilePosition, Vector2.Zero, ModContent.ProjectileType<InfiniteIngotTileProj>(), 9999, 0);
            }
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
            Tile t = Main.tile[i, j];
            int frameXPos = t.TileFrameX;
            int frameYPos = t.TileFrameY;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 offset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + offset;
            Color drawColor = CWRUtils.MultiLerpColor(Main.GameUpdateCount % 60 / 60f, HeavenfallLongbow.rainbowColors);

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
