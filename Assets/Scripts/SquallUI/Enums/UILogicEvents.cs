using System;

/// <summary>
/// UI 逻辑事件派发
/// </summary>
public enum UILogicEvents
{
    None = 0,
    //region 进入游戏逻辑
    //加载平台配置;
    LoadPlatformConfig = 1,
    //加载客户端配��
    LoadClientConfig = 2,
    //获取渠道配置;
    GetChannelConfig = 3,
    //渠道文件获取失败;
    ChannelGetFailed = 4,
    //渠道获取成功
    ChannelGetSucc = 5,
    //获取新版��
    GetNewVersion = 6,
    //资源准备;
    ResourcesPrepare = 7,
    //资源更新错误;
    ResUpdateError = 8,
    //资源更新状��
    ResUpdateState = 9,
    //DownloadManager初始化错��
    AssetBundleManagerInitError = 10,
    //配置初始化进��
    ConfigInitProgress = 11,
    //无网络连��
    NetworkNotConnected = 12,
    //新版本下载地址出错
    AppDownloadUrlError = 13,
    //解析客户端本地配��
    ParseClientConfig = 14,

    //货币刷新通知
    MoneyInfoUpdate = 15,
    //背包_角色穿上装备
    GameBag_EquipOn = 16,
    // sdk回退登录界面
    SDKReturnLogin = 17,
    //资源更新完成
    ResUpdaterFinish = 18,
    //加载场景进度
    LoadSceneProgress = 24,
    //打开背包物品详情，带操作按键
    GameBag_OpenItemDetail = 25,
    //打开界面
    Game_ShowView = 30,
    //显示遮罩界面
    MaskViewStart = 31,
    //强化装备回调
    StrengthenEquipCallback = 52,
    // 洗炼词条更新
    EquipWashEntry = 60,
    // 加载界面停顿问题
    LoadingFinish = 72,
    // 多人副本加载进度
    BattleLoadingPlayerProgress = 89,
    //装备分解成功
    EquipDecompositionSucceed = 103,
    // 打开根据品质出售界面
    SellByQualityOpen = 118,
    // 道具出售界面
    ItemSellOpen = 120,
    //打开物品tips界面，不带操作按键，物品可能还没有到玩家身上
    ShowResTips = 130,
    // 开始界面显示类��
    Startview_State = 140,
    // sdk初始化完��
    SDKInit_Finish = 141,
    // 充值请求返��
    ReChargeResp = 142,

    // 装备主体长度变化
    EquipMainPosLengthChanged = 150,
    // 装备主体粗细变化
    EquipMainPosScaleChanged = 151,
    // 装备主体是否显示骨骼
    EquipMainPosShowBone = 152,
    // 装备配件长度变化
    EquipSubPosLengthChanged = 153,
    // 装备配件粗细变化
    EquipSubPosScaleChanged = 154,
    RefreshDarkMazeView = 155,
    // 装备打造界面类��
    EquipProduceViewType = 160,
    // 装备设计界面类型
    EquipDesignViewType = 161,
    // 显示对话界面
    ShowSystemMessageView = 162,
    // 显示确认��
    ShowConfirmView = 163,
    // 装备打造界面关闭界��
    EquipProduceViewHideType = 164,
    // 装备设计界面关闭界面
    EquipDesignViewHideType = 165,
    // 显示选角界面
    ShowSelectRoleView = 170,

    // 技能业务数据更��- GameSkillData
    ClientSkillInfoUpdate = 171,

    //刷新选角界面数据
    RefreshSelectRoleViewData = 172,

    //刷新技能页combos
    UpdateSkillPageCombos = 173,
    //购买技能页返回
    BuySkillPageSuccess = 174,
    //选择技能页返回
    SelectSkillPageSuccess = 175,
    //修改技能页名字
    ChangeSkillPageName = 176,
    //技能等级变��
    SkillLevelChange = 177,
    //刷新整个技能页数据
    UpdateSkillPage = 178,

