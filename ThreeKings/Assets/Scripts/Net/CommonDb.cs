using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class CommonDb
{
    /// <summary>
    /// 获取区服信息接口
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="limit">每页数量</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetServerSheet(int page, int limit,
        Action<ServerSheetResponse> onSuccess, Action<string> onFail)
    {
        // 构建查询参数
        string queryParams = $"?page={page}&limit={limit}";

        HttpManager.Instance.Get1("/index/serverSheet" + queryParams,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<ServerSheetResponse>(response);
                    if (result.code == 1)
                    {
                        onSuccess?.Invoke(result);
                    }
                    else
                    {
                        onFail?.Invoke(result.msg);
                    }
                }
                catch (Exception ex)
                {
                    onFail?.Invoke($"获取区服信息失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取区服信息失败: {error}");
            }
        );
    }

    /// <summary>
    /// 获取用户协议接口
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetUserProtocol(Action<UserProtocolResponse> onSuccess, Action<string> onFail)
    {
        HttpManager.Instance.Get1("/index/userPro",
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<UserProtocolResponse>(response);
                    if (result.code == 1)
                    {
                        onSuccess?.Invoke(result);
                    }
                    else
                    {
                        onFail?.Invoke(result.msg);
                    }
                }
                catch (Exception ex)
                {
                    onFail?.Invoke($"获取用户协议失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取用户协议失败: {error}");
            }
        );
    }

    /// <summary>
    /// 获取隐私政策接口
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetPrivacyPolicy(Action<PrivacyPolicyResponse> onSuccess, Action<string> onFail)
    {
        HttpManager.Instance.Get1("/index/hidePro",
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<PrivacyPolicyResponse>(response);
                    if (result.code == 1)
                    {
                        onSuccess?.Invoke(result);
                    }
                    else
                    {
                        onFail?.Invoke(result.msg);
                    }
                }
                catch (Exception ex)
                {
                    onFail?.Invoke($"获取隐私政策失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取隐私政策失败: {error}");
            }
        );
    }

    #region 数据模型

    [Serializable]
    public class ServerSheetResponse
    {
        public int code;
        public string msg;
        public string time;
        public ServerSheetData data;
    }

    [Serializable]
    public class ServerSheetData
    {
        public int total;
        public List<ServerInfo> data;
    }

    [Serializable]
    public class ServerInfo
    {
        public int id;
        public string name;
        public int number;
        public int status; // 状态：1-新服 2-推荐 3-火爆 4-维护
        public int def;    // 默认服务器：0-否 1-是
        public string open_time;

        // 状态文本描述（辅助属性）
        public string StatusText
        {
            get
            {
                return status switch
                {
                    1 => "新服",
                    2 => "推荐",
                    3 => "火爆",
                    4 => "维护",
                    _ => "未知"
                };
            }
        }

        // 是否可进入（辅助属性）
        public bool CanEnter
        {
            get { return status != 4; } // 状态4为维护，不可进入
        }
    }

    [Serializable]
    public class UserProtocolResponse
    {
        public int code;
        public string msg;
        public string time;
        public UserProtocolData data;
    }

    [Serializable]
    public class UserProtocolData
    {
        public string val; // HTML格式的用户协议内容
    }

    [Serializable]
    public class PrivacyPolicyResponse
    {
        public int code;
        public string msg;
        public string time;
        public PrivacyPolicyData data;
    }

    [Serializable]
    public class PrivacyPolicyData
    {
        public string val; // HTML格式的隐私政策内容
    }

    #endregion
}