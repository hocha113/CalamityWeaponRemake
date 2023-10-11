using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Projectiles;

namespace CalamityWeaponRemake.Content.Items
{
    internal class GodIarmHem : ModItem
    {
        public override string Texture => CWRConstant.placeholder;

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
            Item.shoot = 10;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int w = 1200;
            int h = 1200;
            int types = ModContent.ProjectileType<DoGDeaths>();
            for (int j = 0; j < 4; j++)
            {
                Main.NewText(13);
                for (int i = 0; i < 15; i++)
                {
                    Vector2 spanPos = new Vector2(player.position.X + w, player.position.Y + h);
                    Projectile.NewProjectile(
                        source,
                        spanPos,
                        new Vector2(0, -3),
                        types,
                        20,
                        0,
                        -1
                        );
                    w -= 210;
                }
            }
            
            return false;
        }
    }
}
