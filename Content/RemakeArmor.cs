using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class RemakeArmor : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.BeetleScaleMail)
            {
                item.defense = 15;
            }
            if (item.type == ItemID.BeetleShell)
            {
                item.defense = 42;
            }
            if (item.type == ItemID.BeetleHelmet)
            {
                item.defense = 20;
            }
            if (item.type == ItemID.BeetleLeggings)
            {
                item.defense = 16;
            }
        }
    }
}
