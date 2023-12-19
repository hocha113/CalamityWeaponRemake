using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using System.Collections.Generic;

namespace CalamityWeaponRemake.Content.UIs.SupertableUI
{
    internal class SupertableUI : CWRUIPanel
    {
        public static SupertableUI instance;

        public override Texture2D Texture => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/MainValue");

        List<int> fullItemTypes;

        public Item[] items;

        public Item inputItem;

        public Rectangle inputRec;

        public bool Active;

        private Vector2 topLeft;

        private int cellWid;

        private int cellHig;

        private int maxCellNumX;

        private int maxCellNumY;

        private Point mouseInCellCoord;

        private int inCoordIndex => mouseInCellCoord.Y * maxCellNumX + mouseInCellCoord.X;

        private Rectangle mainRec;

        private bool onMainP;

        private bool onInputP;

        public override void Load()
        {
            instance = this;
        }

        public override void Initialize()
        {
            DrawPos = (new Vector2(Main.screenWidth, Main.screenHeight) - new Vector2(Texture.Width, Texture.Height + 500)) / 2;
            topLeft = new Vector2(22, 46) + DrawPos;
            cellWid = 36;
            cellHig = 36;
            maxCellNumX = maxCellNumY = 9;
            if (items == null)
            {
                items = new Item[maxCellNumX * maxCellNumY];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new Item();
                }
            }
            if (inputItem == null)
            {
                inputItem = new Item();
            }
            Vector2 inUIMousePos = MouPos - topLeft;
            int mouseXGrid = (int)(inUIMousePos.X / cellWid);
            int mouseYGrid = (int)(inUIMousePos.Y / cellHig);
            mouseInCellCoord = new Point(mouseXGrid, mouseYGrid);
            mainRec = new Rectangle((int)topLeft.X, (int)topLeft.Y, cellWid * maxCellNumX, cellHig * maxCellNumY);
            inputRec = new Rectangle((int)(DrawPos.X + 412), (int)(DrawPos.Y + 185), 52, 52);
            onMainP = mainRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onInputP = inputRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
        }

        private void FullItem()
        {
            fullItemTypes = new List<int>();
            foreach (string value in SupertableRecipeDate.FullItems)
            {
                if (int.TryParse(value, out int intValue))
                {
                    fullItemTypes.Add(intValue);
                }
                else
                {
                    string[] fruits = value.Split('/');
                    fullItemTypes.Add(ModLoader.GetMod(fruits[0]).Find<ModItem>(fruits[1]).Type);
                }
            }
        }

        private void OutFillValue()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\模组资源\\AAModPrivate\\input.cs"))
                {
                    sw.Write("string[] fullItems = new string[] {");
                    foreach (var item in items)
                    {
                        if (item.ModItem == null)
                        {
                            sw.Write($"\"{item.type}\"");
                        }
                        else
                        {
                            sw.Write($"\"{item.ModItem.FullName}\"");
                        }
                        sw.Write(", ");
                    }
                    sw.Write("};");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        public override void Update(GameTime gameTime)
        {
            Initialize();
            int museS = DownStartL();
            if (onMainP)
            {
                player.mouseInterface = true;
                OutItem();
                InsMesItem(ref items[inCoordIndex], ref Main.mouseItem, museS);
            }
            if (onInputP)
            {
                player.mouseInterface = true;
                InsMesItem(ref inputItem, ref Main.mouseItem, museS);
            }

            if (Main.LocalPlayer.PressKey(false))
            {
                FullItem();
            }
        }

        public Vector2 ArcCellPos(int index)
        {
            int y = index / maxCellNumX;
            int x = index - y * maxCellNumX;
            return new Vector2(x, y) * new Vector2(cellWid, cellHig) + topLeft;
        }

        public void OutItem()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].type != fullItemTypes[i])
                {
                    if (inputItem.type != ItemID.None)
                    {
                        inputItem = new Item();
                    }
                    return;
                }
            }
            inputItem = new Item(ModContent.ItemType<InfinitePick>());
        }

        /// <summary>
        /// 处理输入格子的点击事件
        /// </summary>
        /// <param name="onitem">输入格的物品状态</param>
        /// <param name="holdItem">鼠标上的物品</param>
        /// <param name="mouseS">点击状态</param>
        public void InsMesItem(ref Item onitem, ref Item holdItem, int mouseS)
        {
            if (mouseS == 1)
            {
                if (onitem.type != ItemID.None && holdItem.type == ItemID.None)
                {
                    _ = SoundEngine.PlaySound(SoundID.Grab);
                    holdItem = onitem;
                    onitem = new Item();
                    return;
                }
                if (onitem.type == holdItem.type)
                {
                    return;
                }
                if (onitem.type == ItemID.None && holdItem.type != ItemID.None)
                {
                    _ = SoundEngine.PlaySound(SoundID.Grab);
                    Utils.Swap(ref holdItem, ref onitem);
                    holdItem = new Item();
                }
                else
                {
                    _ = SoundEngine.PlaySound(SoundID.Grab);
                    (holdItem, onitem) = (onitem, holdItem);
                }
                OutItem();
            }
        }

        /// <summary>
        /// 用于绘制格子中的目标物品
        /// </summary>
        /// <param name="spriteBatch">批处理对象</param>
        /// <param name="item">物品</param>
        /// <param name="drawpos">绘制位置</param>
        public void DrawItemIcons(SpriteBatch spriteBatch, Item item, Vector2 drawpos, Color drawColor = default)
        {
            if (item != null && item.type != ItemID.None)
            {
                Rectangle rectangle = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value) : TextureAssets.Item[item.type].Value.Frame(1, 1, 0, 0);
                Vector2 vector = rectangle.Size();
                Vector2 size = TextureAssets.Item[item.type].Value.Size();
                float slp = 1;
                if (size.X > 32)
                {
                    slp = 35f / size.X;
                }
                spriteBatch.Draw(TextureAssets.Item[item.type].Value, drawpos + new Vector2(cellWid, cellHig) / 2f, new Rectangle?(rectangle), drawColor == default ? Color.White : drawColor, 0f, vector / 2, slp, 0, 0f);
                if (item.stack > 1)
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), drawpos.X, drawpos.Y + 25, Color.White, Color.Black, new Vector2(0.3f), 1f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                if (item != null)
                {
                    DrawItemIcons(spriteBatch, item, ArcCellPos(i));
                }
            }
            if (inputItem != null && inputItem?.type != 0)
            {
                DrawItemIcons(spriteBatch, inputItem, DrawPos + new Vector2(418, 190));
            }
            if (onMainP && inCoordIndex >= 0 && inCoordIndex <= 99)
            {
                Item overItem = items[inCoordIndex];
                Main.HoverItem = overItem.Clone();
                Main.hoverItemName = overItem.Name;
            }
            if (onInputP && inputItem != null && inputItem?.type != 0)
            {
                Main.HoverItem = inputItem.Clone();
                Main.hoverItemName = inputItem.Name;
            }
        }
    }
}
