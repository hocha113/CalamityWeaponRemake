using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RDragonRage : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.DragonRage>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.DragonRage>(item))
            {
                item.damage = 1075;
                item.knockBack = 7.5f;
                item.useAnimation = (item.useTime = 25);
                item.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
                item.noMelee = true;
                item.channel = true;
                item.autoReuse = true;
                item.shootSpeed = 14f;
                item.shoot = ModContent.ProjectileType<RemakeDragonRageStaff>();
                item.width = 128;
                item.height = 140;
                item.noUseGraphic = true;
                item.useStyle = ItemUseStyleID.Shoot;
                item.UseSound = SoundID.DD2_SkyDragonsFurySwing;
                item.value = CalamityGlobalItem.Rarity15BuyPrice;
                item.rare = ModContent.RarityType<Violet>();
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.DragonRage>(item))
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                {
                    SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                    Main.projectile[proj].ai[1] = 1;
                    Main.projectile[proj].scale = 0.5f;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.DragonRage>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.DragonRage>(item))
            {
                if (player.ownedProjectileCounts[item.shoot] > 0)
                    return false;
            }
            return base.UseItem(item, player);
        }
    }
}
