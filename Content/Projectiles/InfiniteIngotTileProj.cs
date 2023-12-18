﻿using CalamityMod.Graphics.Metaballs;
using CalamityMod.Particles;
using CalamityMod;
using CalamityWeaponRemake.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityWeaponRemake.Content.Particles;
using Terraria.Audio;

namespace CalamityWeaponRemake.Content.Projectiles
{
    internal class InfiniteIngotTileProj : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 900;
        }

        public void ExTile() {
            int maxNum = (int)(12 * Projectile.scale);
            int offset = maxNum / -2;
            int r = maxNum * maxNum / 4;
            Vector2 pos = Projectile.Center / 16;
            for (int x = 0; x < maxNum; x++) {
                for (int y = 0; y < maxNum; y++) {
                    Vector2 pos2 = new Vector2(offset + x, offset + y) + pos;
                    if (pos2.To(pos).LengthSquared() < r) {
                        Tile tile = CWRUtils.GetTile(pos2);
                        tile.LiquidAmount = 0;
                        tile.HasTile = false;
                        tile.WallType = 0;
                    }
                }
            }
        }

        public override void AI() {
            Projectile.scale += 0.01f;
            if (Projectile.timeLeft % 10 == 0) {
                SoundEngine.PlaySound(ModSound.BlackHole, Projectile.Center);
                ExTile();
            }
            CWRUtils.ForceFieldEffect(Projectile.Center, (int)(Projectile.scale * 96), 2, false);
            for (int i = 0; i < Main.item.Length; i++) {
                Item item = Main.item[i];
                if (item.Center.To(Projectile.Center).LengthSquared() < 32 * 32) {
                    Main.item[i] = new Item();
                }
            }
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 2; i++) {
                    Vector2 spawnPosition = Projectile.Center;
                    DarkMatterMetaBall.SpawnParticle(spawnPosition, Main.rand.NextVector2Circular(3f, 3f), 215f * Projectile.scale);
                }

                for (int i = 0; i < 2; i++) {
                    Vector2 spawnPosition = Projectile.Center;
                    DarkMatterMetaBall.SpawnParticle(spawnPosition, Main.rand.NextVector2Circular(3f, 3f), 215f * Projectile.scale);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            return CWRUtils.CircularHitboxCollision(Projectile.Center, Projectile.scale * 96, targetHitbox);
        }
    }
}
