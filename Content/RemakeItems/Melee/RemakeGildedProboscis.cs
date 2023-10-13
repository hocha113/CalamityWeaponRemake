using CalamityMod.Items;
using CalamityMod;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Sounds;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RemakeGildedProboscis : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.Spears[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GildedProboscis>()] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.GildedProboscis>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (GameUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GildedProboscis>(item))
            {
                item.width = 66;
                item.damage = 315;
                item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
                item.noMelee = true;
                item.useTurn = true;
                item.noUseGraphic = true;
                item.useAnimation = 19;
                item.useStyle = ItemUseStyleID.Shoot;
                item.useTime = 19;
                item.knockBack = 8.75f;
                item.UseSound = SoundID.Item1;
                item.autoReuse = true;
                item.height = 66;
                item.value = CalamityGlobalItem.Rarity11BuyPrice;
                item.rare = 11;
                item.shoot = ModContent.ProjectileType<RemakeGildedProboscisProj>();
                item.shootSpeed = 13f;
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (GameUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GildedProboscis>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (GameUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.GildedProboscis>(item))
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                {
                    SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                    Main.projectile[proj].ai[1] = 1;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
