﻿using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Melee.RemakeProjectiles;
using Terraria.DataStructures;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Common.AuxiliaryMeans;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class StreamGouge : ModItem
    {
        public const int SpinTime = 45;

        public const int SpearFireTime = 24;

        public const int PortalLifetime = 30;

        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override string Texture => CWRConstant.Item_Melee + "StreamGouge";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 100;
            Item.damage = 440;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 19;
            Item.useTime = 19;
            Item.useStyle = 5;
            Item.knockBack = 9.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.shoot = ModContent.ProjectileType<RemakeStreamGougeProj>();
            Item.shootSpeed = 15f;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.altFunctionUse == 2)
                Main.projectile[proj].localAI[1] = 1;
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>(CWRConstant.Item_Melee + "StreamGougeGlow").Value);
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<RemakeStreamGougeProj>()] <= 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
