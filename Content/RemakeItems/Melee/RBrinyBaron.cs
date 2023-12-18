﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RBrinyBaron : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrinyBaron>();
        public override int ProtogenesisID => ModContent.ItemType<BrinyBaron>();
        public override void Load() {
            SetReadonlyTargetID = TargetID;
        }
        public override void SetDefaults(Item item)
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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "BrinyBaron", 2);
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(BuffID.Wet, 180);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-10)), type, damage, knockback, player.whoAmI);
            return true;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2) {
                damage = (int)(player.GetTotalDamage(DamageClass.Melee).ApplyTo(item.OriginalDamage) * 0.2f);
                type = ModContent.ProjectileType<Razorwind>();
            }
            else {
                type = 0;
            }
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.Wet, 120);
            int newDef = target.defDefense - 3;
            if (newDef < 0) newDef = 0;
            target.defense = newDef;
            modifiers.CritDamage *= 0.5f;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 speed = CWRUtils.RandomBooleanValue(2, 1, true) ? new Vector2(16, 0) : new Vector2(-16, 0);
            if (Main.projectile.Count(n => n.active && n.type == ModContent.ProjectileType<SeaBlueBrinySpout>() && n.ai[1] == 1) <= 2) {
                int proj = Projectile.NewProjectile(Common.CWRUtils.parent(player), target.Center, speed, ModContent.ProjectileType<SeaBlueBrinySpout>(), item.damage, item.knockBack, player.whoAmI);
                Main.projectile[proj].timeLeft = 60;
                Main.projectile[proj].localAI[1] = 30;
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (player.altFunctionUse == 2) {
                item.noMelee = true;
            }
            else {
                item.noMelee = false;
            }
            return null;
        }

        public override void UseAnimation(Item item, Player player)
        {
            item.noUseGraphic = false;
            item.UseSound = SoundID.Item1;
            if (player.altFunctionUse == 2) {
                item.noUseGraphic = true;
                item.UseSound = SoundID.Item84;
            }
        }
    }
}
