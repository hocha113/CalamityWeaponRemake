using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Pets;
using Terraria.ModLoader.IO;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.DragonsScaleGreatswordProj;

namespace CalamityWeaponRemake.Content.Items.Melee.Extras
{
    internal class DragonsScaleGreatsword : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "DragonsScaleGreatsword";

        public Texture2D Value => CWRUtils.GetT2DValue(Texture);

        public override void SetDefaults() {
            Item.height = 54;
            Item.width = 54;
            Item.damage = 232;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 18;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2.5f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.shoot = ModContent.ProjectileType<DragonsScaleGreatswordBeam>();
            Item.shootSpeed = 7f;
            Item.CWR().remakeItem = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Poisoned, 1200);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            for (int i = 0; i < 6; i++) {
                int dust = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.JungleSpore);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(0.5f, 2.2f);
            }
        }

        public override void AddRecipes() {
            CreateRecipe().
                AddIngredient<PerennialBar>(15).
                AddIngredient<UelibloomBar>(15).
                AddIngredient(ItemID.ChlorophyteBar, 15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
