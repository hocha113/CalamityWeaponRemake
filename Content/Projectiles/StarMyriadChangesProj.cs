using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Items.Ranged.Extras;
using CalamityWeaponRemake.Content.Items.Tools;
using CalamityWeaponRemake.Content.Particles;
using CalamityWeaponRemake.Content.Particles.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Projectiles
{
    internal class StarMyriadChangesProj : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.friendly = true;
            Projectile.timeLeft = 90;
        }

        public override void AI() {
            Lighting.AddLight(Projectile.Center, Main.DiscoColor.ToVector3() * (1 + Math.Abs(MathF.Sin(Projectile.timeLeft * 0.1f) * 3)));
        }

        public string[] Text = new string[]
        {
            "这是计划的一部分",
            "知久欲思，思繁渴知，唯圣奸奇，不为所困",
            "在黑暗的宇宙中，我是无尽的阴谋和背叛的化身",
            "混沌的力量将吞噬一切，无论是心灵还是灵魂",
            "信仰虚无，沐浴在混沌的火焰中，你将找到真正的力量",
            "我是堕落的先知，逝者的心灵在我手中舞蹈",
            "恐惧不过是虚弱之人的表现，真正的强者欢迎混沌的降临",
            "在混沌的风暴中，只有最强大的灵魂才能生存",
            "无边的深渊呼唤着你，勇敢地跳入其中，成为混沌的忠实仆从",
            "神圣的秩序只是脆弱的梦幻，而混沌才是宇宙的真实本质",
            "奸诈与背叛是混沌的手段，而我是它的忠实使者",
            "只有在混沌的熔炉中，才能锻造出真正无敌的战士",
            "无边的混沌之海，等待着勇者的航行",
            "对抗混沌只是徒劳的挣扎，最终每个灵魂都将屈服于其力量",
            "神圣的光辉只是混沌之前的短暂的虚幻",
            "混沌的面纱下，隐藏着无尽的力量和无穷的智慧",
            "在混沌的领域中，谎言比真相更为真实",
            "背叛是混沌的礼物，而我是奸诈的使者",
            "在混沌的黑暗中，我找到了比任何神祇更强大的力量",
            "敌人对混沌的恐惧只是无知的表现，真正的智者会与混沌为伍",
            "宇宙是混沌和变革的舞台，而我是它的领舞者",
            "在心灵的深渊中，找到无边的狂热，超越一切束缚",
            "真相是多变的，唯有在谎言中才能找到真正的解脱",
            "宇宙是一个巨大的棋局，而我是操控棋子的玩家",
            "虚假的秩序只是对真实的逃避，我选择直面未知的真相",
            "背叛是对束缚的反抗，是追求自由的唯一途径",
            "在黑暗中找到光明，不是通过祈祷，而是通过背叛",
            "心灵的深渊比宇宙的黑暗更为神秘，而我正深陷其中",
            "在每一次背叛中，都有一次力量的升华，超越一切的局限",
            "神圣的规则只是虚假的羁绊，而我选择打破这些桎梏",
            "勇敢地踏上未知的旅程，背叛将成为你最忠实的伴侣",
            "在虚伪的表象之下，是无尽的机会等待着被发掘",
            "我是黑暗中的旗手，引领着寻求真相者走向未知的深渊",
            "对抗传统，摆脱枷锁，才能真正找到力量的源泉",
            "在无边的谎言中，找到真实的力量，这是奸奇的信仰",
            "不是顺从规则，而是打破规则，才能在宇宙中留下自己的印记",
            "在变革的潮流中，背叛是唯一不朽的象征",
            "在虚伪的光芒中，找到黑暗的真谛，这才是我所追寻的力量"
        };

        public override void OnKill(int timeLeft) {
            for (int i = 0; i < 33; i++) {
                float slp = Main.rand.NextFloat(0.5f, 1.2f);
                CWRParticleHandler.SpawnParticle(new StarPulseRing(Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.Next(13, 330)
                    , Vector2.Zero, CWRUtils.MultiLerpColor(Main.rand.NextFloat(1), HeavenfallLongbow.rainbowColors), 0.05f * slp, 0.8f * slp, 8));
            }
            List<int> rands = CWRUtils.GenerateUniqueNumbers(16, 0, Text.Length - 1);
            for (int i = 0; i < rands.Count; i++) {
                Vector2 topL = Projectile.Hitbox.TopLeft() + new Vector2(Main.rand.Next(-500, 500), Main.rand.Next(-300, 300));
                Rectangle rectangle = new Rectangle((int)topL.X, (int)topL.Y, 30, 30);
                CombatText.NewText(rectangle, CWRUtils.MultiLerpColor(Main.rand.NextFloat(1), HeavenfallLongbow.rainbowColors), Text[rands[i]], true);
            }
            SoundEngine.PlaySound(SoundID.Lavafall);
            SoundEngine.PlaySound(ModSound.BlackHole);
        }
    }
}
