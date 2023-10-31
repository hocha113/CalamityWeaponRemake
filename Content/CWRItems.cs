﻿using CalamityMod;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityWeaponRemake.Content
{
    public class CWRItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

        /// <summary>
        /// 用于存储物品的状态值，对这个数组的使用避免了额外类成员的创建
        /// (自建类成员数据对于修改物品而言总是令人困惑)。
        /// 这个数组不会自动的网络同步，需要在合适的时机下调用同步指令
        /// </summary>
        public float[] ai = new float[] { 0, 0, 0 };
        /// <summary>
        /// 是否是一个重制物品
        /// </summary>
        public bool remakeItem;
        /// <summary>
        /// 是否正在真近战
        /// </summary>
        public bool closeCombat;
        /// <summary>
        /// 正在手持这个物品的玩家实例
        /// </summary>
        public Player HoldOwner = null;
        /// <summary>
        /// 一般用于近战类武器的充能值
        /// </summary>
        public float MeleeCharge;

        public override void NetSend(Item item, BinaryWriter writer)
        {
            base.NetSend(item, writer);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            base.NetReceive(item, reader);
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("_MeleeCharge", MeleeCharge);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            MeleeCharge = tag.GetFloat("_MeleeCharge");
        }

        public override void HoldItem(Item item, Player player)
        {
            OwnerByDir(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (remakeItem)
            {
                TooltipLine nameLine = tooltips.FirstOrDefault((x) => x.Name == "ItemName" && x.Mod == "Terraria");
                ApplyNameLineColor(
                    new Color(1f, 0.72f + 0.2f * Main.DiscoG / 255f, 0.45f + 0.5f * Main.DiscoG / 255f)
                    , nameLine
                    );

                AppAwakeningLine(tooltips);
            }
        }

        private void OwnerByDir(Item item, Player player)
        {
            if (item.useStyle == ItemUseStyleID.Swing && player.whoAmI == Main.myPlayer && (player.PressKey() || player.PressKey(false)))
            {
                player.direction = Math.Sign(player.position.To(Main.MouseWorld).X);
            }
        }

        private void ApplyNameLineColor(Color color, TooltipLine nameLine) => nameLine.OverrideColor = color;

        public static void AppAwakeningLine(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(CalamityWeaponRemake.Instance, "CalamityWeaponRemake",
                    CalamityUtils.ColorMessage(
                        GameUtils.Translation("- 觉醒 -", "- Awakening -")
                        , new Color(196, 35, 44))
                    );
            tooltips.Add(line);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CWRPlayer modPlayer = player.CWR();
            int theReLdamags = damage / 3;
            Vector2 vr = player.Center.To(Main.MouseWorld).UnitVector() * 13;

            if (player.whoAmI == Main.myPlayer)
            {
                if (modPlayer.theRelicLuxor == 1)
                {
                    if (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<TrueMeleeNoSpeedDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI, 1);
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRogue>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<RangedDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRanged>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<MagicDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMagic>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<SummonDamageClass>()
                        && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorSummon>()] < 3)
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorSummon>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                }
                if (modPlayer.theRelicLuxor == 2)
                {
                    theReLdamags += 15;

                    if (item.CountsAsClass<MeleeDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI, 1);
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRogue>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<RangedDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRanged>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<MagicDamageClass>())
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMagic>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<SummonDamageClass>()
                        && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorSummon>()] < 6)
                    {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorSummon>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}