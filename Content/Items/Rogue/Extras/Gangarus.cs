using CalamityMod.Items;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Rogue.GangarusProjectiles;
using Mono.Cecil;
using Terraria.Audio;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using CalamityMod.Rarities;
using System;
using Terraria.GameContent;

namespace CalamityWeaponRemake.Content.Items.Rogue.Extras
{
    internal class Gangarus : ModItem
    {
        public static SoundStyle BelCanto = new("CalamityWeaponRemake/Assets/Sounds/BelCanto") { Volume = 3.5f };
        public static SoundStyle AT = new("CalamityWeaponRemake/Assets/Sounds/AT") { Volume = 1.5f };
        public int ChargeGrade;
        public override string Texture => CWRConstant.Item + "Rogue/Gangarus";
        public LocalizedText Legend { get; private set; }

        public static void ZenithWorldAsset() {
            if (Main.zenithWorld) {
                TextureAssets.Item[CWRIDs.Gangarus] = CWRUtils.GetT2DAsset(CWRConstant.Item + "Rogue/Gangarus3");
            }
            else {
                TextureAssets.Item[CWRIDs.Gangarus] = CWRUtils.GetT2DAsset(CWRConstant.Item + "Rogue/Gangarus");
            }
        }

        public override void SetStaticDefaults() => Legend = this.GetLocalization(nameof(Legend));
        public override void SetDefaults() {
            Item.width = 44;
            Item.damage = 4480;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.shoot = ModContent.ProjectileType<GangarusProjectile>();
            Item.shootSpeed = 15f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine cumstops = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[tips]") && x.Mod == "Terraria");
            if (cumstops == null) return;
            KeyboardState state = Keyboard.GetState();
            if ((state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))) {
                cumstops.Text = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.Gangarus.Legend");
                cumstops.OverrideColor = Color.Lerp(Color.Gold, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
            else {
                cumstops.Text = CWRUtils.Translation("按下[Shift]聆听故事...", "Press [Shift] to listen to the story...");
                cumstops.OverrideColor = Color.Lerp(Color.Red, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            }
        }

        public override void HoldItem(Player player) {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GangarusHeldProjectile>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<GangarusProjectile>()] == 0) {
                Projectile.NewProjectile(player.parent(), player.Center, Vector2.Zero, ModContent.ProjectileType<GangarusHeldProjectile>(), 0, 0, player.whoAmI);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (ChargeGrade > 0) {
                int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<GangarusProjectile>(), damage * ChargeGrade, knockback, player.whoAmI);
                Main.projectile[proj].Calamity().stealthStrike = true;
                Main.projectile[proj].ai[1] = ChargeGrade;
                ChargeGrade = 0;
                return false;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
