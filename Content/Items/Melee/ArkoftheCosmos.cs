﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using System.Linq;
using CalamityMod;
using CalamityWeaponRemake.Common;

namespace CalamityWeaponRemake.Content.Items.Melee
{
    internal class ArkoftheCosmos : ModItem
    {
        public float Combo;

        public float Charge;

        public static float NeedleDamageMultiplier = 0.7f;

        public static float MaxThrowReach = 650f;

        public static float snapDamageMultiplier = 1.2f;

        public static float chargeDamageMultiplier = 1.2f;

        public static float chainDamageMultiplier = 0.1f;

        public static int DashIframes = 10;

        public static float SlashBoltsDamageMultiplier = 0.2f;

        public static float SnapBoltsDamageMultiplier = 0.1f;

        public static float blastDamageMultiplier = 0.5f;

        public static float blastFalloffSpeed = 0.1f;

        public static float blastFalloffStrenght = 0.75f;

        public static float SwirlBoltAmount = 6f;

        public static float SwirlBoltDamageMultiplier = 0.7f;

        public new string LocalizationCategory => "Items.Weapons.Melee";

        public override string Texture => CWRConstant.Item_Melee + "ArkoftheCosmos";

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (tooltips != null && Main.player[Main.myPlayer] != null)
            {
                TooltipLine comboTooltip = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[COMBO]") && x.Mod == "Terraria");
                if (comboTooltip != null)
                {
                    comboTooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("ComboInfo"));
                    comboTooltip.OverrideColor = Color.Lerp(Color.Gold, Color.Goldenrod, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
                }
                TooltipLine parryTooltip = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[PARRY]") && x.Mod == "Terraria");
                if (parryTooltip != null)
                {
                    parryTooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("ParryInfo"));
                    parryTooltip.OverrideColor = Color.Lerp(Color.Cyan, Color.DeepSkyBlue, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.75f);
                }
                TooltipLine blastTooltip = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[BLAST]") && x.Mod == "Terraria");
                if (blastTooltip != null)
                {
                    blastTooltip.Text = Lang.SupportGlyphs(this.GetLocalizedValue("BlastInfo"));
                    blastTooltip.OverrideColor = Color.Lerp(Color.HotPink, Color.Crimson, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.625f);
                }
            }
        }

        public override void SetDefaults()
        {
            base.Item.width = (base.Item.height = 136);
            base.Item.damage = 1770;
            base.Item.DamageType = DamageClass.MeleeNoSpeed;
            base.Item.noMelee = true;
            base.Item.noUseGraphic = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 15;
            base.Item.useTurn = true;
            base.Item.useStyle = 5;
            base.Item.knockBack = 9.5f;
            base.Item.UseSound = null;
            base.Item.autoReuse = true;
            base.Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            base.Item.shoot = 10;
            base.Item.shootSpeed = 28f;
            base.Item.rare = ModContent.RarityType<Violet>();
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += 15f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
            if (CanUseItem(player) && Combo != 4f)
            {
                base.Item.channel = false;
            }
            if (Combo == 4f)
            {
                base.Item.channel = true;
            }
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.projectile.Any((Projectile n) => n.active && n.owner == player.whoAmI && n.type == ModContent.ProjectileType<ArkoftheCosmosSwungBlade>());
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (Charge > 0f && player.controlUp)
                {
                    float angle = velocity.ToRotation();
                    Projectile.NewProjectile(source, player.Center + angle.ToRotationVector2() * 90f, velocity, ModContent.ProjectileType<ArkoftheCosmosBlast>(), (int)((float)damage * Charge * chargeDamageMultiplier * blastDamageMultiplier), 0f, player.whoAmI, Charge);
                    if (Main.LocalPlayer.Calamity().GeneralScreenShakePower < 3f)
                    {
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 3f;
                    }
                    Charge = 0f;
                }
                else if (!Main.projectile.Any((Projectile n) => n.active && n.owner == player.whoAmI && (n.type == ModContent.ProjectileType<ArkoftheAncientsParryHoldout>() || n.type == ModContent.ProjectileType<TrueArkoftheAncientsParryHoldout>() || n.type == ModContent.ProjectileType<ArkoftheElementsParryHoldout>() || n.type == ModContent.ProjectileType<ArkoftheCosmosParryHoldout>())))
                {
                    Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<ArkoftheCosmosParryHoldout>(), damage, 0f, player.whoAmI);
                }
                return false;
            }
            if (Charge > 0f)
            {
                damage = (int)(chargeDamageMultiplier * (float)damage);
            }
            float scissorState = ((Combo == 4f) ? 2f : (Combo % 2f));
            Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<ArkoftheCosmosSwungBlade>(), damage, knockback, player.whoAmI, scissorState, Charge);
            if (scissorState != 2f)
            {
                Projectile.NewProjectile(source, player.Center + velocity.SafeNormalize(Vector2.Zero) * 20f, velocity * 1.4f, ModContent.ProjectileType<RendingNeedle>(), (int)((float)damage * NeedleDamageMultiplier), knockback, player.whoAmI);
            }
            Combo += 1f;
            if (Combo > 4f)
            {
                Combo = 0f;
            }
            Charge -= 1f;
            if (Charge < 0f)
            {
                Charge = 0f;
            }
            return false;
        }

        public override ModItem Clone(Item item)
        {
            ModItem modItem = base.Clone(item);
            if (modItem is ArkoftheCosmos a && item.ModItem is ArkoftheCosmos a2)
            {
                a.Charge = a2.Charge;
            }
            return modItem;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(Charge);
        }

        public override void NetReceive(BinaryReader reader)
        {
            Charge = reader.ReadSingle();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D handleTexture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/ArkoftheCosmosHandle", (AssetRequestMode)2).Value;
            Texture2D bladeTexture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/ArkoftheCosmosGlow", (AssetRequestMode)2).Value;
            float bladeOpacity = ((Charge > 0f) ? 1f : (MathHelper.Clamp((float)Math.Sin(Main.GlobalTimeWrappedHourly % (float)Math.PI) * 2f, 0f, 1f) * 0.7f + 0.3f));
            spriteBatch.Draw(handleTexture, position, (Rectangle?)null, drawColor, 0f, origin, scale, (SpriteEffects)0, 0f);
            spriteBatch.Draw(bladeTexture, position, (Rectangle?)null, drawColor * bladeOpacity, 0f, origin, scale, (SpriteEffects)0, 0f);
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {           
            if (!(Charge <= 0f))
            {
                Texture2D barBG = ModContent.Request<Texture2D>("CalamityMod/UI/MiscTextures/GenericBarBack", (AssetRequestMode)2).Value;
                Texture2D barFG = ModContent.Request<Texture2D>("CalamityMod/UI/MiscTextures/GenericBarFront", (AssetRequestMode)2).Value;
                float barScale = 3f;
                Vector2 barOrigin = barBG.Size() * 0.5f;
                float yOffset = 50f;
                Vector2 drawPos = position + Vector2.UnitY * scale * ((float)frame.Height - yOffset);
                Rectangle frameCrop = default(Rectangle);
                frameCrop = new Rectangle(0, 0, (int)(Charge / 10f * (float)barFG.Width), barFG.Height);
                Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.75f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.1f);
                spriteBatch.Draw(barBG, drawPos, (Rectangle?)null, color, 0f, barOrigin, scale * barScale, (SpriteEffects)0, 0f);
                spriteBatch.Draw(barFG, drawPos, (Rectangle?)frameCrop, color * 0.8f, 0f, barOrigin, scale * barScale, (SpriteEffects)0, 0f);
            }
        }
    }
}
