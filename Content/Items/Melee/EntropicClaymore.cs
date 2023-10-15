using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using CalamityWeaponRemake.Content.Projectiles.Melee;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class EntropicClaymore : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "EntropicClaymore";

        public static readonly Color EntropicColor1 = new Color(25, 5, 9);

        public static readonly Color EntropicColor2 = new Color(25, 5, 9);

        public static readonly SoundStyle SwingSound = SoundID.Item1;

        public override void SetDefaults()
        {
            Item.damage = 122;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 78;
            Item.useTime = 78;
            Item.knockBack = 5.25f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = CalamityGlobalItem.Rarity9BuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.shoot = ModContent.ProjectileType<EntropicClaymoreHoldoutProj>();
            Item.shootSpeed = 12f;            
            Item.CWR().remakeItem = true;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            hitbox = CalamityUtils.FixSwingHitbox(118f, 118f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.initialize();
            Item.CWR().ai[0]++;
            if (Item.CWR().ai[0] > 2)
                Item.CWR().ai[0] = 0;
            Item.NetStateChanged
            Projectile proj = Projectile.NewProjectileDirect(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI,
                ai2 : Item.useTime
                );
            proj.timeLeft = Item.useTime;
            proj.localAI[0] = Item.CWR().ai[0];
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
            }
        }
    }
}
