using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Ranged.HeldProjs;
using CalamityWeaponRemake.Common.AuxiliaryMeans;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    /// <summary>
    /// 烈风
    /// </summary>
    internal class Galeforce : ModItem
    {
        public new string LocalizationCategory => "Items.Weapons.Ranged";

        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Galeforce";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 52;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override void HoldItem(Player player)
        {
            Item.initialize();
            Projectile heldProj = AiBehavior.GetProjectileInstance((int)Item.CWR().ai[0]);
            if (heldProj != null && heldProj.type == ModContent.ProjectileType<GaleforceHeldProj>())
            {
                heldProj.localAI[1] = Item.CWR().ai[1];
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.initialize();
            Item.CWR().ai[1] = type;
            int heldType = ModContent.ProjectileType<GaleforceHeldProj>();
            if (player.ownedProjectileCounts[heldType] <= 0)
            {
                Item.CWR().ai[0] = Projectile.NewProjectile(source, position, Vector2.Zero
                , heldType
                , damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
