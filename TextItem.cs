using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake
{
    internal class TextItem : ModItem
    {
        public override string Texture => "CalamityWeaponRemake/icon";

        public override void SetDefaults() {
            Item.width = 80;
            Item.height = 80;
            Item.damage = 9999;
            Item.DamageType = DamageClass.Default;
            Item.useAnimation = Item.useTime = 13;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void HoldItem(Player player) {
            Main.NewText(Mod.Name);
            base.HoldItem(player);
        }

        public override bool? UseItem(Player player) {
            return base.UseItem(player);
        }

        public void CedTile() {
            int wid = 11;
            int hig = 11;
            Vector2 offset = new Vector2(wid, hig) / -2 * 16;
            Vector2 mouPos = Main.MouseWorld + offset;
            Vector2 landDTIlePos = CWRUtils.WEPosToTilePos(mouPos);
            for (int y = 0; y < hig; y++) {
                for (int x = 0; x < wid; x++) {
                    //CWRUtils.HasSolidTile
                }
            }
        }
    }
}
