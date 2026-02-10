
//---------------------------------------------------------------------
// Alert
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-16 11:17:08
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LieyouFramework
{
	public class Alert : Dialog
	{
		private Image _bgImage;
		private TextMeshProUGUI _messageText;
		private Animator _animator;

		private void Awake()
		{
			_bgImage = GetCompoentWithPath<Image>("AlertBg");
			_messageText = GetCompoentWithPath<TextMeshProUGUI>("AlertBg/MessgaeText");
			_animator = GetComponent<Animator>();
		}

		public void Init(string _infoStr, float _time)
		{
			_messageText.text = _infoStr;

			float min = 500;
			float max = Screen.width * 0.56f;
			float w = _messageText.preferredWidth;

			if (w < min)
			{
				w = min;
			}
			else if (w > max)
			{
				w = max;
			}

			Utility.UGUI.SetWidth(_bgImage.gameObject, w);
			CoroutineManager.Instance.Delay(_time, () =>
			{
				Close();
			});
		}

		void Close() 
		{
			CoroutineManager.Instance .Delay(0.5f, () =>
			{
				_animator.Play("AlertAnimationOut");
				Destroy(gameObject);
			});
		}
	}
}
