using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包模块接口服务（单例模式）
/// 处理背包所有POST接口请求，与项目现有网络/认证逻辑无缝兼容
/// </summary>
public static class BackpackDb
{
    #region 通用工具方法（与项目其他接口脚本完全一致，保证风格统一）
   

    /// <summary>
    /// 清洗JSON字符串（处理转义字符/特殊字符，避免解析失败）
    /// 自动将 \/ 转为 /，移除BOM/零宽空格等无效字符
    /// </summary>
    private static string CleanJsonString(string json)
    {
        if (string.IsNullOrEmpty(json)) return json;
        return json.TrimStart('\ufeff', '\u200b')
                   .Replace("\\/", "/")
                   .Trim();
    }
    #endregion

    #region 响应模型定义（严格匹配接口返回JSON结构，无冗余/缺失字段）
    /// <summary>
    /// 背包分类项模型（/back/backAttr 返回的单条分类）
    /// </summary>
    [Serializable]
    public class BackpackAttrItem
    {
        public int id;    // 分类ID 1-物品 2-装备 3-书籍 等
        public string name; // 分类名称
    }

    /// <summary>
    /// 背包物品基础信息模型（物品/装备等的核心信息）
    /// </summary>
    [Serializable]
    public class BackpackGoodsInfo
    {
        public int id;        // 物品ID
        public string name;   // 物品名称
        public string image;  // 物品图标URL（已处理转义，可直接下载）
        public int bind;      // 是否绑定 0-未绑定 1-绑定
        public int upper;     // 堆叠上限
        public string detail; // 物品描述
    }

    /// <summary>
    /// 背包物品项模型（/back/userBackSheet 返回的单条物品）
    /// </summary>
    [Serializable]
    public class BackpackItem
    {
        public int id;        // 背包物品记录ID
        public int goods_id;  // 关联的物品基础ID
        public int num;       // 物品数量
        public BackpackGoodsInfo goods; // 物品基础信息
    }

    /// <summary>
    /// 背包分类物品列表数据模型（/back/userBackSheet 核心返回数据）
    /// </summary>
    [Serializable]
    public class BackpackSheetData
    {
        public int total;                 // 该分类下物品总数
        public List<BackpackItem> data;   // 物品列表
    }

    /// <summary>
    /// 背包接口基础泛型响应模型（所有接口通用，统一回调格式）
    /// </summary>
    [Serializable]
    public class BackpackBaseResponse<T>
    {
        public int code;    // 1=成功，其他=失败
        public string msg;  // 操作提示信息
        public string time; // 服务器时间戳字符串
        public T data;      // 泛型数据，适配不同接口的返回结构
    }
    #endregion

    #region 通用POST请求封装（内部使用，统一处理解析/错误回调）
    /// <summary>
    /// 通用POST请求发送方法，封装重复的网络请求/解析逻辑
    /// </summary>
    private static void SendPostRequest<T>(string url, WWWForm form, Action<BackpackBaseResponse<T>> success, Action<string> fail)
    {
        HttpManager.Instance.Post6(url, form.data, (response) =>
        {
            try
            {
                // 1. 清洗JSON，处理转义/特殊字符
                string cleanJson = CleanJsonString(response);
                // 2. 反序列化为指定的泛型响应模型
                var result = JsonConvert.DeserializeObject<BackpackBaseResponse<T>>(cleanJson);
                // 3. 成功回调
                success?.Invoke(result);
            }
            catch (Exception e)
            {
                // 解析失败，返回详细错误信息
                fail?.Invoke($"JSON解析失败：{e.Message}\n原始响应数据：{response}");
            }
        }, (error) =>
        {
            // 网络请求失败，返回错误信息+接口地址
            fail?.Invoke($"POST请求失败：{error}（接口地址：{url}）");
        }, UserDb.GetAuthHeader());
    }
    #endregion

    #region 接口1：POST /back/backAttr - 获取背包所有分类（无请求参数）
    /// <summary>
    /// 获取背包所有分类（物品/装备/书籍/皮肤等）
    /// </summary>
    /// <param name="success">成功回调（返回分类列表）</param>
    /// <param name="fail">失败回调（返回错误信息）</param>
    public static void GetBackpackAttr(Action<BackpackBaseResponse<List<BackpackAttrItem>>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/back/backAttr";
        // 无请求参数，创建空的WWWForm
        WWWForm form = new WWWForm();
        // 发送POST请求
        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口2：POST /back/userBackSheet - 获取指定分类下的用户物品
    /// <summary>
    /// 获取背包指定分类下的所有用户物品
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="attrId">背包分类ID（1-物品 2-装备 3-书籍等）</param>
    /// <param name="success">成功回调（返回该分类下的物品列表）</param>
    /// <param name="fail">失败回调（返回错误信息）</param>
    public static void GetUserBackpackSheet(int userRoleId, int attrId,
        Action<BackpackBaseResponse<BackpackSheetData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}back/userBackSheet";
        WWWForm form = new WWWForm();
        // 添加接口要求的请求参数
        form.AddField("user_role_id", userRoleId);
        form.AddField("attr_id", attrId);
        // 发送POST请求
        SendPostRequest(url, form, success, fail);
    }
    #endregion
}