using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RBurntSienna : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.BurntSienna>(item))
            {
                item.initialize();
                item.CWR().ai[0]++;
                if (item.CWR().ai[0] > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vr = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))) * Main.rand.NextFloat(0.75f, 1.12f);
                        int proj = Projectile.NewProjectile(
                            source,
                            position,
                            vr,
                            ProjectileID.SandBallGun,
                            item.damage / 2,
                            item.knockBack,
                            player.whoAmI
                            );
                        Main.projectile[proj].hostile = false;
                        Main.projectile[proj].friendly = true;
                    }
                    item.CWR().ai[0] = 0;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
