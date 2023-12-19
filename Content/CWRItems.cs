using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.Ravager;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.NPCs.RavagerAs;
using CalamityWeaponRemake.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
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

        public override void SetDefaults(Item item) {
            base.SetDefaults(item);
            if (CWRIDs.OnLoadContentBool) {
                if (item.createTile != -1 && !CWRIDs.TileToItem.ContainsKey(item.createTile)) {
                    CWRIDs.TileToItem.Add(item.createTile, item.type);
                }
                if (item.createWall != -1 && !CWRIDs.WallToItem.ContainsKey(item.createWall)) {
                    CWRIDs.WallToItem.Add(item.createWall, item.type);
                }
            }
        }

        public override void NetSend(Item item, BinaryWriter writer) {
            base.NetSend(item, writer);
        }

        public override void NetReceive(Item item, BinaryReader reader) {
            base.NetReceive(item, reader);
        }

        public override void SaveData(Item item, TagCompound tag) {
            tag.Add("_MeleeCharge", MeleeCharge);
        }

        public override void LoadData(Item item, TagCompound tag) {
            MeleeCharge = tag.GetFloat("_MeleeCharge");
        }

        public override void HoldItem(Item item, Player player) {
            OwnerByDir(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (remakeItem) {
                TooltipLine nameLine = tooltips.FirstOrDefault((x) => x.Name == "ItemName" && x.Mod == "Terraria");
                ApplyNameLineColor(
                    new Color(1f, 0.72f + 0.2f * Main.DiscoG / 255f, 0.45f + 0.5f * Main.DiscoG / 255f)
                    , nameLine
                    );

                AppAwakeningLine(tooltips);
            }
        }

        private void OwnerByDir(Item item, Player player) {
            if (player.whoAmI == Main.myPlayer && item.useStyle == ItemUseStyleID.Swing
                && (item.createTile == -1 && item.createWall == -1)
                && (player.PressKey() || player.PressKey(false))) {
                player.direction = Math.Sign(player.position.To(Main.MouseWorld).X);
            }
        }

        private void ApplyNameLineColor(Color color, TooltipLine nameLine) => nameLine.OverrideColor = color;

        public static void AppAwakeningLine(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(CWRMod.Instance, "CalamityWeaponRemake",
                    CalamityUtils.ColorMessage(
                        CWRUtils.Translation("- 觉醒 -", "- Awakening -")
                        , new Color(196, 35, 44))
                    );
            tooltips.Add(line);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            CWRPlayer modPlayer = player.CWR();
            int theReLdamags = damage / 3;
            Vector2 vr = player.Center.To(Main.MouseWorld).UnitVector() * 13;

            if (player.whoAmI == Main.myPlayer) {
                if (modPlayer.theRelicLuxor == 1) {
                    if (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<TrueMeleeNoSpeedDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI, 1);
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRogue>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<RangedDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRanged>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<MagicDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMagic>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<SummonDamageClass>()
                        && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorSummon>()] < 3) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorSummon>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                }
                if (modPlayer.theRelicLuxor == 2) {
                    theReLdamags += 15;

                    if (item.CountsAsClass<MeleeDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMelee>(), theReLdamags, 0f, player.whoAmI, 1);
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRogue>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<RangedDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorRanged>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<MagicDamageClass>()) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorMagic>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                    else if (item.CountsAsClass<SummonDamageClass>()
                        && player.ownedProjectileCounts[ModContent.ProjectileType<TheRelicLuxorSummon>()] < 6) {
                        Projectile.NewProjectile(source, position, vr, ModContent.ProjectileType<TheRelicLuxorSummon>(), theReLdamags, 0f, player.whoAmI, 0);
                    }
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void OnConsumeItem(Item item, Player player) {
            base.OnConsumeItem(item, player);

        }

        public override bool CanUseItem(Item item, Player player) {
            if (CWRUtils.RemakeByItem<DeathWhistle>(item))//不管如何，不希望任意两种Boss存在时可以再次使用该物品
            {
                return !NPC.AnyNPCs(ModContent.NPCType<RavagerBody>())
                    && !NPC.AnyNPCs(ModContent.NPCType<RavagerABody>())
                    && player.ZoneOverworldHeight
                    && !BossRushEvent.BossRushActive;
            }
            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player) {
            //if (CWRUtils.RemakeByItem<DeathWhistle>(item))//这种修改方法会不可避免的调用原行为，导致生成两个甚至多个Boss实体，因而注释
            //{
            //    SoundEngine.PlaySound(SoundID.ScaryScream, player.Center);
            //    int types = ModContent.NPCType<RavagerBody>();
            //    if (DownedBossSystem.downedProvidence) 
            //        types = ModContent.NPCType<RavagerABody>();
            //    if (Main.netMode != NetmodeID.MultiplayerClient)
            //    {
            //        int npc = NPC.NewNPC(new EntitySource_BossSpawn(player), (int)(player.position.X + Main.rand.Next(-250, 251)), (int)(player.position.Y - 500f), types, 1);
            //        Main.npc[npc].timeLeft *= 20;
            //        CalamityUtils.BossAwakenMessage(npc);
            //    }
            //    else
            //        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, types);
            //    return true;
            //}
            return base.UseItem(item, player);
        }
    }
}
