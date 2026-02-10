using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum Lisiner
    {
        overPanel,                      //调用过渡页
        resourceUpdate,                 //资源更新
        areaItemClick,                  //地区选择点击
        TipShow,                        //提示信息显示
        TipNoticeShow,                  //公告信息显示
        TeaBuildChange,                 //茶园建筑变化
        TeaCountTip,                    //茶叶数量变更通知
        TeaAutoSelect,                  //自动种植选择的茶种
        AutoTeaGame,                    //茶叶自动开关广播
        TaskRedRefresh,                 //任务红点刷新 
        DaySignUpdate,                  //每日签到更新    
        PaySuccess,                     //支付成功
        WXLoginCallBack,                //微信登录回调  
        ReSetUserInfo,                  //重置用户信息
        CloseUI,                        //关闭UI界面
        CardPropChange,                 //卡片数量变更
        TutorialNotice,                 //新手引导播报
        VipSelectChange,                //VIP选择变化
    }

    public enum SceneType
    {
        Loading,
        Hall,
        TeaGarden,
        Turntable,
    }

    public enum ItemType
    {
        Resource,                  //资源
        Item,                      //物品
        Seed,                      //种子
        Workshop,                  //作坊
        Farmer,                    //农夫
        diamond,                  //钻石
        gold,                     //金币
        rmb,                      //人民币
        callCard,                 //召唤卡
    }
}

//010_resource 表
public class Sheet_Resource
{
    public int DaoID { get; set; }
    public int DaoType { get; set; }
    public string DaoName_zh { get; set; }

    public string DaoName_en { get; set; }
    public string DaoShuoMing_zh { get; set; }
    public string DaoShuoMing_en { get; set; }
    public int ImageID { get; set; }
    public int Gold { get; set; }
    public int Get1 { get; set; }
    public int Get2 { get; set; }
    public int Get3 { get; set; }
    public int Get4 { get; set; }
    public int Get5 { get; set; }
    public int ZuanNum { get; set; }
    public bool IsLook { get; set; }
}
//031_workshop
public class Sheet_Workshop
{
    public int ZuoID { get; set; }
    public string ZuoName_zh { get; set; }
    public string ZuoName_en { get; set; }
    public int ModID { get; set; }
    public int EffectId { get; set; }
    public int ImageID { get; set; }
    public int Get1 { get; set; }
    public int Get2 { get; set; }
    public List<GetRmbItem> Get_zs { get; set; } // 更新为 List<GetRmbItem>
    public List<GetRmbItem> Get_jb { get; set; }
    public List<GetRmbItem> Get_rmb { get; set; } // 更新为 List<GetRmbItem>
    public List<int> Tea { get; set; }
    public float Tea_s { get; set; }
    public int Tea_m { get; set; }
}
//020_item
public class Sheet_Item
{
    public int DaoID{ get; set; }
    public int DaoType{ get; set; }
    public string DaoName_zh { get; set; }
    public string DaoName_en { get; set; }
    public string DaoShuoMing_zh { get; set; }
    public string DaoShuoMing_en { get; set; }
    public int ImageID { get; set; }
    public int Gold { get; set; }
    public int Get1 { get; set; }
    public int Get2 { get; set; }
    public int Get3{ get; set; }
    public int Get4 { get; set; }
    public int Get5 { get; set; }
    public int ZuanNum { get; set; }
    public bool IsLook { get; set; }
}

//030_seed
public class Sheet_Seed
{
    public int ZhongID { get; set; }
    public string ZhongName_zh { get; set; }
    public string ZhongName_en { get; set; }
    public int ModID { get; set; }
    public int EffectId { get; set; }
    public int ImageID { get; set; }
    public int Get1 { get; set; }
    public int Get2 { get; set; }
    public int Get3 { get; set; }
    public int Get_zs { get; set; }
    public int Get_gg { get; set; }
}

//032_farmer
public class Sheet_Farmer
{
    public int WorkerID { get; set; }
    public string WorkerName_zh { get; set; }
    public string WorkerName_en { get; set; }
    public int ModID { get; set; }
    public int EffectId { get; set; }
    public int ImageID { get; set; }
    public int Color { get; set; }
    public float Color_xs { get; set; }
    public int Hp_s { get; set; }
    public int Hp_m { get; set; }
    public float Tea_s { get; set; }
    public int Tea_m { get; set; }
}

public class GetRmbItem
{
    public int Cost { get; set; }
    public int Amount { get; set; }
}