using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RWindBlade : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.WindBlade>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.WindBlade>(item))
            {
                item.width = 78;
                item.damage = 250;
                item.DamageType = DamageClass.Melee;
                item.useAnimation = 15;
                item.useStyle = ItemUseStyleID.Swing;
                item.useTime = 15;
                item.useTurn = true;
                item.knockBack = 8f;
                item.UseSound = SoundID.Item1;
                item.autoReuse = true;
                item.height = 94;
                item.value = CalamityGlobalItem.Rarity14BuyPrice;
                item.rare = ModContent.RarityType<DarkBlue>();
                item.shoot = ModContent.ProjectileType<ExcelsusMain>();
                item.shootSpeed = 12f;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.WindBlade>(item))
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback, player.whoAmI);
                if (player.altFunctionUse == 2)
                {
                    Main.projectile[proj].ai[0] = 1;
                    Main.projectile[proj].timeLeft = 360;
                    Main.projectile[proj].damage = damage / 3;

                    for (int i = 0; i <= 360; i += 3)
                    {
                        Vector2 vr = new Vector2(3f, 3f).RotatedBy(MathHelper.ToRadians(i));
                        int num = Dust.NewDust(player.Center, player.width, player.height, DustID.Smoke, vr.X, vr.Y, 200, new Color(232, 251, 250, 200), 1.4f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position = player.Center;
                        Main.dust[num].velocity = vr;
                    }
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.WindBlade>(item))
            {
                if (player.altFunctionUse == 2)
                {
                    item.noMelee = true;
                }
                else
                {
                    item.noMelee = false;
                }
            }
            return base.UseItem(item, player);
        }

        public override void UseAnimation(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.WindBlade>(item))
            {
                item.noUseGraphic = false;
                item.UseSound = SoundID.Item1;
                if (player.altFunctionUse == 2)
                {
                    item.noUseGraphic = true;
                    item.UseSound = SoundID.Item60;
                }
            }
            base.UseAnimation(item, player);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.WindBlade>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }
    }
}