    //刷新快速挂机数��
    UpdateHangUpQuickNum = 179,
    //刷新挂机奖励数据
    RefreshHangUpRewardList = 180,
    //刷新队伍数据
    RefreshTeamInfo = 181,
    //刷新队伍列表数据
    RefreshTeamInfoList = 182,
    //显示飘字提示
    ShowTipsView = 183,
    //初始化任务列��
    InitQuestList = 184,
    //查询的队��
    RefreshQueryTeam = 185,
    //刷新队伍申请列表
    RefreshTeamApplyInfo = 186,
    //刷新组队副本翻咔信息
    RefreshFlipCardItemInfo = 187,
    //刷新组队副本信息
    RefreshTeamInstanceInfo = 188,
    //刷新组队副本列表信息
    RefreshTeamInstanceListInfo = 189,
    //刷新组队副本翻咔
    RefreshFlipCardItem = 190,
    //刷新组队翻咔点击事件
    FlipCardRemoveListener = 191,
    // 打开战斗力变化弹��
    ShowPowerTipsView = 192,
    // 打开一键分解界��
    DecomposeByQualityOpen = 193,
    // 刷新背包通知
    UpdateBackPackView = 194,
    //刷新组队地图列表
    RefreshTeamAllMapList = 195,
    //队伍界面
    ShowTeamView = 196,
    //刷新角色摇杆位置
    UpdateJoystickMovePos = 197,
    //刷新地图列表
    RefreshMapList = 198,
    //刷新地图列表
    RefreshMainFBTeamNotice = 199,
    //显示对话��
    ShowDialogView = 201,
    // 模态框
    ShowModalView = 202,
    // 任务界面
    ShowQuestView = 203,
    //提交物品界面
    SubmitItemView = 204,
    //刷新选角界面角色信息
    RefreshSelectRoleInfo = 205,
    //刷新选角界面角色选中状��
    RefreshSelectRoleSatus = 206,
    //刷新职业界面职业选中状��
    RefreshSelectJobSatus = 207,
    //显示视频播放��
    ShowVideoView = 208,
    //刷新子职业显��
    RefreshSubJobShow = 209,
    //刷新高清视频展示
    RefreshHDVideoShow = 210,
    //刷新精炼界面
    EquipSlotRefineUpdate = 211,
    //刷新队伍列表
    RefreshTeamList = 212,
    //刷新任务列表
    RefreshQuestList = 213,
    //打开主线副本详情界面
    ShowDetailsView = 214,
    //打开规则说明界面
    ShowExplainView = 215,
    //刷新主线副本关卡数据
    RefreshMainFBDeatilsData = 216,
    //显示物品掉落
    ShowDropItemView = 217,
    //刷新技能页mode
    UpdateSkillPageMode = 218,
    // 刷新当前精通等级任��
    UpdateEquipProficientTask = 219,
    //显示主线副本主界��
    ShowMainFBView = 220,
    //设置主线副本关卡难度
    SetMainFBDifficulty = 221,
    //掉落展示
    ShowDropTipView = 222,
    //刷新精通界��
    UpdateEquipProficientView = 223,
    //显示获得物品提示
    ShowGetItemTipsView = 224,
    //刷新个人副本界面
    RefreshBossFBView = 225,
    //打开组队副本准备界面
    ShowMainFBNoticeView = 226,
    //刷新组队设置界面章节选择
    RefresTeamSettingViewChapter = 227,
    //刷新队伍界面显示
    RefresTeamView = 228,
    //刷新结算界面
    RefreshFinishView = 229,
    //显示翻卡界面
    ShowFlipCardView = 230,
    //显示战斗界面
    ShowBattleView = 231,
    //刷新快速通关奖励
    ShowQuickLevelReward = 232,
    //背包整理返回
    BagClearUp = 233,
    //Boss来袭
    BossCome = 234,
    //暂停战斗
    PauseBattle = 235,
    //恢复战斗
    ResumeBattle = 236,
    //显示找回体力购买弹窗
    ShowRetrievePopView = 237,
    //刷新其他属��
    RefreshOtherAttr = 238,
    //显示获得物品提示
    ShowItemTipsView = 239,
    //显示装备Tip
    ShowEquipTip = 240,
    //开始战��
    StartBattle = 241,
    //结束战斗开始的黑屏过渡
    FinishBattleBlackScreen = 242,
    //显示战斗数据统计界面
    ShowStatsView = 243,
    //显示快捷使用
    ShowQuickPanel = 244,
    //刷新附魔部位
    UpdateEnchantGridPosData = 245,
    // 打开选择附魔卡界��
    ShowSelectEnchantCard = 246,
    // 选择附魔��
    SelectEnchantCard = 247,
    // 打开附魔提示界面
    ShowEnchantPopView = 248,
    // 主线副本UI地图放大
    MainFBViewBig = 249,
    //显示复活界面
    ShowRevivePopView = 250,
    //显示兑换商店
    ShowExchangeMallView = 251,
    // 刷新选中的附魔卡升级材料
    UpdateEnchanteLevelUpCard = 252,
    // 刷新附魔卡升级面��
    UpdateEnchantLevelUpPanel = 253,
    //转职界面切换不同的进阶职��
    UpdateTransferView = 254,
    //显示6元礼包界��
    ShowSixGiftBagView = 255,
    //刷新Boss首杀界面
    RefreshBossFirstKillView = 256,
    //显示首杀界面红包
    ShowBossFirstKillRedPacketView = 257,
    //刷新活动奖励领取
    RefreshActivityGetReward = 258,
    //显示个人副本界面
    ShowBossFBView = 259,
    //获取服务器任务状��
    GetServerTaskStatus = 260,
    //转职成功
    TransferSuccess = 261,
    // 打开附魔合成预览界面
    ShowEnchantMixPreviewView = 262,
    // 合成成功界面
    ShowEnchantMixCardRewardView = 263,
    // 刷新附魔卡合成界��
    UpdateEnchantMix = 264,
    //打开升级提示界面
    ShowLevelUpdateView = 265,
    //刷新功能开启状��
    RefreshPlayOpenStatus = 266,
    //显示深渊副本
    ShowAbyssFBView = 267,
    //放大深渊副本
    AbyssFBViewBig = 268,
    //刷新深渊副本详细数据
    RefreshAbyssFBDeatilsData = 269,
    //显示深渊副本详情
    ShowAbyssDetailsView = 270,
    //设置深渊副本详情
    SetAbyssFBDifficulty = 271,
    //显示一个技能的详细信息
    ShowSkillContent = 272,
    //显示章节奖励
    ShowChapterRewardView = 273,
    //刷新章节奖励
    RefreshChapterRewardView = 274,
    //刷新物品购买
    RefreshGoodsBuy = 275,
    //获取最新的权益数据消息
    RefreshEquityData = 276,
    //刷新章节奖励进度
    RefreshChapterRewardProcess = 277,
    //显示主线副本主界面并显示关卡��
    ShowMainFBViewOpenLevelGroup = 278,
    //显示购买界面
    ShowItemBuyView = 279,
    // 显示附魔卡详情界��
    ShowEnchantInfoView = 280,
    // 显示礼包详情界面
    ShowGiftInfoView = 281,
    // 显示 物品获取途径界面
    ShowAccessView = 282,
    // 显示邮箱简要信��
    ShowBriefMailData = 283,
    // 显示邮箱详细信息
    ShowDetailMailData = 284,
    // 选中某个id的邮��
    ChooseMailData = 285,
    //刷新冒险数据
    RefreshRiskPanel = 286,
    //刷新冒险奖励数据
    RefreshRiskItem = 287,
    //刷新任务引导
    RefreshQuestGuide = 288,
    //显示签到界面
    ShowSignInView = 289,
    //显示登录界面
    ShowLoginView = 290,
    //接收结束战斗的消��
    ReceiveFinishBattleResp = 291,
    //刷新签到界面
    RefreshSignInView = 290,
    //显示迷宫界面
    ShowDarkMazeView = 292,
    //顯示寵物技能
    ShowPetSkillDescView = 293,
    //引导事件
    GuideEvent = 300,
    //显示通用消息弹窗
    ShowComMessageView = 301,
    //刷新多人战斗loading进度
    RefreshFigherLoadingProgress = 302,
    //刷新兑换商店
    RefreshExchangeMallView = 303,
    //引导界面手指点击
    GuideViewFingerTip = 304,
    //刷新6元礼包数��
    RefreshSixGiftBagReward = 305,
    //引导提示界面
    ShowGuideTip = 306,
    //收到新的聊天消息
    ReceiveNewChatMsg = 307,
    //打开称号详情界面
    ShowTitleInfoView = 308,
    //刷新背包称号
    UpdateBagTitle = 309,
    //显示战斗loading
    ShowLoadingView = 310,
    //刷新服务器公��
    RefreshServerNotice = 311,
    //子弹buff数量
    RefreshTrajectoryChange = 312,
    //刷新活动红点
    RefreshActivityRed = 313,
    //刷新红点
    RefreshAllRed = 314,
    //显示商城界面
    ShowShopView = 315,
    //刷新商城界面
    RefreshShopView = 316,
    //显示签到提示界面
    ShowSignInTipView = 317,
    //隐藏鼓舞道具和抗魔值道具显��
    HideRssItemAndInspireItem = 318,
    //转职成功界面表现
    TransferSuccessViewPlay = 319,
    //显示活动界面
    ShowActivityView = 320,
    //刷新主界面活动按钮开启状��
    RefreshMainActivityView = 321,
    //显示引导跳过界面
    ShowGuideJumpView = 322,
    //显示锻造界��
    ShowSlotRefineView = 323,
    //刷新充值返��
    RechargeResultRefresh = 324,
    //刷新技能背��
    UpdateSkillBag = 325,
    //刷新关卡挑战次数
    RefreshLevelDayTime = 326,
    //重置摇杆类型
    ResetJoyType = 327,
    //打开月卡暂存箱界��
    ShowMonthCardRewardsView = 328,
    //打开时装详情界面
    ShowFashionInfoView = 329,
    //刷新时装背包
    UpdateFashionBag = 330,
    //打开时装重选界��
    ShowFashionResetPanel = 331,
    //刷新时装重选界��
    UpdateFashionResetPanel = 332,
    //刷新boss选择
    RefreshBossSelectView = 333,
    //首领讨伐的进度数��
    GetBossCrusadeProcess = 334,
    //讨伐首领领取奖励状态返��
    GetBossCrusadeReward = 335,
    //显示讨伐排行��
    ShowBossRewardView = 336,
    //刷新购买战斗次数
    RefreshBossBuyFightCount = 337,
    //打开时装续费界面
    ShowFashionRenewView = 338,
    //刷新时装穿戴
    UpdateFashionWear = 339,
    //移除要分解的时装
    RemoveDecomposeFashion = 340,
    //刷新时装分解界面
    UpdateFashionDecomposeFashion = 341,
    //显示购买精益界面
    ShowGainPotionPopView = 342,
    //显示获取奖励界面
    ShowGetRewardPopView = 343,
    //诸神宝藏返回数据
    GetPreciousInfo = 344,
    //宝藏系统捐献装备
    RefreshPreciousDonate = 345,
    //宝藏系统捐献装备返回
    PreciousDonateResNet = 346,
    //显示宝藏提示
    ShowPreciousDepositsTipsView = 347,
    //显示捐献获得界面
    ShowDonateGetReward = 348,
    //添加时装合成界面
    UpdateFashionMixView = 349,
    //选中米拉梦境楼层
    SelectTowerLevel = 350,
    //刷新冒险笔记数据
    RefreshAdventureNotes = 351,
    //忽略列表里的限时物品提醒
    IgnoringItem = 352,
    //显示玩家交互界面
    ShowInteractView = 353,
    //社交界面消息最��
    FriendNewsNear = 354,
    //社交界面消息陌生��
    FriendNewsStranger = 355,
    //查找好友返回
    SearchFriendRes = 356,
    //更新好友
    FriendUpdateList = 357,
    //刷新远古副本详情
    RefreshPrimevalFBDeatilsData = 358,
    //显示远古副本详情
    ShowPrimevalDetailsView = 359,
    //设置远古副本详情
    SetPrimevalFBDifficulty = 360,
    //刷新队伍邀请列��
    RefreshTeamInviteList = 361,
    //播放奖励特效
    PlayPreciousDepositsEffect = 362,
    //更新请求添加为好友列表更��
    UpdateFriendRequestList = 363,
    //刷新远古宝库
    RefreshPrimevalTreasuryView = 364,
    //打开宠物喂食界面
    ShowPetFeedView = 365,
    //打开深渊副本显示详情
    ShowAbyssFBViewOpenLevelGroup = 366,
    //打开宠物详情界面
    ShowPetInfoView = 367,
    //删除聊天记录
    DeleteChatRecord = 368,
    //宠物喂食刷新
    UpdatePetFeed = 369,
    //显示时空畸变
    ShowSpaceDistortionView = 370,
    //显示异界入侵界面
    ShowFarplaneDetailsView = 371,
    //异界入侵界面buff等级刷新
    ShowFarplaneDetailsBuff = 3711,
    //刷新异界入侵
    RefreshFarplaneFBDeatilsData = 372,
    //设置异界入侵
    SetFarplaneFBDifficulty = 373,
    //刷新副本特殊翻牌次数
    RefreshFBSpecialCount = 374,
    //刷新社交红点
    RefreshFriendRed = 375,
    //异界副本获取的体��
    RefreshFBInvadeEnergy = 376,
    //战令系统buff信息
    RefreshGetWarOrderBuff = 377,
    //刷新宠物出战
    UpdatePetFight = 378,
    //显示自选礼包界��
    ShowOptionGiftView = 379,
    //刷新宠物跟随
    UpdatePetFollow = 380,
    //关闭喂食界面刷新宠物界面模型
    UpdatePetModel = 381,
    //打开背包界面并且跳到指定页签
    ShowBackpackViewByType = 382,
    //刷新限时活动数据
    RefreshLimitTimeInfo = 383,
    //刷新活动界面队列
    RefreshActivityQueue = 384,
    //添加活动界面队列
    AddActivityQueue = 385,
    //显示礼包购买界面
    ShowShopGiftBuyPopView = 386,
    //显示时装购买界面
    ShowFashionBuyPopView = 387,
    //刷新商城
    RefreshShopPanel = 388,
    //显示套装信息
    ShowFashionGiftInfoView = 389,
    //显示通用购买弹窗
    ShowShopBuyView = 390,
    //选择时装商城职业组Id
    SelectFashionShopPanelJobGroupId = 391,
    //选择时装商城职业Id
    SelectFashionShopPanelJobId = 392,
    //选择时装商城套装
    FashionShopSelectSuit = 393,
    //选择时装商城单品试穿
    FashionShopSelectSingleFitting = 394,
    //清除时装商城单品试穿
    FashionShopClearSingleFitting = 395,
    //显示每日礼包界面
    ShowDayGiftView = 396,
    //选择商城页签
    SelectShopTabView = 397,
    //bannerLink
    BannerLinkJump = 398,
    //刷新物品到期提醒面板
    UpdateFashionOverdueView = 399,
    //魔盒抽奖
    ShowMagicBoxView = 400,
    //魔盒抽奖结果
    ShowMagicBoxResultView = 401,
    //魔盒抽奖翻牌
    MagicBoxResultOpen = 402,
    //魔盒道具合成
    MagicBoxConsumeMerge = 403,
    //选择新服目标��
    SelectNewServerTargetDay = 404,
    //刷新累计充��
    RefreshAccumulatePay = 405,
    //刷新时装特惠
    RefreshFashionSale = 406,
    //显示奖励预览
    PreviewReward = 407,
    //魔盒道具合成返回
    MagicBoxMergeResp = 408,
    //刷新战令界面
    UpdateWarTokenView = 409,
    //打开战令购买界面
    ShowWarTokenPayPanel = 410,
    //显示累计
    ShowAccumulatePayBuyView = 411,
    //新服目标任务进度同步
    RefreshNewServerTask = 412,
    //更新每日礼包红点
    RefreshDayGiftRed = 413,
    //请求盟会列表返回
    GetGuildListRes = 414,
    //刷新后台商品
    RefreshBackMailBuy = 415,
    //刷新异界战令红点
    UpdateWarTokenRed = 416,
    //更新成员信息
    UpdateGuildMembers = 417,
    //更新申请列表
    UpdateGuildApplyList = 418,
    //更新被邀请加入的盟会列表
    UpdateGuildInviteList = 419,
    //查找的盟会返��
    QueryGuildRes = 420,
    //刷新购买商品
    RefreshShopItem = 421,
    //更新盟会信息
    RefreshGuildInfo = 422,
    //获取玩家信息失败
    GetRoleInfoFail = 423,
    //刷新活动数据
    RefreshActivity = 424,
    //退出公��
    SignoutGuild = 425,
    //打开使用道具提示面板
    ShowUseItemTipsView = 426,
    //刷新活动页签
    RefreshActivityTag = 427,
    //时限界面全部忽略
    OverdueViewIgnoreAll = 428,
    //盟会升级成功
    GuildUpgradeSucc = 429,
    //刷新签到界面
    UpdateGuildSingInView = 430,
    //发送文本超链接消息
    SendTxtLinkMsg = 431,
    //显示排行榜界��
    ShowRankView = 432,
    //排行榜列表刷��
    RankListRefresh = 433,
    // 排行榜详细信息返��
    RankDetailsRefresh = 434,
    // 选择排行榜数��
    SelectRankListItem = 435,
    // 查看他人界面关闭
    LookPlayerInfoViewClose = 436,
    //刷新增益界面
    UpdateGuildBuffView = 437,
    //查看别人装备信息界面
    ShowOtherEquipInfoView = 438,
    //选择头衔
    SelectMilitaryRankToggleItem = 439,
    //刷新头衔
    RefreshMilitaryRank = 440,
    //获取通天塔ID
    RefreshTowerBossInfo = 441,
    //显示新功能开��
    ShowFunctionOpenView = 442,
    //更新世界聊天的每日免费次��
    UpdateFreeMsgCount = 443,
    //刷新队伍显示数据
    RefreshTeamInfoView = 444,
    //刷新副本数据
    FBTypeTimeUpdate = 445,
    //点击选择合成的魔��
    ClickMagicBox = 446,
    //打开魔盒制作选取装备界面
    ShowMagicBoxSelectView = 447,
    //点击选择合成魔盒消耗的装备
    ClickMagickBoxMakeEquip = 448,
    //点击选择合成魔盒消耗装备界面确认按��
    ClickMagickBoxMakeEquipOkBtn = 449,
    //选择许愿武器
    SelectWishingEquip = 450,
    //刷新许愿显示数据
    RefreshWishingData = 451,
    //道具期限刷新更新界面
    UpdateItemDeadline = 452,
    //刷新活动更新界面
    UpdateActivityData = 453,
    //点击装备进化目标装备
    ClickEquipUpEquip = 454,
    //打开装备进化防具选取列表
    ShowEquipUpSelectView = 455,
    //刷新装备进化界面
    UpdateEquipUpView = 456,
    //显示制作界面选择
    ShowManufactureView = 457,
    //装备进化界面材料选择
    EquipUpSelectConsume = 458,
    //刷新任务完成列表
    RefreshQuestFinishList = 459,
    //打开队伍列表
    OpenTeamListViewByLevelId = 460,
    //显示时空漩涡副本主界面并显示关卡��
    SpaceTimeVortexFBViewBig = 461,
    //显示时空漩涡副本主界��
    ShowSpaceTimeVortexFBView = 462,
    //显示时空漩涡副本主界面并打开关卡��
    ShowSpaceTimeVortexFBViewOpenLevelGroup = 463,
    //刷新时空漩涡详情数据
    RefreshSpaceTimeVortexFBDeatilsData = 464,
    //设置时空漩涡详情数据
    SetSpaceTimeVortexFBDifficulty = 465,
    //选择材料
    SelectStuffFinish = 466,
    //获取异界强化券开放时��
    GetEnhanceFactory = 467,
    //合成强化石返��
    MakeEnhanceMateRes = 468,
    //提升强化券返��
    UpEnhanceRes = 469,
    //制作强化券返��
    MakeEnhanceRes = 470,
    //打开自动强化界面
    ShowEquipAutoReinforceView = 471,
    //通用进度条组件刷新数��
    UpdateCommonSliderValue = 472,
    //打开物品批量使用界面
    ShowItemUseView = 473,
    //删除角色通知
    DelRoleMessage = 474,
    //刷新恢复角色
    RefreshRecoverRole = 475,
    //显示删除提示面板
    ShowDelRoleMessageView = 476,
    //等级投资活动状态返��
    GetLvInvestInfoRes = 477,
    //等级投资红点
    RefreshInvestRed = 478,
    //显示等级投资购买提示
    ShowLevelInvestTip = 479,
    //至尊魔盒开启返��
    SupremeBoxRspNet = 480,
    //领取至尊魔盒奖励返回
    GetSupremeBoxRewardRspNet = 481,
    //更新副本类型数据
    RefreshFbTypeInfo = 482,
    //选择抽魔��
    SelectDrawMagicbox = 483,
    //获取抽奖的滚动奖��
    GetSupremeDrawInfo = 484,
    // 角色头像变化
    PlayerHeadUpdate = 485,
    //打开荣耀战令界面
    ShowHonourPassView = 486,
    //刷新荣耀战令奖励界面
    UpdateHonourRewardPanel = 487,
    //刷新荣耀战令任务界面
    UpdateHonourTaskPanel = 488,
    //刷新荣耀战令商店界面
    UpdateHonourShopPanel = 489,
    //刷新个人活动
    UpdateSingleActivity = 490,
    //单人活动红点状��
    SingleActivityRedPoint = 491,
    //打开荣耀战令开通成功界��
    ShowHonourPassOpenPanel = 492,
    //显示盟会boss副本
    ShowGuildBossView = 493,
    //显示盟会boss未开始预��
    ShowGuildBossNotStartView = 494,
    //关闭至尊魔盒获奖界面
    ColseSupremeRewardPopView = 495,
    //显示盟会boss增益购买
    ShowGuildBossBuyGainView = 496,
    //更新盟会buff叠加次数
    UpdateBuyGuildActBuff = 497,
    //更新盟会活动玩法个人伤害奖励领取返回
    UpdateFetchGuildActDamageReward = 498,
    //选择神佑时装职业��
    SelectGodFashionPanelJobGroupId = 499,
    //选择神佑时装职业
    SelectGodFashionPanelJobId = 500,
    //刷新时装制作窗口
    UpdateFashionMakeView = 501,
    //装备异化返回
    EquipGrowthReturn = 502,
    //装备破损修复返回
    EquipDurabilityReturn = 503,
    //装备强化异化替换界面
    ShowEquipReinforceGrowthView = 504,
    //选择时装镶嵌选择
    SelectChangeFashionBadgeItem = 505,
    //刷新盟会Boss数据
    UpdateGuildBossView = 506,
    //刷新许愿数据
    UpdateParyBagData = 507,
    //更新时装槽位徽章
    UpdateFashionSlotBadge = 508,
    //显示神佑时装属性窗��
    ShowBlessedFashionAttrsView = 509,
    //徽章tip
    ShowBadgeTips = 510,
    //打开异化转移选择装备弹窗
    ShowEquipTransferPopView = 511,
    //确定转移目标
    SetEquipTransferTarget = 512,
    //装备异化转移返回
    EquipGrowthAttrCopyReturn = 513,
    //打开装备异化转移预览界面
    ShowEquipTransferPreview = 514,
    //打开异化转移提示��
    ShowEquipTransferHintView = 515,
    //跳转到装备修复界��
    JumpEquipRepairPanel = 516,
    //装备异常修复返回
    EquipAbnormalReturn = 517,
    //属性重置返��
    EquipResetReturn = 518,
    //荣耀战令等级提升通知
    HonourPassLevelUp = 519,
    //荣耀战令红点返回
    HonourPassRedUpdate = 520,
    //选择洗炼装备
    SelectEquipWashItem = 521,
    //选择服务器组索引
    SelectServerGroupIdx = 522,
    //选择服务��
    SelectServerItem = 523,
    //打开装备升级异化等级继承弹窗
    ShowEquipUpHintView = 524,

