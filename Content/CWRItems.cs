using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles;
using CalamityWeaponRemake.Content.Projectiles.Weapons;
using CalamityWeaponRemake.Content.RemakeItems.Vanilla;
using CalamityWeaponRemake.Content.UIs.SupertableUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <summary>
        /// 是否是一个无尽物品，这个的设置决定物品是否会受到湮灭机制的影响
        /// </summary>
        internal bool isInfiniteItem;
        /// <summary>
        /// 是否湮灭，在设置<see cref="isInfiniteItem"/>为<see langword="true"/>后启用，决定无尽物品是否湮灭
        /// </summary>
        internal bool noDestruct;
        /// <summary>
        /// 湮灭时间
        /// </summary>
        internal int destructTime;
        /// <summary>
        /// 这个物品所属的终焉合成内容，这决定了它的物品简介是否绘制终焉合成表格
        /// </summary>
        internal string[] OmigaSnyContent;

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
            if (isInfiniteItem) {
                destructTime = 5;
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
            tag.Add("_noDestruct", noDestruct);
        }

        public override void LoadData(Item item, TagCompound tag) {
            MeleeCharge = tag.GetFloat("_MeleeCharge");
            noDestruct = tag.GetBool("_noDestruct");
        }

        public override void HoldItem(Item item, Player player) {
            if (player.HeldItem.type != CWRIDs.FoodStallChair) {
                OwnerByDir(item, player);
            }
            else {
                player.direction = -1;
            }
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

        public override void PostUpdate(Item item) {
            if (isInfiniteItem) {
                Destruct(item, item.position, CWRUtils.InPosFindPlayer(item.position, 9999));
            }
        }

        public override void UpdateInventory(Item item, Player player) {
            if (isInfiniteItem) {
                Destruct(item, player.position, player);
            }
        }

        public void Destruct(Item item, Vector2 pos, Player player) {
            destructTime--;
            Item[] inven = player.inventory;
            if (!noDestruct && destructTime <= 0 && inven.Count((Item n) => n.type == CWRIDs.EndlessStabilizer) == 0) {
                if (item.type != CWRIDs.StarMyriadChanges) {
                    Projectile.NewProjectile(new EntitySource_WorldEvent()
                    , pos, Vector2.Zero, ModContent.ProjectileType<InfiniteIngotTileProj>(), 9999, 0); 
                }
                else {
                    Projectile.NewProjectile(new EntitySource_WorldEvent()
                    , pos, Vector2.Zero, ModContent.ProjectileType<StarMyriadChangesProj>(), 1, 0);
                }
                item.TurnToAir();
            }
        }

        public override GlobalItem Clone(Item from, Item to) {
            return base.Clone(from, to);
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
            WaterBottle.OnUse(item, player);
        }

        public override bool CanUseItem(Item item, Player player) {
            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player) {
            return base.UseItem(item, player);
        }

        public override void PostDrawTooltip(Item item, ReadOnlyCollection<DrawableTooltipLine> lines) {
            base.PostDrawTooltip(item, lines);
        }

        public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {
            if (OmigaSnyContent != null && InItemDrawRecipe.Instance != null && SupertableUI.Instance != null) {
                if (!InItemDrawRecipe.Instance.OnSupTale) {
                    InItemDrawRecipe.Instance.Draw(Main.spriteBatch, new Vector2(800, 200), OmigaSnyContent);
                }
            }
            return base.PreDrawTooltip(item, lines, ref x, ref y);
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}
