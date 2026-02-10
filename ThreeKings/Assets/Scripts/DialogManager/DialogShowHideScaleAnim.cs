
//---------------------------------------------------------------------
// DialogShowHideScaleAnim
// Copyright © 2019-2023 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-10-26 09:12:52
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LieyouFramework
{
	public class DialogShowHideScaleAnim : DialogShowHideBaseAnim
	{
		[SerializeField] Ease _showEase = Ease.OutBack;
		[SerializeField] Ease _hideEase = Ease.InBack;
		private Tween _twn;
		private float _oldScale;

		public override void ShowWithAnim(Action _ac = null)
		{
			base.ShowWithAnim(_ac);
			transform.localScale = Vector3.zero;
			_twn?.Kill();
			DialogFix fix = GetComponent<DialogFix>();
			_oldScale = 1;
			if (fix != null)
            {
				_oldScale = fix.GetScale();
            }

			_twn = transform.DOScale(Vector3.one * _oldScale, durationIn).SetEase(_showEase).OnComplete(() =>
			{
				_ac?.Invoke();
			});
		}

		public override void CloseWithAnim(Action _ac = null)
		{
			base.CloseWithAnim(_ac);
			transform.localScale = Vector3.one * _oldScale;
			_twn?.Kill();
			_twn = transform.DOScale(Vector3.zero, durationOut).OnComplete(() => { _ac?.Invoke(); }).SetEase(_hideEase);
		}

		private void OnDestroy()
		{
			_twn?.Kill();
		}
	}
}
