using CalamityMod;
using CalamityWeaponRemake.Common;
using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Buffs
{
    internal class TyrantsFury : ModBuff
    {
        public override string Texture => CWRConstant.Buff + "TyrantsFury";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.Calamity().tFury = true;
        }
    }
}
