using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityWeaponRemake.Common;
using CalamityWeaponRemake.Content.Projectiles.Weapons.Melee.StormGoddessSpearProj;
using CalamityWeaponRemake.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Content.Items.Melee.Extras
{
    internal class StormGoddessSpear : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "StormGoddessSpear";
        public LocalizedText LegendText;
        public override void SetDefaults() {
            Item.width = 100;
            Item.height = 100;
            Item.damage = 440;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 19;
            Item.useTime = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.shoot = ModContent.ProjectileType<StormGoddessSpearProj>();
            Item.shootSpeed = 15f;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.CWR().remakeItem = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return true;
        }

        public override bool CanUseItem(Player player) {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StormGoddessSpearProj>()] <= 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine cumstops = tooltips.FirstOrDefault((TooltipLine x) => x.Text.Contains("[tips]") && x.Mod == "Terraria");
            if (cumstops == null) return;

            KeyboardState state = Keyboard.GetState();
            if ((state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))) {
                cumstops.Text = CWRUtils.Translation(
                    "\n在泰拉大陆的幽暗之境，昔日的女战神艾莉娜如今已陷入无尽的混沌和绝望之中，成为众人唇边轻声呼唤的风暴女神。" +
                    "\n曾经，她是大陆上无可匹敌的战士，其无畏之勇与刀锋之锐如同闪电一般统御着一片宁谧的天空。" +
                    "\n然而，命运的阴影在她的脚下蔓延。在光与暗的终焉战役中，天堂崩坏陨落，黑暗被光明所蚕食。" +
                    "\n风暴女神亲身经历了至高神的残酷。傲慢的神明夺走了她的同袍、亲人，摧毁了她曾经庇护的一切。" +
                    "\n至高神的罪孽令她对曾经虔信的正义产生疑虑，最终陷入深不可测的堕落之渊。" +
                    "\n随着战火的余烬飘散，艾莉娜回归泰拉，却已不再是昔日的神勇战士。" +
                    "\n内心的苦痛与无尽的绝望使她抛却了崇高的信仰，寻求新的力量的源泉。" +
                    "\n在一场血腥的仪式中，她如愿获得了那禁忌的赐福。无尽的力量汹涌而至，却以灵魂的永恒堕落为代价。" +
                    "\n如今的风暴女神成为了泰拉深渊的象征，她的铠甲上嵌入着如夜空一般的黑曜石，眼中闪烁着的堕落之辉，混沌涌动，憎恶的情绪投射向泰拉大陆。" +
                    "\n她的剑锋犹如黑夜中的雷霆，带有毁灭之力的电弧在其周围舞动。她不再为正义而战，而是沦为一道摧毁一切的黑暗势力。" +
                    "\n尽管她依旧保留着昔日的武技和战斗技巧，但此刻她已是泰拉的公敌，遭受着无尽的唾弃。" +
                    "\n风暴女神残酷地踏上孤寂而堕落的征途，在黑暗中徘徊。她的存在本身便是对泰拉昔日荣光的嘲讽，她成为一场吞噬一切希望的风暴。" +
                    "\n如同堕入深渊的耀星，她在无边的黑暗中绽放，携带着宇宙的哀伤和毁灭。"
                    ,
                    "\nIn the shadowed realms of Terra, the once radiant warrior goddess, Alina, " +
                    "\nnow finds herself plunged into an endless abyss of chaos and despair, emerging as the whispered harbinger known as the Storm Goddess." +
                    "\nOnce, she stood as an unmatched warrior on the continent, her fearless courage and razor-sharp blade commanding a serene sky like lightning. " +
                    "\nHowever, the shadows of fate began to creep beneath her feet. In the twilight clash between light and darkness, as heaven crumbled and darkness consumed the remnants, " +
                    "\nthe Storm Goddess bore witness to the cruelty of the Supreme Deity. The arrogant god stripped her of comrades, kin, and obliterated all she had once sheltered." +
                    "\nThe sins of the Supreme Deity cast doubts upon her once unwavering faith in justice, leading her to descend into the unfathomable abyss of corruption." +
                    "\nAs the embers of war settled, Alina returned to Terra, but she was no longer the valiant warrior of yore." +
                    "\nThe internal agony and boundless despair propelled her to forsake the lofty beliefs she once held, seeking a new wellspring of power. " +
                    "\nIn a gruesome ritual, she obtained the forbidden blessing – an influx of boundless power, paid for with the eternal corruption of her soul." +
                    "\nNow, the Storm Goddess stands as an emblem of abyssal descent, her armor adorned with obsidian akin to a night sky, " +
                    "\neyes shimmering with the corruptive radiance. The chaotic currents surge, projecting malice towards Terra. Her blade," +
                    "\na thunderous force in the obsidian night, dances with arcs of annihilative power. No longer fighting for justice, she has become a force set on obliterating all." +
                    "\nThough she retains the martial prowess of yore, she is now the nemesis of Terra, scorned by countless tongues. " +
                    "\nThe Storm Goddess cruelly embarks on a solitary and corruptive journey, lingering in the shadows. Her very existence mocks the bygone glory of Terra," +
                    "\na tempest consuming every vestige of hope." +
                    "\nAs a fallen star plunges into the abyss, she blossoms in the boundless darkness, carrying the universe's sorrow and destruction."
                    );
            }
            else {
                cumstops.Text = CWRUtils.Translation("按下[Shift]聆听故事...", "Press [Shift] to listen to the story...");
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ThunderSpear)
                .AddIngredient<CalamityMod.Items.Weapons.Melee.StormRuler>()
                .AddIngredient<CalamityMod.Items.Weapons.Rogue.StormfrontRazor>()
                .AddIngredient<StormlionMandible>(5)
                .AddIngredient(ItemID.LunarBar, 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