    //工会活动刷新
    GuildActivityRefresh = 526,
    //刷新嘉年华活动红��
    RefreshCarnivalRed = 527,
    //选择嘉年华活动异界关��
    SelectCarnivalFarplaneData = 528,
    // 排行榜活动状态更��
    RankActivityRefresh = 529,
    // 排行活动排行榜更��
    RankActivityListRefresh = 530,
    // 排行活动排行榜详情更��
    RankActivityRoleInfoRefresh = 531,
    //
    SelectRankActivityListItem = 532,
    // 排行活动时间刷新
    RankActivityRemainTimeRefresh = 533,
    // 排行活动类型时间刷新
    RankActivityTypeTimeRefresh = 534,
    // 刷新嘉年华活动状��
    RefreshCarnivalActiveStatus = 535,
    // 转盘转到的奖��
    CarnivalTurntableRewardId = 536,
    // 雕像信息更新
    CityRankNpcDataUpdate = 537,
    // 盟会竞拍信息
    GuildAuctionNotify = 538,
    // 盟会竞拍信息更新
    GuildAuctionUpdate = 539,
    // 盟会页签红点
    GuildTableRedPoint = 540,
    //支付失败
    PayFail = 541,
    //盟会分红返回
    GuildAuctionStatusRes = 542,
    //获取盟会宝库信息返回
    GetGuildTreasury = 543,
    //开启盟会宝库返��
    OpenGuildTreasury = 544,
    //开启盟会宝库宝箱返��
    OpenGuildTreasuryBox = 545,
    //服务器通知盟会宝库信息更新
    NotifyGuildTreasuryInfo = 546,
    //打开盟会界面同时打开盟会宝库
    ShowGuildTreasuryView = 547,
    //打开星光套未集齐提示窗口
    ShowFashionWarningTipsView = 548,
    //打开秘典进化选择装备窗口
    ShowGrimoireUpSelectConsume = 549,
    //打开秘典进化提示窗口
    ShowGrimoireUpTipView = 550,
    //更新异界战令双倍状��
    UpdateAbyssDoubleStatus = 551,
    //刷新累计冒险状��
    UpdateRiskStatus = 552,
    //显示活动购买
    ShowActivityBuyView = 553,
    //徽章突破返回
    BadgeBreachBack = 554,
    //点击仓库中的物品
    ShowStoreGridItemDetailView = 555,
    //点击仓库下拉类型��
    SelectStoreHousePanelDropItem = 556,
    //个人仓库等级变化
    WareHouseLevelUpdate = 557,
    //批量异化返回
    BatchGrowthReturn = 558,
    //仓库数据返回同步事件
    StoreHouseRefreshInfoData = 559,
    //更新仓库等级刷新
    StoreHouseRefreshLevel = 560,
    //点仓库锁住的格子更新仓库
    StoreHouseUpgradeShow = 561,
    //从仓库中显示装备详情，带操作按钮
    ShowEquipInfoViewFromStore = 562,
    //从仓库中显示道具详情，带操作按钮
    ShowItemInfoViewFromStore = 563,
    //仓库整理返回,暂时不用请求服务��
    StoreBagClearUp = 564,
    //显示弹幕
    ShowBulletScreen = 565,
    //仓库初始化数据更��
    StoreHouseInitInfoData = 566,
    //团队详情
    TBTeamDetail = 567,
    //团本开��
    TuanBenStart = 568,
    //团本准备
    TuanBenReady = 569,
    //更新外观幻化
    UpdateFashionAppearance = 570,
    //打开解锁外观幻化界面
    UnlockFashionAppearance = 571,
    //设置魅力
    UpdateFashionAppearCharm = 572,
    //打开幻化info界面
    ShowFashionAppearanceInfo = 573,
    //团本战斗开��
    RaidFightStart = 574,
    //关闭团本快捷聊天
    CloseRaidFastView = 575,
    //解锁幻化成功
    FashionAppearanceSucc = 576,
    //显示幻化模型并播放动��
    FashionAppearanceShow = 577,
    //刷新团本翻牌
    RefreshRaidFlipCardInfo = 578,
    //显示选择仓库物品
    ShowSelectStoreHouseItem = 579,
    //刷新团本队伍
    UpdateRaidTeam = 580,
    //刷新团本列表
    UpdateRaidList = 581,
    //团本队伍返回
    RaidListRes = 582,
    //被团队邀请通知
    BeInviteRaidNotify = 583,
    //加入团队
    JoinRaid = 584,
    //申请入团通知
    ApplyRaidNotify = 585,
    //刷新时装界面和幻化相关的红点
    UpdateFashionUIAppearanceRed = 586,
    //点击宠物技能按��
    OnClickPetSkillBtn = 587,
    //点击宠物全部属性按��
    OnClickPetAllAttrBtn = 588,
    //显示团本战况
    ShowRaidBattleStatusView = 589,
    //更新团本阶段信息
    UpdateRaidStageData = 590,
    //更新团本阶段剩余奖励次数
    UpdateRaidStageRewardNum = 591,
    //选择团本boss查看详情
    SelectRaidBattleStatusBoss = 592,
    //隐藏团本boss查看详情
    HideRaidBattleStatusDetails = 593,
    //显示阶段结算
    ShowRaidBattleFinishView = 594,
    //显示团本翻牌
    ShowRaidFlipCardView = 595,
    //通知宠物装备背包刷新
    UpdatePetEquipBagView = 596,
    //显示宠物普通装备info窗口
    ShowPetEquipInfoView = 597,
    //显示宠物核心装备info窗口
    ShowPetCoreEquipInfoView = 598,
    //离开团队
    OutRaid = 599,
    //刷新宠物装备显示
    UpdatePetEquipShow = 600,
    //战斗fps过低
    BattleFpsLow = 601,
    //刷新附魔卡捐��
    RefreshEnchantDonate = 602,
    //刷新附魔卡捐赠消��
    EnchantDonateResNet = 603,
    //刷新宠物回收界面宠物列表
    UpdateAfterDecomposePet = 604,
    //隐藏界面中的宠物模型
    HidePetModel = 605,
    //显示活动副本详情
    ShowActivityDetailsView = 606,
    //刷新活动副本详情数据
    RefreshActivityFBDeatilsData = 607,
    //设置活动副本难度
    SetActivityFBDifficulty = 608,
    //点击珠宝镶嵌部位
    UpdateGridPosJewelryData = 609,
    //打开选取珠宝界面
    ShowSelectJewelryView = 610,
    //选择珠宝
    SelectJewelry = 611,
    //打开珠宝升级选取界面
    ShowSelectJewelryLevelUpView = 612,
    //打开珠宝替换界面
    ShowJewelryChangeView = 613,
    //刷新镶嵌珠宝部位
    UpdateJewelryGridPosData = 614,
    //刷新珠宝升级界面
    UpdateJewelryLevelUpPanel = 616,
    //显示通用掉落展示
    ShowComoDropTipView = 617,
    //打开珠宝信息界面
    ShowJewelryInfoView = 618,
    //珠宝分解返回
    UpdateJewelryDecomposeFashion = 619,
    //进入宠物图鉴界面时按宠物配置id选中
    PetAtlasViewSelectPetById = 620,
    //查询团队返回
    SerchRaidRes = 621,
    //团本状态开��
    GroupPlayOpenStatus = 622,
    //更新当前聊天频道的消��
    UpdateNowChatChannel = 623,
    //装备重铸服务端返��
    OnEquipRecastRsp = 624,
    //打开神器养成界面
    ShowArtifactMakeView = 625,
    //刷新神谕装备刻印属性解封升级界��
    UpdateArtifactChangePanel = 626,
    //显示战斗界面系统tip
    ShowBattleSysMechTip = 627,
    //隐藏战斗界面系统tip
    HideBattleSysMechTip = 628,
    //申请入团返回
    ReqJoinRaidRes = 629,
    //客户端组合消息用于滚��
    SplicingMsg = 630,
    //战斗品质tip
    BattleToggleShow = 631,
    //打开附魔卡绘制界��
    ShowEnchantSelectView = 632,
    //开始自动淬炼异化券
    StartTicketAutoUpgrade = 633,
    //显示快捷购买界面
    ShowQuickBuyView = 634,
    //团队邀请玩家列��
    RaidInviteView = 635,
    //更新通组件输入数��
    UpdateCommInput = 636,
    //显示pvp加入房间界面
    ShowPvpRoomAddView = 637,
    //刷新pk周积��
    RefreshWeekPkScore = 638,
    //显示pvp界面
    ShowPvpRoomView = 639,
    //获取集字活动数据成功
    OnGetCollectWordInfoSuccess = 640,
    //打开时装制作合成界面
    ShowFashionMakeView = 641,
    //显示星光璀璨时装属性窗��
    ShowStarlitFashionAttrsView = 642,
    //购买特惠礼包返回
    BuyDiscountPackageRes = 643,
    //领取特惠礼包累购天数奖励返回
    GetDiscountPackageDailyReward = 644,
    //返回领取特惠礼包累购消耗奖��
    GetDiscountPackageConsumeReward = 645,
    //特惠礼包开启新礼包
    DayPreferentialHaveNewGift = 646,
    //打开快捷购买礼包界面
    ShowCommonGiftBuyPopView = 647,
    //选择秘境关卡
    SelectMysteryRaidItem = 648,
    //显示抽奖预览
    ShowDrawPreviewView = 649,
    //更新新年活动数据
    UpdateNewYearActInfo = 650,
    //显示秘境分享界面
    MysteryRaidShareView = 651,
    //选择奖励池奖励ID
    SelectRewardPoolRewardId = 652,
    //更新尘封铁盒信息
    UpdateIronBoxInfo = 653,
    //通用奖池预览
    CommonPreviewReward = 654,
    //谁在讲话
    SomeOneSpeacking = 655,
    //是否屏蔽其他人语��
    ShieldOtherVoice = 656,
    //显示新年年兽模型
    UpdateNewYearActModel = 657,
    //获取尘封铁盒是否可以开��
    GetIronBoxCanOpen = 658,
    //更新麦克风状��
    UpdateMicState = 659,
    //显示特惠礼包界面
    ShowDayPreferntialView = 660,
    //显示特惠礼包界面特定界面
    ShowDayPreferntialViewPage = 661,
    //显示技能页推荐面板
    ShowSkillViewRecmd = 662,
    //攻击年兽
    AttackZodiac = 663,
    //显示年兽攻击界面
    ShowYearMonsterFightView = 664,
    //打开上一次打开的主线副本详情界��
    OverMainViewBackToFB = 665,
    //显示技能插槽空tip
    ShowSkillPointTipView = 666,
    //刷新角色名字
    UpdatePlayerName = 667,
    //更新玩家境界共鸣信息
    UpdateResonanceInfo = 668,
    //更新回收选择
    RefreshRecycleSelect = 669,
    //商店回购返回
    GetShopRedInfoResNet = 670,
    //商店回购返回
    ShopBuyBackResNet = 671,
    //显示福利活动本地活动界面
    ShowLocalActivityView = 672,
    //网络检测返��
    DetectNetworkRspNet = 673,
    //网络DNS检测返��
    DetectBattleNetworkRspNet = 674,
    //仓库多��
    StoreMultiSelect = 675,
    //打开幻境心核信息界面
    ShowUnchartedCoreInfoView = 676,
    //打开幻境星匙信息界面
    ShowUnchartedKeyInfoView = 677,
    //打开幻境逆幻石信息界��
    ShowUnchartedStoneInfoView = 678,
    //打开幻境道具分解界面
    ShowUnchartedDecomposeView = 679,
    //心核镶嵌返回
    CoreCastingBack = 680,
    //心核镶嵌选择
    CoreCastingSelect = 681,
    //显示心核背包
    ShowCoreBackage = 682,
    //刷新大秘境数��
    RefreshMysteryInfo = 683,
    //大秘境玩法信��
    RefreshMysteryGetInfo = 684,
    //幻匙 重铸/合成打开窗口
    UnchartedKeyOp = 685,
    //幻匙 重铸服务端返��
    SCUnchartedKeyRecastRsp = 687,
    //幻匙 合成服务端返��
    SCUnchartedKeySynthesisRsp = 688,
    //显示心核镶嵌界面
    ShowUnchartedCoreCasting = 689,
    //显示心核融合界面
    ShowUnchartedCoreCompose = 690,
    //打开幻境道具信息界面
    ShowUnchartedItemInfoView = 691,
    //幻石 合成/重塑打开窗口
    UnchartedStoneOp = 692,
    //幻石 合成服务端返��
    SCUnchartedStoneSynthesisRsp = 693,
    //新增加道具通知
    AddItemNotify = 694,
    //幻石 重塑服务端返��
    SCUnchartedStoneRestoreRsp = 695,
    //逆幻石穿戴返��
    UnchartedStoneInlayReturn = 696,
    //刷新活动奖励领取
    RefreshActGetReward = 697,
    //交易��> 购买
    BourseBuy = 698,
    //交易��> 寄售
    BourseSell = 699,
    //交易��> 兑换
    BourseExchange = 700,
    //显示周四狂欢��
    ShowThursdayPartyView = 701,
    //交易��> 个人寄售信息
    ShowBourseLog = 702,
    //显示排位赛界��
    ShowQualifyingView = 703,
    //排位赛匹配返��
    QualifyingStartMatch = 704,
    //显示排位赛界��
    ShowQualifyingPanel = 705,
    //取消匹配排位��
    QualifyingCancelMatch = 706,
    //开始修��
    StartPractice = 707,
    //卸载符文��
    UnloadAllRune = 708,
    //启用符文��
    EnableRunePage = 709,
    //更换符文
    ChangeRune = 710,
    //对战记录服务器返��
    QualifyingLogRsp = 711,
    //排位赛排行榜服务器返��
    QualifyingRankRsp = 712,
    //排位赛宝库个人信息更��
    QualifyingTreasureRsp = 713,
    //刷新端午副本详情
    RefreshDragonBoatFBDeatilsData = 714,
    //打开符文道具信息界面
    ShowRuneItemInfoView = 715,
    //打开幻境道具分解界面
    ShowRuneDecomposeView = 716,
    //显示活动遮罩
    ShowActivityMask = 717,
    //幻化 改造信��返回
    GetAppearanceReformInfoRsp = 718,
    //幻化 改造配件解��返回
    AppearanceReformPartUnlockRsp = 719,
    //幻化 改造预设槽位解��返回
    AppearanceReformPresetUnlockRsp = 720,
    //幻化 改造预设信息修��返回
    AppearanceReformPresetSaveRsp = 721,
    //幻化 改造预设信息使��返回
    AppearanceReformPresetUseRsp = 722,
    //执行当前活动队列
    PopContentActivityQueue = 723,
    //遮照关闭
    MaskViewHide = 724,
    //刷新夏日塔罗牌翻牌信��
    UpdateSummerTarotCard = 725,
    //关闭领取奖励弹窗
    HideGetRewardPopView = 726,
    //夏日活动翻牌完毕
    SummerTarotCardFlipOver = 727,
    //打开魅力等级界面
    ShowCharmLevelView = 728,
    //打开角色详情界面
    LookPlayerInfoViewShow = 729,
    //显示夏日塔罗牌奖励界��
    ShowSummerActivityRewardView = 730,
    //魂晶觉醒
    PetEquipAwakenRspNet = 731,
    //更新名人堂数��
    UpdateHallOfFameData = 732,
    //显示名人堂排名数��
    ShowHallOfFameRankList = 733,
    //打开异化共鸣界面
    ShowGrowthResonanceView = 734,
    //打开夏日活动副本界面
    ShowSummerFBView = 735,
    //显示夏日副本详情界面
    ShowSummerFBDetailsView = 736,
    //更新夏日副本详情界面
    UpdateSummerFBDetailsView = 737,
    //刷新觉醒消耗图��
    RefreshAwakeConsume = 738,
    //选择夏日副本幕间
    SetSummerFBDifficulty = 739,
    //更新宠物界面魂晶觉醒红点
    RefreshCoreAwakeRed = 740,
    //更新主城界面宠物图标红点
    RefreshMainCityPetRed = 741,
    //重置夏日塔罗牌界��
    ResetSummerTarotCard = 742,
    //装备打造结果返��
    EquipForgeRsp = 743,
    //6人团本通知其他小队当前房间信息
    NotifyBattleProgressNet = 744,
    //刷新6人段位二阶段充能特效显示
    RefreshCrisisBattleStatusEffect = 745,
    //刷新主界面活动图��
    UpdateMainActivityIcon = 746,
    //选择的小团本节点
    SelectCrisisBattleStatusItem = 747,
    //刷新核心能量宝箱领取
    UpdateGroupCoreEnergyReward = 748,
    //刷新对撞机选择道具
    RefreshDissimilationColliderSelectItem = 749,
    //打开对撞机选择道具界面
    ShowDissimilationColliderSelectItemView = 750,
    //刷新对撞机界��
    UpdateActivityCollider = 751,
    //打开周年战令界面
    ShowAnniversaryPassView = 752,
    //巅峰对决主界面红点刷��
    UpdatePeakFightRed = 753,
    //巅峰对决匹配准备通知
    PeakFightReadyUpdate = 754,
    //巅峰对决队伍位置改变通知
    PeakFightTeamUpdate = 755,
    //关闭异化、强化成功率界面
    ClosePorbPanel = 756,
    //巅峰对决奖励领取
    PeakFightGetReward = 756,
    //刷新周年庆副本详��
    RefreshAnniversaryFBDeatilsData = 757,
    //刷新周年令红��
    UpdateAnniversaryPassRed = 758,
    //SNK时装合成结果返回
    SNKFashionMakeRsp = 759,
    //SNK时装合成次数信息返回
    SNKFashionsCountRsp = 760,
    //刷新SNK任务数据
    UpdateSNKTaskView = 761,
    //刷新SNK任务红点数据
    SNKTaskRedUpdate = 762,
    //刷新拳皇擂台选择英雄
    UpdateArenaSelectClick = 763,
    //确定选择英雄返回
    ArenaRankSelectHeroRspNet = 764,
    //选择出场顺序返回
    ArenaRankSetOrderReturn = 765,
    //打开拳皇擂台英雄详情
    ShowArenaHeroInfo = 766,
    //刷新胜场、奖励领取信��
    UpdateArenaRewardInfo = 767,
    //拳皇擂台排行榜返��
    ArenaRankInfoReturn = 768,
    //拳皇擂台排行榜返��
    ShowSNKActivityView = 769,
    //3v3巅峰对决 任务奖励返回
    PeakFightTaskRewardReturn = 770,
    //显示创角界面
    ShowCreateJobView = 771,
    //显示创角赠送职��
    ShowNewJobPreviewView = 772,
    //显示丰收种植花费界面
    ShowHarvestPlantCostView = 773,
    //丰收节种植数��
    HarvestFarmInfoUpdate = 774,
    //显示丰收节一键播种界��
    ShowHarvestPlantSowView = 775,
    //显示丰收节一键收获界��
    ShowHarvestPlantReapView = 776,
    //刷新丰收节种植红点数��
    HarvestPlantRedUpdate = 777,
    //刷新丰收节种植种子选中状��
    HarvestPlantAreaSelectItem = 778,
    //刷新丰收节单个种子种��
    HarvestPlantSinglePlant = 779,
    //显示丰收节活动界��
    ShowHarvestFestivalView = 780,
    //刷新高级月卡红点数据
    AdvancedMonthCardRedRedUpdate = 781,
    //显示万圣节活动界��
    ShowHalloweenView = 782,
    //刷新万圣节砸蛋活动砸蛋特效结��
    RefreshSmashEggsEffectFinish = 783,
    //显示砸蛋活动金蛋奖励界面
    ShowSmashEggsRewardView = 784,
    //星源铸造结果返��
    StarForgeRsp = 785,
    //显示月卡权益激活提示界��
    ShowEquityActivateTipsView = 786,
    //显示斗罗组队伙伴列表
    ShowSoulLandTeamListView = 787,
    //显示斗罗副本界面
    ShowSoulLandFBView = 788,
    //选择斗罗伙伴
    SelectSoulLandTeamListItem = 789,
    //斗罗副本大界��
    SoulLandFbViewBig = 790,
    //显示斗罗副本详情界面
    ShowSoulLandFBDetailsView = 791,
    //斗罗 > 魂师 > 封号升级
    DouroTitleLevelUped = 792,
    //扭蛋活动数据更新
    GashaponInfoUpdate = 793,
    //刷新扭蛋活动红点数据
    GashaponRedUpdate = 794,
    //点击魂环列表头像
    ClickDouroSoulRoleItem = 795,
    //点击魂环打开魂环升级列表
    ShowDouroSoulRingView = 796,
    //打开魂环升级成功界面
    ShowDouroSoulRingUpView = 797,
    //点击魂环升级界面魂环
    ClickDouroSoulRingUpItem = 798,
    //显示解锁新伙伴弹��
    ShowSoulLandNewPratner = 799,
    //刷新活动站令数据
    RefreshActivityWarOrderInfo = 800,
    //刷新魂环数据
    UpdateDouroSoulRing = 801,
    //打开斗罗背包界面
    ShowDouroBackpackView = 802,
    //打开斗罗武魂界面
    ShowDouroSoulView = 803,
    //打开斗罗武魂信息界面
    ShowDouroSoulInfoView = 804,
    //显示任务开启新功能
    ShowTaskFunctionOpenView = 805,
    //回归礼通知
    PlayerReturnGiftNotify = 806,
    //打开每日收益提示面板
    ShowDouroEarningsTips = 807,
    //回归礼第一次获取通知
    PlayerReturnGiftFirst = 808,
    //打开斗罗奖励获得界面
    ShowDouroShowRewardView = 809,
    //打开道具使用滑动条面��
    ShowUseItemSliderView = 810,
    //添加提炼材料
    AddDouroRefineryItem = 811,
    //斗罗精炼返回
    DouroRefineryReturn = 812,
    //刷新每日收益红点
    UpdateEarningsNew = 813,
    //显示捣蛋活动抽奖特效
    ShowGashaponDrawEffect = 814,
    //添加页面队列
    AddViewQueue = 815,
    //显示盟会详情界面
    ShowGuildDetailsView = 816,
    //点击魂师界面 伙伴头像
    ClickDouroRole = 817,
    //刷新队伍副本状��
    RefreshFBTeamNotice = 818,
    //手动重置夏日塔罗牌界��
    ManualResetSummerTarotCard = 819,
    //打开年兽副本界面
    ShowNianMonsterFBView = 820,
    //新年红包h5活动按钮
    ShowNewYearRedPacketOpenURL = 821,
    //新春排行榜数据刷��
    NewYearRankListRefresh = 822,
    //刷新高级深渊副本难度选择
    RefreshAlienAbyssFBDeatilsData = 823,
    //获取高级深渊副本关卡Id
    GetActAbyssId = 824,
    //刷新高级深渊副本界面
    UpdateActAbyssConsume = 825,
    //刷新体力挑战
    UpdatePhyReward = 826,
    //打开春季活动副本界面
    ShowSpringFBDetailsView = 827,
    //打开游园会活动副本界��
    ShowGardenPartyFbView = 828,
    //显示游园会活动副本详��
    ShowGardenPartyFBDetails = 829,
    //选择游魂之塔领取道具
    SelectGhostTowerItem = 830,
    //显示游魂之塔道具详情
    ShowGhostTowerItemInfoView = 831,
    //装备共鸣消耗返��
    EquipSympathizeConsumeRsp = 832,
    //装备共鸣升级返回
    EquipSympathizeLevelUpRsp = 833,
    //打开巅峰装备封存界面
    ShowParagonTipsView = 834,
    //刷新游园会副本爬塔数��
    UpdateGardenPartyFBInfo = 835,
    //刷新异界关卡列表难度
    UpdateSpaceRiftAbyssDif = 836,
    //显示选择放入门票
    ShowSpaceRiftAbyssSelectItemView = 837,
    //刷新异界深渊显示
    UpdateSpaceRiftAbyssListPanel = 838,
    //刷新游园会副本爬塔首通奖��
    UpdateGardenPartyFBFirstRewardInfo = 839,
    //打开巅峰装备Tips
    ShowParagonEquipTipsView = 840,
    //刷新巅峰装备
    UpdateParagonInfo = 841,
    //显示砸蛋活动奖励获得记录
    ShowSmashEggsRewardRecord = 842,
    //获取游魂之塔数据
    GetWanderingSoulTowerInfo = 843,
    //兑换游魂之塔宝箱物品
    UpdateWanderingSoulTowerExchangeItem = 844,
    //打开查看玩家信息巅峰等级界面
    ShowParagonPlayerInfoView = 845,
    //装备属性比较，获取途径
    ShowEquipViewGetPath = 846,
    //刷新基金数据
    UpdateFundInfo = 847,
    //刷新基金奖励数据
    UpdateRewardFundInfo = 848,
    //打开金装魔盒制作充能界面
    ShowGoldMagicBoxMakePopView = 849,
    //刷新金装魔盒制作充能界面
    UpdateGoldMagicBoxMakePopView = 850,
    //打开制作成功 快捷使用界面
    ShowGetRewardOpenPopView = 851,
    //打开巅峰等级升降界面
    ShowParagonDownTipsView = 852,
    //打开珍贵装备界面
    ShowPreciousEquipTipsView = 853,
    //显示滚屏消息
    ShowScrollingScreenMsg = 854,
    //打开开启宝箱互动特效界��
    ShowItemUseAniView = 855,
    //刷新新功能前瞻奖��
    UpdateNewFunctionProspectReward = 856,
    //播放晨星之塔特效
    PlayWanderingSoulTowerClearEffect = 857,
    //打开精通界��
    ShowEquipProficientView = 858,
    //珍贵装备界面取消选择
    PreciousEquipTipsViewDeselect = 856,
    //打开充能道具选择界面
    ShowItemChargeView = 857,
    //确定充能道具数量
    ItemChargeCount = 858,
    //通知战斗其他信息
    NotifyBattleOtherInfoNet = 859,
    //幻神时装合成结果返回
    PhantomFashionMakeRsp = 860,
    //幻神时装合成次数信息返回
    PhantomFashionsCountRsp = 861,
    //刷新积分状��
    UpdateScoreRewardStatus = 862,
    //打开快捷购买多选消耗界��
    ShowQuickBuyByShopIdsView = 863,
    //打开快捷制作界面
    ShowQuickMakeView = 864,
    //刷新快捷制作
    UpdateQuickMake = 865,
    //打开求助界面
    ShowActivityResortView = 866,
    //打开赠送界��
    ShowActivityGiveView = 867,
    //刷新刮刮乐活动数��
    UpdateScratchTicketActData = 868,
    //刷新刮刮乐活动刮奖数��
    UpdateScratchTicketActRewardData = 869,
    //显示刮刮乐活动奖励界��
    ShowScratchTicketRewardView = 870,
    //跳转到好友联系人
    LinkToFriend = 871,
    //刷新H5活动详情数据
    UpdateSubsidyMonthTurntableInfo = 872,
    //刷新H5活动抽奖数据
    UpdateSubsidyMonthTurntableRewardData = 873,
    //刷新H5活动任务领取奖励数据
    UpdateSubsidyMonthTurntableTaskData = 874,
    //打开h5活动中奖名单界面
    ShowSubsidyMonthTurntableRecordView = 875,
    //进入自定义房��
    EnterCustomRoom = 900,
    //自定义房间列表刷��
    CustomRoomListUpdate = 901,
    //自定义房间信息刷��
    CustomRoomInfoUpdate = 902,
    //自定义房间创��
    CustomRoomSettingView = 903,
    //自定义房间加��
    CustomRoomJoinView = 904,
    //自定义房��v3匹配准备通知
    CustomRoomReadyUpdate = 905,
    //自定义房��v3队伍位置改变通知
    CustomRoomTeamUpdate = 906,
    //装备推荐界面选中
    EquipRecommendItemSelected = 907,
    //属性详情
    ShowPropertyDetail = 908,
    // 充值
    UpdateChargeInfo = 909,
    //变强系统数据
    UpdateStrongerViewDetail = 910,
    //组队邀请Tips数据
    ShowTeamInviteTips = 911,
    //组队准备Tips数据
    UpdateTeamPrepareTips = 912,
    //异化券淬取成功页面
    ShowTicketUpgradeSuccView = 913,
    //异化券制作选择素材
    ShowSelectStuffView = 914,
    //打开捐赠装备提示
    ShowPreciousDonationTipView = 915,
    //打开捐赠装备选择提示
    ShowPreciousDonationSelectTipView = 916,
    //更改聊天面板
    UpdateChatChannel = 917,

