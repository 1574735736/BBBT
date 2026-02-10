
//---------------------------------------------------------------------
// DialogFix
// Copyright © 2019-2023 LieYou Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-11-08 10:36:24
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LieyouFramework
{
	/// <summary>
	/// 框架的分辨率是以1080x1920或1920x1080做的
	/// 但是实际项目中可能是另外的分辨率，所以或导致弹窗很大或者很小
	/// 该类只要是对框架的弹窗做适配的
	/// </summary>
	public class DialogFix : MonoBehaviour
	{
		private void Awake()
		{
			SetScale();
		}

		void SetScale() 
		{
			DialogShowHideScaleAnim sacle = GetComponent<DialogShowHideScaleAnim>();
			if (sacle != null) return;
			float s = GetScale();
			transform.localScale = Vector3.one * s;
		}

		public float GetScale() 
		{
			float x = FrameworkConfig.ReferenceResolution.x;
			float y = FrameworkConfig.ReferenceResolution.y;
			if (x > y)
			{
				float sx = x / 1920;
				float sy = y / 1080;
				float s = Mathf.Max(sx, sy);
				return s;
			}
			else
			{
				float sx = x / 1080;
				float sy = y / 1920;
				float s = Mathf.Max(sx, sy);
				return s;
			}
		}
	}
}
