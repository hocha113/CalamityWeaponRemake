using CalamityMod.Items;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue.GangarusProjectiles;
using Mono.Cecil;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Items.Rogue.Extras
{
    internal class Gangarus : ModItem
    {
        public static SoundStyle BelCanto = new("CalamityWeaponRemake/Assets/Sounds/BelCanto") { Volume = 3.5f };
        public static SoundStyle AT = new("CalamityWeaponRemake/Assets/Sounds/AT") { Volume = 1.5f };
        public override string Texture => CWRConstant.Item + "Rogue/Gangarus";
        public int ChargeGrade;
        public override void SetDefaults() {
            Item.width = 44;
            Item.damage = 4480;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<GangarusProjectile>();
            Item.shootSpeed = 15f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override void HoldItem(Player player) {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GangarusHeldProjectile>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<GangarusProjectile>()] == 0) {
                Projectile.NewProjectile(player.parent(), player.Center, Vector2.Zero, ModContent.ProjectileType<GangarusHeldProjectile>(), 0, 0, player.whoAmI);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (ChargeGrade > 0) {
                int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<GangarusProjectile>(), damage * ChargeGrade, knockback, player.whoAmI);
                Main.projectile[proj].Calamity().stealthStrike = true;
                Main.projectile[proj].ai[1] = ChargeGrade;
                ChargeGrade = 0;
                return false;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
