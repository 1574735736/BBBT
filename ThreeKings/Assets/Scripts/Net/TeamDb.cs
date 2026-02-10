using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

/// <summary>
/// 组队模块网络接口封装（严格匹配实际API文档）
/// 所有接口基于POST请求，统一处理JSON解析、异常捕获、权限头信息
/// </summary>
public static class TeamDb
{
    #region 通用工具方法（复用项目原有逻辑，不修改）
    /// <summary>
    /// 清理JSON字符串（去除BOM、转义符、首尾空白，统一路径分隔符）
    /// </summary>
    private static string CleanJsonString(string json)
    {
        if (string.IsNullOrEmpty(json)) return json;
        return json.TrimStart('\ufeff', '\u200b')
                   .Replace("\\/", "/")
                   .Trim();
    }
    #endregion

    #region 通用响应模型（基础结构，所有接口统一继承）
    /// <summary>
    /// 所有组队接口的通用返回格式（固定code/msg/time/data）
    /// </summary>
    [Serializable]
    public class TeamBaseResponse<T>
    {
        public int code;           // 1=成功 其他=失败
        public string msg;         // 接口提示信息
        public string time;        // 服务器时间戳字符串
        public T data;             // 各接口自定义业务数据
    }

    /// <summary>
    /// 分页列表通用模型（多个接口返回的data均为 total+data数组 结构，复用）
    /// </summary>
    [Serializable]
    public class PageData<T>
    {
        public int total;          // 数据总数
        public List<T> data;       // 分页数据数组
    }
    #endregion

    #region 业务实体模型（1:1匹配接口返回的JSON字段，大小写/名称完全一致）
    /// <summary>
    /// 用户角色信息模型（复用：head_info/one_info/user_role等字段均为此结构）
    /// </summary>
    [Serializable]
    public class UserRoleInfo
    {
        public int id;             // 角色ID
        public string nickname;    // 角色昵称
        public string number;      // 角色编号
        public string image;       // 角色头像地址
    }

    /// <summary>
    /// 队伍简易信息模型（获取全部队伍列表 /team/teamSheet 返回的单条队伍数据）
    /// </summary>
    [Serializable]
    public class TeamSimpleInfo
    {
        public int id;             // 队伍ID
        public string name;        // 队伍名称
        public int min_level;      // 队伍最低加入等级
        public int sum;            // 队伍当前人数
    }

    /// <summary>
    /// 队伍详情信息模型（我的队伍/转让/踢人/解散/退出 接口返回的队伍完整数据）
    /// </summary>
    [Serializable]
    public class TeamDetailInfo
    {
        public int id;             // 队伍ID
        public string name;        // 队伍名称
        public int min_level;      // 队伍最低加入等级
        public string pass;        // 队伍密码（空则无密码）
        public int? head;          // 队长角色ID（可空，用?避免null解析报错）
        public int? one;           // 队员1角色ID（可空）
        public int? two;           // 队员2角色ID（可空）
        public int? three;         // 队员3角色ID（可空）
        public int? four;          // 队员4角色ID（可空）
        public UserRoleInfo head_info; // 队长信息
        public UserRoleInfo one_info;   // 队员1信息（null则无）
        public UserRoleInfo two_info;   // 队员2信息（null则无）
        public UserRoleInfo three_info; // 队员3信息（null则无）
        public UserRoleInfo four_info;  // 队员4信息（null则无）
        public bool HasMember(int index)
        {
            switch (index)
            {
                case 0: return head_info != null;
                case 1: return one_info != null;
                case 2: return two_info != null;
                case 3: return three_info != null;
                case 4: return four_info != null;
                default: return false;
            }
        }
    }

    /// <summary>
    /// 我的队伍数据模型（/team/getMyTeam 接口返回的业务数据）
    /// </summary>
    [Serializable]
    public class MyTeamData
    {
        public int type;           // 队伍类型：0=无队伍，1=有队伍
        public TeamDetailInfo team; // 直接映射为车队详情对象（type=0 时为 null）

        // 简化获取车队信息的方法，增加空值保护
        public TeamDetailInfo GetTeamInfo()
        {
            // 仅当 type=1 且 team 非空时返回，否则返回 null
            return (type == 1 && team != null) ? team : null;
        }
    }

