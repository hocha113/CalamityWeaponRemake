using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.Interfaces;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.CWRUtils;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    internal class DragonsBreath : ModItem
    {
        public override string Texture => CWRConstant.Item + "Ranged/" + "DragonsBreath";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 328;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 124;
            Item.height = 78;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.reuseDelay = 15;
            Item.useLimitPerAnimation = 2;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6.5f;
            Item.shoot = ModContent.ProjectileType<DragonsBreathHeldProj>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        int dbpType => ModContent.ProjectileType<DragonsBreathHeldProj>();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.initialize();
            if (player.ownedProjectileCounts[dbpType] == 0)
            {
                Item.CWR().ai[0] = Projectile.NewProjectile(CWRUtils.parent(player), position, velocity, dbpType, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                {
                    Main.projectile[(int)Item.CWR().ai[0]].ai[0] = 1;
                }
            }
            Projectile projectile = GetProjectileInstance((int)Item.CWR().ai[0]);
            if (projectile != null)
            {
                projectile.localAI[1] = type;
            }
            return false;
        }
    }
}