    ShowPlayCreateJobView = 920,



    // j2 demo 副本
    ShowJ2FBDetailsView = 888,
    ShowJ2FBRefreshCount = 889,

    // 剧情
    // 剧情状态
    StoryProgress = 930,



    //////////////////////
    //离线战斗结束
    OfflineElite_Finish = 1001,

    ////////////////////-
    //角色等级变更
    LevelUpdate = 1500,

    //////////////////////
    // 更新主城内玩家位��
    City_UpdateCityActorMove = 2000,
    // 主城添加玩家
    City_AddCityActor = 2001,
    // 主城移除玩家
    City_RemoveActor = 2002,
    // 主城显示NPC对话按钮
    City_ShowNPCInteraction = 2003,
    // 主城隐藏NPC对话按钮
    City_HideNPCInteraction = 2004,
    // 点击NPC对话按钮
    City_ClickNPCInteraction = 2005,
    // 更新主城内玩家的模型信息
    City_UpdateCityActorModel = 2006,
    // 更新主城内玩家的称号
    City_UpdateCityActorTitle = 2007,
    // 更新主城内玩家的宠物
    City_UpdateCityActorPet = 2008,
    // 更新主城内玩家的头衔
    City_UpdateCityActorMilitaryRank = 2009,
    // 删除所有city角色
    City_DestroyAllActor = 2010,
    // 更新主城玩家名称
    City_UpdateCityActorName = 2011,

