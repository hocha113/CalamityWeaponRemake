using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.AnnihilatingUniverseProj;
using CalamityMod.Items.Materials;
using CalamityWeaponRemake.Content.Tiles;
using CalamityWeaponRemake.Content.Items.Placeable;
using System.Buffers;
using System.Linq;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    internal class AnnihilatingUniverse : ModItem
    {
        public override string Texture => CWRConstant.Item_Ranged + "AnnihilatingUniverse";
        public new string LocalizationCategory => "Items.Weapons.Ranged";

        public override void SetStaticDefaults() {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override void SetDefaults() {
            Item.damage = 1350;
            Item.width = 62;
            Item.height = 128;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AnnihilatingUniverseHeldProj>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.CWR().remakeItem = true;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool(3) && player.ownedProjectileCounts[Item.shoot] > 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y
                , ModContent.ProjectileType<AnnihilatingUniverseHeldProj>(), damage, knockback, player.whoAmI, ai2: player.altFunctionUse == 0 ? 0 : 1);
            return false;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<CalamityMod.Items.Weapons.Ranged.Deathwind>()
                .AddIngredient<CalamityMod.Items.Weapons.Ranged.Alluvion>()
                .AddIngredient<CalamityMod.Items.Weapons.Magic.Apotheosis>()
                .AddIngredient<Rock>()
                .AddIngredient<CosmiliteBar>(150)//宇宙锭
                .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => {
                    if (CWRIDs.MaterialsTypes2.Contains(type)) {
                        amount = 0;
                    }
                })
                .AddOnCraftCallback(CWRRecipes.SpawnAction)
                .AddTile(ModContent.TileType<TransmutationOfMatter>())
                .Register();
        }
    }
}
