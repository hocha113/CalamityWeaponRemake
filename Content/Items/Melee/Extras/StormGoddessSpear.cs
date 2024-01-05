﻿using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.StormGoddessSpearProj;
using CalamityWeaponRemake.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee.Extras
{
    internal class StormGoddessSpear : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "StormGoddessSpear";
        public LocalizedText LegendText;
        public LocalizedText Legend { get; private set; }
        public override void SetStaticDefaults() => Legend = this.GetLocalization(nameof(Legend));
        public override void SetDefaults() {
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
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.shoot = ModContent.ProjectileType<StormGoddessSpearProj>();
            Item.shootSpeed = 15f;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.CWR().remakeItem = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return true;
        }

        public override bool CanUseItem(Player player) {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StormGoddessSpearProj>()] <= 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine cumstops = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[tips]") && x.Mod == "Terraria");
            if (cumstops == null) return;

            KeyboardState state = Keyboard.GetState();
            if ((state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))) {
                cumstops.Text = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.StormGoddessSpear.Legend");
                cumstops.OverrideColor = Color.Lerp(Color.BlueViolet, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else {
                cumstops.Text = CWRUtils.Translation("按下[Shift]聆听故事...", "Press [Shift] to listen to the story...");
                cumstops.OverrideColor = Color.Lerp(Color.Blue, Color.Gold, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ThunderSpear)
                .AddIngredient<CalamityMod.Items.Weapons.Melee.StormRuler>()
                .AddIngredient<CalamityMod.Items.Weapons.Rogue.StormfrontRazor>()
                .AddIngredient<StormlionMandible>(5)
                .AddIngredient(ItemID.LunarBar, 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