    OpenBadgeLvUpView = 2012,

    FriendChatUpdate = 2013,
    FriendChatUpdateBlack = 2014,
    ShowFashionConfigInfoView = 2015,

    OpenSpaceView = 2016,
    RefreshNewDay = 2017,

    SelectChatShare = 2018,
    ArcadeRankRefresh = 2019,
    Draw_SupremeMagicBox = 2020,
    Draw_SupremeMagicSuccess = 2021,
    Draw_SupremeMagicInfo = 2022,
    Draw_SupremeHouse = 2023,

    GuildAuctionUpdateCollect = 2024,

    GuildAuctionRecord = 2025,

    OpenGuildAuction = 2026,

    HideSpaceTimeVortexBtn = 2027,

    RefreshRaidStatus = 2028,

    CreateRaid = 2029,

    UpdateRaid = 2030,
    LeavaRaid = 2031,

    LookOtherRaidInfo = 2032,
    LookOtherRaidInfoDetail = 2033,
    UpdateRaidApplyList = 2034,

    UpdateRaidInvitelyList = 2035,

    UpdateRaidInviteSelfList = 2036,

    UpdateRaidBattleReady = 2037,

    RaidBattleInTo = 2038,

    RaidGroupPlayInfoUpdate = 2039,

    OpenGuildDetailView = 2040,

    SelectRaidLevelInfoItem = 2041,

    ShowRaidBattleClearingView = 2042,

    ShowSkillBookClick = 2043,

    RaidFlopCard = 2044,

    RaidFlopCardBegin = 2045,
    //////////////////////
    //战斗血量修��
    Battle_HpChange = 3000,
    //战斗中玩家死��
    Battle_HeroDead = 3001,
    //战斗中玩家复��
    Battle_HeroRebirth = 3002,
    //战斗中玩家离��
    Battle_HeroLeave = 3003,
    //战斗玩家中毒
    Battle_HeroBanBuff = 3004,
    //战斗玩家恢复血条颜��
    Battle_ResetHpColor = 3005,
    //玩家添加buff
    Battle_HeroAddBuff = 3006,
    //玩家删除buff
    Battle_HeroRemoveBuff = 3007,
    //预体验技能飞��
    PreSkillFly = 3008,
    //开始战斗场景传��
    BeginBattleSceneTransfer = 3009,
    //结束战斗场景传��
    FinishBattleSceneTransfer = 3010,
    //战斗玩家掉线
    Battle_PlayerOffline = 3011,
    //战斗玩家重连
    Battle_PlayerReconnect = 3012,
    //播放buff消失动画
    Battle_HeroBuffIconPlay = 3013,
    //停止播放buff消失动画
    Battle_HeroBuffIconStop = 3014,
    //刷新潘多��0211初始化数��
    Battle_UpdatePandora30211 = 3015,
    //刷新潘多��0213界面
    Battle_UpdatePandora30213 = 3016,
    //观战列表返回
    BattleObsResp = 3017,
    DeleteRole = 3018,
    // 主界面按钮红��
    MainBtnRed = 3019,
    // 主界面按钮发生修��
    MainBtnChange = 3020,
    MainBtnChangeInPos = 3021,
    MainBtnChangeOutPos = 3022,
    MainBtnDataChange = 3023,
    /// <summary>
    /// 选择商店物品
    /// </summary>
    SelectShopItem = 3024,
    /// <summary>
    /// 打开商店
    /// </summary>
    OpenShop = 3025,
    /// <summary>
    /// 时装礼包购买
    /// </summary>
    FashionGiftOpen = 3026,

    // 好友推荐
    FriendRecommond = 3027,

    ShowBookPetSkill = 3028,

    ShowEquipRecastTipsView = 3029,
    //预体验技能
    PreExperienceSkill = 3030,
    // 战斗界面 - 重置技能
    BattleView_ResetSkills = 3031,
    // 战斗界面 - 物品更新
    BattleView_UpdateItem = 3032,
    // 战斗界面 - 关闭《获得史诗装备》窗口自动隐藏
    BattleView_DisableLegendEquipPanelAutoClose = 3033,
    //玩家添加pandora
    Battle_HeroAddPandora = 3034,
    //玩家删除pandora
    Battle_HeroRemovePandora = 3035,


    //////////////////////

    //app版本控制号不一��
    VcsDiff = 3500,
    //挂机金币掉落
    HangupGoldDrop = 4000,
    //打开挑战精英结算界面
    OpenExploreAwardView = 5000,
    //播放挂机奖励特效
    PlayHangupAwardEffect = 6000,
    //同步服务器时��
    GetServerTime = 7000,

    EquipDecompositionOpen,
    FashionDecompositionOpen,
    // 显示界面，用于SendToOpenVieWithArgs，不需要每个界面都单独定义一个事件
    ShowView,
    // 通过新手引导配置控制显示界面
    ShowGuideView,
    // 高亮显示控件（提高控件层级）
    HighlightControl,
    // 高亮显示控件区域，并显示文本
    HighlightControlRect,
    // 引导点击(手指效果 - GuidePanel)
    // 参数1：引导ID
    // 参数2：一级子界面ID
    // 参数3：二级子界面ID
    // 参数4：界面控件ID
    // 参数5：是否强制引导
    // 参数6：点击回调
    ShowGuidePanel,
    GuideCreateRoleName,
    // 显示或隐藏界面按钮
    ShowOrHideControl,
    // 更新界面
    UpdateView,

    ShowBattleTipsView,
    // 街机副本
    RefreshArcadeType,   // 刷新副本类型
    RefreshArcadeRoleData,  // 刷新副本个人数据
    RefreshArcadeSelectData,    // 刷新选角数据
    // 切换子标��
    SwitchSubTabIndex = 10001,

    //符文
    RefreshRuneSetting,
    SetUseRuneSetting,
    TakeOffRune,
    PutOnRune,
    RefreshRuneType,
    SelectRuneSlot,
    ChangeRuneItem,
    CancelChangeRune,
    SelectChangeRuneItem,
    ShowRunePreviewView,
    //评价系统
    OpenEvaluationView,
    GetEvaluationDetail,
    EvaluateEnterCallback,
    EvaluateSubmitSuccess,
    GetEvaluateHistory,
    UpdateEvaluationBrief,
    //自传播系统
    UpdatePropagationMsg,
    UpdatePropagationRwd,

    ShowStoreHousePanel,
    BackpackChoose,
    UpdateStoreHousePanel,
    UpdateStoreHouseType,
    OnClickGetInStore,
    CheckStoreChoose,
    ShowStoreHouseUseItemTipsView,
    ShowStoreMultiItemsView,

