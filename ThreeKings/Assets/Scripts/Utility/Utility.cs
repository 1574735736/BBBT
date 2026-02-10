
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UtilityBase
{
    public static string ConvertTimestampToCountdown(long targetTimestamp)
    {
        // 1. 获取当前时间的Unix时间戳（秒）
        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // 2. 计算剩余秒数（若目标时间已过，剩余秒数为0）
        long remainingSeconds = Math.Max(0, targetTimestamp - currentTimestamp);

        // 3. 转换剩余秒数为天、时、分、秒
        int days = (int)(remainingSeconds / 86400); // 1天 = 86400秒
        int hours = (int)(remainingSeconds % 86400 / 3600); // 1小时 = 3600秒
        int minutes = (int)(remainingSeconds % 3600 / 60); // 1分钟 = 60秒
        int seconds = (int)(remainingSeconds % 60);

        // 4. 格式化输出（根据需求调整格式）
        if (days > 0)
        {
            // 有天数时：xx天 xx:xx:xx
            return $"{days}天{hours:D2}时{minutes:D2}分";
            //$":{seconds:D2}";
        }
        else
        {
            // 无天数时：xx:xx:xx（若只需到分钟则改为xx:xx）
            return $"{hours:D2}时{minutes:D2}分";   //{seconds:D2}";
        }
    }

    public static string FormatWealth(float amount)
    {
        if (amount >= 100000000) // 1亿
        {
            return FormatNumber(amount / 100000000f, "e");
        }
        else if (amount >= 1000000) // 1百万
        {
            return FormatNumber(amount / 1000000f, "m");
        }
        else if (amount >= 1000) // 1千
        {
            return FormatNumber(amount / 1000f, "k");
        }
        else
        {
            return FormatNumber(amount, "");
        }
    }

    private static string FormatNumber(float number, string suffix)
    {
        string formatted = number.ToString("0.00");
        if (formatted.EndsWith(".00"))
        {
            formatted = formatted.Substring(0, formatted.Length - 3);
        }
        return $"{formatted}{suffix}";
    }

    /// <summary>
    /// 获取到期时间的剩余时间
    /// </summary>
    /// <param name="expireTimestamp"></param>
    /// <returns></returns>
    public static string GetRemainingTime(long expireTimestamp)
    {
        // 获取当前时间（使用UTC避免时区问题）
        DateTime now = DateTime.UtcNow;

        // 将Unix时间戳转换为DateTime（UTC时间）
        DateTime expireTime = DateTime.UnixEpoch.AddSeconds(expireTimestamp).ToUniversalTime();

        // 检查是否过期
        if (now >= expireTime)
        {
            return "";
        }

        // 计算时间差
        TimeSpan remaining = expireTime - now;

        // 获取整天数和剩余小时数
        int days = remaining.Days;
        int hours = remaining.Hours;  // TimeSpan自动扣除整天后的小时数

        // 处理不足1小时的进位（确保至少显示1小时）
        if (days == 0 && hours == 0 && remaining.Minutes > 0)
        {
            hours = 1;  // 不足1小时按1小时显示
        }

        return $"{days}天";//$"{days}天{hours}小时";
    }
}
