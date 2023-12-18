using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static CalamityWeaponRemake.Content.RemakeItems.Core.BaseRItem;

namespace CalamityWeaponRemake.Content.RemakeItems.Core
{
    internal class GCWRItems : GlobalItem
    {
        public static void ProcessRemakeAction(Item item, Action<BaseRItem> action) {
            foreach (BaseRItem rItem in CWRMod.RItemInstances) {
                if (rItem.TargetID == item.type && CWRConstant.ForceReplaceResetContent) {
                    action(rItem);
                }
            }
        }

        public static bool? ProcessRemakeAction(Item item, Func<BaseRItem, bool?> action) {
            bool? result = null;
            foreach (BaseRItem rItem in CWRMod.RItemInstances) {
                if (rItem.TargetID == item.type && CWRConstant.ForceReplaceResetContent) {
                    result = action(rItem);
                }
            }
            return result;
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            ProcessRemakeAction(item, (inds) => inds.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale));
        }

        public override bool AllowPrefix(Item item, int pre) {
            return base.AllowPrefix(item, pre);
        }

        public override bool AltFunctionUse(Item item, Player player) {
            bool? rest = ProcessRemakeAction(item, (inds) => inds.AltFunctionUse(item, player));
            return rest ?? base.AltFunctionUse(item, player);
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player) {
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override bool? CanBeChosenAsAmmo(Item ammo, Item weapon, Player player) {
            return base.CanBeChosenAsAmmo(ammo, weapon, player);
        }

        public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player) {
            return base.CanBeConsumedAsAmmo(ammo, weapon, player);
        }

