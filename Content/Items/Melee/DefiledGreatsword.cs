using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Common.DrawTools;
using CalamityWeaponRemake.Content.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class DefiledGreatsword : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "DefiledGreatsword";

        public const float DefiledGreatswordMaxRageEnergy = 15000;

        private float rageEnergy
        {
            get => Item.CWR().RageEnergy;
            set => Item.CWR().RageEnergy = value;
        }

        public override void SetDefaults()
        {
            Item.width = 102;
            Item.damage = 112;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 102;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<BlazingPhantomBlade>();
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
            if (rageEnergy > DefiledGreatswordMaxRageEnergy)
                rageEnergy = DefiledGreatswordMaxRageEnergy;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            rageEnergy -= damage;
            if (rageEnergy < 0) rageEnergy = 0;

            if (rageEnergy == 0)
            {
                Projectile.NewProjectile(
                    source,
                    position,
                    velocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI,
                    1
                    );
            }
            else
            {

                for (int i = 0; i < 3; i++)
                {
                    float rot = MathHelper.ToRadians(-10 + i * 10);
                    Projectile.NewProjectile(
                        source,
                        position,
                        velocity.RotatedBy(rot),
                        type,
                        damage,
                        knockback,
                        player.whoAmI,
                        1
                    );
                }
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

            int type = ModContent.ProjectileType<SunlightBlades>();
            int randomLengs = Main.rand.Next(150);
            for (int i = 0; i < 3; i++)
            {
                Vector2 offsetvr = HcMath.GetRandomVevtor(-15, 15, 900 + randomLengs);
                Vector2 spanPos = target.Center + offsetvr;
                Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(player),
                    spanPos,
                    offsetvr.UnitVector() * -13,
                    type,
                    Item.damage / 3,
                    0,
                    player.whoAmI
                    );
                Vector2 offsetvr2 = HcMath.GetRandomVevtor(165, 195, 900 + randomLengs);
                Vector2 spanPos2 = target.Center + offsetvr2;
                Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(player),
                    spanPos2,
                    offsetvr2.UnitVector() * -13,
                    type,
                    Item.damage / 3,
                    0,
                    player.whoAmI
                    );
            }

            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 300);
            target.AddBuff(70, 150);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            int type = ModContent.ProjectileType<SunlightBlades>();
            int offsety = 180;
            for (int i = 0; i < 16; i++)
            {
                Vector2 offsetvr = new Vector2(-600, offsety);
                Vector2 spanPos = offsetvr + player.Center;
                Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(player),
                    spanPos,
                    offsetvr.UnitVector() * -13,
                    type,
                    Item.damage / 3,
                    0,
                    player.whoAmI
                    );
                Vector2 offsetvr2 = new Vector2(600, offsety);
                Vector2 spanPos2 = offsetvr + player.Center;
                Projectile.NewProjectile(
                    AiBehavior.GetEntitySource_Parent(player),
                    spanPos2,
                    offsetvr2.UnitVector() * -13,
                    type,
                    Item.damage / 3,
                    0,
                    player.whoAmI
                    );

                offsety -= 20;
            }
            player.AddBuff(ModContent.BuffType<TyrantsFury>(), 300);
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
            Rectangle backRec = new Rectangle(0, 0, (int)(rageEnergyBar.Width * (rageEnergy / DefiledGreatswordMaxRageEnergy)), rageEnergyBar.Height);

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
