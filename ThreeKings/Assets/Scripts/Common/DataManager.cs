
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private int _gold = 0;                      //游戏金币
    private bool isHall = true;
    private Enums.SceneType _curScene;          //当前场景

    private int _serverId = 0;

    private int _diamond = 0;                   //游戏钻石

    private int _emery = 0;                     //能量

    //private string _loginMobil = "";

    private int _roleID = -1;                   //角色ID

    private int _vipLevel = 0;                   //VIP等级
    private string _totalRmb = "0";              //总充值金额

    private int _userIconId = 0;                //用户头像ID
    // 最后更新时间（用于检测跨天/跨周）
    private DateTime _lastUpdateDate = DateTime.MinValue;
    private DateTime _lastWeekStart = DateTime.MinValue;

    private Dictionary<string,string> _LoginDataInfo = new Dictionary<string,string>();

    private int _realNameIdx = 0;
    private int _netErrorCount = 0;

    private int _tutorialLevel = 0;             //引导进行的阶段
    private List<int> _isTutorialed = new List<int>();        //没有完成所有引导

    public readonly Dictionary<int, string> ItemIdToName = new Dictionary<int, string> //物品ID对应的名称
    {
        {00000, "通知"},
        {10001, "钻石"},
        {10002, "金币"},
        {10003, "普通月卡"},
        {10004, "尊贵月卡"},
        {10005, "豪华月卡"},
        {10006, "普通招聘卡"},
        {10007, "特级招聘卡"},
        {10008, "专属招聘卡"},
        {10009, "施粥卡"},
        {10010, "征兵卡"},
        {30001, "普通绿茶"},
        {30002, "普通红茶"},
        {30003, "凤凰单枞"},
        {30004, "西湖龙井"},
        {30005, "铁观音"},
        {30006, "普通红袍"},
    };

    public readonly Dictionary<int, string> ItemIdToIcon = new Dictionary<int, string>  //物品ID对应的图标路径
    {
        {00000, ""},
        {10001, "AddressablesUI/zuanshiicon.png"},
        {10002, "AddressablesUI/jinbiicon.png"},
        {10003, "monthCard/monthCard_1.png"},
        {10004, "monthCard/monthCard_2.png"},
        {10005, "monthCard/monthCard_3.png"},
        {10006, "Recruitment/Recruitment_1.png"},
        {10007, "Recruitment/Recruitment_2.png"},
        {10008, "Recruitment/Recruitment_3.png"},
        {10009, ""},
        {10010, ""},
        {30001, "Tea/chayuan_daoju_30001.png"},
        {30002, "Tea/chayuan_daoju_30002.png"},
        {30003, "Tea/chayuan_daoju_30003.png"},
        {30004, "Tea/chayuan_daoju_30004.png"},
        {30005, "Tea/chayuan_daoju_30005.png"},
        {30006, "Tea/chayuan_daoju_30006.png"},
    };

    public readonly Dictionary<int, string> ItemIdToBackGround = new Dictionary<int, string>  //物品ID对应的背景图标路径
    {
        {00000, "PublicIcon/ziyuankuang_putong.png"},
        {10001, "PublicIcon/ziyuankuang_purple.png"},
        {10002, "PublicIcon/ziyuankuang_yellow.png"},
        {10003, "PublicIcon/ziyuankuang_putong.png"},
        {10004, "PublicIcon/ziyuankuang_putong.png"},
        {10005, "PublicIcon/ziyuankuang_putong.png"},
        {10006, "PublicIcon/ziyuankuang_putong.png"},
        {10007, "PublicIcon/ziyuankuang_putong.png"},
        {10008, "PublicIcon/ziyuankuang_putong.png"},
        {10009, "PublicIcon/ziyuankuang_putong.png"},
        {10010, "PublicIcon/ziyuankuang_putong.png"},
        {30001, "PublicIcon/ziyuankuang_putong.png"},
        {30002, "PublicIcon/ziyuankuang_putong.png"},
        {30004, "PublicIcon/ziyuankuang_putong.png"},
        {30005, "PublicIcon/ziyuankuang_putong.png"},
        {30006, "PublicIcon/ziyuankuang_putong.png"},
    };

    protected override void Init()
    {
        _isTutorialed = LoadData<List<int>>(DataKey.TotalFruits, new List<int>());
        _LoginDataInfo = LoadData<Dictionary<string, string>>(DataKey.LoginData, new Dictionary<string, string>());

    }

    private void Start()
    {
        
    }

    public void ResetData()
    {
        _realNameIdx = 0;
        _netErrorCount = 0;
    }

    public void InitData()
    {
        //Gold = LoginDb.CurrentUser.gold;
        ////Diamond = LoginDb.CurrentUser.diamond;
        ////TotalFruits = float.Parse(LoginDb.CurrentUser.sum_tea);
        ////DayPlantingCount = LoginDb.CurrentUser.day_sum_seep;
        ////WeekPlantingCount = LoginDb.CurrentUser.week_sum_seep;
        //VipLevel = LoginDb.UserOthers.vip_level;
        //TotalRmb = LoginDb.UserOthers.total_rmb;
        //_userIconId = 1;
        //if (!string.IsNullOrEmpty(LoginDb.CurrentUser.avatar))
        //{
        //    if (int.TryParse(LoginDb.CurrentUser.avatar, out int parsedId))
        //    {
        //        _userIconId = parsedId;
        //    }
        //}
    }

    //public string LoginDate
    //{
    //    get { return _loginMobil; }
    //    set { _loginMobil = value;
    //        SaveData<string>(DataKey.LoginData, _loginMobil);
    //    }
    //}

    public int RoleID
    {
        get { return _roleID; }
        set { _roleID = value; }
    }

    /// <summary>
    /// 服务器ID
    /// </summary>
    public int ServerId
    {
        get { return _serverId; }
        set { _serverId = value; }
    }

    // <summary>
    /// 游戏金币
    /// </summary>
    public int Gold {
        get { return _gold; }
        set
        {
            _gold = value;
            if (_gold < 0)
            {
                _gold = 0;
            }
            //SaveData<int>(DataKey.Gold, _gold);
        }
    }

    public bool CanLogin(string key ,string value)
    {
        foreach (var item in _LoginDataInfo)
        {
            if (item.Key.Equals(key))
            {
                return item.Value == value;
            }
        }
        return false;
    }

    public bool HasLoginInfo(string key)
    {
        if (_LoginDataInfo.ContainsKey(key))
        {
            return true;
        }
        return false;
    }


    public void ChangeLoginData(string key, string value)
    {
        if (!_LoginDataInfo.ContainsKey(key))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "当前账号不存在");
            return;
        }
        _LoginDataInfo[key] = value;
        EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "密码修改成功");
        SaveData<Dictionary<string, string>>(DataKey.LoginData, _LoginDataInfo);
    }

    public void RegestLoginData(string key,string value)
    {
        if (_LoginDataInfo.ContainsKey(key))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "已存在相同账号");
            return;
        }
        _LoginDataInfo.Add(key, value);
        SaveData<Dictionary<string, string>>(DataKey.LoginData, _LoginDataInfo);
    }

    /// <summary>
    /// 能量
    /// </summary>
    public int Emery
    {
        get { return _emery; }
        set
        {
            _emery = value;
            if (_emery < 0)
            {
                _emery = 0;
            }
            //SaveData<int>(DataKey.Gold, _gold);
        }
    }

    /// <summary>
    /// 钻石
    /// </summary>
    public int Diamond
    {
        get { return _diamond; }
        set
        {
            _diamond = value;
            if (_diamond < 0)
            {
                _diamond = 0;
            }
            //SaveData<int>(DataKey.Diamond, _diamond);  // 设置钻石时自动保存
        }
    }

   
    /// <summary>
    /// 引导  默认0没开始引导， 1为邮箱引导， 2为一键领取引导 3为茶园引导
    /// </summary>
    public int TutorialLevel
    {
        get { return _tutorialLevel; }
        set { 
            _tutorialLevel = value;
        }
    }
    //判定是否已经引导过了
    public bool IsTutorialed
    {
        get {
            return true;  //临时处理改为不需要引导
            //return _isTutorialed.Contains(_tutorialLevel);
            }       
    }

    public void SetTutorialed(int index)
    {
        if (!_isTutorialed.Contains(index))
        {
            _isTutorialed.Add(index);
            SaveData<List<int>>(DataKey.TotalFruits,_isTutorialed);
        }
    }


    public bool IsHall()
    {
        return _curScene == Enums.SceneType.Hall;
    }

    public Enums.SceneType CurScene
    {
        get { return _curScene; }
        set {
            _curScene = value;   
        }
    }

   
    // 当前周的开始日期（周一）
    private DateTime CurrentWeekStart
    {
        get
        {
            DateTime today = DateTime.Today;
            int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysSinceMonday < 0) daysSinceMonday += 7; // 处理周日情况
            return today.AddDays(-daysSinceMonday);
        }
    }

    // 检查并重置每周计数（如果跨周）
    private void CheckAndResetWeekly()
    {
        DateTime weekStart = CurrentWeekStart;

        // 如果是新的一周，重置每周计数
        if (weekStart != _lastWeekStart)
        {
            //_weekPlantingCount = 0;
            _lastWeekStart = weekStart;
            Debug.Log("新的一周开始，重置每周种植计数");
        }
    }

    private void CheckAndResetDaily()
    {
        DateTime today = DateTime.Today;

        // 如果是新的一天，重置每日计数
        if (today != _lastUpdateDate)
        {
            //_dayPlantingCount = 0;
            _lastUpdateDate = today;
            Debug.Log("新的一天，重置每日种植计数");
        }
    }

   

    public int VipLevel
    {
        get { return _vipLevel; }
        set { _vipLevel = value; }
    }

    public int UserIconId
    {
        get { return _userIconId; }
        set { _userIconId = value; }
    }


    public string TotalRmb
    {
        get { return _totalRmb; }
        set { _totalRmb = value; }
    }
    //实名认证次数
    public int RealNameIdx
    {
        get { return _realNameIdx; }
        set { _realNameIdx = value; }
    }

    public int NetErrorCount
    {
        get { return _netErrorCount; }
        set { 
            _netErrorCount = value;
            if (_netErrorCount >= 3)
            {
                UIManager.Instance.OpenNetontinue();
            }
        }
    }

    // 示例：根据ID获取物品名称
    public string GetItemNameById(int itemId)
    {
        // 尝试获取，不存在则返回默认值（如"未知物品"）
        if (ItemIdToName.TryGetValue(itemId, out string itemName))
        {
            return itemName;
        }
        return "未知物品";
    }

    // 示例：根据ID获取物品图标
    public string GetItemIconById(int itemId)
    {
        // 尝试获取，不存在则返回默认值（如"未知物品"）
        if (ItemIdToIcon.TryGetValue(itemId, out string itemName))
        {
            return itemName;
        }
        return "未知物品";
    }

    public string GetItemBackGround(int itemId)
    {
        // 尝试获取，不存在则返回默认值（如"未知物品"）
        if (ItemIdToBackGround.TryGetValue(itemId, out string itemName))
        {
            return itemName;
        }
        return "未知背景框";
    }

    /// <summary>
    /// 随机生成手机号码，登录使用
    /// </summary>
    /// <returns></returns>
    public string GenerateRandomPhoneNumber()
    {
        // 生成手机号的第一位，范围是 3-9
        int firstDigit = UnityEngine.Random.Range(3, 10);

        // 拼接手机号的前3位
        StringBuilder phoneNumber = new StringBuilder();
        phoneNumber.Append("1");
        phoneNumber.Append(firstDigit);
        phoneNumber.Append(UnityEngine.Random.Range(0, 10)); // 生成第二位，范围是 0-9

        // 生成剩余的8位数字
        for (int i = 0; i < 8; i++)
        {
            phoneNumber.Append(UnityEngine.Random.Range(0, 10));
        }

        return phoneNumber.ToString();
    }

    public void SaveData<T>(DataKey key, T value)
    {
        string dataString;
        dataString = JsonConvert.SerializeObject(value);
        PlayerPrefs.SetString(key.ToString(), dataString);
        PlayerPrefs.Save();
    }

    public void SaveData<T>(string key, T value)
    {
        string dataString;
        dataString = JsonConvert.SerializeObject(value);

        PlayerPrefs.SetString(key, dataString);
        PlayerPrefs.Save();
    }

    public T LoadData<T>(string key, T defaultValue)
    {
        string dataString = PlayerPrefs.GetString(key.ToString(), "");

        if (string.IsNullOrEmpty(dataString))
        {
            return defaultValue;
        }
        else
        {
            return JsonConvert.DeserializeObject<T>(dataString);
        }
    }
    public T LoadData<T>(DataKey key, T defaultValue)
    {
        string dataString = PlayerPrefs.GetString(key.ToString(), "");

        if (string.IsNullOrEmpty(dataString))
        {
            return defaultValue;
        }
        else
        {
            return JsonConvert.DeserializeObject<T>(dataString);
        }      
    }

    /// <summary>
    /// 支付完成后调用，同步用户数据
    /// </summary>
    public void ReSetUserInfo()
    {
        //LoginDb.UserInfo((success, message) => {
        //    if (success)
        //    {
        //        InitData();
        //        EventManager.Instance.TriggerEvent(Enums.Lisiner.ReSetUserInfo, null);
        //        EventManager.Instance.TriggerEvent(Enums.Lisiner.resourceUpdate, "");
        //    }
        //    else
        //    {
        //        Debug.LogError(message);
        //    }
        //});
    }
}

public enum PayType
{
    alipay,
    gold,
}

public enum DataKey
{
    NoviceGuide,            //新手引导
    Gold,                   //游戏金币
    AudioKey,               //音乐
    MusicKey,               //音效
    Diamond,                //钻石
    TotalFruits,            //总水果数量  
    BuildingId,             //使用中的建筑等级
    LoginData,              //登录数据
    TeaSceneLevel,          //场景选择
    TeaPlantId,             //自动种植时候的茶种选择
    UserTutorial,           //用户新手引导

    LoginAccount,           //登录账号
    LoginPassword,          //登录密码
}
