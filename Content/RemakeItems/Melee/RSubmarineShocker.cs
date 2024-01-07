using CalamityMod.Items;
using CalamityMod;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee;
using CalamityWeaponRemake.Content.RemakeItems.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.RemakeProjectiles;
using Microsoft.Xna.Framework;
using CalamityWeaponRemake.Content.Items.Melee;
using System.Collections.Generic;
using CalamityWeaponRemake.Common;
using Terraria.Localization;
using System.Security.Policy;

namespace CalamityWeaponRemake.Content.RemakeItems.Melee
{
    internal class RSubmarineShocker : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.SubmarineShocker>();
        public override int ProtogenesisID => ModContent.ItemType<SubmarineShocker>();
        public override void SetDefaults(Item item) {
            item.width = 32;
            item.height = 32;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.Rapier;
            item.damage = 90;
            item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            item.useAnimation = item.useTime = 10;
            item.shoot = ModContent.ProjectileType<RSubmarineShockerProj>();
            item.shootSpeed = 2f;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item1;
            item.value = CalamityGlobalItem.Rarity5BuyPrice;
            item.rare = ItemRarityID.Pink;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].Name == "ItemName") {
                    tooltips[i].Text = Language.GetText($"Mods.CalamityWeaponRemake.Items.SubmarineShocker.DisplayName").Value;
                }
            }
            CWRUtils.OnModifyTooltips(CWRMod.Instance, item, tooltips, "SubmarineShocker");
        }

    }
}
