using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.UIs.SupertableUIs
{
    internal class SupertableUI : CWRUIPanel
    {
        public static SupertableUI instance;

        public override Texture2D Texture => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/MainValue");

        private int[] fullItemTypes;

        public Item[] items;

        public List<string[]> recipeDate;

        public List<int> recipeDateTarget;

        public Item inputItem;

        public Rectangle inputRec;

        public bool Active;

        private Vector2 topLeft;

        private int cellWid;

        private int cellHig;

        private int maxCellNumX;

        private int maxCellNumY;

        private Point mouseInCellCoord;

        private Point oldMouseInCellCoord;

        private int inCoordIndex => (mouseInCellCoord.Y * maxCellNumX) + mouseInCellCoord.X;

        private Rectangle mainRec;

        private bool onMainP;

        private bool onInputP;

        public override void Load() {
            instance = this;
        }

        public override void Initialize() {
            DrawPos = (new Vector2(Main.screenWidth, Main.screenHeight) - new Vector2(Texture.Width, Texture.Height + 500)) / 2;
            topLeft = new Vector2(22, 46) + DrawPos;
            cellWid = 36;
            cellHig = 36;
            maxCellNumX = maxCellNumY = 9;

            if (recipeDate == null) {
                recipeDate = new List<string[]>()
                {
                    SupertableRecipeDate.FullItems,
                    SupertableRecipeDate.FullItems2
                };
            }

            if (items == null) {
                items = new Item[maxCellNumX * maxCellNumY];
                for (int i = 0; i < items.Length; i++) {
                    items[i] = new Item();
                }
            }

            if (fullItemTypes == null || fullItemTypes?.Length != items.Length) {
                fullItemTypes = new int[items.Length];
                FullItem(SupertableRecipeDate.FullItems);
            }
                
            if (recipeDateTarget == null) {
                recipeDateTarget = new List<int>();
                foreach (string[] value in recipeDate) {
                    string name = value[value.Length - 1];
                    if (int.TryParse(name, out int intValue)) {
                        recipeDateTarget.Add(intValue);
                    }
                    else {
                        string[] fruits = name.Split('/');
                        recipeDateTarget.Add(ModLoader.GetMod(fruits[0]).Find<ModItem>(fruits[1]).Type);
                    }
                }
            }

            inputItem ??= new Item();

            Vector2 inUIMousePos = MouPos - topLeft;
            int mouseXGrid = (int)(inUIMousePos.X / cellWid);
            int mouseYGrid = (int)(inUIMousePos.Y / cellHig);
            mouseInCellCoord = new Point(mouseXGrid, mouseYGrid);

            mainRec = new Rectangle((int)topLeft.X, (int)topLeft.Y, cellWid * maxCellNumX, cellHig * maxCellNumY);
            inputRec = new Rectangle((int)(DrawPos.X + 412), (int)(DrawPos.Y + 185), 52, 52);
            onMainP = mainRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onInputP = inputRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
        }

        private int[] FullItem(string[] arg) {
            int[] toValueTypes = new int[arg.Length];
            for (int i = 0; i < arg.Length; i++) {
                string value = arg[i];
                if (int.TryParse(value, out int intValue)) {
                    toValueTypes[i] = intValue;
                }
                else {
                    string[] fruits = value.Split('/');
                    toValueTypes[i] = ModLoader.GetMod(fruits[0]).Find<ModItem>(fruits[1]).Type;
                }
            }
            return toValueTypes;
        }


        private void OutFillValue() {
            try {
                int ogeindex = 0;
                using StreamWriter sw = new("D:\\模组资源\\AAModPrivate\\input.cs");
                sw.Write("string[] fullItems = new string[] {");
                foreach (Item item in items) {
                    ogeindex++;
                    if (item.ModItem == null) {
                        sw.Write($"\"{item.type}\"");
                    }
                    else {
                        sw.Write($"\"{item.ModItem.FullName}\"");
                    }
                    sw.Write(", ");
                    if (ogeindex >= 9) {
                        sw.WriteLine();
                        ogeindex = 0;
                    }
                }
                sw.Write("};");
            }
            catch (Exception e) {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        /// <summary>
        /// 在只利用一个数字索引的情况下反向计算出对应的格坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 ArcCellPos(int index) {
            int y = index / maxCellNumX;
            int x = index - (y * maxCellNumX);
            return (new Vector2(x, y) * new Vector2(cellWid, cellHig)) + topLeft;
        }

        /// <summary>
        /// 进行输出制作结果的操作，应当注意他的使用方式防止造成不必要的性能浪费
        /// </summary>
        public void OutItem() {
            foreach (string[] arg in recipeDate) {
                fullItemTypes = FullItem(arg);
                if (items.Length != fullItemTypes.Length - 1) {
                    if (inputItem.type != ItemID.None) {
                        inputItem = new Item();
                    }
                    goto End;
                }
                
                for (int i = 0; i < fullItemTypes.Length - 1; i++) {
                    if (items[i].type != fullItemTypes[i]) {
                        if (inputItem.type != ItemID.None) {
                            inputItem = new Item();
                        }
                        goto End;
                    }
                }
                if (inputItem.type == ItemID.None) {
                    inputItem = new Item(fullItemTypes[fullItemTypes.Length - 1]);
                }
End:;
            }
        }

        public override void Update(GameTime gameTime) {
            Initialize();
            int museS = DownStartL();
            int museSR = DownStartR();

            if (onMainP) {
                player.mouseInterface = true;
                //Main.NewText(items[inCoordIndex]);
                if (museS == 1) {
                    HandleItemClick(ref items[inCoordIndex], ref Main.mouseItem);
                }
                    
                if (museSR == 1) {
                    HandleRightClick(ref items[inCoordIndex], ref Main.mouseItem);
                }

                if (Main.LocalPlayer.PressKey(false)) {
                    DragDorg(ref items[inCoordIndex], ref Main.mouseItem);
                }

                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G)) {
                    GatheringItem(inCoordIndex, ref Main.mouseItem);
                }
            }

            if (onInputP) {
                player.mouseInterface = true;
                if (museS == 1) {
                    GetResult(ref inputItem, ref Main.mouseItem, ref items);
                }
            }

            //if (Main.LocalPlayer.PressKey(false)) {
            //    OutFillValue();
            //}
        }

        private void GetResult(ref Item onitem, ref Item holdItem, ref Item[] arg) {
            if (holdItem.type == ItemID.None && onitem.type != ItemID.None) {
                PlayGrabSound();

                foreach (string[] value in recipeDate) {
                    int types = 0;
                    string name = value[value.Length - 1];
                    if (int.TryParse(name, out int intValue)) {
                        types = intValue;
                    }
                    else {
                        string[] fruits = name.Split('/');
                        types = ModLoader.GetMod(fruits[0]).Find<ModItem>(fruits[1]).Type;
                    }
                    if (types == onitem.type) {
                        fullItemTypes = FullItem(value);
                    }
                }
                
                for (int i = 0; i < items.Length; i++) {
                    if (items[i].type == fullItemTypes[i]) {
                        items[i].stack -= 1;
                        if (items[i].stack <= 0)
                            items[i] = new Item();
                    }    
                }
                holdItem = onitem;
                onitem = new Item();
            }
        }

        /// <summary>
        /// 处理输入格子的点击事件，负责处理物品的交互和堆叠逻辑
        /// </summary>
        /// <param name="onitem">输入格的物品状态</param>
        /// <param name="holdItem">鼠标上的物品</param>
        /// <param name="mouseS">点击状态</param>
        public void HandleItemClick(ref Item onitem, ref Item holdItem) {
            // 如果输入格和鼠标上的物品都为空，无需处理
            if (onitem.type == ItemID.None && holdItem.type == ItemID.None) {
                return;
            }

            // 捡起物品逻辑
            if (onitem.type != ItemID.None && holdItem.type == ItemID.None) {
                PlayGrabSound();
                holdItem = onitem;
                onitem = new Item();
                OutItem();
                return;
            }

            // 同种物品堆叠逻辑
            if (onitem.type == holdItem.type && holdItem.type != ItemID.None) {
                PlayGrabSound();
                onitem.stack += holdItem.stack;
                holdItem = new Item();
                return;
            }

            // 不同种物品交换逻辑
            if (onitem.type == ItemID.None && holdItem.type != ItemID.None) {
                PlayGrabSound();
                Utils.Swap(ref holdItem, ref onitem);
                holdItem = new Item();
                OutItem();
            }
            else {
                // 不同种物品交换逻辑
                PlayGrabSound();
                (holdItem, onitem) = (onitem, holdItem);
                OutItem();
            }
        }

        /// <summary>
        /// 处理右键点击事件，用于在物品格之间进行右键交互
        /// </summary>
        /// <param name="onitem">目标物品格的物品状态</param>
        /// <param name="holdItem">鼠标上的物品</param>
        public void HandleRightClick(ref Item onitem, ref Item holdItem) {
            // 如果目标格和鼠标上的物品都为空，无需处理
            if (onitem.type == ItemID.None && holdItem.type == ItemID.None) {
                return;
            }

            // 同种物品右键增加逻辑
            if (onitem.type == holdItem.type && holdItem.type != ItemID.None) {
                PlayGrabSound();

                // 如果物品堆叠上限为1，则不进行右键增加操作
                if (onitem.maxStack == 1) {
                    return;
                }

                onitem.stack += 1;
                holdItem.stack -= 1;

                // 如果鼠标上的物品数量为0，则清空鼠标上的物品
                if (holdItem.stack == 0) {
                    holdItem = new Item();
                }

                OutItem();
                return;
            }

            // 不同种物品交换逻辑
            if (onitem.type != holdItem.type && onitem.type != ItemID.None && holdItem.type != ItemID.None) {
                PlayGrabSound();
                Utils.Swap(ref holdItem, ref onitem);
                OutItem();
                return;
            }

            // 鼠标上有物品且目标格为空物品，进行右键放置逻辑
            if (onitem.type == ItemID.None && holdItem.type != ItemID.None) {
                PlayGrabSound();
                PlaceItemOnGrid(ref onitem, ref holdItem);
                OutItem();
            }
        }

        private void DragDorg(ref Item onitem, ref Item holdItem) {
            if (onitem.type == ItemID.None && holdItem.type != ItemID.None) {
                holdItem.stack -= 1;
                Item intoItem = holdItem.Clone();
                intoItem.stack = 1;
                onitem = intoItem;
                OutItem();
            }
        }

        private void GatheringItem(int index, ref Item holdItem) {
            if (holdItem.type == ItemID.None && items[index].type != ItemID.None) {
                for (int i = 0; i < items.Length; i++) {
                    if (index == i) {
                        continue;
                    }
                    Item value = items[i].Clone();
                    if (value.type == items[index].type) {
                        items[index].stack += value.stack;
                        items[i] = new Item();
                    }
                }
            }
        }

        /// <summary>
        /// 播放抓取音效
        /// </summary>
        private void PlayGrabSound() {
            _ = SoundEngine.PlaySound(SoundID.Grab);
        }

        /// <summary>
        /// 在目标物品格上放置物品
        /// </summary>
        /// <param name="onitem">目标物品格的物品状态</param>
        /// <param name="holdItem">鼠标上的物品</param>
        private void PlaceItemOnGrid(ref Item onitem, ref Item holdItem) {
            Item inToItem = holdItem.Clone();
            inToItem.stack = 1;
            onitem = inToItem;
            holdItem.stack -= 1;

            // 如果鼠标上的物品数量为0，则清空鼠标上的物品
            if (holdItem.stack == 0) {
                holdItem = new Item();
            }

            OutItem();
        }

        /// <summary>
        /// 用于绘制格子中的目标物品
        /// </summary>
        /// <param name="spriteBatch">批处理对象</param>
        /// <param name="item">物品</param>
        /// <param name="drawpos">绘制位置</param>
        public void DrawItemIcons(SpriteBatch spriteBatch, Item item, Vector2 drawpos, Color drawColor = default) {
            if (item != null && item.type != ItemID.None) {
                Rectangle rectangle = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value) : TextureAssets.Item[item.type].Value.Frame(1, 1, 0, 0);
                Vector2 vector = rectangle.Size();
                Vector2 size = TextureAssets.Item[item.type].Value.Size();
                float slp = 1;
                if (size.X > 32) {
                    slp = 35f / size.X;
                }
                spriteBatch.Draw(TextureAssets.Item[item.type].Value, drawpos + (new Vector2(cellWid, cellHig) / 2f), new Rectangle?(rectangle), drawColor == default ? Color.White : drawColor, 0f, vector / 2, slp, 0, 0f);
                if (item.stack > 1) {
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), drawpos.X, drawpos.Y + 25, Color.White, Color.Black, new Vector2(0.3f), 1f);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, DrawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出UI主体
            if (items != null) {
                for (int i = 0; i < items.Length; i++)//遍历绘制出UI格中的所有物品
                {
                    if (items[i] != null) {
                            Item item = items[i];
                            if (item != null) {
                                DrawItemIcons(spriteBatch, item, ArcCellPos(i));
                            }
                        }
                    }
                }
            
            if (inputItem != null && inputItem?.type != 0)//如果输出格有物品，那么将它画出来
            {
                DrawItemIcons(spriteBatch, inputItem, DrawPos + new Vector2(418, 190));
            }
            if (onMainP && inCoordIndex >= 0 && inCoordIndex <= 99)//处理鼠标在UI格中查看物品的事情
            {
                Item overItem = items[inCoordIndex];
                Main.HoverItem = overItem.Clone();
                Main.hoverItemName = overItem.Name;
            }
            if (onInputP && inputItem != null && inputItem?.type != 0)//处理查看输出结果物品的事情
            {
                Main.HoverItem = inputItem.Clone();
                Main.hoverItemName = inputItem.Name;
            }
        }
    }
}
