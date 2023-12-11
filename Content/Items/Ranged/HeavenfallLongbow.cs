using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.AnnihilatingUniverseProj;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Ranged.HeavenfallLongbowProj;
using CalamityMod.Sounds;
using CalamityWeaponRemake.Content.Particles.Core;
using CalamityWeaponRemake.Content.Particles;
using System;
using Terraria.Audio;
using CalamityMod.Items.Materials;
using CalamityWeaponRemake.Content.Tiles;
using CalamityWeaponRemake.Content.Items.Materials;
using Terraria.UI.Chat;
using Terraria.Localization;

namespace CalamityWeaponRemake.Content.Items.Ranged
{
    internal class HeavenfallLongbow : ModItem
    {
        public const int MaxVientNum = 13;
        public static Color[] rainbowColors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
        public override string Texture => CWRConstant.Item_Ranged + "HeavenfallLongbow";
        public new string LocalizationCategory => "Items.Weapons.Ranged";

        public int ChargeValue;
        public bool spanInfiniteRuneBool = true;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void SetDefaults()
        {
            Item.damage = 9999;
            Item.width = 62;
            Item.height = 128;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HeavenfallLongbowHeldProj>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.CWR().remakeItem = true;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void HoldItem(Player player)
        {
            if (ChargeValue >= 200 && Main.myPlayer == player.whoAmI)//当充能达到阈值时，会释放一次无尽符文，此时可以按下技能键触发技能
            {
                SpanInfiniteRune(player);

                if (CWRKeySystem.HeavenfallLongbowSkillKey.JustPressed)
                {
                    int types = ModContent.ProjectileType<VientianePunishment>();
                    if (player.ownedProjectileCounts[types] < MaxVientNum)
                    {
                        int randomOffset = Main.rand.Next(MaxVientNum);//生成一个随机的偏移值，这样可以让所有的弓都有机会出现
                        int frmer = 0;
                        for (int i = 0; i < MaxVientNum; i++)
                        {
                            int proj = Projectile.NewProjectile(player.parent(), player.Center, Vector2.Zero, types, Item.damage, 0, player.whoAmI, i + randomOffset);//给予ai[0]一个可排序的索引量，这决定了该万象弹幕使用什么样的贴图
                            if (i == 0)//让第一个万象弹幕作为主弹幕，负责多数代码执行
                                frmer = proj;//将首号弹幕的索引储存起来
                            VientianePunishment vientianePunishment = Main.projectile[proj].ModProjectile as VientianePunishment;
                            if (vientianePunishment != null)
                            {
                                vientianePunishment.Index = i;//给每个万象弹幕分配合适索引，这决定了它们能否正确排序
                                vientianePunishment.FemerProjIndex = frmer;
                                vientianePunishment.Projectile.netUpdate = true;
                                vientianePunishment.Projectile.netUpdate2 = true;
                            }
                        }
                    }
                    ChargeValue = 0;//清空能量
                    spanInfiniteRuneBool = true;//重置符文生成开关
                }
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName" && line.Mod == "Terraria")
            {
                Color rarityColor = Main.DiscoColor;
                Vector2 basePosition = Main.MouseWorld - Main.screenPosition + new Vector2(23, 23);
                string Txet = Language.GetTextValue("Mods.CalamityWeaponRemake.Items.HeavenfallLongbow.DisplayName");
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.Font, Txet, basePosition, rarityColor, line.Rotation, line.Origin, line.BaseScale * 1.06f, line.MaxWidth, line.Spread);
                return false;
            }
            return true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool(3) && player.ownedProjectileCounts[Item.shoot] > 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y
                , ModContent.ProjectileType<HeavenfallLongbowHeldProj>(), damage, knockback, player.whoAmI, ai2: player.altFunctionUse == 0 ? 0 : 1);
            return false;
        }

        public void SpanInfiniteRune(Player player)
        {
            if (spanInfiniteRuneBool)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.PlasmaBoltSound, player.Center);
                float rot = 0;
                for (int j = 0; j < 500; j++)
                {
                    rot += MathHelper.TwoPi / 500f;
                    float scale = 2f / (3f - (float)Math.Cos(2 * rot)) * 25;
                    float outwardMultiplier = MathHelper.Lerp(4f, 220f, Utils.GetLerpValue(0f, 120f, 13, true));
                    Vector2 lemniscateOffset = scale * new Vector2((float)Math.Cos(rot), (float)Math.Sin(2f * rot) / 2f);
                    Vector2 pos = player.Center + lemniscateOffset * outwardMultiplier;
                    Vector2 particleSpeed = Vector2.Zero;
                    Color color = CWRUtils.MultiLerpColor(j / 500f, rainbowColors);
                    CWRParticle energyLeak = new LightParticle(pos, particleSpeed
                        , 1.5f, color, 120, 1, 1.5f, hueShift: 0.0f, _entity: player, _followingRateRatio: 1);
                    CWRParticleHandler.SpawnParticle(energyLeak);
                }
                Projectile.NewProjectile(player.parent(), player.Center, Vector2.Zero, ModContent.ProjectileType<InfiniteRune>(), 99999, 0, player.whoAmI);
                spanInfiniteRuneBool = false;
            }
        }

        public static void Obliterate(Vector2 origPos)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.Center.To(origPos).LengthSquared() > 90000)
                    continue;
                if (npc.active)
                {
                    npc.CWR().ObliterateBool = true;
                    npc.dontTakeDamage = true;
                    npc.SimpleStrikeNPC(npc.lifeMax, 0);
                    npc.life = 0;
                    npc.checkDead();
                    npc.HitEffect();
                    npc.NPCLoot();
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CalamityMod.Items.Weapons.Ranged.Drataliornus>()
                .AddIngredient<CalamityMod.Items.Weapons.Ranged.HeavenlyGale>()
                .AddIngredient<CalamityMod.Items.Weapons.Magic.Eternity>()
                .AddIngredient<InfiniteIngot>(15)
                .AddTile(ModContent.TileType<TransmutationOfMatter>())
                .Register();
        }
    }
}
