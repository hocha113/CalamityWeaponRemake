using CalamityMod.Items;
using CalamityMod;
using CalamityWeaponRemake.Content.Items.Rogue;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Rogue;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.RemakeItems.Rogue
{
    internal class RWaveSkipper : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.WaveSkipper>();
        public override int ProtogenesisID => ModContent.ItemType<WaveSkipper>();
        public override void SetStaticDefaults() {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.WaveSkipper>()] = true;
        }

        public override void SetDefaults(Item item) {
            item.width = 44;
            item.damage = 80;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useAnimation = item.useTime = 22;
            item.useStyle = ItemUseStyleID.Swing;
            item.knockBack = 4f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 44;
            item.value = CalamityGlobalItem.Rarity5BuyPrice;
            item.rare = ItemRarityID.Pink;
            item.shoot = ModContent.ProjectileType<RWaveSkipperProjectile>();
            item.shootSpeed = 12f;
            item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool? AltFunctionUse(Item item, Player player) {
            return true;
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.altFunctionUse == 2) {
                if (player.Calamity().StealthStrikeAvailable()) {
                    for (int i = 0; i < 9; i++) {
                        Vector2 spanPos = position + new Vector2(Main.rand.Next(-160, 160), Main.rand.Next(-560, -500));
                        Vector2 vr = spanPos.To(Main.MouseWorld).UnitVector().RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.Next(13, 17);
                        int stealth = Projectile.NewProjectile(source, spanPos, vr, ModContent.ProjectileType<WaveSkipperProjectile>(), damage, knockback, player.whoAmI);
                        if (stealth.WithinBounds(Main.maxProjectiles)) {
                            Main.projectile[stealth].Calamity().stealthStrike = true;
                            Main.projectile[stealth].tileCollide = false;
                            Main.projectile[stealth].MaxUpdates = 3;
                        }

                    }
                    return false;
                }
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WaveSkipperProjectile>(), damage, knockback, player.whoAmI);
            }
            else {
                if (player.Calamity().StealthStrikeAvailable()) {
                    for (int i = -WaveSkipper.SpreadAngle; i < WaveSkipper.SpreadAngle * 2; i += WaveSkipper.SpreadAngle) {
                        Vector2 spreadVelocity = player.SafeDirectionTo(Main.MouseWorld).RotatedBy(MathHelper.ToRadians(i)) * item.shootSpeed;
                        int stealth = Projectile.NewProjectile(source, position, spreadVelocity, ModContent.ProjectileType<RWaveSkipperProjectile>(), damage, knockback, player.whoAmI);
                        if (stealth.WithinBounds(Main.maxProjectiles))
                            Main.projectile[stealth].Calamity().stealthStrike = true;
                    }
                    return false;
                }
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<RWaveSkipperProjectile>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
