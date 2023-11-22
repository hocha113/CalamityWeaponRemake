using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Buffs;
using CalamityWeaponRemake.Content.Items.Melee;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RTerrorBlade : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.TerrorBlade>(item))
            {
                item.width = 88;
                item.damage = 560;
                item.DamageType = DamageClass.Melee;
                item.useAnimation = 18;
                item.useTime = 18;
                item.useTurn = true;
                item.useStyle = ItemUseStyleID.Swing;
                item.knockBack = 8.5f;
                item.UseSound = SoundID.Item1;
                item.autoReuse = true;
                item.height = 80;
                item.shoot = ModContent.ProjectileType<RemakeTerrorBeam>();
                item.shootSpeed = 20f;
                item.value = CalamityGlobalItem.Rarity13BuyPrice;
                item.rare = ModContent.RarityType<PureGreen>();
                item.CWR().remakeItem = true;
            }
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.TerrorBlade>(item))
            {
                if (item.CWR().HoldOwner != null && item.CWR().MeleeCharge > 0)
                {
                    DrawRageEnergyChargeBar(item.CWR().HoldOwner, item);
                }
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.TerrorBlade>(item))
            {
                UpdateBar(item);
            }
            base.UpdateInventory(item, player);
        }

        public override void HoldItem(Item item, Player player)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.TerrorBlade>(item))
            {
                if (item.CWR().HoldOwner == null)
                {
                    item.CWR().HoldOwner = player;
                }

                UpdateBar(item);

                if (item.CWR().MeleeCharge > 0)
                {
                    item.damage = 360;
                    item.shootSpeed = 20f;
                    item.useAnimation = 10;
                    item.useTime = 10;
                }
                else
                {
                    item.damage = 560;
                    item.shootSpeed = 15f;
                    item.useAnimation = 18;
                    item.useTime = 18;
                }
            }
            base.HoldItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.TerrorBlade>(item))
            {
                bool shootBool = false;
                if (!item.CWR().closeCombat)
                {
                    bool olduseup = item.CWR().MeleeCharge > 0;//这里使用到了效差的流程思想，用于判断能量耗尽的那一刻            
                    if (item.CWR().MeleeCharge > 0)
                    {
                        item.CWR().MeleeCharge -= damage / 10;
                        Projectile.NewProjectileDirect(
                            source,
                            position,
                            velocity,
                            type,
                            damage,
                            knockback,
                            player.whoAmI,
                            1
                            );
                        shootBool = false;
                    }
                    else
                    {
                        shootBool = true;
                    }
                    bool useup = item.CWR().MeleeCharge > 0;
                    if (useup != olduseup)
                    {
                        SoundEngine.PlaySound(ModSound.Peuncharge, player.Center);
                    }
                }

                item.CWR().closeCombat = false;
                return shootBool;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.TerrorBlade>(item))
            {
                item.CWR().closeCombat = true;
                target.AddBuff(ModContent.BuffType<SoulBurning>(), 600);

                bool oldcharge = item.CWR().MeleeCharge > 0;//与OnHitPvp一致，用于判断能量出现的那一刻
                item.CWR().MeleeCharge += hit.Damage / 5;
                bool charge = item.CWR().MeleeCharge > 0;
                if (charge != oldcharge)
                {
                    SoundEngine.PlaySound(ModSound.Pecharge, player.Center);
                }
            }
            else
            {
                base.OnHitNPC(item, player, target, hit, damageDone);
            }
        }

        private static void UpdateBar(Item item)
        {
            if (item.CWR().MeleeCharge > TerrorBlade.TerrorBladeMaxRageEnergy)
                item.CWR().MeleeCharge = TerrorBlade.TerrorBladeMaxRageEnergy;
        }

        public static void DrawRageEnergyChargeBar(Player player, Item item)
        {
            if (player.HeldItem != item) return;
            Texture2D rageEnergyTop = CWRUtils.GetT2DValue(CWRConstant.UI + "FrightEnergyChargeTop");
            Texture2D rageEnergyBar = CWRUtils.GetT2DValue(CWRConstant.UI + "FrightEnergyChargeBar");
            Texture2D rageEnergyBack = CWRUtils.GetT2DValue(CWRConstant.UI + "FrightEnergyChargeBack");
            float slp = 3;
            int offsetwid = 4;
            Vector2 drawPos = CWRUtils.WDEpos(player.Center + new Vector2(rageEnergyBar.Width / -2 * slp, 135));
            Rectangle backRec = new Rectangle(offsetwid, 0, (int)((rageEnergyBar.Width - offsetwid * 2) * (item.CWR().MeleeCharge / TerrorBlade.TerrorBladeMaxRageEnergy)), rageEnergyBar.Height);

            Main.spriteBatch.ResetBlendState();
            Main.EntitySpriteDraw(
                rageEnergyBack,
                drawPos,
                null,
                Color.White,
                0,
                Vector2.Zero,
                slp,
                SpriteEffects.None,
                0
                );

            Main.EntitySpriteDraw(
                rageEnergyBar,
                drawPos + new Vector2(offsetwid, 0) * slp,
                backRec,
                Color.White,
                0,
                Vector2.Zero,
                slp,
                SpriteEffects.None,
                0
                );

            Main.EntitySpriteDraw(
                rageEnergyTop,
                drawPos,
                null,
                Color.White,
                0,
                Vector2.Zero,
                slp,
                SpriteEffects.None,
                0
                );
            Main.spriteBatch.ResetUICanvasState();
        }
    }
}
