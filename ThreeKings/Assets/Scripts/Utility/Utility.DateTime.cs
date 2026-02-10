
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LieyouFramework
{
	public static partial class Utility 
	{
		/// <summary>
		/// 日期和时间
		/// </summary>
		public static class DateTime 
		{
			private static System.DateTime s_startTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

			/// <summary>
			/// 获取当前时间的毫秒级时间戳
			/// </summary>
			/// <returns></returns>
			public static long GetTimeStamp() 
			{
				TimeSpan ts = System.DateTime.UtcNow - s_startTime;
				return Convert.ToInt64(ts.TotalMilliseconds);
			}

			/// <summary>
			/// 获取指定时间的毫秒级时间戳
			/// </summary>
			/// <param name="time">指定时间</param>
			/// <returns></returns>
			public static long GetTimeStamp(System.DateTime time)
			{
				TimeSpan ts = time - s_startTime;
				return Convert.ToInt64(ts.TotalMilliseconds);
			}

			/// <summary>
			/// 获取当前时间的秒级时间戳
			/// </summary>
			/// <returns></returns>
			public static long GetTimeStampSecond()
			{
				TimeSpan ts = System.DateTime.UtcNow - s_startTime;
				return Convert.ToInt64(ts.TotalSeconds);
			}

			/// <summary>
			/// 获取指定时间的秒级时间戳
			/// </summary>
			/// <param name="time">指定时间</param>
			/// <returns></returns>
			public static long GetTimeStampSecond(System.DateTime time)
			{
				TimeSpan ts = time - s_startTime;
				return Convert.ToInt64(ts.TotalSeconds);
			}

			/// <summary>
			/// 毫秒级时间戳转换为时间
			/// </summary>
			/// <param name="timeStamp"></param>
			/// <returns></returns>
			public static System.DateTime TimeStampToTime(long timeStamp)
			{
				DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp);
				System.DateTime localDateTime = dateTimeOffset.LocalDateTime;
				return localDateTime;
			}

			/// <summary>
			/// 秒级时间戳转换为时间
			/// </summary>
			/// <param name="timeStamp"></param>
			/// <returns></returns>
			public static System.DateTime TimeStampToTimeSecond(long timeStamp)
			{
				DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
				System.DateTime localDateTime = dateTimeOffset.LocalDateTime;
				return localDateTime;
			}

			/// <summary>
			/// 剩余时间显示-分钟
			/// </summary>
			/// <param name="left">剩余时间</param>
			/// <param name="format">格式</param>
			/// <returns></returns>
			public static string FormatLeftTimeToMinutes(int left, string format = "{0:D2}:{1:D2}")
			{
				int m = left / 60;
				int s = left % 60;
				string str = string.Format(format, m, s);
				return str;
			}

			/// <summary>
			/// 剩余时间显示-小时
			/// </summary>
			/// <param name="left">剩余时间</param>
			/// <param name="format">格式</param>
			/// <returns></returns>
			public static string FormatLeftTimeToHoure(int left, string format = "{0:D2}:{1:D2}:{2:D2}")
			{
				int h = left / (60 * 60);
				int m = left % (60 * 60) / 60;
				int s = left % (60 * 60) % 60;
				string str = string.Format(format, h, m, s);
				return str;
			}
		}
	}
}
