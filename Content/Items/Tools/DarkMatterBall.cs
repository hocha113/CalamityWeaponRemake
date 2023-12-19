using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityWeaponRemake.Content.Items.Tools
{
    internal class DarkMatterBall : ModItem
    {
        public override string Texture => CWRConstant.Item + "Tools/" + (isEmpty ? "Empty" : "Full");
        public bool isEmpty;
        public List<int> dorpTypes = new List<int>();
        public List<Item> dorpItems;
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults() {
            Item.maxStack = 1;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Purple;
        }

        public override void UpdateInventory(Player player) {
            if (Main.GameUpdateCount % 10f == 0) {
                isEmpty = !isEmpty;
                TextureAssets.Item[Type] = CWRUtils.GetT2DAsset(Texture);
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed) {
            if (Main.GameUpdateCount % 10f == 0) {
                isEmpty = !isEmpty;
                TextureAssets.Item[Type] = CWRUtils.GetT2DAsset(Texture);
            }
        }

        public override bool CanRightClick() {
            return true;
        }

        public void LoadDorp() {
            if (dorpTypes.Count > 0) {
                dorpTypes.Sort();
                var groupedDrops = dorpTypes.GroupBy(x => x);
                foreach (var group in groupedDrops) {
                    List<int> items = group.ToList();
                    int types = items[0];
                    Item dorpItemValue = new Item(types);
                    dorpItemValue.stack = items.Count;
                    if (dorpItemValue != null)
                        dorpItems.Add(dorpItemValue);
                }
            }
        }

        public override void RightClick(Player player) {
            if (dorpItems == null) {
                dorpItems = new List<Item>();
                LoadDorp();
            }
            if (dorpTypes.Count > 0) {
                dorpTypes.Sort();
                var groupedDrops = dorpTypes.GroupBy(x => x);
                foreach (var group in groupedDrops) {
                    List<int> items = group.ToList();
                    int types = items[0];
                    Item dorpItemValue = new Item(types);
                    player.QuickSpawnItem(player.parent(), dorpItemValue, items.Count);
                }
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (dorpItems == null) {
                dorpItems = new List<Item>();
                LoadDorp();
            }
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                Color rarityColor = Main.DiscoColor;
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                for (int i = 0; i < dorpItems.Count; i++) {
                    Item item = dorpItems[i];
                    string text = item.HoverName;
                    ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, text, basePosition + new Vector2(0, 22 * i + 66), Color.White, 0f, Vector2.Zero, Vector2.One * 0.9f);
                }
            }
            return true;
        }
    }
}
