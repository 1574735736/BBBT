using LieyouFramework;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSettings;

public static class UserDb
{
    // 保存用户token的静态变量
    public static string UserToken { get; private set; }

    // 保存用户信息的静态变量
    public static UserInfo CurrentUserInfo { get; private set; }

    // 保存服务器列表的静态变量
    public static List<ServerInfo> ServerList { get; private set; }

    /// <summary>
    /// 修改密码
    /// </summary>
    public static void ChangeRoleAvatar(string mobile, string old_pass,string pass,
      Action<ChangeAvatarResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/index/changePass";

        var form = new WWWForm();
        form.AddField("mobile", mobile);
        form.AddField("old_pass", old_pass);
        form.AddField("pass", pass);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<ChangeAvatarResponse>(response);
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
                    onFail?.Invoke($"修改密码失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"修改密码失败: {error}");
            },
            GetAuthHeader()
        );
    }


    /// <summary>
    /// 检查昵称是否已使用
    /// </summary>
    /// <param name="nickname">昵称</param>
    /// <param name="serverId">服务器ID</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void CheckNickname(string nickname, int serverId,
        Action<CheckNicknameResponse> onSuccess, Action<string> onFail)
    {
        // 构建查询参数
        string queryParams = $"?nickname={Uri.EscapeDataString(nickname)}&server_id={serverId}";

        HttpManager.Instance.Get1("/role/checkNickName" + queryParams,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<CheckNicknameResponse>(response);
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
                    onFail?.Invoke($"检查昵称失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"检查昵称失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 上传图片接口
    /// </summary>
    /// <param name="fileData">文件数据(byte数组)</param>
    /// <param name="fileName">文件名</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void UploadImage(byte[] fileData, string fileName,
        Action<UploadResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/common/upload";

        var form = new WWWForm();
        form.AddBinaryData("file", fileData, fileName);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<UploadResponse>(response);
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
                    onFail?.Invoke($"上传图片失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"上传图片失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 修改角色头像接口
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="avatar">头像URL</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void ChangeRoleAvatar(int userRoleId, string avatar,
        Action<ChangeAvatarResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/role/changeRoleAvatar";

        var form = new WWWForm();
        form.AddField("user_role_id", userRoleId);
        form.AddField("avatar", avatar);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<ChangeAvatarResponse>(response);
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
                    onFail?.Invoke($"修改头像失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"修改头像失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 获取头像列表接口
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetAvatarSheet(Action<AvatarSheetResponse> onSuccess, Action<string> onFail)
    {
        HttpManager.Instance.Get1("/role/avatarSheet",
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<AvatarSheetResponse>(response);
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
                    onFail?.Invoke($"获取头像列表失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取头像列表失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 获取角色列表接口
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetRoleSheet(Action<RoleSheetResponse> onSuccess, Action<string> onFail)
    {
        HttpManager.Instance.Get1("/role/roleSheet",
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<RoleSheetResponse>(response);

                    Debug.Log("角色列表：" + response);

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
                    onFail?.Invoke($"获取角色列表失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取角色列表失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 创建角色接口
    /// </summary>
    /// <param name="serverId">服务器ID</param>
    /// <param name="roleId">角色ID</param>
    /// <param name="nickname">昵称</param>
    /// <param name="avatar">头像</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void DrawRole(int serverId, int roleId, string nickname, string avatar,
        Action<DrawRoleResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/role/drawRole";

        var form = new WWWForm();
        form.AddField("server_id", serverId);
        form.AddField("role_id", roleId);
        form.AddField("nickname", nickname);
        form.AddField("avatar", avatar);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<DrawRoleResponse>(response);
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
                    onFail?.Invoke($"创建角色失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"创建角色失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 加入服务器接口
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void JoinServer(int userRoleId, Action<JoinServerResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/role/joinServer";

        var form = new WWWForm();
        form.AddField("user_role_id", userRoleId);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<JoinServerResponse>(response);
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
                    onFail?.Invoke($"加入服务器失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"加入服务器失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 获取用户角色信息接口
    /// </summary>
    /// <param name="userRoleId">用户角色ID</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetUserRoleInfo(int userRoleId, Action<UserRoleInfoResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/role/userRoleInfo";

        var form = new WWWForm();
        form.AddField("user_role_id", userRoleId);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<UserRoleInfoResponse>(response);
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
                    onFail?.Invoke($"获取用户角色信息失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取用户角色信息失败: {error}");
            },
            GetAuthHeader()
        );
    }


    /// <summary>
    /// 用户注册接口
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <param name="pass">密码</param>
    /// <param name="code">验证码</param>
    /// <param name="shareCode">分享码</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void Register(string mobile, string pass, string code, string shareCode,
        Action<RegisterResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/user/register";

        var form = new WWWForm();
        form.AddField("mobile", mobile);
        form.AddField("pass", pass);
        form.AddField("code", code);
        form.AddField("share_code", shareCode);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<RegisterResponse>(response);
                    if (result.code == 1)
                    {
                        // 保存token
                        UserToken = result.data.userinfo.token;
                        CurrentUserInfo = result.data.userinfo;
                        onSuccess?.Invoke(result);
                    }
                    else
                    {
                        onFail?.Invoke(result.msg);
                    }
                }
                catch (Exception ex)
                {
                    onFail?.Invoke($"解析响应失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"注册失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 用户登录接口
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="pass">密码</param>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void Login(string account, string pass,
        Action<LoginResponse> onSuccess, Action<string> onFail)
    {
        string url = FrameworkConfig.BaseUrl + "/user/login";

        var form = new WWWForm();
        form.AddField("account", account);
        form.AddField("pass", pass);

        HttpManager.Instance.Post6(url, form.data,
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<LoginResponse>(response);
                    if (result.code == 1)
                    {
                        // 保存token和用户信息
                        UserToken = result.data.userinfo.token;
                        CurrentUserInfo = result.data.userinfo;
                        onSuccess?.Invoke(result);
                    }
                    else
                    {
                        onFail?.Invoke(result.msg);
                    }
                }
                catch (Exception ex)
                {
                    onFail?.Invoke($"解析响应失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"登录失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 获取我的服务器列表
    /// </summary>
    /// <param name="onSuccess">成功回调</param>
    /// <param name="onFail">失败回调</param>
    public static void GetMyServer(Action<MyServerResponse> onSuccess, Action<string> onFail)
    {
        //string url = FrameworkConfig.BaseUrl + "/user/myServer";

        HttpManager.Instance.Get1("/user/myServer",
            (response) =>
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<MyServerResponse>(response);
                    if (result.code == 1)
                    {
                        ServerList = result.data.data;
                        onSuccess?.Invoke(result);
                    }
                    else
                    {
                        onFail?.Invoke(result.msg);
                    }
                }
                catch (Exception ex)
                {
                    onFail?.Invoke($"解析服务器列表失败: {ex.Message}");
                }
            },
            (error) =>
            {
                onFail?.Invoke($"获取服务器列表失败: {error}");
            },
            GetAuthHeader()
        );
    }

    /// <summary>
    /// 获取认证Header
    /// </summary>
    /// <returns>包含token的Header字典</returns>
    public static Dictionary<string, string> GetAuthHeader()
    {
        var header = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(UserToken))
        {
            header.Add("token", UserToken);
            //header.Add("Content-Type", "application/json");
        }
        return header;
    }

    /// <summary>
    /// 清除用户登录信息
    /// </summary>
    public static void ClearUserData()
    {
        UserToken = null;
        CurrentUserInfo = null;
        ServerList = null;
    }

    #region 数据模型类

    // 新增的数据模型
    [Serializable]
    public class CheckNicknameResponse
    {
        public int code;
        public string msg;
        public string time;
        public CheckNicknameData data;
    }

    [Serializable]
    public class CheckNicknameData
    {
        public int use; // 1表示已使用，0表示未使用
    }

    [Serializable]
    public class UploadResponse
    {
        public int code;
        public string msg;
        public string time;
        public UploadData data;
    }

    [Serializable]
    public class UploadData
    {
        public string url; // 上传成功后的文件URL
        // 根据实际返回结构调整
    }

    [Serializable]
    public class ChangeAvatarResponse
    {
        public int code;
        public string msg;
        public string time;
        public ChangeAvatarData data;
    }

    [Serializable]
    public class ChangeAvatarData
    {
        public int use; // 根据实际返回结构调整
    }

    [Serializable]
    public class RegisterResponse
    {
        public int code;
        public string msg;
        public string time;
        public RegisterData data;
    }

    [Serializable]
    public class RegisterData
    {
        public UserInfo userinfo;
    }

    [Serializable]
    public class LoginResponse
    {
        public int code;
        public string msg;
        public string time;
        public LoginData data;
    }

    [Serializable]
    public class LoginData
    {
        public UserInfo userinfo;
    }

    [Serializable]
    public class MyServerResponse
    {
        public int code;
        public string msg;
        public string time;
        public ServerListData data;
    }

    [Serializable]
    public class ServerListData
    {
        public List<ServerInfo> data;
    }

    [Serializable]
    public class UserInfo
    {
        public int id;
        public string token;
        public int user_id;
        public long createtime;
        public long expiretime;
        public int expires_in;
    }

    [Serializable]
    public class ServerInfo
    {
        public int id;
        public string name;
        public List<RoleInfo> role;
    }

    [Serializable]
    public class RoleInfo
    {
        public int id;
        public string nickname;
        public string image;
        public int role_sheet_id;
        public RoleDetailInfo role_info;
    }

    [Serializable]
    public class RoleDetailInfo
    {
        public int id;
        public string name;
        public string image;
    }

    [Serializable]
    public class AvatarSheetResponse
    {
        public int code;
        public string msg;
        public string time;
        public AvatarSheetData data;
    }

    [Serializable]
    public class AvatarSheetData
    {
        public List<string> data;
    }

    [Serializable]
    public class RoleSheetResponse
    {
        public int code;
        public string msg;
        public string time;
        public RoleSheetData data;
    }

    [Serializable]
    public class RoleSheetData
    {
        public List<RoleSheetItem> data;
    }

    [Serializable]
    public class RoleSheetItem
    {
        public int id;
        public string name;
        public string image;
        public string detail;
    }

    [Serializable]
    public class DrawRoleResponse
    {
        public int code;
        public string msg;
        public string time;
        public DrawRoleData data;
    }

    [Serializable]
    public class DrawRoleData
    {
        public List<RoleSheetItem> data;
    }

    [Serializable]
    public class JoinServerResponse
    {
        public int code;
        public string msg;
        public string time;
        public object data; // 根据实际返回调整
    }

    [Serializable]
    public class UserRoleInfoResponse
    {
        public int code;
        public string msg;
        public string time;
        public UserRoleInfoData data;
    }

    [Serializable]
    public class UserRoleInfoData
    {
        public UserRoleInfoDetail data;
    }

    [Serializable]
    public class UserRoleInfoDetail
    {
        public int level;
        public int exp;
        public int blood;
        public int energy;
        public int attack;
        public int speed;
        public string image;
        public string nickname;
        public string number;
    }


    #endregion
}