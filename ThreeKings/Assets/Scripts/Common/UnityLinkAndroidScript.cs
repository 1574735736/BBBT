using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnityLinkAndroidScript : Singleton<UnityLinkAndroidScript>
{
    AndroidJavaClass javaClass;
    AndroidJavaObject javaObject;
    AndroidJavaClass WXEntryActivityClass;
    
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        javaObject = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
        //u2a_add(100, 200);

        WXEntryActivityClass = new AndroidJavaClass("com.kaixinworld.game.wxapi.WXEntryActivity");
#endif

    }

    /// <summary>
    /// Unity调用Android
    /// </summary>
    void u2a_add(int a, int b)
    {
        //标准操作，获取MainActivity对象

        //调用方法
        var result = javaObject.Call<int>("u2a_add", a, b);
        //EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, $"UnityCallAndroid {a} + {b} = {result}");
        //text1.text = $"UnityCallAndroid {a} + {b} = {result}";
    }

    /// <summary>
    /// 给android调用的方法
    /// </summary>
    /// <param name="objs"></param>
    void a2u_add(string objs)
    {
        var arrays = objs.Split('|');
        var a = int.Parse(arrays[0]);
        var b = int.Parse(arrays[1]);
        var result = a + b;
        //EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, $"AndroidCallUnity {a} + {b} = {result}");
        //text2.text = $"AndroidCallUnity {a} + {b} = {result}";
    }

    public void WxLoaded()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        javaObject.Call("Login");
#endif
    }

    private void WXLoginCallBack(string str)
    {
        Debug.Log("微信登录回调: " + str);
        string strinfo = "";
        if (str != "用户取消" && str != "用户拒绝" && str != "其他错误")
        {
            strinfo = "微信登录成功，code是：" + str;
            //text3.text += str + "\r\n";
            EventManager.Instance.TriggerEvent(Enums.Lisiner.WXLoginCallBack, str);
        }
        else
        {
            strinfo = "微信登录失败，code是：" + str;
            //text3.text += str + "\r\n";
            EventManager.Instance.TriggerEvent(Enums.Lisiner.WXLoginCallBack, "");
        }
        //EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, strinfo);
        
    }


    //public 

    /// <summary>
    /// 分享文字
    /// </summary>
    public void WxShareText()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        object[] args = new object[] { "你好,欢迎游玩王者之心", 1 };
        javaObject.Call("ShareText", args);
#endif
    }

    /// <summary>
    /// 分享图片
    /// </summary>
    public void WxShareImage()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        Sprite sp = Resources.Load<Sprite>("Textures/64");
        byte[] imageData = sp.texture.EncodeToPNG();
        //object[] objects = new object[] { share, share,1 };
        //javaObject.Call("ShareImage", objects);

        byte[] thumbData = CreateThumbnail(sp.texture, 150, 150);

        javaObject.Call("ShareImage", imageData, thumbData, 1);
