
//---------------------------------------------------------------------
// BaseMonoBehaviour
// Copyright © 2019-2023 LieYou Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-30 11:29:52
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LieyouFramework
{
	public class BaseMonoBehaviour : MonoBehaviour
	{
		public T GetCompoentWithPath<T>(string objPath) where T : Component
		{
			Transform obj = transform.Find(objPath);
			return obj.GetComponent<T>();
		}

		public T GetCompoentWithParent<T>(Transform parent, string objPath) where T : Component
		{
			Transform obj = parent.Find(objPath);
			return obj.GetComponent<T>();
		}

		public T GetOrAndCompoentWithPath<T>(string objPath) where T : Component
		{
			GameObject obj = transform.Find(objPath).gameObject;
			return obj.GetComponent<T>() ?? obj.AddComponent<T>();
		}
	}
}
