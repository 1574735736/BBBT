
//---------------------------------------------------------------------
// CoroutineManager
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-07 11:03:30
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LieyouFramework
{
	public class CoroutineManager
	{
		public static readonly CoroutineManager Instance = new CoroutineManager();

		private GCoroutineMono _coroutineMono;

		CoroutineManager() 
		{
			Utility.Debug.LogFM("CoroutineManager init");
			_coroutineMono = new GameObject("GCoroutineMono").AddComponent<GCoroutineMono>();
		}

		public GCoroutineMono GetRunObject()
		{ 
			return _coroutineMono;
		}

		/// <summary>
		/// 启动一个全局协程
		/// </summary>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public Coroutine StartCoroutine(string methodName) 
		{ 
			return _coroutineMono.StartCoroutine(methodName);
		}

		/// <summary>
		/// 启动一个全局协程
		/// </summary>
		/// <param name="routine"></param>
		/// <returns></returns>
		public Coroutine StartCoroutine(IEnumerator routine) 
		{ 
			return _coroutineMono.StartCoroutine(routine);
		}

		/// <summary>
		/// 停止一个全局协程
		/// </summary>
		/// <param name="routine"></param>
		public void Stop(Coroutine routine)
		{
			if (routine != null)
			{
				_coroutineMono.StopCoroutine(routine);
			}
		}

		/// <summary>
		/// 停止一个全局协程
		/// </summary>
		/// <param name="routine"></param>
		public void Stop(IEnumerator routine)
		{
			if (routine != null) 
			{
				_coroutineMono.StopCoroutine(routine);
			}
		}

		/// <summary>
		/// 延时调用
		/// </summary>
		/// <param name="time">时间</param>
		/// <param name="callback">回调</param>
		/// <returns></returns>
		public Coroutine Delay(float time, Action callback)
		{
			return _coroutineMono.Delay(time, callback);
		}

		/// <summary>
		/// 在主线程执行
		/// </summary>
		/// <param name="action"></param>
		public void ExecuteOnMainThread(Action action)
		{
			_coroutineMono.ExecuteOnMainThread(action);
		}

	}
}
