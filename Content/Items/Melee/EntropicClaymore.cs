using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class EntropicClaymore : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "EntropicClaymore";

        public override void SetDefaults()
        {
            Item.width = 130;
            Item.height = 106;
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.useTime = 26;
            Item.useTurn = true;
            Item.knockBack = 5.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.Rarity9BuyPrice;
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<EntropicFlechetteSmall>();
            Item.shootSpeed = 12f;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            hitbox = CalamityUtils.FixSwingHitbox(118f, 118f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(4, 6);
            for (int index = 0; index < num6; index++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
                float damageMult = 0.5f;
                switch (index)
                {
                    case 0:
                        type = ModContent.ProjectileType<EntropicFlechetteSmall>();
                        break;
                    case 1:
                        type = ModContent.ProjectileType<EntropicFlechette>();
                        damageMult = 0.65f;
                        break;
                    case 2:
                        type = ModContent.ProjectileType<EntropicFlechetteLarge>();
                        damageMult = 0.8f;
                        break;
                }
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)((float)damage * damageMult), knockback, player.whoAmI);
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 27);
            }
        }
    }
}
