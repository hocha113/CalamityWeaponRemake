using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Bosses.RavagerAProjs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.NPCs.RavagerAs
{
    internal class EvilSoul : ModNPC
    {
        public override string Texture => CWRConstant.RavagerA + "EvilSoul";

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 32;
            NPC.lifeMax = 36000;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.alpha = 0;
            NPC.damage = 125;
            NPC.noTileCollide = true;
            NPC.boss = true;
            Music = 78;//这个音乐是占位使用——谁让它这么酷呢？
        }

        public ref float Time => ref NPC.ai[0];

        public override void AI()
        {
            //if (Time == 0)
            //SoundEngine.PlaySound(new SoundStyle(CWRConstant.Sound + "ShenA"));
            //Music = MusicLoader.GetMusicSlot(Mod, "Assets/ShenA");
            //Main.NewText(MusicLoader.MusicExists(CalamityWeaponRemake.Instance, "Assets/Sounds/Music/ShenA"));
            CWRUtils.ClockFrame(ref NPC.frameCounter, 5, 3);
            NPC.alpha += 2;
            if (NPC.alpha > 255)
                NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            if (NPC.target >= 0 && Main.player[NPC.target].dead)//在合适条件下寻找玩家
            {
                NPC.TargetClosest();
                if (Main.player[NPC.target].dead)
                    NPC.active = false;
            }
            NPC.position += Main.player[NPC.target].velocity * 0.75f;
            NPC.ChasingBehavior(Main.player[NPC.target].Center + new Vector2(0, -280), 13);
            NPC.timeLeft = 2;

            switch (Time)
            {
                case 175:
                    CWRUtils.Text(langs[0], new Color(45, 46, 70));
                    break;
                case 350:
                    CWRUtils.Text(langs[1], new Color(45, 46, 70));
                    break;
                case 600:
                    CWRUtils.Text(langs[2], new Color(45, 46, 70));
                    break;
                case 750:
                    CWRUtils.Text(langs[3], new Color(45, 46, 70));
                    break;
                case 900:
                    CWRUtils.Text(langs[4], new Color(45, 46, 70));
                    break;
                case 1150:
                    CWRUtils.Text(langs[5], new Color(146, 30, 68));
                    break;
            }
            if (Time >= 1200)
            {
                if (!CWRUtils.isClient)
                {
                    int boss = CWRUtils.NewNPCEasy(new EntitySource_BossSpawn(NPC), NPC.position, ModContent.NPCType<RavagerABody>());
                    Main.npc[boss].timeLeft *= 20;
                }
                Projectile.NewProjectile(NPC.parent(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveBoom>(), 0, 1f, Main.myPlayer, 0f, 0f);
                NPC.active = false;
            }
            Time++;
        }

        public static string[] langs = new string[]
        {
            CWRUtils.Translation("寄生在烈日里的家伙已经被你击败了？"),
            CWRUtils.Translation("令人称奇，然不过尔尔"),
            CWRUtils.Translation("寻神者，你的路令你痛苦，不如在此止步"),
            CWRUtils.Translation("上一个像你的家伙已经下场可不好"),
            CWRUtils.Translation("若你不肯停止你的杀戮"),
            CWRUtils.Translation("我  不  介  意  让  你  成  为  我  身  体  上  的  装  饰  之  一")
        };

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D value = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(
                value,
                NPC.Center - Main.screenPosition,
                CWRUtils.GetRec(value, (int)NPC.frameCounter, 4),
                Color.White * (NPC.alpha / 255f),
                0,
                CWRUtils.GetOrig(value, 4),
                NPC.scale,
                SpriteEffects.None
                );
            return false;
        }
    }
}
