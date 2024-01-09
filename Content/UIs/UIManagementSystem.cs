using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
using Terraria.ModLoader.IO;
using System;

namespace CalamityWeaponRemake.Content.UIs
{
    internal class UIManagementSystem : ModSystem
    {
        public override void SaveWorldData(TagCompound tag) {
            if (SupertableUI.Instance != null && SupertableUI.Instance?.items != null) {
                for (int i = 0; i < SupertableUI.Instance.items.Length; i++) {
                    if (SupertableUI.Instance.items[i] == null) {
                        SupertableUI.Instance.items[i] = new Item(0);
                    }
                }
                tag.Add("SupertableUI_ItemDate", SupertableUI.Instance.items);
            }
        }

        public override void LoadWorldData(TagCompound tag) {
            if (SupertableUI.Instance != null && tag.ContainsKey("SupertableUI_ItemDate")) {
                Item[] loadSupUIItems = tag.Get<Item[]>("SupertableUI_ItemDate");
                for (int i = 0; i < loadSupUIItems.Length; i++) {
                    if (loadSupUIItems[i] == null) {
                        loadSupUIItems[i] = new Item(0);
                    }
                }
                SupertableUI.Instance.items = tag.Get<Item[]>("SupertableUI_ItemDate");
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseIndex = layers.FindIndex((GameInterfaceLayer layer) => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex != -1) {
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("DFs UI", delegate {
                    if (Main.LocalPlayer.CWR().CompressorPanelID != -1)
                        CompressorUI.instance.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Sp UI", delegate {
                    if (SupertableUI.Instance.Active) {
                        SupertableUI.Instance.Draw(Main.spriteBatch);
                        RecipeUI.Instance.Draw(Main.spriteBatch);
                        DragButton.instance.Draw(Main.spriteBatch);
                    }  
                    return true;
                }, InterfaceScaleType.UI));
            }
        }

        public override void UpdateUI(GameTime gameTime) {
            base.UpdateUI(gameTime);
            if (Main.LocalPlayer.CWR().CompressorPanelID != -1)
                CompressorUI.instance.Update();
            if (SupertableUI.Instance.Active) {
                SupertableUI.Instance.Update(gameTime);
                RecipeUI.Instance.Update(gameTime);
                DragButton.instance.Update(gameTime);
            }  
        }
    }
}