    // 天梯PVP
    RefreshLadderMatchState,   // 匹配状态
    RefreshLadderSeasonRankInfo,    // 赛季信息
    RefreshHistoryRankList, // 历史排行
    RefreshLadderLogRsp,  // 录像数据
    RefreshLadderLogRecordRsp, // 录像回放数据
    RefreshLadderTreasureInfo,  // 荣耀宝藏
    RefreshLadderRankList,    // 排行刷新
    RefreshLadderWatchBattleRsp,  // 观战数据

    ShowLookPlayerSkillInfoPanel,
    LevelUpdateNew,
    ShowPlaySkillVideoView,
    ConfirmFight,
    ShowSkillBiggodListView,
    ShowSkillRecommendView,
    SetSkillBigGodData,
    RefreshRecommendPage,
    RefreshCityName,

    RreshGuildRed,
    // CG相关
    CGEvent = 11001,

    // 新手引导事件
    NewGuideEvent = 11201,
    ShowRepairRadioView,

    OnJoystickTypeChanged,

    OnMainCityViewTouchArea_BeginDrag,
    OnMainCityViewTouchArea_Drag,
    OnMainCityViewTouchArea_EndDrag,
    ShowWeakGuide,
    FBEarthCoreNumRefresh,
    OnGuideProgressRefresh,
    AdaptScreenArea,
    UpDateCodeFuncOpen,
    AwakeSuccess,
    RefreshCustomerService,
    SkillDownNewSkillRefresh,
    PetUpLevelOpen,
    PetGiveUpOpen,
    ShowFightValueUp,
    PetListUpdate,

    BaoDiUpdate,

    OnViewShow,
    OnViewHide,

    SetActivityQueuePopup,

    //显示特殊复活界面
    ShowSpecialRevivePopView,
    SetSpecialRevivePopView,
    ShowBagAction,
    HideBagAction,


    ShowMilitaryRankUpLevelView,
    RefreshMainFbRank,
    ShowEndFightView,
    ShowFrontLineView,
    CancelMatchTeam,
    ShowTeamMatchView,
    ChangeMatchTeamState,
    ExpandList,
    DeleteChatMsg,
    ShowReportView,
    ShowEarthCoreView,
    ShowFashionBuyListView,
    // 队伍开始 11301 ~ 11400
    TeamMemberItemsUpdate = 11301,      // 队伍成员物品信息变更
    TeamEnd = 11400,
    // 队伍结束

    // 电玩城 11401 ~ 11500
    VGameUpdateSelectedItem = 11401,
    VGameCityInfoUpdate = 11402,
    VGameEnd = 11500,
    //个人Boss
    PersonalBossListUpdate,
    // 刷新标签
    UpdatePlayerTag,
    UpdatePlayerTagLabel,
    UpdateEquipAliLevel,
    //添加弹窗广告
    AddPopAdsActivity,
    UpdateAddPopAdsData,
    ShowCharactorInfoView,
    ShowDialogSkipView,
    SkipDialog,
    //玩家反馈
    FeedBackHistoryUpdate,
    FeedBackStatusSummaryUpdate,
    OpenFeedBackSubmitView,
    OpenFeedBackDetailView,
    FeedBackDetailSubmitCallback,
    FeedBackDetailGetRoleHead,
    FeedBackRankDataUpdate,
    FeedBackSubmitSuccess,
    GetPlayerPhoto,
    ShowWearedModelView,
    // 打开子包更新界面
    ShowLateUpdateWaitView,
    ShowSubPackUpdateView,
    SubPackageAllowMetered,
    TransferSelect,
    // 子包分等级下载时间
    SubPackageStatusChanged,        // 分包状态发生变化（分包等级、是否分包状态发生变化）--- 由SubPackageManager发出

    UpdateLoadingConfig,
    GetRandomName,
    ShowCenterActivity,
    UpdateQuestion,
    UpdateServerTime,
    HideQuickPanel,
    ShowGiftBagPanel,
    ShowColdUpdateView,
    ShowCarnivalActivityView,
    //刷新纪念币商城界面
    RefreshCarnivalShopView,
    UpdateCarnivalActivityData,
    EquipSynthesisChoose,
    EquipSynthesisCancel,
    EquipSynthesisRefresh,
    UpdateCouponSale,
    RefreshCouponSale,
    ShowEquipLinkTipsView,
    RefreshCarnivalSpeed,
    RefreshCarnivalShop,
    RefreshCarnivalRecharge,
    RefreshCarnivalTask,
    RefreshCarnivalGift,
    RefreshCarnivalGiftTask,
    CarnivalRechargeTotal,
    ShowWeakGuideClose,
    RefreshCarnivalSignin,
    RefreshMilitaryRank1,
    RefreshMainCItyRedPoint,
    UpdateMainBtnCountDown,
    UpdateLimitedTimePackage,
    UpdateLimitedTimePackages,
    ActivitySignCanival,
    ShowManufacturePanel,
    RefreshMainFbDoubleCard,
    ShowequipEnchantPanel,
    ShowSkillChangeNameView,
    RefreshSkillPageName,
    ShowCommonRewardView,
    ShowVGameDropTipView,//电玩空间掉落单独展示
    ShowDogeGuide,
    RefreshEquipInheritance,// 装备继承
    UpdateInheritanceEquip,// 装备继承
    RefreshEquipReinForce,// 装备继承

    SetCarrierUIActive,//载具设置ui
    SetJoystickGuide,//摇杆引导
    EnterBattleTipsShow,
    RefreshEquipSysGiftBox,
    ChooseEquipSysGiftBox,
    ShowSkillPutOnEffect,
    UpdateNavigateState,
    ShowAbyssDetails,
    //末日深渊
    ShowAbyssNormal,
    ShowEquipSynGiftBoxRwd,
    HideGuidePanel,
    ShowBattleQuestGuide,
    HideEquipSysGiftBox,

    RefreshFarPlaneView,

    OnPatchDownloadStatusChanged,
    RefreshSkillEquity,
    SetEquipScore,
    LavaTowerSelectBoss,
    SetManufactureViewType,
    SwithTab,
    UpdateGuildStoreHouseBag,
    UpdateStoreChooseScore,
    ClearStoreChoose,
    UpdateGuildStoreHouseRecord,
    UpdateGuildStoreHouseScore,
    TotalPayRetUpdate,      // 充值返利状态更新

    BaoDiDropTipView,

    // 网络重连
    NetworkReconnect,
    ShowEquipUpTipView,
    ContinueMakeEquip,

    // 源能争夺战(12000~12100)
    ResourcePVPActivityInfoUpdate = 12000,          // 活动信息发生变化（魁首）
    ResourcePVPGetAttackListUpdate = 12001,         // 获取周围攻击目标信息返回
    ResourcePVPGetWaitListUpdate = 12002,           // 获取周围攻击目标信息返回
    ResourcePVPMyInfoUpdate = 12003,                // 自己的信息发生变化
    ResourcePVPNeedWaitingUpdate = 12004,           // 我的攻击目标等待队列发生变化
    ResourcePVPItemInfoChanged = 12005,             // 物品信息发生变化
    ResourcePVPAutoStartBattleResult = 12006,       // 开始下一场战斗消息回复

    // 源能争夺战--场景(12101~12200)
    SceneRoleBriefChanged = 12101,                  // 玩家简要信息发生变化
    SceneMineInfoChanged = 12102,                   // 资源矿信息同步
    UpdateResPVPMineProcess = 12103,
    UpdateResPVPSelfMineState = 12104,
    UpdateResPVPMineSelfCount = 12105,

    // 交易行
    TradeDataUpdate,        // 交易行数据更新
    TradeItemDataUpdate,    // 交易行单个物品数据更新
    TradeSaleResult,        // 交易行出售结果
    TradeBuyResult,         // 交易行出售结果
    TradeCollectUpdate,     // 交易行收藏数据变动
    TradeMySellListUpdate,  // 我的上架记录数据变动
    TradeBuyPageData,       // 交易行购买记录数据变动
    TradeSellPageData,      // 交易行出售记录数据变动
    TradeNotify,            // 交易行通知

    AbyssBoxOpen,
    //清除缓存
    ClearCache_Setting,
    ClearCache_Login,
    RefreshGetStronger,
    // ClearCache_Login
    UpdateResPVPSelfRoleState,
    UpdateCustomRoomMatchSchedule,
    // 对结算界面广播消息
    BroadcastFinishNotice,
    // 广播验证码
    BroadcastBindPhone,
    BindPhoneTips,
    FreshBindPhone,
    OpenInheritTipView,
    UpdateIsAddWorldExp,
    RefreshInheritTipView,
    OpenResourceReplenishView,
    FightValueUpdate,
    NoticeFightValue,
    //冲刺活动
    SprintActivityView,
    SprintActivitySingleData,
    SCGameDiceRoll,
    SCGameDiceNext,
    FashionPanelNotice,
    FashionPanelAllClear,
    FashionPanelUpdate,
    FashionPanelClear,
    ChooseServerSignDay,
    HideTowerViewBtn,
    ChooseMilitaryRank,
    SetSkinItemCallback,//设置皮肤回调
    RefreshOnePassLevel,
    UpdateResPVPSelfMineCount,
    //PC端DropDown通知
    SettingViewDropdownOpen,
    SettingViewDropdownClose,
    //时装特惠粉色时装
    NewServerFashionSalePink,
    //时装特惠紫色时装
    NewServerFashionSalePurple,

    // --------------- 冒险团 ---------------
    AdventureGroupBaseInfoChanged,      // 基础信息发生变化
    AdventureGroupTaskInfoChanged,      // 任务信息发生变化

    // 团本
    RaidUpdateInviteList,
    UpdateSpaceRiftView,
    /// <summary>
    /// 回归活动任务红点刷新
    /// </summary>
    ReturnActivityRedRefresh,
    /// <summary>
    /// 回归活动任务面板刷新
    /// </summary>
    ReturnActivityTaskPanelRefresh,
    /// <summary>
    /// 回归活动礼包面板刷新
    /// </summary>
    ReturnActivityGiftBagPanelRefresh,
    /// <summary>
    /// 回归活动签到面板刷新
    /// </summary>
    ReturnActivitySignInPanelRefresh,
    /// <summary>
    /// 回归活动资源找回面板刷新
    /// </summary>
    ReturnActivityResourcePanelRefresh,
    /// <summary>
    /// 回归活动资源找回面板刷新经验
    /// </summary>
    ReturnActivityResourcePanelRefreshExp,
    /// <summary>
    /// 回归活动任务红点刷新
    /// </summary>
    ReturnActivityPopBackGift,
    ReturnActivityPopRewardsReclaiming,
    // 装备升星
    EquipRankUpMaterialChanged, // 升星材料变化
    EquipRankUpResult, // 升星材料变化
    // 装备喂养
    EquipFeedResult,    // 装备喂养结束
    EquipFeedUpResult,  // 装备升品结束
    MarqueeViewAddMsg,
    SelectPreciousReward,
    // 预转职
    OnLocalPreJobChanged,

    TiktokActivityRefresh,
    TiktokActivityJumpSuccess,
    FashionBuySendActivityRefresh,
    BattlePowerTargetActivityRefresh,
    CostApActivityRefresh,
    CostApActivityUpdate,
    CostApSpecialAwardsActivityUpdate,
    BindPCActivityRefresh,
    ClickFashionAppearToggle,
    ClickFashionAppearItem,
    TitleInfoCustomRefresh,
    FBWeekScoreRewardUpdate,
    ChooseEmoji,
    UpdateEnergySupplyStatus,
    //临时背包被清空
    TempBackpackRefresh,
    //临时背包结束
    TempBackpackFinish,
    ShowNpcInteractView,
    PlayRadioAudio,
    ShowHudBubbletNextStory,
    HideHudBubbletNextStory,
    ShowSampleDialog,
    //鸿运通奖励轮次
    SupremePreviewRound,
    RefeshPetWashItemBind,
    ShowPetWashResult,
    UpdatePetWashAttr,
    // 对话框
    OnDialogFinished,
    //世界BOSS
    WorldBossTeamRankListUpdated,
    WorldBossRoleRankListUpdated,
    // 任务状态发生变化
    OnQuestStatusChanged,
    //老虎机
    SlotsGoldRefresh,
    SlotsGoldAwardsNotify,
    SlotsGoldGetAwards,
    SlotsGoldChargeNotify,
    ResetToggle,
    //水果机结
    FruitMacResultNotify,
    FruitMacTaskNotify,
    FruitMacChargeNotify,
    ShowBattleTransJobView,
    ShowDropNoticeView,
    ShowEquipSuitPanel,
    OpenTeamListViewByChapterId,
    //冲刺活动现金礼包通知
    SprintActivityChargeRefresh,
    //多买多送活动单独定制的礼包购买
    FashionBuySendGiftOpen,
    //世界Boss相关
    EnterWorldBossPanel,
    ShowWorldBossRewards,
    WorldBossBattleRankUpdate,    //世界BOSS战斗中排行列表刷新
    WorldBossInfoUpdate,    //世界BOSS战斗中Boss状态信息刷新
    WorldBossInfoStart,    //世界BOSS战斗中Boss状态信息开始
    //活跃引导活跃度通知
    WeeklyActivityLiveness,
    //新活动规则说明通知
    NewServerActivityHelp,
    RefreshMonthCardRewards,
    WorldBossRankDetailsRefresh,
    ClearActionEvent,//清除角色事件队列
    RaidChangeGroupRolePosResponse,     // 修改团队成员位置返回
}

