using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content
{
    public class CWRPlayer : ModPlayer
    {
        public int theRelicLuxor = 0;

        public int CompressorPanelID = -1;

        public bool inFoodStallChair;

        public override void Initialize()
        {
            theRelicLuxor = 0;
        }

        public override void ResetEffects()
        {
            theRelicLuxor = 0;
            inFoodStallChair = false;
        }

        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            base.OnHurt(info);
        }

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
