
//---------------------------------------------------------------------
// AlertLoading
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-16 11:18:00
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LieyouFramework
{
	public class AlertLoading : BaseMonoBehaviour
	{
		private TextMeshProUGUI _loadingText;

		private void Awake()
		{
			_loadingText = GetCompoentWithPath<TextMeshProUGUI>("LoadingText");
		}

		public virtual void Show(string msg) 
		{
			gameObject.SetActive(true);
			transform.SetAsLastSibling();
			if (!string.IsNullOrEmpty(msg) && _loadingText != null)
			{
				_loadingText.gameObject.SetActive(true);
				_loadingText.text = msg;
			}
			else 
			{
				_loadingText.gameObject.SetActive(false);
			}
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
