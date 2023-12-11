using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityWeaponRemake.Common;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    /// <summary>
    /// 圣火巨刃
    /// </summary>
    internal class HolyCollider : ModItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "HolyCollider";
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 80;
            Item.scale = 1f;
            Item.damage = 230;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.useTurn = true;
            Item.knockBack = 3.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HolyColliderHolyFires>();
            Item.shootSpeed = 10f;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Projectile.NewProjectile(source, player.Center + Main.rand.NextVector2Unit() * Main.rand.Next(342, 468), velocity / 3, ModContent.ProjectileType<HolyColliderHolyFires>(), damage, knockback, player.whoAmI);
                for (int j = 0; j < 3; j++)
                {
                    Vector2 pos = player.Center + Main.rand.NextVector2Unit() * Main.rand.Next(342, 468);
                    Vector2 particleSpeed = pos.To(player.Center).UnitVector() * 7;
                    CWRParticle energyLeak = new HolyColliderLightParticle(pos, particleSpeed
                        , Main.rand.NextFloat(0.5f, 0.7f), Color.Gold, 90, 1, 1.5f, hueShift: 0.0f);
                    CWRParticleHandler.SpawnParticle(energyLeak);
                }
            }
            return false;
        }
    }
}
