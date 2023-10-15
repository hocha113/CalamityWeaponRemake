using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class BlightedCleaver : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "BlightedCleaver";

        public const float BlightedCleaverMaxRageEnergy = 5000;

        private float rageEnergy
        {
            get => Item.CWR().RageEnergy;
            set => Item.CWR().RageEnergy = value;
        }

        public override void SetDefaults()
        {
            Item.width = 88;
            Item.damage = 64;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 26;
            Item.useTurn = true;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 88;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<RemakeBlazingPhantomBlade>();
            Item.shootSpeed = 12f;
            Item.CWR().remakeItem = true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position
            , Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            if (Item.CWR().HoldOwner != null && rageEnergy > 0)
            {
                DrawRageEnergyChargeBar(Item.CWR().HoldOwner);
            }
        }

        public override void UpdateInventory(Player player)
        {
            UpdateBar();
            base.UpdateInventory(player);
        }

        public override void HoldItem(Player player)
        {
            if (Item.CWR().HoldOwner == null)
            {
                Item.CWR().HoldOwner = player;
            }

            UpdateBar();
            base.HoldItem(player);
        }

        private void UpdateBar()
        {
            if (rageEnergy > BlightedCleaverMaxRageEnergy) 
                rageEnergy = BlightedCleaverMaxRageEnergy;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (rageEnergy > 0)
            {
                rageEnergy -= damage / 2;
                return true;
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 106);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {           
            float addnum = hit.Damage;
            rageEnergy += addnum;
            CombatText.NewText(player.Hitbox, new Color(242, 24, 20)
                , $"—— + {addnum} ——", dramatic: true);

            int type = ModContent.ProjectileType<HyperBlade>();
            for (int i = 0; i < 16; i++)
            {
                Vector2 offsetvr = HcMath.GetRandomVevtor(-127.5f, -52.5f, 360);
                Vector2 spanPos = target.Center + offsetvr;
                int proj = Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(player),
                    spanPos,
                    -offsetvr.UnitVector() * 12,
                    type,
                    Item.damage / 4,
                    0,
                    player.whoAmI
                    );
                Main.projectile[proj].timeLeft = 50;
            }

            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 180);
            target.AddBuff(70, 150);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 180);
            target.AddBuff(70, 150);
        }

        public void DrawRageEnergyChargeBar(Player player)
        {
            if (player.HeldItem.type != Item.type) return;
            Texture2D rageEnergyTop = DrawUtils.GetT2DValue(CWRConstant.UI + "RageEnergyTop");
            Texture2D rageEnergyBar = DrawUtils.GetT2DValue(CWRConstant.UI + "RageEnergyBar");
            Texture2D rageEnergyBack = DrawUtils.GetT2DValue(CWRConstant.UI + "RageEnergyBack");
            float slp = 3;
            Vector2 drawPos = DrawUtils.WDEpos(player.Center + new Vector2(rageEnergyBar.Width / -2 * slp, 135));
            Rectangle backRec = new Rectangle(0, 0, (int)(rageEnergyBar.Width * (rageEnergy / BlightedCleaverMaxRageEnergy)), rageEnergyBar.Height);

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
                drawPos,
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
