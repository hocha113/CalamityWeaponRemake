using CalamityMod.Items.Tools;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.Items.Ranged;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Projectiles;
using CalamityWeaponRemake.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Tools
{
    internal class InfinitePick : ModItem
    {
        public bool IsPick = true;
        public override string Texture => CWRConstant.Item + "Tools/" + (IsPick ? "Pickaxe" : "Hammer");
        public Texture2D value => CWRUtils.GetT2DValue(Texture);
        private bool oldRDown;
        private bool rDown;
        public override void SetDefaults() {
            Item.damage = 9999;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 999);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = int.MaxValue;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 16));
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool? UseItem(Player player) {
            if (IsPick) {
                Item.pick = int.MaxValue;
                Item.hammer = 0;
                Item.useAnimation = Item.useTime = 10;

            }
            else {
                Item.pick = 0;
                Item.hammer = int.MaxValue;
                Item.useAnimation = Item.useTime = 30;
            }


            return base.UseItem(player);
        }

        public override void HoldItem(Player player) {
            if (CWRKeySystem.InfinitePickSkillKey.JustPressed) {
                IsPick = !IsPick;
                SoundEngine.PlaySound(!IsPick ? ModSound.Pecharge : ModSound.Peuncharge, player.Center);
                TextureAssets.Item[Type] = CWRUtils.GetT2DAsset(Texture);
            }

            rDown = player.PressKey(false);
            bool justRDown = rDown && !oldRDown;
            oldRDown = rDown;
            if (justRDown && !Main.playerInventory) {
                SoundEngine.PlaySound(new SoundStyle(CWRConstant.Sound + "Pedestruct"), Main.MouseWorld);
                if (!IsPick) {
                    for (int i = 0; i < 78; i++) {
                        HeavenHeavySmoke spark = new HeavenHeavySmoke(Main.MouseWorld, Main.rand.NextVector2Unit() * Main.rand.Next(3, 57)
                            , CWRUtils.MultiLerpColor(Main.rand.NextFloat(), HeavenfallLongbow.rainbowColors)
                            , Main.rand.Next(30, 75), Main.rand.NextFloat(1.5f, 6.2f), 1, 0.1f);
                        CWRParticleHandler.SpawnParticle(spark);
                    }
                    int maxX = 500;
                    int maxY = 500;
                    Vector2 pos = Main.MouseWorld - new Vector2(maxX, maxY) / 2;
                    Item ball = new Item(ModContent.ItemType<DarkMatterBall>());
                    DarkMatterBall darkMatterBall = (DarkMatterBall)ball.ModItem;
                    if (darkMatterBall != null) {
                        for (int x = 0; x < maxX; x++) {
                            for (int y = 0; y < maxY; y++) {
                                Vector2 tilePos = CWRUtils.WEPosToTilePos(pos + new Vector2(x, y));
                                Tile tile = CWRUtils.GetTile(tilePos);
                                if (tile.HasTile && tile.TileType != TileID.Cactus) {
                                    int dorptype = CWRUtils.GetTileDorp(tile);
                                    if (dorptype != 0)
                                        darkMatterBall.dorpTypes.Add(dorptype);
                                    if (tile.WallType != 0) {
                                        if (CWRIDs.WallToItem.TryGetValue(tile.WallType, out int value))
                                            darkMatterBall.dorpTypes.Add(value);
                                    }
                                    tile.LiquidAmount = 0;
                                    tile.HasTile = false;
                                    tile.WallType = 0;
                                    if (Main.netMode != NetmodeID.SinglePlayer)
                                        NetMessage.SendTileSquare(player.whoAmI, x, y);
                                }
                            }
                        }
                        Projectile.NewProjectile(player.parent(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<InfinitePickProj>(), Item.damage * 10, 0, player.whoAmI);
                        if (darkMatterBall.dorpTypes.Count > 0)
                            player.QuickSpawnItem(player.parent(), darkMatterBall.Item, 1);
                    }
                }
                else {
                    int proj = Projectile.NewProjectile(player.parent(), player.Center, player.Center.To(Main.MouseWorld).UnitVector() * 32, ModContent.ProjectileType<InfinitePickProj>(), Item.damage * 10, 0, player.whoAmI, 1);
                    Main.projectile[proj].width = Main.projectile[proj].height = 64;
                }
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                string text = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.InfinitePick.DisplayName");
                InfiniteIngot.drawColorText(Main.spriteBatch, line, text, basePosition);
                return false;
            }
            return true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            HeavenHeavySmoke spark = new HeavenHeavySmoke(player.Center, Main.rand.NextVector2Unit() * Main.rand.Next(13, 17), CWRUtils.MultiLerpColor(Main.rand.NextFloat(), HeavenfallLongbow.rainbowColors), 30, 1, 1, 0.1f);
            CWRParticleHandler.SpawnParticle(spark);
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<CrystylCrusher>()
                .AddIngredient<AbyssalWarhammer>()
                .AddIngredient<AerialHamaxe>()
                .AddIngredient<AstralHamaxe>()
                .AddIngredient<AstralPickaxe>()
                .AddIngredient<AxeofPurity>()
                .AddIngredient<BeastialPickaxe>()
                .AddIngredient<BerserkerWaraxe>()
                .AddIngredient<BlossomPickaxe>()
                .AddIngredient<FellerofEvergreens>()
                .AddIngredient<Gelpick>()
                .AddIngredient<GenesisPickaxe>()
                .AddIngredient<Grax>()
                .AddIngredient<GreatbayPickaxe>()
                .AddIngredient<InfernaCutter>()
                .AddIngredient<ReefclawHamaxe>()
                .AddIngredient<SeismicHampick>()
                .AddIngredient<ShardlightPickaxe>()
                .AddIngredient<SkyfringePickaxe>()
                .AddIngredient<TectonicTruncator>()
                .AddIngredient<InfiniteIngot>(5)
                .AddTile(ModContent.TileType<TransmutationOfMatter>())
                .Register();
        }
    }
}
