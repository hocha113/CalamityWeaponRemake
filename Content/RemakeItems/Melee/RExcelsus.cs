using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RExcelsus : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[ModContent.ItemType<CalamityMod.Items.Weapons.Melee.Excelsus>()] = true;
        }

        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Excelsus>(item))
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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Excelsus>(item))
            {
                CWRUtils.OnModifyTooltips(Mod, item, tooltips, "Excelsus", 2);
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Excelsus>(item))
            {
                if (player.altFunctionUse == 2)
                {
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ExcelsusBomb>(), damage * 3, knockback, player.whoAmI);
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float speedX = velocity.X + Main.rand.NextFloat(-1.5f, 1.5f);
                        float speedY = velocity.Y + Main.rand.NextFloat(-1.5f, 1.5f);
                        switch (i)
                        {
                            case 0:
                                type = ModContent.ProjectileType<ExcelsusMain>();
                                break;
                            case 1:
                                type = ModContent.ProjectileType<ExcelsusBlue>();
                                break;
                            case 2:
                                type = ModContent.ProjectileType<ExcelsusPink>();
                                break;
                        }

                        Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, type, damage, knockback, player.whoAmI);
                    }
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Excelsus>(item))
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Excelsus>(item))
                Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center, Vector2.Zero, ModContent.ProjectileType<LaserFountains>()
                    , item.damage, 0f, player.whoAmI, target.whoAmI);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.Excelsus>(item))
                Projectile.NewProjectile(player.GetSource_ItemUse(item), target.Center, Vector2.Zero, ModContent.ProjectileType<LaserFountains>()
                    , item.damage / 2, 0f, player.whoAmI, target.whoAmI);
        }
    }
}
