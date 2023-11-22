using CalamityMod;
using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Ranged
{
    internal class RGaleforce : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Galeforce>(item))
            {
                item.damage = 18;
                item.DamageType = DamageClass.Ranged;
                item.width = 32;
                item.height = 52;
                item.useTime = 20;
                item.useAnimation = 20;
                item.useStyle = ItemUseStyleID.Shoot;
                item.noMelee = true;
                item.noUseGraphic = true;
                item.knockBack = 3f;
                item.value = CalamityGlobalItem.Rarity3BuyPrice;
                item.rare = ItemRarityID.Orange;
                item.UseSound = SoundID.Item5;
                item.autoReuse = true;
                item.shoot = ProjectileID.WoodenArrowFriendly;
                item.shootSpeed = 20f;
                item.useAmmo = AmmoID.Arrow;
                item.Calamity().canFirePointBlankShots = true;
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            //if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Galeforce>(item))
            //{
            //    item.initialize();
            //    Projectile heldProj = CWRUtils.GetProjectileInstance((int)item.CWR().ai[0]);
            //    if (heldProj != null && heldProj.type == ModContent.ProjectileType<ArbalestHeldProj>())
            //    {
            //        heldProj.localAI[1] = item.CWR().ai[1];
            //    }
            //}
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Galeforce>(item))
            {
                int heldType = ModContent.ProjectileType<GaleforceHeldProj>();
                if (player.ownedProjectileCounts[heldType] <= 0)
                {
                    item.CWR().ai[0] = Projectile.NewProjectile(source, position, Vector2.Zero
                    , heldType
                    , damage, knockback, player.whoAmI);
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Ranged.Galeforce>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }
    }
}
