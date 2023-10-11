using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.Interfaces;
using CalamityWeaponRemake.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityWeaponRemake.Common.AuxiliaryMeans.AiBehavior;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    internal class DragonsBreath : CustomItems
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
            Item.shoot = ModContent.ProjectileType<DragonsBreathProjectiles>();
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

        int dbpType => ModContent.ProjectileType<DragonsBreathProjectiles>();
        int heldProj = -1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[dbpType] == 0)
            {
                heldProj = Projectile.NewProjectile(GetEntitySource_Parent(player), position, velocity, dbpType, damage, knockback, player.whoAmI);
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
