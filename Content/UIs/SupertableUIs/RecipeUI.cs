using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Placeable;
using CalamityWeaponRemake.Content.UIs.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.UIs.SupertableUIs
{
    internal class RecipeUI : CWRUIPanel
    {
        public static RecipeUI instance;

        public int index;

        private static List<Item> itemValue = new List<Item>();

        private static List<string[]> itemNames = new List<string[]>();

        private Rectangle mainRec;

        private Rectangle rAow;

        private Rectangle lAow;

        private bool onM;

        private bool onR;

        private bool onL;

        public override Texture2D Texture => CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/RecPBook");

        public override void Initialize() {
            if (SupertableUI.instance != null) {
                DrawPos = SupertableUI.instance.DrawPos + new Vector2(545, 80);
            }
            mainRec = new Rectangle((int)(DrawPos.X), (int)(DrawPos.Y), Texture.Width, Texture.Height);
            rAow = new Rectangle((int)DrawPos.X + 65, (int)DrawPos.Y + 20, 25, 25);
            lAow = new Rectangle((int)(DrawPos.X - 30), (int)(DrawPos.Y + 20), 25, 25);
            onM = mainRec.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onR = rAow.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
            onL = lAow.Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
        }

        public override void Load() {
            instance = this;
            for (int i = 0; i < SupertableUI.AllRecipes.Count; i++) {
                Console.WriteLine($"正在装载配方：{i} --:-- {SupertableUI.AllRecipes.Count}");
                itemValue.Add(new Item(SupertableUI.AllRecipes[i].Target));
                itemNames.Add(SupertableUI.AllRecipes[i].Values);
            }
        }

        public void LoadZenithWRecipes() {
            if (Main.zenithWorld) {
                if (itemValue.Count < SupertableUI.AllRecipes.Count) {
                    int index = SupertableUI.AllRecipes.Count - 1;
                    itemValue.Add(new Item(SupertableUI.AllRecipes[index].Target));
                    itemNames.Add(SupertableUI.AllRecipes[index].Values);
                }
            }
            else {
                for (int i = 0; i < itemValue.Count; i++) {
                    if (itemValue[i].type == ModContent.ItemType<InfiniteToiletItem>()) {
                        itemValue.RemoveAt(i);
                        itemNames.RemoveAt(i);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime) {
            Initialize();
            int museS = DownStartL();
            if (museS == 1) {
                if (onR) {
                    SoundEngine.PlaySound(SoundID.Chat);
                    index += 1;
                }
                if (onL) {
                    SoundEngine.PlaySound(SoundID.Chat);
                    index -= 1;
                }
                if (index < 0) {
                    index = itemValue.Count - 1;
                }
                if (index > itemValue.Count - 1) {
                    index = 0;
                }

                LoadPsreviewItems();
            }
        }

        public void LoadPsreviewItems() {
            if (SupertableUI.instance != null) {
                if (SupertableUI.instance.previewItems == null) {
                    SupertableUI.instance.previewItems = new Item[81];
                }
                if (SupertableUI.instance.items == null) {
                    SupertableUI.instance.items = new Item[81];
                }
                SupertableUI.instance.previewItems = new Item[SupertableUI.instance.items.Length];
                string[] names = itemNames[index];
                if (names != null) {
                    Item sdItem = new Item();
                    for (int i = 0; i < 81; i++) {
                        Item item = new Item(0);
                        string value = names[i];
                        item = new Item(SupertableUI.InStrGetItemType(value, true));
                        SupertableUI.instance.previewItems[i] = item;
                    }
                }
                else {
                    for (int i = 0; i < 81; i++) {
                        SupertableUI.instance.previewItems[i] = new Item();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (DragButton.instance != null) {
                if (DragButton.instance.onDrag) {//为了防止拖拽状态下出现位置更新的延迟所导致的果冻感，这里用于在拖拽状态下进行一次额外的更新
                    Initialize();
                }
            }
            Texture2D arow = CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/BlueArrow");
            Texture2D arow2 = CWRUtils.GetT2DValue("CalamityWeaponRemake/Assets/UIs/SupertableUIs/BlueArrow2");
            spriteBatch.Draw(Texture, DrawPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);//绘制出UI主体
            spriteBatch.Draw(onR ? arow : arow2, DrawPos + new Vector2(65, 20), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(onL ? arow : arow2, DrawPos + new Vector2(-30, 20), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
            string text2 = $"{index + 1} -:- {itemValue.Count}";
            Terraria.Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text2, DrawPos.X - text2.Length * 5 + 40, DrawPos.Y + 65, Color.White, Color.Black, new Vector2(0.3f), 0.8f);
            if (itemValue != null && SupertableUI.instance != null && index >= 0 && index < itemValue.Count) {
                SupertableUI.instance.DrawItemIcons(spriteBatch, itemValue[index], DrawPos + new Vector2(5, 5), alp: 0.6f, overSlp: 1.5f);
                string name = itemValue[index].HoverName;
                string text = $"查看配方：{(name == "" ? "无" : name)}";
                Terraria.Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, DrawPos.X - text.Length * 5, DrawPos.Y - 25, Color.White, Color.Black, new Vector2(0.3f), 0.8f);
            }
            if (onM) { //处理鼠标在UI格中查看物品的事情
                Item overItem = itemValue[index];
                if (overItem != null && overItem?.type != ItemID.None) {
                    Main.HoverItem = overItem.Clone();
                    Main.hoverItemName = overItem.Name;
                }
            }
        }
    }
}
