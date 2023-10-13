using Terraria.ModLoader;

namespace CalamityWeaponRemake.Common
{
    //这个全局物品类的创建是一种妥协，因为替换原模组内容的手段无非是Global类与反射等机制代码，
    //但这些方法都无法很好且高效的解决物品类中的特有成员值的适配问题，
    //比如，重制物品A类的相对于原物品类新增加了成员a，但如何让原模组的物品A在应用重制修改后也能正常使用成员a?
    //我无法找到更好的解决方法，于是只能创建并即将大量应用这个CWRItems类
    public class CWRItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public float BansheeHookCharge = 500;
        public int KevinCharge = 500;
    }
}