    /// <summary>
    /// 入队申请信息模型（/team/teamApplySheet 接口返回的单条申请数据）
    /// </summary>
    [Serializable]
    public class TeamApplyInfo
    {
        public int id;             // 申请记录ID
        public int user_role_id;   // 申请人角色ID
        public long createtime;    // 申请时间戳
        public UserRoleInfo user_role; // 申请人信息
        public string create_time_text; // 申请时间格式化文本
    }
    #endregion

    #region 通用POST请求封装（复用项目原有逻辑，不修改）
    /// <summary>
    /// 封装组队模块POST请求，统一处理头信息、JSON解析、异常捕获
    /// </summary>
    private static void SendPostRequest<T>(string path, WWWForm form, Action<TeamBaseResponse<T>> success, Action<string> fail)
    {
        string url = FrameworkConfig.BaseUrl + path;
        HttpManager.Instance.Post6(url, form.data, (response) =>
        {
            try
            {
                string cleanJson = CleanJsonString(response);
                var jsonSettings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore, // 忽略模型中没有的JSON字段
                    NullValueHandling = NullValueHandling.Ignore,         // 忽略null值
                    Error = (sender, args) =>
                    {
                        Debug.LogWarning($"JSON反序列化警告：{args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true; // 标记错误已处理，避免抛出异常
                    }
                };
                var result = JsonConvert.DeserializeObject<TeamBaseResponse<T>>(cleanJson, jsonSettings);
                success?.Invoke(result);
            }
            catch (Exception e)
            {
                fail?.Invoke($"组队接口JSON解析失败：{e.Message}\n原始响应数据：{response}");
            }
        }, (error) =>
        {
            fail?.Invoke($"组队接口POST请求失败：{error}\n接口地址：{url}");
        }, UserDb.GetAuthHeader());
    }
    #endregion

