using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Ranged.HeldProjs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.CWRUtils;

namespace CalamityWeaponRemake.Content.RemakeItems.Ranged
{
    internal class RemakeDragonsBreath : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<DragonsBreath>() && CWRConstant.ForceReplaceResetContent)
            {
                item.damage = 328;
                item.DamageType = DamageClass.Ranged;
                item.width = 124;
                item.height = 78;
                item.useTime = 9;
                item.useAnimation = 18;
                item.reuseDelay = 15;
                item.useLimitPerAnimation = 2;
                item.autoReuse = true;
                item.useStyle = ItemUseStyleID.Shoot;
                item.noMelee = true;
                item.noUseGraphic = true;
                item.knockBack = 6.5f;
                item.shoot = ModContent.ProjectileType<DragonsBreathHeldProj>();
                item.shootSpeed = 12f;
                item.useAmmo = AmmoID.Bullet;
                item.Calamity().canFirePointBlankShots = true;
                item.rare = ModContent.RarityType<Violet>();
                item.value = CalamityGlobalItem.Rarity15BuyPrice;
            }
        }

        static int heldProj = -1;
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            if (item.type == ModContent.ItemType<DragonsBreath>() && CWRConstant.ForceReplaceResetContent)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<DragonsBreathHeldProj>()] == 0)
                {
                    heldProj = Projectile.NewProjectile(player.parent(), position, velocity, ModContent.ProjectileType<DragonsBreathHeldProj>(), damage, knockback, player.whoAmI);
                    if (player.altFunctionUse == 2)
                    {
                        Main.projectile[heldProj].ai[0] = 1;
                    }
                }
                if (heldProj.ValidateIndex(Main.projectile))
                {
                    Main.projectile[heldProj].localAI[1] = type;
                }
                return false;
            }
            else
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
