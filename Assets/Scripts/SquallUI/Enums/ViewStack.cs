// 游戏栈模式，用于控制界面显示流程
public enum ViewStack
{
    /// <summary>
    /// 全屏界面，只显示此界面（隐藏其他界面）
    /// </summary>
    FullOnly = 0,

    /// <summary>
    /// 覆盖主界面，当最顶层全屏为主界面（MainCityView）时，隐藏主界面
    /// </summary>
    OverMain = 1,

    /// <summary>
    /// 叠加，界面显示时一直向上叠加
    /// </summary>
    OverLay = 2,
}