﻿using CalamityWeaponRemake.Common;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Tools
{
    internal class EndlessStabilizer : ModItem
    {
        public override string Texture => CWRConstant.Item + "Tools/EndlessStabilizer";
        public override void SetStaticDefaults() {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 16));
        }

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.expert = true;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.CWR().EndlessStabilizerBool = true;
        }
    }
}
