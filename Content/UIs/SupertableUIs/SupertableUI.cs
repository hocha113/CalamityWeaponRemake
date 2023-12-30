using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.Items.Placeable;
using CalamityWeaponRemake.Content.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.UIs.SupertableUIs
{
    internal class SupertableUI : CWRUIPanel
    {
        public static SupertableUI instance;

        public override Texture2D Texture => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/MainValue2");

        private int[] fullItemTypes;

        public Item[] items;

        public Item[] previewItems;

        public Item inputItem;

        public Rectangle inputRec;

        public Rectangle closeRec;

        public bool Active;

        public bool loadOrUnLoadZenithWorldAsset = true;

        public bool initializeBool = true;

        private Vector2 topLeft;

        private int cellWid;

        private int cellHig;

        private int maxCellNumX;

        private int maxCellNumY;

        private Point mouseInCellCoord;

        private int inCoordIndex => (mouseInCellCoord.Y * maxCellNumX) + mouseInCellCoord.X;
        /// <summary>
        /// 主UI的面板矩形
        /// </summary>
        private Rectangle mainRec;
        /// <summary>
        /// 物品放置格子的面板矩形
        /// </summary>
        private Rectangle mainRec2;

        private bool onMainP;

        private bool onMainP2;

        private bool onInputP;

        private bool onCloseP;

        public static List<RecipeData> AllRecipes = new List<RecipeData>();

        public override void Load() {
            instance = this;
            LoadRecipe();
        }

        public static void LoadRecipe() {
            Type type = typeof(SupertableRecipeDate);
            FieldInfo[] stringArrayFields = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                                                .Where(f => f.FieldType == typeof(string[]))
                                                .ToArray();
            PropertyInfo[] stringArrayProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                                                      .Where(p => p.PropertyType == typeof(string[]))
                                                      .ToArray();
            var allMembers = stringArrayFields.Concat<MemberInfo>(stringArrayProperties).ToArray();
            var stringArrays = allMembers.Select(member => {
                if (member is FieldInfo field) {
                    return (string[])field.GetValue(null);
                }
                else if (member is PropertyInfo property) {
                    return (string[])property.GetValue(null);
                }
                return null;
            }).Where(array => array != null).ToArray();

            foreach (string[] value in stringArrays) {
                RecipeData recipeData = new RecipeData{
                    Values = value,
                    Target = InStrGetItemType(value[value.Length - 1])
                };
                AllRecipes.Add(recipeData);
            }
            Console.WriteLine($"得到配方表容量：{AllRecipes.Count}");
        }

        public void UpdateUIElementPos() {
            if (DrawPos == Vector2.Zero && initializeBool) {
                DrawPos = (new Vector2(Main.screenWidth, Main.screenHeight) - new Vector2(Texture.Width - Main.screenWidth / 2, Texture.Height + 400)) / 2;
                initializeBool = false;
            } 
            topLeft = new Vector2(15, 30) + DrawPos;
            cellWid = 48;
            cellHig = 46;
            maxCellNumX = maxCellNumY = 9;

            Vector2 inUIMousePos = MouPos - topLeft;
            int mouseXGrid = (int)(inUIMousePos.X / cellWid);
            int mouseYGrid = (int)(inUIMousePos.Y / cellHig);
            mouseInCellCoord = new Point(mouseXGrid, mouseYGrid);

            mainRec = new Rectangle((int)topLeft.X, (int)topLeft.Y, cellWid * maxCellNumX + 200, cellHig * maxCellNumY);
            mainRec2 = new Rectangle((int)topLeft.X, (int)topLeft.Y, cellWid * maxCellNumX, cellHig * maxCellNumY);
            inputRec = new Rectangle((int)(DrawPos.X + 555), (int)(DrawPos.Y + 215), 92, 90);
            closeRec = new Rectangle((int)(DrawPos.X), (int)(DrawPos.Y), 30, 30);
            onMainP = mainRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onMainP2 = mainRec2.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onInputP = inputRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onCloseP = closeRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
        }

        public override void Initialize() {
            UpdateUIElementPos();

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

            inputItem ??= new Item();

            if (loadOrUnLoadZenithWorldAsset) {
                int infiniteToiletItemType = ModContent.ItemType<InfiniteToiletItem>();
                if (Main.zenithWorld) {
                    // 判断是否已经存在 InfiniteToiletItem 的配方，如果不存在则添加
                    if (!AllRecipes.Any(n => n.Target == infiniteToiletItemType)) {
                        string[] value = SupertableRecipeDate.FullItems1000.ToArray();
                        RecipeData recipeData = new RecipeData
                        {
                            Values = value,
                            Target = InStrGetItemType(value[value.Length - 1])
                        };
                        AllRecipes.Add(recipeData);
                    }
                }
                else {
                    // 移除所有 InfiniteToiletItem 的配方
                    AllRecipes.RemoveAll(n => n.Target == infiniteToiletItemType);
                }
                // 加载配方并更新 UI
                RecipeUI.instance.LoadZenithWRecipes();
                // 标记已经加载或者卸载了 Zenith World 资产
                loadOrUnLoadZenithWorldAsset = false;
            }
        }

        /// <summary>
        /// 播放抓取音效
        /// </summary>
        public static void PlayGrabSound() {
            _ = SoundEngine.PlaySound(SoundID.Grab);
        }

        /// <summary>
        /// 解析字符串键并获取对应的物品类型
        /// </summary>
        /// <param name="key">用于解析的字符串键，可以是整数类型或模组/物品名称的组合</param>
        /// <returns>解析后得到的物品类型</returns>
        public static int InStrGetItemType(string key, bool loadVanillaItem = false) {
            if (int.TryParse(key, out int intValue)) {
                if (loadVanillaItem && !CWRUtils.isServer)
                    Main.instance.LoadItem(intValue);
                return (intValue);
            }
            else {
                string[] fruits = key.Split('/');
                return (ModLoader.GetMod(fruits[0]).Find<ModItem>(fruits[1]).Type);
            }
        }

        /// <summary>
        /// 将字符串数组中的每个键转换为对应的物品类型，并返回结果数组
        /// </summary>
        /// <param name="arg">要转换的字符串数组，每个元素可以是整数类型或模组/物品名称的组合</param>
        /// <returns>包含每个字符串键对应的物品类型的数组</returns>
        public static int[] FullItem(string[] arg) {
            int[] toValueTypes = new int[arg.Length];
            for (int i = 0; i < arg.Length; i++) {
                string value = arg[i];
                toValueTypes[i] = InStrGetItemType(value);
            }
            return toValueTypes;
        }

        /// <summary>
        /// 在只利用一个数字索引的情况下反向计算出对应的格坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 ArcCellPos(int index) {
            if (maxCellNumX != 0) {
                int y = index / maxCellNumX;
                int x = index - (y * maxCellNumX);
                return (new Vector2(x, y) * new Vector2(cellWid, cellHig)) + topLeft;
            }
            else {
                return Vector2.Zero;
            }
        }

        // 重置输入物品
        private void ResetInputItem() {
            if (inputItem.type != ItemID.None) {
                inputItem = new Item();
            }
        }

        /// <summary>
        /// 进行输出制作结果的操作，应当注意他的使用方式防止造成不必要的性能浪费
        /// </summary>
        public void OutItem() {
            foreach (RecipeData data in AllRecipes) {
                string[] arg = data.Values;
                fullItemTypes = FullItem(arg);
                
                if (items.Length != fullItemTypes.Length - 1) {
                    ResetInputItem();
                    goto End;
                }

                for (int i = 0; i < fullItemTypes.Length - 1; i++) {
                    if (items?[i]?.type != fullItemTypes[i]) {
                        ResetInputItem();
                        goto End;
                    }
                }
                
                if (inputItem.type == ItemID.None) {
                    Item item = new Item(fullItemTypes[fullItemTypes.Length - 1]);
                    
                    if (item.CWR().isInfiniteItem) {
                        item.CWR().noDestruct = true;
                        item.CWR().destructTime = 10;
                    }
                    inputItem = item;
                    break;
                }
End:;
            }
        }

        public static void SetItemIsNull(ref Item item) {
            if (item == null) {
                item = new Item();
            }
        }

        public override void Update(GameTime gameTime) {
            if (Main.myPlayer != Main.LocalPlayer.whoAmI) {
                return;
            }
            Initialize();
            int museS = DownStartL();
            int museSR = DownStartR();
            if (onCloseP) {
                player.mouseInterface = true;
                if (museS == 1) {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    Active = false;
                }               
            }
            if (onMainP) {
                player.mouseInterface = true;
                if (onMainP2) {
                    if (museS == 1) {
                        if (items[inCoordIndex] == null) {
                            items[inCoordIndex] = new Item();
                        }
                        if (CWRKeySystem.TOM_QuickFetch.Current) {
                            GatheringItem2(inCoordIndex, ref Main.mouseItem);
                        }
                        else {
                            HandleItemClick(ref items[inCoordIndex], ref Main.mouseItem);
                        }
                        OutItem();
                    }

                    if (Main.LocalPlayer.PressKey(false)) {
                        DragDorg(ref items[inCoordIndex], ref Main.mouseItem);
                        OutItem();
                    }

                    if (CWRKeySystem.TOM_GatheringItem.Current) {
                        GatheringItem(inCoordIndex, ref Main.mouseItem);
                        OutItem();
                    }

                    if (museSR == 1) {
                        HandleRightClick(ref items[inCoordIndex], ref Main.mouseItem);
                        OutItem();
                    }
                }

                if (CWRKeySystem.TOM_OneClickP.JustPressed) {
                    OneClickPFunc();
                    OutItem();
                }

                if (CWRKeySystem.TOM_GlobalRecall.JustPressed) {
                    TakeAllItem();
                    OutItem();
                }
            }

            if (onInputP) {
                player.mouseInterface = true;
                if (museS == 1) {
                    GetResult(ref inputItem, ref Main.mouseItem, ref items);
                    OutItem();
                }
            }
        }

        public void TakeAllItem() {
            foreach (var item in items) {
                if (item == null)
                    continue;
                Item item1 = item.Clone();
                player.QuickSpawnItem(player.parent(), item1, item1.stack);
                item.TurnToAir();
            }
        }

        public void OneClickPFunc() {
            if (previewItems != null && previewItems?.Length == items.Length) {
                TakeAllItem();
                for (int i = 0; i < previewItems.Length; i++) {
                    Item preItem = previewItems[i];
                    foreach (var backItem in player.inventory) {
                        if (preItem.type == backItem.type && backItem.type != ItemID.None) {
                            Item targetItem = backItem.Clone();
                            targetItem.stack = 1;
                            items[i] = targetItem;
                            backItem.stack -= 1;
                            if (backItem.stack == 0) {
                                backItem.TurnToAir();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理获取结果的逻辑，将 onitem 转移到 holdItem 中，并根据配方数据更新物品槽
        /// </summary>
        /// <param name="onitem">被点击的物品槽</param>
        /// <param name="holdItem">正在拖拽的物品</param>
        /// <param name="arg">用于配方的字符串数组</param>
        private void GetResult(ref Item onitem, ref Item holdItem, ref Item[] arg) {
            if (holdItem.type == ItemID.None && onitem.type != ItemID.None) {
                PlayGrabSound();
                SoundEngine.PlaySound(SoundID.Research);
                for (int i = 0; i < items.Length; i++) {
                    if (items[i].type == previewItems[i].type) {
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
            }
            else {
                // 不同种物品交换逻辑
                PlayGrabSound();
                (holdItem, onitem) = (onitem, holdItem);
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

        /// <summary>
        /// 实现拖拽功能，将 holdItem 拖拽到 onitem 上
        /// </summary>
        /// <param name="onitem">被拖拽的目标物品槽</param>
        /// <param name="holdItem">正在拖拽的物品</param>
        public void DragDorg(ref Item onitem, ref Item holdItem) {
            if (onitem.type == ItemID.None && holdItem.type != ItemID.None) {
                holdItem.stack -= 1;
                Item intoItem = holdItem.Clone();
                intoItem.stack = 1;
                onitem = intoItem;
            }
        }

        /// <summary>
        /// 对指定索引处的物品进行合并操作，将相同类型的物品堆叠到一起
        /// </summary>
        /// <param name="index">要进行合并操作的物品槽索引</param>
        /// <param name="holdItem">正在拖拽的物品</param>
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

        private void GatheringItem2(int inCoordIndex, ref Item item) {
            if (item.type == ItemID.None && items[inCoordIndex].type != ItemID.None) {
                Item item1 = items[inCoordIndex].Clone();
                player.QuickSpawnItem(player.parent(), item1, item1.stack);
                items[inCoordIndex] = new Item();
            }
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
        public void DrawItemIcons(SpriteBatch spriteBatch, Item item, Vector2 drawpos, Color drawColor = default, float alp = 1, float overSlp = 1) {
            if (item != null && item.type != ItemID.None) {
                Rectangle rectangle = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value) : TextureAssets.Item[item.type].Value.Frame(1, 1, 0, 0);
                Vector2 vector = rectangle.Size();
                Vector2 size = TextureAssets.Item[item.type].Value.Size();
                Vector2 offset = new Vector2(cellWid, cellHig) / 2;
                float slp = 1;
                if (size.X > 32) {
                    slp = 32f / size.X;
                }
                slp *= overSlp;
                if (item.type == CWRIDs.DarkMatterBall) {
                    DarkMatterBall.DrawItemIcon(spriteBatch, drawpos + offset, item.type, alp);
                }
                else if (item.type == CWRIDs.BlackMatterStick) {
                    BlackMatterStick.DrawItemIcon(spriteBatch, drawpos + offset, Color.White, item.type, alp);
                }
                else if (item.type == CWRIDs.InfiniteStick) {
                    InfiniteStick.DrawItemIcon(spriteBatch, drawpos + offset, item.type, alp);
                }
                else if (item.type == ModContent.ItemType<DecayParticles>() 
                    || item.type == ModContent.ItemType<DecaySubstance>() 
                    || item.type == ModContent.ItemType<DissipationSubstance>() 
                    || item.type == ModContent.ItemType<SpectralMatter>()) {
                    DecayParticles.DrawItemIcon(spriteBatch, drawpos + offset, drawColor, item.type, alp, slp);
                }
                else {
                    spriteBatch.Draw(TextureAssets.Item[item.type].Value, drawpos + offset, new Rectangle?(rectangle), (drawColor == default ? Color.White : drawColor) * alp, 0f, vector / 2, slp, 0, 0f);
                }
                if (item.stack > 1) {
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), drawpos.X, drawpos.Y + 25, Color.White, Color.Black, new Vector2(0.3f), 1f);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (DragButton.instance != null) {
                if (DragButton.instance.onDrag) {//为了防止拖拽状态下出现位置更新的延迟所导致的果冻感，这里用于在拖拽状态下进行一次额外的更新
                    UpdateUIElementPos();
                }
            }
            spriteBatch.Draw(Texture, DrawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出UI主体
            spriteBatch.Draw(CWRUtils.GetT2DValue("CalamityMod/UI/DraedonSummoning/DecryptCancelIcon"), DrawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出关闭按键
            if (onCloseP) {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "关闭", DrawPos.X, DrawPos.Y, Color.Gold, Color.Black, new Vector2(0.3f), 1.1f + Math.Abs(MathF.Sin(Main.GameUpdateCount * 0.05f) * 0.1f));
            }
            if (previewItems != null) {
                for (int i = 0; i < items.Length; i++) {//遍历绘制出UI格中的所有预览物品
                    if (previewItems[i] != null) {
                        Item item = previewItems[i];
                        if (item != null) {
                            DrawItemIcons(spriteBatch, item, ArcCellPos(i), alp: 0.25f);
                            //Main.DrawItemIcon(spriteBatch, item, ArcCellPos(i), Color.White * 0.25f, 1);
                        }
                    }
                }
            }
            if (items != null) {
                for (int i = 0; i < items.Length; i++) {//遍历绘制出UI格中的所有物品
                    if (items[i] != null) {
                            Item item = items[i];
                            if (item != null) {
                                DrawItemIcons(spriteBatch, item, ArcCellPos(i));
                            }
                    }
                }
            }

            Texture2D arrow = CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/InputArrow2");
            if (inputItem != null && inputItem?.type != 0) {//如果输出格有物品，那么将它画出来
                DrawItemIcons(spriteBatch, inputItem, DrawPos + new Vector2(555, 215), overSlp: 1.5f);
                arrow = CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/InputArrow");
            }
            spriteBatch.Draw(arrow, DrawPos + new Vector2(460, 225), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出输出箭头

            if (onMainP2 && inCoordIndex >= 0 && inCoordIndex <= 80) { //处理鼠标在UI格中查看物品的事情
                Item overItem = items[inCoordIndex];
                if (overItem == null)
                    overItem = new Item();
                Main.HoverItem = overItem.Clone();
                Main.hoverItemName = overItem.Name;
                if (Main.mouseItem.type == ItemID.None && items[inCoordIndex]?.type == ItemID.None && previewItems != null) {
                    Item previewItem = previewItems[inCoordIndex];
                    Main.HoverItem = previewItem.Clone();
                    Main.hoverItemName = previewItem.Name;
                }
            }
            if (onInputP && inputItem != null && inputItem?.type != 0) {//处理查看输出结果物品的事情
                Main.HoverItem = inputItem.Clone();
                Main.hoverItemName = inputItem.Name;
            }
        }
    }
}