        public override bool? CanCatchNPC(Item item, NPC target, Player player) {
            return base.CanCatchNPC(item, target, player);
        }

        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded) {
            return base.CanEquipAccessory(item, player, slot, modded);
        }

        public override bool? CanHitNPC(Item item, Player player, NPC target) {
            return base.CanHitNPC(item, player, target);
        }

        public override bool CanHitPvp(Item item, Player player, Player target) {
            return base.CanHitPvp(item, player, target);
        }

        public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target) {
            return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
        }

        public override bool CanPickup(Item item, Player player) {
            return base.CanPickup(item, player);
        }

        public override bool CanReforge(Item item) {
            return base.CanReforge(item);
        }

        public override bool CanResearch(Item item) {
            return base.CanResearch(item);
        }

        public override bool CanRightClick(Item item) {
            return base.CanRightClick(item);
        }

        public override bool CanShoot(Item item, Player player) {
            return base.CanShoot(item, player);
        }

        public override bool CanStack(Item destination, Item source) {
            return base.CanStack(destination, source);
        }

        public override bool CanStackInWorld(Item destination, Item source) {
            return base.CanStackInWorld(destination, source);
        }

        public override bool CanUseItem(Item item, Player player) {
            bool? rest = ProcessRemakeAction(item, (inds) => inds.CanUseItem(item, player));
            return rest ?? base.CanUseItem(item, player);
        }

        public override void CaughtFishStack(int type, ref int stack) {
            base.CaughtFishStack(type, ref stack);
        }

        public override bool ConsumeItem(Item item, Player player) {
            return base.ConsumeItem(item, player);
        }

        public override void HoldItem(Item item, Player player) {
            ProcessRemakeAction(item, (inds) => inds.HoldItem(item, player));
        }

        public override void HoldItemFrame(Item item, Player player) {
            ProcessRemakeAction(item, (inds) => inds.HoldItemFrame(item, player));
        }

        public override void LoadData(Item item, TagCompound tag) {
            base.LoadData(item, tag);
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
            ProcessRemakeAction(item, (inds) => inds.MeleeEffects(item, player, hitbox));
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            NPC.HitModifiers hitNPCModifier = modifiers;
            ProcessRemakeAction(item, (inds) => inds.ModifyHitNPC(item, player, target, ref hitNPCModifier));
            modifiers = hitNPCModifier;
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            Player.HurtModifiers hitPlayerModifier = modifiers;
            ProcessRemakeAction(item, (inds) => inds.ModifyHitPvp(item, player, target, ref hitPlayerModifier));
            modifiers = hitPlayerModifier;
        }

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            base.ModifyItemLoot(item, itemLoot);
            ProcessRemakeAction(item, (inds) => inds.ModifyItemLoot(item, itemLoot));
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale) {
            float slp = scale;
            ProcessRemakeAction(item, (inds) => inds.ModifyItemScale(item, player, ref slp));
            scale = slp;
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) {
            float newReduce = reduce;
            float newMult = mult;
            ProcessRemakeAction(item, (inds) => inds.ModifyManaCost(item, player, ref newReduce, ref newMult));
            reduce = newReduce;
            mult = newMult;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
            ShootStats stats = new ShootStats{
                Position = position,
                Velocity = velocity,
                Type = type,
                Damage = damage,
                Knockback = knockback
            };
            ProcessRemakeAction(item, (inds) => inds.ModifyShootStats(item, player, ref stats));
            position = stats.Position;
            velocity = stats.Velocity;
            type = stats.Type;
            damage = stats.Damage;
            knockback = stats.Knockback;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            ProcessRemakeAction(item, (inds) => inds.ModifyTooltips(item, tooltips));
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
            base.ModifyWeaponCrit(item, player, ref crit);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            base.ModifyWeaponDamage(item, player, ref damage);
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback) {
            base.ModifyWeaponKnockback(item, player, ref knockback);
        }

        public override void OnConsumeAmmo(Item weapon, Item ammo, Player player) {
            base.OnConsumeAmmo(weapon, ammo, player);
        }

        public override void OnConsumedAsAmmo(Item ammo, Item weapon, Player player) {
            base.OnConsumedAsAmmo(ammo, weapon, player);
        }

        public override void OnConsumeItem(Item item, Player player) {
            base.OnConsumeItem(item, player);
        }

        public override void OnConsumeMana(Item item, Player player, int manaConsumed) {
            base.OnConsumeMana(item, player, manaConsumed);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            base.OnHitNPC(item, player, target, hit, damageDone);
            ProcessRemakeAction(item, (inds) => inds.OnHitNPC(item, player, target, hit, damageDone));
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            base.OnHitPvp(item, player, target, hurtInfo);
            ProcessRemakeAction(item, (inds) => inds.OnHitPvp(item, player, target, hurtInfo));
        }

        public override void OnMissingMana(Item item, Player player, int neededMana) {
            base.OnMissingMana(item, player, neededMana);
        }

        public override bool OnPickup(Item item, Player player) {
            return base.OnPickup(item, player);
        }

        public override void OnSpawn(Item item, IEntitySource source) {
            base.OnSpawn(item, source);
        }

        public override void OnStack(Item destination, Item source, int numToTransfer) {
            base.OnStack(destination, source, numToTransfer);
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback) {
            base.PickAmmo(weapon, ammo, player, ref type, ref speed, ref damage, ref knockback);
        }

        public override void RightClick(Item item, Player player) {
            base.RightClick(item, player);
        }

        public override void SaveData(Item item, TagCompound tag) {
            base.SaveData(item, tag);
        }

        public override void SetDefaults(Item entity) {
            base.SetDefaults(entity);
            ProcessRemakeAction(entity, (inds) => inds.SetDefaults(entity));
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            bool? rest = ProcessRemakeAction(item, (inds) => inds.Shoot(item, player, source, position, velocity, type, damage, knockback));
            return rest ?? base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void SplitStack(Item destination, Item source, int numToTransfer) {
            base.SplitStack(destination, source, numToTransfer);
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed) {
            base.Update(item, ref gravity, ref maxFallSpeed);
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            base.UpdateAccessory(item, player, hideVisual);
        }

        public override void UpdateInventory(Item item, Player player) {
            base.UpdateInventory(item, player);
        }

        public override void UseAnimation(Item item, Player player) {
            ProcessRemakeAction(item, (inds) => inds.UseAnimation(item, player));
        }

        public override bool? UseItem(Item item, Player player) {
            return base.UseItem(item, player);
        }

        public override void UseItemFrame(Item item, Player player) {
            base.UseItemFrame(item, player);
        }

        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox) {
            base.UseItemHitbox(item, player, ref hitbox, ref noHitbox);
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame) {
            base.UseStyle(item, player, heldItemFrame);
        }

        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
            base.VerticalWingSpeeds(item, player, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
        }

        public override bool WingUpdate(int wings, Player player, bool inUse) {
            return base.WingUpdate(wings, player, inUse);
        }
    }
}
