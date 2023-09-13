using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common.AuxiliaryMeans
{
    public static class Languages
    {
        /// <summary>
        /// 根据语言选项返回字符
        /// </summary>
        public static string Translation(string Chinese = null, string English = null, string Japanese = null, string Russian = null)
        {
            if (Language.ActiveCulture.Name == "zh-Hans") return Chinese;
            if (Language.ActiveCulture.Name == "en-Hans") return English;
            if (Language.ActiveCulture.Name == "ja-Hans") return Japanese;
            if (Language.ActiveCulture.Name == "ru-Hans") return Russian;
            return null;
        }
    }
}
