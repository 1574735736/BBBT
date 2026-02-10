
//---------------------------------------------------------------------
// DialogShowHideBaseAnim
// Copyright © 2019-2023 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-10-26 09:08:58
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LieyouFramework
{
	public class DialogShowHideBaseAnim : MonoBehaviour
	{
		public float durationIn = 0.4f;
		public float durationOut = 0.3f;

		private Action _showAnimEnd;

		private void OnEnable()
		{
			ShowWithAnim(ShowAnimEnd);
		}

		public virtual void ShowWithAnim(Action ac = null)
		{

		}

		public virtual void CloseWithAnim(Action ac = null)
		{

		}

		private void ShowAnimEnd() 
		{
			_showAnimEnd?.Invoke();
			_showAnimEnd = null;
		}

		public void SetShowAnimEndCallback(Action cb)
		{
			_showAnimEnd = cb;
		}
	}
}
