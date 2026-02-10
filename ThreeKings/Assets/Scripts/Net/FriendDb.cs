using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏好友接口服务（单例模式）
/// 处理所有好友相关POST接口请求
/// </summary>
public static class FriendDb 
{

    #region 通用工具方法

    /// <summary>
    /// 清洗JSON字符串（处理转义字符/特殊字符，避免解析失败）
    /// </summary>
    private static string CleanJsonString(string json)
    {
        if (string.IsNullOrEmpty(json)) return json;
        // 移除BOM/零宽空格/转义斜杠，修剪首尾空白
        return json.TrimStart('\ufeff', '\u200b')
                   .Replace("\\/", "/")
                   .Trim();
    }
    #endregion

    #region 响应模型定义（严格匹配JSON结构）
    /// <summary>
    /// 角色信息模型（好友/申请人的角色信息）
    /// </summary>
    [Serializable]
    public class RoleInfo
    {
        public int id;
        public string nickname;
        public string number;
        public string image; // 头像URL（已处理转义）
    }

    /// <summary>
    /// 我的好友列表项模型
    /// </summary>
    [Serializable]
    public class HailFriendItem
    {
        public int id;
        public int hail_user_id;
        public string hail_nickname;
        public long createtime;
        public RoleInfo hail_role;
        public string create_time_text;
    }

    /// <summary>
    /// 我的好友列表响应数据模型
    /// </summary>
    [Serializable]
    public class MyHailData
    {
        public int total;
        public List<HailFriendItem> data;
    }

    /// <summary>
    /// 好友申请列表项模型
    /// </summary>
    [Serializable]
    public class ApplyHailItem
    {
        public int id;
        public int user_id;
        public string detail;
        public long createtime;
        public RoleInfo role;
        public string create_time_text;
    }

    /// <summary>
    /// 好友申请列表响应数据模型
    /// </summary>
    [Serializable]
    public class MyApplyHailData
    {
        public int total;
        public List<ApplyHailItem> data;
    }

    /// <summary>
    /// 好友接口基础响应模型（通用）
    /// </summary>
    [Serializable]
    public class FriendBaseResponse<T>
    {
        public int code; // 1=成功，其他=失败
        public string msg; // 操作提示信息
        public string time; // 时间戳字符串
        public T data; // 泛型数据，适配不同接口的返回结构
    }
    #endregion

    #region 接口1：获取我的好友列表 /hail/myHail
    /// <summary>
    /// 获取我的好友列表
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="status">好友状态:0正常好友;1拉黑好友</param>
    /// <param name="page">页码（从1开始）</param>
    /// <param name="limit">每页条数</param>
    /// <param name="success">成功回调（返回好友列表数据）</param>
    /// <param name="fail">失败回调（返回错误信息）</param>
    public static void GetMyHailList(int userRoleId, int status, int page, int limit,
        Action<FriendBaseResponse<MyHailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/hail/myHail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("status", status);
        form.AddField("page", page);
        form.AddField("limit", limit);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口2：添加好友 /hail/addHail
    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="hailUserId">被添加用户角色ID</param>
    /// <param name="detail">添加备注/说明</param>
    /// <param name="success">成功回调（返回基础响应）</param>
    /// <param name="fail">失败回调（返回错误信息）</param>
    public static void AddHailFriend(int userRoleId, int hailUserId, string detail,
        Action<FriendBaseResponse<object>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/hail/addHail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("hail_user_id", hailUserId);
        form.AddField("detail", detail);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口3：获取好友申请列表 /hail/myApplyHail
    /// <summary>
    /// 获取好友申请列表
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="page">页码（从1开始）</param>
    /// <param name="limit">每页条数</param>
    /// <param name="success">成功回调（返回申请列表数据）</param>
    /// <param name="fail">失败回调（返回错误信息）</param>
    public static void GetMyApplyHailList(int userRoleId, int page, int limit,
        Action<FriendBaseResponse<MyApplyHailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/hail/myApplyHail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("page", page);
        form.AddField("limit", limit);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口4：同意/拒绝好友申请（默认接口名：/hail/handleApplyHail，可根据实际修改）
    /// <summary>
    /// 处理好友申请（同意/拒绝）
    /// </summary>
    /// <param name="applyId">申请ID</param>
    /// <param name="status">1=通过，-1=拒绝</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void HandleApplyHail(int applyId, int status,
        Action<FriendBaseResponse<MyApplyHailData>> success, Action<string> fail)
    {
        // 若后端接口名不是这个，请替换为实际地址
        string url = $"{FrameworkConfig.BaseUrl}/hail/handleApplyHail";
        WWWForm form = new WWWForm();
        form.AddField("apply_id", applyId);
        form.AddField("status", status);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口5：一键忽略好友申请 /hail/allDelHail
    /// <summary>
    /// 一键忽略所有好友申请
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void AllDelHailApply(int userRoleId,
        Action<FriendBaseResponse<MyApplyHailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/hail/allDelHail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口6：删除/拉黑好友 /hail/delBlockHail
    /// <summary>
    /// 删除/拉黑好友
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="hailUserId">目标好友角色ID</param>
    /// <param name="status">1=拉黑，-1=删除</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void DelBlockHailFriend(int userRoleId, int hailUserId, int status,
        Action<FriendBaseResponse<MyApplyHailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/hail/delBlockHail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("hail_user_id", hailUserId);
        form.AddField("status", status);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 接口7：取消拉黑好友 /hail/cancelBlockHail
    /// <summary>
    /// 取消拉黑好友
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="hailUserId">被拉黑好友角色ID</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    public static void CancelBlockHailFriend(int userRoleId, int hailUserId,
        Action<FriendBaseResponse<MyApplyHailData>> success, Action<string> fail)
    {
        string url = $"{FrameworkConfig.BaseUrl}/hail/cancelBlockHail";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("hail_user_id", hailUserId);

        SendPostRequest(url, form, success, fail);
    }
    #endregion

    #region 通用POST请求发送方法（内部封装，统一处理）
    /// <summary>
    /// 通用POST请求发送方法
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    /// <param name="url">完整接口地址</param>
    /// <param name="form">请求参数</param>
    /// <param name="success">成功回调</param>
    /// <param name="fail">失败回调</param>
    private static void SendPostRequest<T>(string url, WWWForm form, Action<FriendBaseResponse<T>> success, Action<string> fail)
    {
        // 调用项目中已有的HttpManager（与你之前的代码保持一致）
        HttpManager.Instance.Post6(url, form.data, (response) =>
        {
            try
            {
                // 1. 清洗JSON字符串，处理转义/特殊字符
                string cleanJson = CleanJsonString(response);
                // 2. 反序列化为指定类型
                var result = JsonConvert.DeserializeObject<FriendBaseResponse<T>>(cleanJson);
                // 3. 成功回调
                success?.Invoke(result);
            }
            catch (Exception e)
            {
                // 解析失败回调
                fail?.Invoke($"JSON解析失败：{e.Message}\n原始数据：{response}");
            }
        }, (error) =>
        {
            // 请求失败回调（网络错误/接口报错）
            fail?.Invoke($"请求失败：{error}（接口地址：{url}）");
        }, UserDb.GetAuthHeader());
    }
    #endregion
}