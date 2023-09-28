using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    public class RemakeBrinyBaron : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                item.damage = 110;
                item.knockBack = 2f;
                item.useAnimation = item.useTime = 15;
                item.DamageType = DamageClass.Melee;
                item.useTurn = true;
                item.autoReuse = true;
                item.shootSpeed = 4f;
                item.shoot = ModContent.ProjectileType<Razorwind>();
                item.width = 100;
                item.height = 102;
                item.useStyle = ItemUseStyleID.Swing;
                item.UseSound = SoundID.Item1;
                item.value = CalamityGlobalItem.Rarity8BuyPrice;
                item.rare = ItemRarityID.Yellow;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                player.AddBuff(BuffID.Wet, 180);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10)), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-10)), type, damage, knockback, player.whoAmI);
                return true;
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                if (player.altFunctionUse == 2)
                {
                    damage = (int)(player.GetTotalDamage(DamageClass.Melee).ApplyTo(item.OriginalDamage) * 0.2f);
                    type = ModContent.ProjectileType<Razorwind>();
                }
                else
                {
                    type = 0;
                }
                return;
            }
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                target.AddBuff(BuffID.Wet, 120);
                int newDef = target.defDefense - 3;
                if (newDef < 0) newDef = 0;
                target.defense = newDef;
                modifiers.CritDamage *= 0.5f;
            }
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                Vector2 speed = HcMath.RandomBooleanValue(2, 1, true) ? new Vector2(16, 0) : new Vector2(-16, 0);
                if (Main.projectile.Count(n => n.active && n.type == ModContent.ProjectileType<SeaBlueBrinySpout>() && n.ai[1] == 1) <= 2)
                {
                    int proj = Projectile.NewProjectile(AiBehavior.GetEntitySource_Parent(player), target.Center, speed, ModContent.ProjectileType<SeaBlueBrinySpout>(), item.damage, item.knockBack, player.whoAmI);
                    Main.projectile[proj].timeLeft = 60;
                    Main.projectile[proj].localAI[1] = 30;
                }
                return;
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                if (player.altFunctionUse == 2)
                {
                    item.noMelee = true;
                }
                else
                {
                    item.noMelee = false;
                }
                return null;
            }
            else
            {
                return base.UseItem(item, player);
            }

        }

        public override void UseAnimation(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>() && CWRConstant.ForceReplaceResetContent)
            {
                item.noUseGraphic = false;
                item.UseSound = SoundID.Item1;
                if (player.altFunctionUse == 2)
                {
                    item.noUseGraphic = true;
                    item.UseSound = SoundID.Item84;
                }
            }
        }
    }
}
