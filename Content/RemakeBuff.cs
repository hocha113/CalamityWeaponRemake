using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class RemakeBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {
                case 98:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.05f;
                    break;
                case 99:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f;
                    break;
                case 100:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
                    break;
            }
        }
    }
}
