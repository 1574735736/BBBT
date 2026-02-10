
using System;
using UnityEngine;

public class FrameworkConfig
{
    /// <summary>
    /// 版本号
    /// </summary>
    public static string Version = "1.0.0";

    /// <summary>
    /// Http超时时间
    /// </summary>
    public static int HttpTimeout = 5;

    /// <summary>
    /// 默认请求地址
    /// </summary>
    public static string BaseUrl = "http://ce.youtaishenghuo.com/api"; //"http://192.144.185.176/api";

    /// <summary>
    /// 默认请求地址
    /// </summary>
    public static string DataUrl = "https://192.168.0.129";

    /// <summary>
    /// 设计分辨率
    /// </summary>
    public static Vector2 ReferenceResolution = new Vector2(1080, 1920);

    /// <summary>
    /// 弹窗层级
    /// </summary>
    public static int DialogSortingOrder = 99;

    /// <summary>
    /// 弹窗距离相机的距离
    /// </summary>
    public static int DialogPlaneDistance = 100;

    /// <summary>
    /// Alert层级
    /// </summary>
    public static int AlertSortingOrder = 999;

    /// <summary>
    /// Alert距离弹窗的距离
    /// </summary>
    public static int AlertPlaneDistance = 100;

    /// <summary>
    /// 框架相机深度
    /// </summary>
    public static int FMCameraDepth = 10;

    /// <summary>
    /// 全局菊花预制体路径,可自定义样式
    /// </summary>
    public static string FMLoadingPanelPath = "Prefabs/CW_LoadingPanel";

    /// <summary>
    /// 警告弹窗路径,可自定义样式
    /// </summary>
    public static string FMAlertOKPath = string.Empty;

    /// <summary>
    /// 多语言图片路径
    /// </summary>
    public static string GameLanguageImagePath = "Textures/Language/";

    /// <summary>
    /// 多语言文本路径
    /// </summary>
    public static string GameLanguageTextPath = "Language/";

    /// <summary>
    /// 是否使用框架相机
    /// </summary>
    public static bool UseFMCamera = false;

    /// <summary>
    /// 画布渲染模式
    /// </summary>
    public static RenderMode RenderMode = RenderMode.ScreenSpaceOverlay;

    /// <summary>
    /// 配置表密钥
    /// 取值范围：0-255
    /// </summary>
    public static int ConfigKey = 33;

    public static bool FreeBuy = false;
}

