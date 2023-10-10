using CalamityMod.Items;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
using CalamityMod.Sounds;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class GildedProboscis : ModItem
    {
        public const float TargetingDistance = 2084f;

        public const int LightningArea = 2800;

        public override string Texture => CWRConstant.Item_Melee + "GildedProboscis";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 315;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = 5;
            Item.useTime = 19;
            Item.knockBack = 8.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = CalamityGlobalItem.Rarity11BuyPrice;
            Item.rare = 11;
            Item.shoot = ModContent.ProjectileType<RemakeGildedProboscisProj>();
            Item.shootSpeed = 13f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.altFunctionUse == 2)
            {
                SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                Main.projectile[proj].ai[1] = 1;
            }
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }
}
