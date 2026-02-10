using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class NoticeDb
{
    /// <summary>
    /// 获取公告分类接口
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetNoticeAttr(Action<NoticeAttrResponse> onSuccess, Action<string> onFail)
    {
        HttpManager.Instance.Get1("/index/noticeAttr",
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<NoticeAttrResponse>(response);
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
                    onFail?.Invoke($"获取公告分类失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取公告分类失败: {error}");
            }
        );
    }

    /// <summary>
    /// 获取分类下公告列表接口
    /// </summary>
    /// <param name="attrId">分类ID</param>
    /// <param name="page">页码</param>
    /// <param name="limit">每页数量</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetNoticeSheet(int attrId, int page, int limit,
        Action<NoticeSheetResponse> onSuccess, Action<string> onFail)
    {
        // 构建查询参数
        string queryParams = $"?attr_id={attrId}&page={page}&limit={limit}";

        HttpManager.Instance.Get1("/index/noticeSheet" + queryParams,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<NoticeSheetResponse>(response);
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
                    onFail?.Invoke($"获取公告列表失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取公告列表失败: {error}");
            }
        );
    }

    /// <summary>
    /// 获取公告详情接口
    /// </summary>
    /// <param name="id">公告ID</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetNoticeInfo(int id, Action<NoticeInfoResponse> onSuccess, Action<string> onFail)
    {
        // 构建查询参数
        string queryParams = $"?id={id}";

        HttpManager.Instance.Get1("/index/noticeInfo" + queryParams,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<NoticeInfoResponse>(response);
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
                    onFail?.Invoke($"获取公告详情失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取公告详情失败: {error}");
            }
        );
    }

    #region 数据模型

    [Serializable]
    public class NoticeAttrResponse
    {
        public int code;
        public string msg;
        public string time;
        public NoticeAttrData data;
    }

    [Serializable]
    public class NoticeAttrData
    {
        public List<NoticeAttrItem> data;
    }

    [Serializable]
    public class NoticeAttrItem
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class NoticeSheetResponse
    {
        public int code;
        public string msg;
        public string time;
        public NoticeSheetData data;
    }

    [Serializable]
    public class NoticeSheetData
    {
        public int total;
        public List<NoticeSheetItem> list;
    }

    [Serializable]
    public class NoticeSheetItem
    {
        public int id;
        public string title;
        public string image;
        public int hot;
    }

    [Serializable]
    public class NoticeInfoResponse
    {
        public int code;
        public string msg;
        public string time;
        public NoticeInfoData data;
    }

    [Serializable]
    public class NoticeInfoData
    {
        public NoticeDetailData data;
    }

    [Serializable]
    public class NoticeDetailData
    {
        public int id;
        public string title;
        public string image;
        public int hot;
        public string content;
        public long createtime;
        public string create_time_text;
    }

    #endregion
}