public enum E_View_Type
{
    None = 0,
    LoadingView,
    InitView,
    TipsView,
    LoginView,
    ExplainView,
    DropTipView,
    InteractView,

    RegisterView,
    RealNameAuthView,
    SDKTipsView,
    ArticleView,

    ServerNoticeView,   // 公告
    MainCityView,
    MiniMapView,    // 小地图

    EquipTransferPopView,
    BatchGrowthRecordView,
    JobChangeTipsView,
    TransferSuccessView,
    ChatView,
    FriendView,

    LinkPackageView,

    GetEnergyPopView,
    BourseExchangeView,
    ChapterRewardView,
    TeamView,
    MainFBView,
    AbyssFBView,
    SpaceTimeVortexFBView,
    PlaneDreamlandView,

    J2FBDetailsView,
    PvpEnterHomeView,
    ChallengeFbView,
    RaidView,
    SpaceRiftPopupView,

    ExchangeMallView,

    PrimevalTreasuryView,

    FarplaneFBDetailsView,

    NetBlockView,

    SystemMessageView,
    NetConnectView,
    QuestView,

    SignInView,
    FirstChargeGiftBagView,
    GetRewardPopView,
    NewServerActivityView,
    NewServerTargetView,
    SkillView,
    EvaluateView,
    EvaluateSubmitView,
    AccountDelView,
    ShareSystemLinkView,
    ShareSystemInputView,
    CombatOperationsView,
    SkillContentView,
    SelectPlayerView,
    SelectJobGroupView,
    SelectJobGroupView2,
    SelectJobGroupNewView,
    PlayerUnLockView,
    CreateJobView,
    LateUpdateWaitView,
    SubPackUpdateView,
    SubPackLevelUpdateView,
    DeletePlayerView,
    SystemMsgView,
    MaskView,

    HonourPassOpenView,
    HonourPassView,
    MailView,
    UpgradeView,
    BossFBView,
    QualifyingView,
    PeakFightView,
    ArenaView,
    TowerView,
    ChallengeView,
    CustomRoomView,
    GardenPartyFbView,
    AppStoreRatingView,
    MysteryRaidView,
    GMButtonView,
    GMView,
    PerformanceMenuView,
    BackpackView,
    RuneDecomposeView,
    UnchartedStoneAttrView,

    ItemTipsView,
    ItemInfoView,
    CoinInfoView,
    MainBtnSetView,
    EquipSlotRefineView,
    EquipProficientAttrView,
    EquipCompareView,
    RecoverView,
    ResonanceView,
    GrowthResonanceView,
    RankView,
    WorldBossRankRewardView,
    EnchantPopView,
    DarkMazeView,
    OneKeyDecomposeView,
    EnchantMixCardRewardView,

    DialogView,
    OneKeyEnchantMixView,
    EnchantInfoView,
    GiftInfoView,
    JewelryInfoView,
    UnchartedKeyInfoView,
    UnchartedStoneInfoView,
    UnchartedCoreInfoView,
    RuneInfoView,
    DouroItemInfoView,
    TitleInfoView,
    SkinInfoView,
    FashionInfoView,
    UseItemTipsView,
    EquipDecomposeView,
    ManufactureView,
    EquipAutoReinforceView,
    EquipGrowthPopView,
    BatchGrowthView,
    EquipRecommendView,

    MagickBoxMakeSelectView,
    EquipTransferHintView,

    StoreHouseView,
    StarMysteryView,
    ProlocutorActivityView,
    MagicBoxView,
    [Obsolete("Use PlaneDreamRankView instead", true)]
    PlaneDreamlandRankView,
    UnchartedDevelopView,
    BourseView,
    EnergyCoreView,
    EquipForgeView,
    ChallengeEnterView,
    NianMonsterFBView,
    MainFBDetailsView,
    MainFBDetailsBossView,
    ItemSellView,
    ItemUseView,
    StoreHouseTransformTipView,
    UseRenameCardView,

    MilitaryRankView,   // 头衔
    TransferPreviewView, // 转职预览
    TransformJobView,//转职重写
    TransformJobViewB,//B版本转职重写
    TransformPopView,//确认转职页面
    TransformIllustrationView,

    WelfareView,//福利页面
    RetrievePopView,//经验找回子页
    GiftBagPanel,//等级福利

    ShopView,// 商店
    PetFeedView,
    PetEquipInfoView,
    FashionIntegrationView,
    QuickMakeView,
    QuickBuyByShopIdsView,
    AcquisitionPathView,    // 物品途径
    AutoDecomposeSetView,   // 自动分解设置
    DecomposeEquipView,     // 装备分解选择
    GuildListView,
    GuildCreatView,
    GuildDetailView,
    GuildAuctionView,
    GuildSignView,
    GuildPowerView,
    GuildManagerView,
    MagicBoxMergeView,
    //深渊战令
    WarTokenView,
    //普通深渊
    AbyssNormalView,
    SupremeMagicboxView,
    RewardPreviewView,
    GuildCreatTipView,
    GuildUpgradeView,
    GuildShowAttrView,
    OptionalGiftView,
    ItemUseAniView,
    TotalPayRetView,

    PetView,    // 宠物
    PetRecycleView, // 宠物回收
    PetInfoView,    // 宠物信息界面
    PetCoreEquipInfoView,
    PetAtalsView,
    PetSkillDescView,
    FashionGiftBuyPopView,
    FashionBuySendPopView,
    FashionMixView,
    FashionDecomposeView,
    FashionInlayView,
    AbyssFbListView,
    SpaceRiftAbyssSelectItemView,
    SpaceDistortionView,
    AbyssFBDetailsView,

    MainFBNoticeView,   // 主副��
    BattleView,         // 战斗页面
    BattleTipsView,     // 战斗提示界面
    HangupView,
    BattleFogView,
    TimeLineBgView,

    RevivePopView,
    FashionBadgeUpgradeView,
    MainFBFinishView,   // 主副本完成
    BossFBFinishView,
    TowerFinishView,
    FarplaneFinishView,
    GuildBossFinishView,
    LadderFinishView,
    PVPRoomFinishView,
    PeakFightFinishView,
    PromotionView,
    ArenaFinishView,
    QualifyingLoadingView,
    CustomRoom1v1LoadingView,
    PeakFightLoadingView,
    CustomRoom3v3LoadingView,
    ArenaLoadingView,
    TeamBattleLoadingView,

    SpecialFlipCardView,
    SpaceTimeVortexCardView,
    AreaTestCardView,
    AreaTestCardDefaultView,
    FlipCardView,
    SignInTipView,
    BattleSysMechTipView,
    FeedbackView,
    RaidFastChatView,
    BattleToggleSettingView,
    PracticeSetView,
    PracticeHitCountView,
    BulletScreenView,
    FightStatsView,
    PhantomFashionAttributesView,
    FashionAttrView,
    LookPlayerinfoView,
    LookPlayPropertyView,
    ChangeNameView,
    ChangeNameNewView,
    GuildTreasuryView,
    GuildRecordView,
    TransformSuccessView,//转职成功动画

    MonthCardRewardsView,//月卡奖励
    MonthCardView,
    MonthCardDealView,
    MonthCardTipView,
    TeamInviteView,
    InviteTeamupView,
    TeamInviteTips,
    TeamPrepareTips,
    PvpRoomView,
    TeamSettingView,

    BeastTideView,
    BeastTideFinishView,

    PlaceHolder___001,// GuideJumpView
    SubmitItemView,
    DayGiftView,
    DayGiftBuyView,
    DayGiftDiscountView,
    FashionIllusionUnlockView,
    AddFriendView,
    FriendTipView,
    FunctionPreviewView,
    FollowGiftView,
    ShopGiftBuyPopView, // 时装商城礼包购买
    FashionGiftInfoView,    // 时装信息
    SettingView,
    FashionBuyPopView,      // 时装部件购买
    StrongerView,
    GuideView,
    PowerTipsView,
    TeamListView,
    TeamMemberView,
    TeamRecruitView,
    TeamInfoView,
    FashionDecomposeSureView,
    FashionRenewView,
    ActivityBuyView,
    FashionChangeBadgeView,
    ResetAttrPanel,
    EnchantMixPreviewView,
    PlayCreateVideoView,
    HandBookView,
    ActivityCenterView,

    // 街机副本
    ArcadeMainView,
    ArcadeFinishView,
    ArcadeMapView,
    ArcadePassView,
    ArcadeRankView,
    ArcadeSelectRoleView,
    ArcadeRewardView,
    ChatShareChannelPanel,


    GrimoireUpTipView,
    ComposeView,
    TicketUpgradeSuccView,
    TicketAutoUpgradeView,
    TicketAutoUpgradeFinishView,
    SelectStuffView,


    SupremeRewardPopView,
    SupremeDrawInfoView,
    SupremePreviewView,
    StoreHouseUseItemTipsView,
    StoreMultiItemsView,
    SupremeWarehouseView,
    RankActivityView,

    //众神宝藏
    PreciousDepositsView,
    DonationSelectView,
    DonateGetRewardView,
    FragmentUpView,
    DonationRecordView,
    StorehouseDropView,
    PreciousDonationTipView,
    PreciousDonationSelectTipView,
    PreciousDepositsTipsView,

    DayPreferentialView,
    DayPreferentialPreviewView,
    SpaceTimeVortexView,

    AdvancedMonthCardView,
    AdvancedMonthCardBuyView,
    EquityActivateTipsView,
    PeakFightReadyView,
    MatchTipsView,

    // 天梯PVP
    LadderMainView,
    LadderMapView,
    LadderPassView,
    LadderRankView,
    LadderSelectRoleView,
    LadderRewardView,
    LadderHonorView,
    LadderLogView,
    LadderTreasureView,
    LadderLoadingView,
    LadderSeasonResultView,
    LadderWatchBattleView,


    GuildBossMainView,
    GuildBossBuyGainView,
    LookPlayerSkillInfoPanel,

    GuildDamageRankView,
    GuildSelfDropView,
    GuildDistributionRewardsView,
    TechnologyPanel,


    //自定義
    CustomRoomEnterView,
    CustomRoomReadyView,
    CustomRoomSettingView,
    CustomRoomJoinView,
    CustomRoom1v1FinishView,
    AccumulatePayBuyView,
    GuildBonusView,
    GuildAuctionRecordView,
    GuildMyAuctionRecordView,
    GuildAuctionTipView,
    PlaySkillVideoView,
    SkillTipView,
    RssTipView,
    GuideTipView,
    CreateCrisisView,
    SkillBiggodListView,
    SkillRecommendView,
    RaidDevilHomeView,
    RaidSetView,
    RaidDevilListView,
    RaidLookTeamView,
    EquipRecastDescView,
    EquipRecastView,
    EquipRecastTipsView,
    ArtifactMakeView,
    RaidApplyListView,
    RaidInviteView,
    RaidPlayerApplyView,
    RaidTeamView,
    ItemBuyView,
    ItemExchangeView,
    SynthesisItemExchangeView,
    RaidBattleStatusView,
    // 时空试炼商人界面
    SpaceExploreActView,
    HarvestFestivalView,
    ThursdayPartyView,
    FirstChargeView,
    ActivityView,

    GuideLoadingView,
    DemoSelectRoleView,
    GuideEffectView,

    RaidBattleClearingView,
    BloodView,
    RepairRadioView,
    GuideChapterStoryView,
    GuideChapterSpineView,
    RaidFinishView,
    RaidFlipCardView,
    FullScreenVideoView,

    RoleStoryView,
    SetAreaView,
    FourWeaponView,
    CustomerServiceView,
    PetGiveUpView,
    PetUpLevelView,
    FightValueUpView,
    EquipAlienationView,
    MilitaryRankUpLevelView,
    CreateRoleNameView,
    MainFBEndFightView,
    SpaceRiftFrontLineView,
    NewFinishView,
    TeamMatchView,
    AgeReadyView,
    ReportView,

    RunePreviewView,
    ThankPlayerView,// 玩家感谢名单

    // 须弥幻境（方尼电玩城）
    // 开始到结束：501~550
    PlaneDreamTreasureView = 501,       // 必带飞宝库
    PlaneDreamRankView = 502,           // 逗趣玩排行
    VGameMainView = 506,                // 电玩空间
    VGameChangeItemView = 507,          // 更换光盘
    VGameLevelUpView = 508,             // 层数升级
    VGameRandIntroduceView = 509,             // 词缀详情
    PlaneDreamTreasureEndFlag = 550,
    PlayerTagsView,
    EquipAliShowView,
    QuestionnaireView,   // 问卷调查界面
    PopAdsView,  //弹窗广告界面
    ActivityServerDropPanel,//全服掉落