    #region 11个实际组队接口实现（严格匹配路径/参数/返回模型）
    #region 1. 获取全部队伍列表 /team/teamSheet
    /// <summary>
    /// 获取全部队伍列表（分页）
    /// </summary>
    /// <param name="page">页码（从1开始）</param>
    /// <param name="limit">每页条数</param>
    /// <param name="success">成功回调（返回分页队伍简易信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void GetTeamSheet(int page, int limit,
        Action<TeamBaseResponse<PageData<TeamSimpleInfo>>> success = null, Action<string> fail = null)
    {
        string path = "/team/teamSheet";
        WWWForm form = new WWWForm();
        form.AddField("page", page.ToString());
        form.AddField("limit", limit.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 2. 获取我的队伍信息 /team/getMyTeam
    /// <summary>
    /// 获取我的队伍详细信息
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="success">成功回调（返回我的队伍完整信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void GetMyTeam(int userRoleId,
        Action<TeamBaseResponse<MyTeamData>> success = null, Action<string> fail = null)
    {
        string path = "/team/getMyTeam";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 3. 创建队伍 /team/createTeam
    /// <summary>
    /// 创建新队伍
    /// </summary>
    /// <param name="userRoleId">创建者（队长）角色ID</param>
    /// <param name="teamName">队伍名称</param>
    /// <param name="minLevel">队伍最低加入等级</param>
    /// <param name="pass">队伍密码（无密码传空字符串""）</param>
    /// <param name="success">成功回调（code=1即为创建成功）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void CreateTeam(int userRoleId, string teamName, int minLevel, string pass,
        Action<TeamBaseResponse<object>> success = null, Action<string> fail = null)
    {
        string path = "/team/createTeam";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        form.AddField("name", teamName);
        form.AddField("min_level", minLevel.ToString());
        form.AddField("pass", pass);
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 4. 申请加入队伍 /team/joinTeam
    /// <summary>
    /// 申请加入指定队伍
    /// </summary>
    /// <param name="userRoleId">申请人角色ID</param>
    /// <param name="teamId">要加入的队伍ID</param>
    /// <param name="pass">队伍密码（无密码传空字符串""）</param>
    /// <param name="success">成功回调（code=1即为申请成功）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void JoinTeam(int userRoleId, int teamId, string pass,
        Action<TeamBaseResponse<object>> success = null, Action<string> fail = null)
    {
        string path = "/team/joinTeam";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        form.AddField("team_id", teamId.ToString());
        form.AddField("pass", pass);
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 5. 获取入队申请列表 /team/teamApplySheet
    /// <summary>
    /// 获取当前队伍的入队申请列表（队长操作）
    /// </summary>
    /// <param name="teamId">队伍ID</param>
    /// <param name="success">成功回调（返回分页申请信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void GetTeamApplySheet(int teamId,
        Action<TeamBaseResponse<PageData<TeamApplyInfo>>> success = null, Action<string> fail = null)
    {
        string path = "/team/teamApplySheet";
        WWWForm form = new WWWForm();
        form.AddField("team_id", teamId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 6. 一键忽略所有入队申请 /team/allTeamApplyBack
    /// <summary>
    /// 一键忽略当前队伍的所有入队申请（队长操作）
    /// </summary>
    /// <param name="teamId">队伍ID</param>
    /// <param name="success">成功回调（code=1即为操作成功）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void AllTeamApplyBack(int teamId,
        Action<TeamBaseResponse<PageData<TeamApplyInfo>>> success = null, Action<string> fail = null)
    {
        string path = "/team/allTeamApplyBack";
        WWWForm form = new WWWForm();
        form.AddField("team_id", teamId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 7. 审核入队申请 /team/teamApplyPro
    /// <summary>
    /// 审核单条入队申请（队长操作）
    /// </summary>
    /// <param name="applyId">申请记录ID</param>
    /// <param name="status">审核状态：-1=拒绝，1=通过</param>
    /// <param name="success">成功回调（code=1即为操作成功）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void TeamApplyPro(int applyId, int status,
        Action<TeamBaseResponse<PageData<TeamApplyInfo>>> success = null, Action<string> fail = null)
    {
        string path = "/team/teamApplyPro";
        WWWForm form = new WWWForm();
        form.AddField("apply_id", applyId.ToString());
        form.AddField("status", status.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 8. 转让队长 /team/transTeamHead
    /// <summary>
    /// 转让队伍队长（原队长操作）
    /// </summary>
    /// <param name="userRoleId">原队长角色ID</param>
    /// <param name="teamId">队伍ID</param>
    /// <param name="transUserRoleId">新队长角色ID</param>
    /// <param name="success">成功回调（返回转让后的队伍信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void TransTeamHead(int userRoleId, int teamId, int transUserRoleId,
        Action<TeamBaseResponse<MyTeamData>> success = null, Action<string> fail = null)
    {
        string path = "/team/transTeamHead";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        form.AddField("team_id", teamId.ToString());
        form.AddField("trans_user_role_id", transUserRoleId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 9. 踢出队员 /api/team/kickTeam（注意路径带/api/）
    /// <summary>
    /// 踢出队伍队员（队长操作）
    /// </summary>
    /// <param name="userRoleId">队长角色ID</param>
    /// <param name="teamId">队伍ID</param>
    /// <param name="kickUserRoleId">被踢队员角色ID</param>
    /// <param name="success">成功回调（返回踢人后的队伍信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void KickTeam(int userRoleId, int teamId, int kickUserRoleId,
        Action<TeamBaseResponse<MyTeamData>> success = null, Action<string> fail = null)
    {
        string path = "/api/team/kickTeam";// 注意接口路径带/api/，严格匹配
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        form.AddField("team_id", teamId.ToString());
        form.AddField("kick_user_role_id", kickUserRoleId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 10. 解散队伍 /team/fraTeam
    /// <summary>
    /// 解散队伍（队长操作）
    /// </summary>
    /// <param name="userRoleId">队长角色ID</param>
    /// <param name="teamId">队伍ID</param>
    /// <param name="success">成功回调（返回解散后的队伍信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void FraTeam(int userRoleId, int teamId,
        Action<TeamBaseResponse<MyTeamData>> success = null, Action<string> fail = null)
    {
        string path = "/team/fraTeam";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        form.AddField("team_id", teamId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion

    #region 11. 退出队伍 /team/backTeam
    /// <summary>
    /// 退出当前所在队伍（队员操作，队长不可直接退出，需先转让/解散）
    /// </summary>
    /// <param name="userRoleId">当前用户角色ID</param>
    /// <param name="teamId">队伍ID</param>
    /// <param name="success">成功回调（返回退出后的队伍信息）</param>
    /// <param name="fail">失败回调（错误信息）</param>
    public static void BackTeam(int userRoleId, int teamId,
        Action<TeamBaseResponse<MyTeamData>> success = null, Action<string> fail = null)
    {
        string path = "/team/backTeam";
        WWWForm form = new WWWForm();
        form.AddField("user_role_id", userRoleId.ToString());
        form.AddField("team_id", teamId.ToString());
        SendPostRequest(path, form, success, fail);
    }
    #endregion
    #endregion
}