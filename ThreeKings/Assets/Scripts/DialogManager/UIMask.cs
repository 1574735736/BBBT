
//---------------------------------------------------------------------
// DalaranFramework
// Copyright © 2013-2020 Dalaran Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2021/3/1 11:39:30
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

namespace LieyouFramework
{
	public class UIMask : MonoBehaviour
	{
		//private List<Tweener> tweeners = new List<Tweener>();

		/// <summary>
		/// 蒙版淡入
		/// </summary>
		/// <param name="_time">淡入时间</param>
		public void FadeIn(float _time = 0.5f)
		{
			CanvasGroup canvasGroup = Utility.UGUI.SetAlpha(gameObject, 0);
			//Tweener fadeIn = canvasGroup.DOFade(1, _time);
			//SetTweenAutoDestory(fadeIn);
		}

		/// <summary>
		/// 蒙版淡出
		/// </summary>
		/// <param name="_time">淡出时间</param>
		public void FadeOut(float _time = 0.4f)
		{
			CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				canvasGroup = gameObject.AddComponent<CanvasGroup>();
			}
			//Tweener fadeIn = canvasGroup.DOFade(0, _time);
			//SetTweenAutoDestory(fadeIn);
		}
	}
}