    EarthCoreView,                 //地核深渊
    FashionBuyListView,
    CharactorInfoView,
    BlackDialogueView,          // 黑屏对白界面
    DialogSkipView,

    FeedBackView,
    FeedBackSubmitView,
    FeedBackDetailView,
    WearedModelView,

    FloatIntroduceView,
    FloatItemBriefView,

    RoleIntroduceView,
    GuideTaskFinishView,
    ItemAndPowerTipsView,
    JieJiShowView,
    BlindView,
    BattleOperationGuideView,
    EquipLinkTipsView,
    ColdUpdateView,
    EquipEnhanceView,
    EquipEnhance1View,
    // 嘉年华主界面
    CarnivalActivityView,
    ShowEquipLinkTipsView,
    RefreshRewardPreviewPanel,
    LimitedTimePackageView,
    UseDoubleItemView,
    //流派指引
    GenreGuidelinesView,
    CommonRewardView,
    UseItemConfirmTipsView,
    EquipInheritanceView,
    EnterBattlePowerView,
    AbyssDetailsView,
    EquipSysGiftBoxView,
    QuestRewardView,
    LavaTowerView,
    GuildStoreManageView,
    ItemsConfirmView,
    EquipUpTipView,
    EquipCanMakeBetterTipView,

    ResourcePVPCityView,
    ResourcePVPInteractView,
    ResourcePVPFinishView,
    ResourcePVP1v1LoadingView,

    AbyssEffectView,
    //清除缓存
    ClearCacheView,
    // 我要变强界面
    GetStrongerView,
    CustomRoomMatchActivityView,
    CustomRoomMatchDetailView,
    CustomRoomMatchScheduleView,
    BindPhoneView,
    BindPhoneTipsView,
    InheritTipView,
    EquipLinkWearView,
    ResourceFbView,
    QuickBuyTimesView,
    ResourceReplenishView,
    EquipFbView,
    WorldBossView,
    SprintActivityView,
    SlotsGoldView,
    DiceGameView,
    TradeView,
    TradeInputView,
    TradeSearchView,
    TradeBuyPopupView,
    TradeSalePopupView,
    TradeEnsureView,
    SkinStoreView,
    ReturnActivityView,
    BuyTipPanel,
    PopReturnActivityRewardsReclaiming,
    PopReturnActivityWelcomeBackGift,

    EquipRankView,
    EquipRankSelectView,
    EquipRankUpResultView,
    EquipFeedResultView,
    EquipFeedUpResultView,
    MarqueeView,
    AdventureGroupView,
    PreciousRecordView,
    PopMonthCardBuyView,
    FashionSettingView,
    FashionAppearView,
    TitleInfoCustomView,
    FashionCharmLvUpView,
    FashionCharmInfoView,
    EnergySupplyConfirmView,
    ChapterStoryView,
    SampleDialogView,
    HighlightAreaInfoView,
    BlackTransitionView,
    //临时背包
    TempBackpackView,
    PetWashTipView,
    PetWashResultView,

    //个人boss关注列表
    PersonalBossListView,
    ExperienceSkillView,
    BattleTransChooseJobView,
    BattleTransJobView,
    //货币兑换
    ActivityCurrencyExchangeView,
    DropNoticeView,
    ActivityCostApKefuView,

    UISurveyView,

    PersonalBossGuideTips1View,
    PersonalBossGuideTips2View,
    WorldBossFinishView,
    WorldBossRewardView,
}

// 加载类型
public enum E_LoadingType
{
    // 战斗加载
    Battle = 0,
    // 普通加��
    Loading = 1,
    //主城加载
    GameCity = 2,
}

// 界面层级
public enum VIEW_LAYER
{

    // 默认
    Bottom0 = 0,
    Bottom = 1,
    Default = 2,
    //中间

    Layer1 = 3,
    Layer2 = 4,
    Layer3 = 5,
    Layer4 = 6,
    //弹出

    Pop = 7,
    Pop2 = 8,
    Top = 9,
    Top2 = 10,
    Tip = 11,
    //引导

    Guide = 12,
    GuideTip = 13,
    GuideJump = 14,
    Loading = 15,
    //NetBlock

    NetBlock = 16,
    //置顶

    Mode = 17,
    //只有InitView界面使用

    Init = 18,

    // SDK界面
    SDK = 19,
}

public class RES_PATH
{
    public static string PLATFORM_CONFIG = "PlatformConfig";
    public static string CLIENT_CONFIG = "ClientConfig";

    public static string CONFIG = "Config";
    public static string CONFIG_ROOM = "Config/LevelRoom";
    public static string CONFIG_SCENE = "Config/LevelScene";
    public static string CONFIG_SKILL = "Config/SkillAction";
    public static string CONFIG_EQUIP = "Config/EquipModify";


    public static string NET_PROTOCOL = "Config/NetProtocol";
    public static string ANIMATIONCLIP = "Animation";

    public static string VIDEO_STORY = "Video/Story";
    public static string VIDEO_SKILL = "Video/SkillDemos";
    public static string VIDEO_JOB = "Video/JobDemos";
    public static string VIDEO_Login = "Video/Login";
    public static string VIDEO_ARCADE = "Video/Arcade";
    public static string VIDEO_NPC = "Video/Npcdemos";

    public static string UI_VIEW = "UI/View";
    public static string UI_VIEW_NEW = "UI/Prefab";

    public static string UI_PREFAB_COMPONENT = "UI/Prefab/PrefabComponent";
    public static string UI_VIEW_COMPONENT = "UI/View/ViewComponent";
    public static string UI_ATLAS = "UI/Atlas";
    public static string UI_SPRITEATLAS = "UIV2/Atlas";
    public static string UI_SPRITE = "UI/Sprite";
    //public static string UI_ROLEICON = "UI/Sprite/RoleIcon";
    //public static string UI_SKILLFITICON = "UI/Sprite/SkillFitIcon";
    public static string UI_BACKGROUND = "UI/Background";
    public static string UI_INTERLUDEICON = "UI/Sprite/InterludeIcon";
    public static string UI_JOB_ORIGINAL = "UI/Sprite/JobOriginal";
    //public static string UI_MonthCard_IMG = "UI/Sprite/MonthcardIcon";
    //public static string UI_PATHICON = "UI/Sprite/AcquisitionPathIcon";
    //public static string UI_ENCHANTICON = "UI/Sprite/EnchantIcon";
    //public static string UI_SKILLComboICON = "UI/Sprite/SkillComboIcon";
    public static string UI_LOADING = "UI/Sprite/Loading";
    public static string UI_DIFFICULTYICON = "UI/Sprite/DifficultyIcon";
    //public static string UI_WEAPONDRAWINGICON = "UI/Sprite/DrawingIcon";
    public static string UI_MODEL = "UI/UIModel";
    public static string UI_ACTIVITY = "UIV2/Sprite/Activityimg";
    public static string UI_ROLE_ICON = "UIV2/Textures/RoleIcon";
    public static string UI_JIEJISHOW_ICON = "UIV2/Textures/JieJiShow";
    public static string UI_TEXTURE_JOB_BG = "UIV2/Textures/Job";
    public static string UI_COMMONBG = "UIV2/Sprite/CommonBg";
    public static string UI_ROLESTORY = "UIV2/Sprite/RoleStory";
    public static string UI_TOWERICON = "UI/Sprite/TowerIcon";
    public static string UI_DARKMAZE = "UI/Sprite/DarkMazeIcon";
    public static string UI_SPRITE_ILLUSTRATION = "UI/Sprite/Illustration";
    public static string UI_SPRITE_HALF_AVATER = "UI/Sprite/HalfAvatar";
    public static string UI_SPRITE_ARTIFACTMAKE = "UIV2/Sprite/ArtifactMake";
    public static string UI_SPRITE_HEADBOX = "UI/Sprite/HeadBox";
    public static string UI_SPRITE_BUBBLETBOX = "UI/Sprite/BubbletBox";
    public static string UI_Texture_DARKMAZE = "UIV2/Sprite/DarkMaze";
    public static string UI_Texture_NPC_HALF_BUST = "UIV2/Sprite/NpcBust";
    public static string UI_Sprite_Story = "UIV2/Sprite/Story";
    //public static string UI_GUILDBOSSPIC = "UI/Sprite/GuildBossPic";
    //public static string UI_RAID = "UI/Sprite/Raid";
    //public static string UI_MYSTERY_RAID_ICON = "UI/Sprite/MysteryRaidIcon";
    //public static string UI_QUALIFYING = "UI/Sprite/QualifyingIcon";
    //public static string UI_CRISIS = "UI/Sprite/Crisis";
    public static string UI_CREATEPLAYER = "UI/Sprite/CreatePlayer";
    public static string UI_Equip = "UI/Sprite/Equip";
    //public static string UI_SPACERIFT = "UI/Sprite/SpaceRift";
    public static string UI_AByssFb = "UI/Sprite/AbyssFb";
    //public static string UI_NEWFUNCTION_PROSPECT = "UI/Sprite/NewFunctionProspect";
    public static string UI_MAIN = "UI/Sprite/Main";
    public static string UI_FASHION = "UI/Sprite/Fashion";
    public static string UI_FUNCTION = "UI/Sprite/FuncOpen";
    public static string MODEL_ACTOR = "Models/Actor";
    public static string MODEL_DROP = "Models/Drop";
    public static string MODEL_MONSTER = "Models/Monster";
    public static string MODEL_PET = "Models/Pets";
    public static string MODEL_NPC = "Models/NPC";
    public static string MODEL_Weapon = "Models";
    public static string MODEL2D_ACTOR = "Models2D/Actor";

    public static string EFFECT = "Effect";
    public static string EFFECT_GRID = "Effect/GridEffect";
    public static string EFFECT_SkillView = "Effect/UI/SkillView";
    public static string EFFECT_TRANSFORM_JOB_VIEW_B = "Effect/UI/TransformJobViewB";


    public static string SHADER = "Shader";
    public static string SHADER_TEX = "ShaderTexture";

    public static string HEADBOX = "UI/Prefab/HeadBox";
    public static string BUBBLETBOX = "UI/Prefab/Bubblet";

    // 子界面路径
    public static string UI_SUBPANEL_BACKPACKVIEW = "UI/Prefab/BackpackView";
    public static string UI_SUBPANEL_EQUIPSLOTREFINEVIEW = "UI/Prefab/EquipSlotRefineView";
    public static string UI_SUBPANEL_SKILLVIEW = "UI/Prefab/SkillView";
    public static string UI_SUBPANEL_HONOURPASSVIEW = "UI/Prefab/HonourPassView";
    public static string UI_SUBPANEL_MANUFACTUREVIEW = "UI/Prefab/ManufactureView";
    public static string UI_SUBPANEL_SHOPVIEW = "UI/Prefab/ShopView";
    public static string UI_SUBPANEL_RUNEVIEW = "UI/Prefab/RuneView";
    public static string UI_SUBPANEL_STRONGERVIEW = "UI/Prefab/StrongerView";
    public static string UI_SUBPANEL_ACTIVITYVIEW = "UI/Prefab/ActivityView";
    public static string UI_SUBPANEL_RETURNACTIVITY = "UI/Prefab/ReturnActivity";
    public static string UI_SUBPANEL_CARNIVAL = "UI/Prefab/Carnival";
    public static string UI_SUBPANEL_FEEDBACK = "UI/Prefab/FeedBack";
    public static string UI_SUBPANEL_PLAYERINFOVIEW = "UI/Prefab/LookPlayerInfoView";
    public static string UI_SUBPANEL_SETTINGVIEW = "UI/Prefab/SettingView";
    public static string UI_SUBPANEL_PETVIEW = "UI/Prefab/PetView";
    public static string UI_SUBPANEL_ARCADEVIEW = "UI/Prefab/ArcadeView";
    public static string UI_SUBPANEL_LADDERVIEW = "UI/Prefab/LadderView";
    public static string UI_SUBPANEL_SLOTREFIN_EQUIPGROWTHPANEL = "UI/Prefab/EquipSlotRefineView/EquipGrowthPanel";
    public static string UI_SUBPANEL_ARTIFACTMAKEVIEW = "UI/Prefab/ArtifactMakeView";
    public static string UI_SUBPANEL_BATTLE = "UI/Prefab/BattleView";
    public static string UI_SUBPANEL_RESOURCEPVP_CITY_VIEW = "UI/Prefab/ResourcePVPCityView";
    public static string UI_SUBPANEL_TRADE_VIEW = "UI/Prefab/TradeView";
    public static string UI_SUBPANEL_EQUIP_RANK_VIEW = "UI/Prefab/EquipRankView";
    public static string UI_SUBPANEL_ADVENTURE_GROUP_VIEW = "UI/Prefab/AdventureGroupView";
    public static string UI_SUBPANEL_RANKVIEW = "UI/Prefab/RankView";

    public static string UI_PREFAB_COMMON = "UI/Prefab/Common";

    public static string FONTS = "Fonts";
    // 称号特效
    public static string UI_MilitaryRank = "UI/Sprite/MilitaryRank";
}