#endif
    }

    /// <summary>
    /// 分享网页
    /// </summary>
    public void WxShareWeb()
    {
        //Sprite sp = Resources.Load<Sprite>("Textures/64");
        //object[] args = new object[] { "https://www.baidu.com", "百度一下，你就知道", "百度",sp.texture.EncodeToPNG(), 1 };
        //javaObject.Call("ShareWeb", args);
#if !UNITY_EDITOR && UNITY_ANDROID
        Sprite sp = Resources.Load<Sprite>("Textures/512");
        byte[] thumbData = CreateThumbnail(sp.texture, 150, 150);

        // 直接传递五个参数
        javaObject.Call("ShareWeb",
            "https://www.baidu.com",
            "百度一下，你就知道",
            "百度",
            thumbData,
            1);
#endif
    }


    /// <summary>
    /// 原生调用支付宝支付
    /// <param name="orderInfo">临时的订单号</param>
    /// </summary>
    public void ShowALiPay(string orderInfo)
    {
        //debugText.text += "\n服务器返回订单号 >>>> " + orderInfo.Substring(0, 20);
        Debug.Log("服务器返回订单号 >>>> " + orderInfo);  // 此处是后端返回的订单信息
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject currentActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
        javaObject.Call("AliPay", orderInfo, "ALiPayResult"); //currentActivity
#endif
    }
    /// <summary>
    ///  支付宝 支付回调
    /// </summary>
    /// <param name="result">支付结果通知，支持本地通知和云通知（结果传回到你的服务端中），这里为了简单只展示了本地通知</param>
    public void ALiPayResult(string result)
    {
        Debug.Log("Unity 获取支付宝回调 >>>> " + result);

        try
        {
            // 解析支付宝返回的结果
            var resultDict = ParseAliPayResult(result);

            // 获取结果状态码
            string resultStatus = resultDict.ContainsKey("resultStatus")
                ? resultDict["resultStatus"]
                : "UNKNOWN";

            // 获取结果描述
            string memo = resultDict.ContainsKey("memo")
                ? Uri.UnescapeDataString(resultDict["memo"])
                : "无描述信息";

            // 解析详细结果（JSON格式）
            string resultJson = resultDict.ContainsKey("result")
                ? resultDict["result"]
                : "{}";

            //PayView payView = FindObjectOfType<PayView>();
            //if (payView != null)
            //{
            //    payView.HandleAliPayResult(resultStatus);
            //}
            //else
            {
                // 如果 PayView 已关闭，直接处理订单
                HandlePaymentResultWithoutView(resultStatus);
            }

            // 根据状态码处理支付结果
            switch (resultStatus)
            {
                case "9000": // 支付成功
                    Debug.Log("支付宝支付成功");
                    // 解析详细支付结果
                    var successResult = JsonUtility.FromJson<AliPaySuccessResponse>(resultJson);
                    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "支付成功！");//订单号：" + successResult.alipay_trade_app_pay_response?.out_trade_no);
                    // 这里可以触发游戏内的支付成功逻辑
                    break;

                case "8000": // 支付处理中
                    Debug.Log("支付宝支付处理中");
                    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "支付处理中，请稍后确认结果");
                    break;

                case "4000": // 支付失败
                case "5000": // 重复请求
                    Debug.LogError($"支付宝支付失败，状态码：{resultStatus}");
                    // 尝试解析错误详情
                    var errorResult = JsonUtility.FromJson<AliPayErrorResponse>(resultJson);
                    string errorMsg = errorResult?.alipay_trade_app_pay_response?.sub_msg ?? memo;
                    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, $"支付失败：{errorMsg}");
                    break;

                case "6001": // 用户取消
                    Debug.Log("用户取消支付");
                    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "您已取消支付");
                    break;

                case "6002": // 网络错误
                    Debug.LogError("支付宝网络连接错误");
                    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "网络连接失败，请检查网络");
                    break;

                default: // 未知状态
                    Debug.LogError($"未知支付状态：{resultStatus}");
                    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, $"支付结果未知，状态码：{resultStatus}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"解析支付宝回调时出错：{ex.Message}");
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "支付结果解析错误，请查看日志");
        }
    }

    // 当支付界面已关闭时的处理
    private void HandlePaymentResultWithoutView(string resultStatus)
    {
        string orderNo = PlayerPrefs.GetString("CurrentOrderNo", "");

        if (string.IsNullOrEmpty(orderNo))
        {
            Debug.LogError("无法获取订单号，支付结果处理失败");
            return;
        }

        if (resultStatus != "9000")
        {
            // 支付失败时取消订单
            //GiftPackageDb.CancelPayPackage(orderNo, (success, msg) =>
            //{
            //    if (!success)
            //    {
            //        Debug.LogError($"取消订单失败: {msg}");
            //    }
            //    else
            //    {
            //        Debug.LogError("取消订单成功");
            //    }
            //});
        }

        // 清除保存的订单号
        PlayerPrefs.DeleteKey("CurrentOrderNo");
        PlayerPrefs.DeleteKey("OrderType");
    }

    /// <summary>
    /// 解析支付宝返回的结果字符串
    /// </summary>
    private Dictionary<string, string> ParseAliPayResult(string result)
    {
        var dict = new Dictionary<string, string>();

        // 分割不同部分（resultStatus, memo, result）
        string[] parts = result.Split(';');

        foreach (string part in parts)
        {
            int index = part.IndexOf('=');
            if (index > 0)
            {
                string key = part.Substring(0, index).Trim();
                string value = part.Substring(index + 1).Trim();

                // 移除值两端的 { }
                if (value.StartsWith("{") && value.EndsWith("}"))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                dict[key] = value;
            }
        }

        return dict;
    }


    private byte[] CreateThumbnail(Texture2D texture, int width, int height)
    {
        // 创建临时纹理
        Texture2D thumb = new Texture2D(width, height, TextureFormat.RGB24, false);

        // 缩放纹理
        Graphics.ConvertTexture(texture, thumb);

        // 压缩为JPG格式（质量75%，体积更小）
        byte[] data = thumb.EncodeToJPG(75);

        // 检查大小
        if (data.Length > 32 * 1024)
        {
            Debug.LogWarning("缩略图仍然过大: " + data.Length + " bytes");
        }

        Destroy(thumb);
        return data;
    }

    private void WXShareCallBack(string result)
    {
        Debug.Log("微信分享结果: " + result);
        //text4.text = "分享结果: " + result;
        EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "分享结果: " + result);
    }


    // 支付宝支付成功响应结构
    [System.Serializable]
    public class AliPaySuccessResponse
    {
        public AliPayTradeAppPayResponse alipay_trade_app_pay_response;
    }

    // 支付宝支付错误响应结构
    [System.Serializable]
    public class AliPayErrorResponse
    {
        public AliPayTradeAppPayErrorResponse alipay_trade_app_pay_response;
    }

    // 支付响应详情
    [System.Serializable]
    public class AliPayTradeAppPayResponse
    {
        public string code;
        public string msg;
        public string out_trade_no; // 商户订单号
        public string trade_no;     // 支付宝交易号
        public string total_amount; // 交易金额
                                    // 可根据需要添加其他字段
    }

    // 错误响应详情
    [System.Serializable]
    public class AliPayTradeAppPayErrorResponse
    {
        public string code;
        public string msg;
        public string sub_code;
        public string sub_msg;
    }

}


