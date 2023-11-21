using CalamityMod.Items;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class REntropicClaymore : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.EntropicClaymore>(item))
            {
                item.damage = 122;
                item.DamageType = DamageClass.Melee;
                item.useAnimation = 78;
                item.useTime = 78;
                item.knockBack = 5.25f;
                item.useStyle = ItemUseStyleID.Swing;
                item.UseSound = SoundID.Item1;
                item.useTurn = true;
                item.autoReuse = true;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.value = CalamityGlobalItem.Rarity9BuyPrice;
                item.rare = ItemRarityID.Cyan;
                item.shoot = ModContent.ProjectileType<EntropicClaymoreHoldoutProj>();
                item.shootSpeed = 12f;
                item.CWR().remakeItem = true;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CWRUtils.RemakeByItem<CalamityMod.Items.Weapons.Melee.EntropicClaymore>(item))
            {
                item.initialize();
                item.CWR().ai[0]++;
                if (item.CWR().ai[0] > 2)
                    item.CWR().ai[0] = 0;
                Projectile proj = Projectile.NewProjectileDirect(
                    source,
                    position,
                    velocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI,
                    ai2: item.useTime
                    );
                proj.timeLeft = item.useTime;
                proj.localAI[0] = item.CWR().ai[0];
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
