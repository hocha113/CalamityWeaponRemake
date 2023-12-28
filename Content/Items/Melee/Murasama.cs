using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityMod.Items.Materials;
using CalamityWeaponRemake.Content.Tiles;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class Murasama : ModItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "Murasama";
        public static int GetOnDamage {
            get {
                int damages = 11;
                if (DownedBossSystem.downedAquaticScourgeAcidRain)
                    damages = 23;
                if (DownedBossSystem.downedSlimeGod)
                    damages = 51;
                if (DownedBossSystem.downedCalamitasClone)
                    damages = 321;
                if (DownedBossSystem.downedRavager)
                    damages = 331;
                if (DownedBossSystem.downedPolterghast)
                    damages = 711;
                if (DownedBossSystem.downedDoG)
                    damages = 1511;
                if (DownedBossSystem.downedYharon)
                    damages = 1911;
                return damages;
            }
        }
        public static float GetOnScale {
            get {
                float slp = 0.5f;
                if (DownedBossSystem.downedAquaticScourgeAcidRain)
                    slp = 0.75f;
                if (DownedBossSystem.downedSlimeGod)
                    slp = 1;
                if (DownedBossSystem.downedCalamitasClone)
                    slp = 1.1f;
                if (DownedBossSystem.downedRavager)
                    slp = 1.3f;
                if (DownedBossSystem.downedPolterghast)
                    slp = 1.5f;
                if (DownedBossSystem.downedDoG)
                    slp = 2f;
                if (DownedBossSystem.downedYharon)
                    slp = 2.5f;
                return slp;
            }
        }

        public new string LocalizationCategory => "Items.Weapons.Melee";
        public int frameCounter = 0;
        public int frame = 0;
        public bool IDUnlocked(Player player) => DownedBossSystem.downedDoG;

        public static readonly SoundStyle OrganicHit = new("CalamityMod/Sounds/Item/MurasamaHitOrganic") { Volume = 0.45f };
        public static readonly SoundStyle InorganicHit = new("CalamityMod/Sounds/Item/MurasamaHitInorganic") { Volume = 0.55f };
        public static readonly SoundStyle Swing = new("CalamityMod/Sounds/Item/MurasamaSwing") { Volume = 0.2f };
        public static readonly SoundStyle BigSwing = new("CalamityMod/Sounds/Item/MurasamaBigSwing") { Volume = 0.25f };
        public override void SetStaticDefaults() {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(2, 13));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults() {
            Item.height = 134;
            Item.width = 90;
            Item.damage = 2222;
            Item.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.knockBack = 6.5f;
            Item.autoReuse = false;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.shoot = ModContent.ProjectileType<MurasamaSlash>();
            Item.shootSpeed = 24f;
            Item.rare = ModContent.RarityType<Violet>();
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
            Item.CWR().remakeItem = true;
        }

        public override void HoldItem(Player player) {
            Item.damage = GetOnDamage;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 61;

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            Texture2D texture;

            if (IDUnlocked(Main.LocalPlayer)) {
                texture = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, 2, 13), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }
            else {
                texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture;

            if (IDUnlocked(Main.LocalPlayer)) {
                texture = ModContent.Request<Texture2D>(Texture).Value;
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 2, 13), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            else {
                texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(texture, Item.position - Main.screenPosition, null, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            if (!IDUnlocked(Main.LocalPlayer))
                return;
            Texture2D texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/MurasamaGlow").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 2, 13, false), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int proj = Projectile.NewProjectile(source, position, velocity, type, GetOnDamage, knockback, player.whoAmI, 0f, 0f);
            Main.projectile[proj].scale = GetOnScale;
            return false;
        }


        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<CalamityMod.Items.Weapons.Melee.Murasama>()
                .Register();
        }
    }
}
