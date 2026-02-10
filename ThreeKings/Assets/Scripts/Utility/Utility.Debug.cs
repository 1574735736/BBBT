
#if LIEYOU_KS
using com.kwai.mini.game;
#endif
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace LieyouFramework
{
	public static partial class Utility
	{
		/// <summary>
		/// 日志类
		/// </summary>
		public static class Debug
		{
			/// <summary>
			/// 是否打印框架日志
			/// </summary>
			public static bool IsLogFM = true;

			/// <summary>
			/// 是否打印SDK日志
			/// </summary>
			public static bool IsLogSDK = true;

			/// <summary>
			/// 是否打印游戏日志
			/// </summary>
			public static bool IsLogGame = true;

			/// <summary>
			/// 打印框架日志
			/// </summary>
			/// <param name="log">日志内容</param>
			public static void LogFM(object log)
			{
				if (!IsLogFM) return;
				if (!UnityEngine.Debug.unityLogger.logEnabled) return;
#if LIEYOU_KS
				KS.Log("LYFM." + log);
#else
				UnityEngine.Debug.Log("LYFM." + log);
#endif
			}

			/// <summary>
			/// 打印SDK日志
			/// </summary>
			/// <param name="log"></param>
			public static void LogSDK(string log)
			{
				if (!IsLogSDK) return;
				if (!UnityEngine.Debug.unityLogger.logEnabled) return;
#if LIEYOU_KS
				KS.Log("LYSDK." + log);
#else
				UnityEngine.Debug.Log("LYSDK." + log);
#endif
			}

			/// <summary>
			/// 打印游戏日志
			/// </summary>
			public static void Log(string log)
			{
				if (!IsLogGame) return;
				if (!UnityEngine.Debug.unityLogger.logEnabled) return;
#if LIEYOU_KS
				KS.Log("LY." + log);
#else
				UnityEngine.Debug.Log("LY." + log);
#endif
			}

			/// <summary>
			/// 打印游戏日志
			/// </summary>
			public static void LogWarning(string log)
			{
				if (!IsLogGame) return;
				if (!UnityEngine.Debug.unityLogger.logEnabled) return;
#if LIEYOU_KS
				KS.Log("LY." + log);
#else
				UnityEngine.Debug.LogWarning("LY." + log);
#endif
			}

			/// <summary>
			/// 打印游戏日志
			/// </summary>
			public static void LogError(string log)
			{
				if (!IsLogGame) return;
				if (!UnityEngine.Debug.unityLogger.logEnabled) return;
#if LIEYOU_KS
				KS.Log("LY." + log);
#else
				UnityEngine.Debug.LogError("LY." + log);
#endif
			}

			/// <summary>
			/// 不输出日志
			/// </summary>
			public static void Release()
			{
				IsLogFM = false;
				IsLogSDK = false;
				IsLogGame = false;
			}
		}
	}
}
