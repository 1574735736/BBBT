using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 邮箱接口服务（单例模式）
/// 处理所有邮箱相关POST/GET接口，兼容项目现有网络/认证逻辑
/// </summary>
public static class EmailDb 
{

    #region 通用工具方法（和项目其他接口脚本保持一致）


    /// <summary>
    /// 清洗JSON字符串（处理转义/特殊字符，避免解析失败）
    /// 处理 \/ → /、移除BOM/零宽空格、修剪首尾空白
    /// </summary>
    private static string CleanJsonString(string json)
    {
        if (string.IsNullOrEmpty(json)) return json;
        return json.TrimStart('\ufeff', '\u200b')
                   .Replace("\\/", "/")
                   .Trim();
    }

    /// <summary>
    /// 物品列表转JSON字符串（适配userSendEmail的article参数）
    /// </summary>
    /// <param name="goodsList">物品列表</param>
    /// <returns>符合服务器要求的article JSON字符串</returns>
    public static string GoodsListToArticleJson(List<EmailGoodsRequestItem> goodsList)
    {
        if (goodsList == null || goodsList.Count == 0) return "[]";
        // 序列化为纯净JSON，无多余转义
        return JsonConvert.SerializeObject(goodsList, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        });
    }
    #endregion

    #region 通用请求模型（接口入参专用）
    /// <summary>
    /// 物品请求项模型（适配userSendEmail的article参数序列化）
    /// </summary>
    [Serializable]
    public class EmailGoodsRequestItem
    {
        [JsonProperty("goods_id")]
        public string goods_id; // 服务器用字符串接收，保持一致
        [JsonProperty("num")]
        public string num;      // 服务器用字符串接收，保持一致

        public EmailGoodsRequestItem(string goodsId, string num)
        {
            this.goods_id = goodsId;
            this.num = num;
        }
    }
    #endregion

    #region 响应模型定义（严格匹配所有接口返回JSON结构）
    /// <summary>
    /// 通用角色信息模型（邮箱发送/接收方角色）
    /// </summary>
    [Serializable]
    public class EmailRoleInfo
    {
        public int id;
        public string nickname;
        public string number;
        public string image; // 头像URL（已处理转义）
    }

    /// <summary>
    /// 基础物品信息模型（所有接口共用）
    /// </summary>
    [Serializable]
    public class EmailGoodsBaseInfo
    {
        public int id;
        public string name;
        public string image;
        public int bind;
        public int upper;
    }

    /// <summary>
    /// 邮箱物品项模型（带物品信息）
    /// </summary>
    [Serializable]
    public class EmailGoodsItem
    {
        [JsonProperty("goods_id")]
        public string goods_id;
        [JsonProperty("num")]
        public string num;
        [JsonProperty("goods_info")]
        public EmailGoodsBaseInfo goods_info;
    }

    /// <summary>
    /// 邮箱列表项模型（/email/emailSheet返回的单条邮件）
    /// </summary>
    [Serializable]
    public class EmailSheetItem
    {
        public int id;
        public int server_role_id;
        public string system_server_role_id;
        public long createtime;
        public EmailRoleInfo role;
        public EmailRoleInfo system;
        public string create_time_text;
    }

    /// <summary>
    /// 获取邮箱信息响应数据（/email/emailSheet）
    /// </summary>
    [Serializable]
    public class EmailSheetData
    {
        public int total;
        public List<EmailSheetItem> data;
    }

    /// <summary>
    /// 邮件详情物品项模型（/email/emailInfo的article_info）
    /// </summary>
    [Serializable]
    public class EmailInfoArticleItem
    {
        [JsonProperty("goods_id")]
        public string goods_id;
        [JsonProperty("num")]
        public string num;
        [JsonProperty("goods_info")]
        public EmailGoodsBaseInfo goods_info;
    }

    /// <summary>
    /// 邮件详情核心信息（/email/emailInfo的info）
    /// </summary>
    [Serializable]
    public class EmailInfoDetail
    {
        public int id;
        public string system_server_role_id;
        public string content;
        public int silver;
        public string article;
        public int extract;
        public int status;
        public long createtime;
        public List<EmailInfoArticleItem> article_info;
        public EmailRoleInfo system; // 可能为null
        public string create_time_text;
    }

    /// <summary>
    /// 邮件详情响应数据（/email/emailInfo）
    /// </summary>
    [Serializable]
    public class EmailInfoData
    {
        public EmailInfoDetail info;
    }

    /// <summary>
    /// 单一领取邮件响应数据（/email/drawEmail）
    /// </summary>
    [Serializable]
    public class DrawSingleEmailData
    {
        public List<EmailGoodsItem> data;
    }

    /// <summary>
    /// 一键领取邮件响应数据（/email/drawAllEmail，二维物品列表）
    /// </summary>
    [Serializable]
    public class DrawAllEmailData
    {
        public List<List<EmailGoodsItem>> data;
    }

    /// <summary>
    /// 邮箱接口基础泛型响应模型（所有接口通用，统一回调）
    /// </summary>
    [Serializable]
    public class EmailBaseResponse<T>
    {
        public int code; // 1=成功，其他=失败
        public string msg; // 操作提示信息
        public string time; // 时间戳字符串
        public T data; // 泛型数据，适配不同接口返回结构
    }
    #endregion

    #region 通用请求封装（内部使用，统一处理POST/GET）
    /// <summary>
    /// 通用POST请求发送（内部封装，统一解析/错误处理）
    /// </summary>
    private static void SendPostRequest<T>(string url, WWWForm form, Action<EmailBaseResponse<T>> success, Action<string> fail)
    {
        HttpManager.Instance.Post6(url, form.data, (response) =>
        {
            try
            {
                string cleanJson = CleanJsonString(response);
                var result = JsonConvert.DeserializeObject<EmailBaseResponse<T>>(cleanJson);
                success?.Invoke(result);
            }
            catch (Exception e)
            {
                fail?.Invoke($"JSON解析失败：{e.Message}\n原始响应：{response}");
            }
        }, (error) =>
        {
            fail?.Invoke($"POST请求失败：{error}（接口：{url}）");
        },UserDb.GetAuthHeader());
    }

    /// <summary>
    /// 通用GET请求发送（内部封装，拼接参数/统一解析）
    /// </summary>
    private static void SendGetRequest<T>(string baseUrl, Dictionary<string, string> paramDict, Action<EmailBaseResponse<T>> success, Action<string> fail)
    {
        // 拼接GET参数：url?key1=value1&key2=value2
        string paramStr = string.Empty;
        if (paramDict != null && paramDict.Count > 0)
        {
            List<string> paramList = new List<string>();
            foreach (var kv in paramDict)
            {
                paramList.Add($"{kv.Key}={kv.Value}");
            }
            paramStr = $"?{string.Join("&", paramList)}";
        }
        string fullUrl = $"{baseUrl}{paramStr}";

        HttpManager.Instance.Get(fullUrl, (response) =>
        {
            try
            {
                string cleanJson = CleanJsonString(response);
                var result = JsonConvert.DeserializeObject<EmailBaseResponse<T>>(cleanJson);
                success?.Invoke(result);
            }
            catch (Exception e)
            {
                fail?.Invoke($"JSON解析失败：{e.Message}\n原始响应：{response}");
            }
        }, (error) =>
        {
            fail?.Invoke($"GET请求失败：{error}（接口：{fullUrl}）");
        },UserDb.GetAuthHeader());
    }
    #endregion

    #region 接口1：POST /email/emailSheet - 获取用户邮箱信息
    /// <summary>
    /// 获取用户邮箱信息
    /// </summary>
    /// <param name="type">邮件类型 1-系统；2-玩家；3-已发送</param>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void GetEmailSheet(int type, int userRoleId,
        Action<EmailBaseResponse<EmailSheetData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/email/emailSheet";
        WWWForm form = new WWWForm();
        form.AddField("type", type);
        form.AddField("user_role_id", userRoleId);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口2：POST /email/emailInfo - 获取邮件详情信息
    /// <summary>
    /// 获取邮件详情信息
    /// </summary>
    /// <param name="emailId">邮件ID</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void GetEmailInfo(int emailId,
        Action<EmailBaseResponse<EmailInfoData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/email/emailInfo";
        WWWForm form = new WWWForm();
        form.AddField("id", emailId); // 服务器参数名是id，对应邮件id

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口3：POST /email/drawEmail - 单一领取系统邮件
    /// <summary>
    /// 单一领取系统邮件奖励
    /// </summary>
    /// <param name="emailId">邮件ID</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void DrawSingleEmail(int emailId,
        Action<EmailBaseResponse<DrawSingleEmailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.DataUrl}/email/drawEmail";
        WWWForm form = new WWWForm();
        form.AddField("email_id", emailId);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口4：POST /email/drawAllEmail - 一键领取所有系统邮件
    /// <summary>
    /// 一键领取所有系统邮件奖励
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void DrawAllEmail(int userRoleId,
        Action<EmailBaseResponse<DrawAllEmailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/email/drawAllEmail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口5：POST /email/userSendEmail - 用户发送邮件（带物品/银两）
    /// <summary>
    /// 用户发送邮件给其他玩家
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="number">收件人编码</param>
    /// <param name="silver">邮箱附带银两</param>
    /// <param name="content">邮件内容</param>
    /// <param name="articleJson">物品信息JSON字符串（可通过GoodsListToArticleJson生成）</param>
    /// <param name="extract">提取所需银两</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void UserSendEmail(int userRoleId, string number, string silver, string content,
        string articleJson, string extract, Action<EmailBaseResponse<object>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/email/userSendEmail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("number", number);
        form.AddField("sliver", silver); // 服务器参数名是sliver，注意拼写
        form.AddField("content", content);
        form.AddField("article", articleJson);
        form.AddField("extract", extract);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口6：GET /email/userDrawEmail - 领取玩家发送的邮件
    /// <summary>
    /// 领取其他玩家发送的邮件
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="emailId">邮件ID</param>
    /// <param name="success">成功回调（code=1即成功）</param>
    /// <param name="fail">失败回调</param>
    public static void UserDrawEmail(int userRoleId, int emailId,
        Action<EmailBaseResponse<object>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/email/userDrawEmail";
        // 拼接GET参数
        Dictionary<string, string> paramDict = new Dictionary<string, string>()
        {
            {"user_role_id", userRoleId.ToString()},
            {"email_id", emailId.ToString()}
        };

        SendGetRequest(url, paramDict, success, fail);
    }
    #endregion

    #region 接口7：GET /userBackEmail - 退回玩家发送的邮件
    /// <summary>
    /// 退回其他玩家发送的邮件
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="emailId">邮件ID</param>
    /// <param name="success">成功回调（code=1即成功）</param>
    /// <param name="fail">失败回调</param>
    public static void UserBackEmail(int userRoleId, int emailId,
        Action<EmailBaseResponse<object>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/email/userBackEmail";
        // 拼接GET参数
        Dictionary<string, string> paramDict = new Dictionary<string, string>()
        {
            {"user_role_id", userRoleId.ToString()},
            {"email_id", emailId.ToString()}
        };

        SendGetRequest(url, paramDict, success, fail);
    }
    #endregion
}