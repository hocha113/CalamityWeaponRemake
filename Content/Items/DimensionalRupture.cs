using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Interfaces;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace CalamityWeaponRemake.Content.Items
{
    internal class DimensionalRupture : CustomItems
    {
        public override string Texture => CWRConstant.Item + "DimensionalRupture";

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {

        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {

        }

        public override void UpdateInventory(Player player)
        {

        }

        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}
