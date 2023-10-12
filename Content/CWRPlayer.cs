using Terraria;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class CWRPlayer : ModPlayer
    {
        public int KevinCharge = 500;
        public float BansheeHookCharge = 500;

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByNPC(npc, ref modifiers);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByProjectile(proj, ref modifiers);
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            return base.CanBeHitByProjectile(proj);
        }
    }
}
