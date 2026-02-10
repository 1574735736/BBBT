
//---------------------------------------------------------------------
// GCoroutineMono
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-07 11:13:29
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LieyouFramework
{
	public class GCoroutineMono : MonoBehaviour
	{
		private static readonly Queue<Action> executeOnMainThreadQueue = new Queue<Action>();
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public Coroutine Delay(float time, Action callback)
		{
			Coroutine coroutine = StartCoroutine(DeleyIEnumerator(time, callback));
			return coroutine;
		}

		/// <summary>
		/// 在主线程执行
		/// </summary>
		/// <param name="action"></param>
		public void ExecuteOnMainThread(Action action)
		{
			lock (executeOnMainThreadQueue)
			{
				executeOnMainThreadQueue.Enqueue(action);
			}
		}

		public IEnumerator DeleyIEnumerator(float time, Action callback)
		{
			yield return new WaitForSeconds(time);
			callback?.Invoke();
		}

		void Update()
		{
			// dispatch stuff on main thread
			while (executeOnMainThreadQueue.Count > 0)
			{
				Action dequeuedAction = null;
				lock (executeOnMainThreadQueue)
				{
					try
					{
						dequeuedAction = executeOnMainThreadQueue.Dequeue();
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}
				if (dequeuedAction != null)
				{
					dequeuedAction.Invoke();
				}
			}
		}

	}
}
