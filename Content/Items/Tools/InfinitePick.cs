using CalamityWeaponRemake.Common;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Items.Materials;
using CalamityWeaponRemake.Content.Tiles;
using CalamityMod.Items.Tools;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Items.Ranged;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod;
using Terraria.GameContent;
using Terraria.ObjectData;
using Terraria.GameInput;

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
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 9));
        }

        int frmae;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            spriteBatch.Draw(value, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            CWRUtils.ClockFrame(ref frmae, 5, 8);
            spriteBatch.Draw(value, Item.Center - Main.screenPosition, CWRUtils.GetRec(value, frmae, 9), lightColor, 0, CWRUtils.GetOrig(value, 9), scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool AltFunctionUse(Player player) {
            return !IsPick;
        }

        public override bool? UseItem(Player player) {
            if (IsPick) {
                Item.pick = int.MaxValue / 2;
                Item.axe = 0;
                Item.hammer = 0;
                Item.useAnimation = Item.useTime = 10;

            }
            else {
                Item.pick = 0;
                Item.axe = int.MaxValue / 5;
                Item.hammer = int.MaxValue;
                Item.useAnimation = Item.useTime = 30;
            }


            return base.UseItem(player);
        }

        public override void HoldItem(Player player) {
            if (CWRKeySystem.InfinitePickSkillKey.JustPressed) {
                IsPick = !IsPick;
                SoundEngine.PlaySound(IsPick ? ModSound.Pecharge : ModSound.Peuncharge, player.Center);
                TextureAssets.Item[Type] = CWRUtils.GetT2DAsset(Texture);
            }

            rDown = player.PressKey(false);
            bool justRDown = rDown && !oldRDown;
            oldRDown = rDown;
            if (justRDown && !Main.playerInventory && !IsPick) {
                SoundEngine.PlaySound(new SoundStyle(CWRConstant.Sound + "Pedestruct"), Main.MouseWorld);
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
                            if (tile.HasTile) {
                                int stye = TileObjectData.GetTileStyle(tile);
                                if (stye == -1)
                                    stye = 0;
                                int dorptype = TileLoader.GetItemDropFromTypeAndStyle(tile.TileType, stye);
                                if (dorptype != 0)
                                    darkMatterBall.dorpTypes.Add(dorptype);
                            }
                            tile.LiquidAmount = 0;
                            tile.HasTile = false;
                            tile.WallType = 0;
                        }
                    }
                    if (darkMatterBall.dorpTypes.Count > 0)
                        player.QuickSpawnItem(player.parent(), darkMatterBall.Item, 1);
                }
            }
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
