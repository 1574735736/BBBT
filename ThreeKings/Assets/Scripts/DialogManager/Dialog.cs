
//---------------------------------------------------------------------
// XRLGame
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2022-10-31 16:13:27
// Feedback: yang2686022430@163.com.
//---------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LieyouFramework
{
	public class Dialog : BaseMonoBehaviour
	{
		public Button CloseButton;

		internal object Result { get; set; }
		internal string DialogPath = string.Empty;
		internal bool touchEnable = true;

		Action<object> _closedCallback;
		GameObject mask;

		//面板过渡动画
		private DialogShowHideBaseAnim[] _showHideEffects;

		public virtual void Init(object param)
		{
			_showHideEffects = transform.GetComponents<DialogShowHideBaseAnim>();
			if (_showHideEffects.Length > 0)
			{
				_showHideEffects[0].SetShowAnimEndCallback(ShowAnimEnd);
			}
			CloseButton?.onClick.AddListener(OnCloseBtnClick);
		}

		/// <summary>
		/// 面板动画结束回调
		/// </summary>
		public virtual void ShowAnimEnd()
		{

		}

		/// <summary>
		/// 初始化遮罩
		/// </summary>
		/// <param name="_maskObj">遮罩物体</param>
		/// <param name="_clickDisappear">是否支持单击消失</param>
		public void InitMask(GameObject maskObj, bool _clickDisappear)
		{
			mask = maskObj;
			if (maskObj != null && _clickDisappear)
			{
				//添加UGUI事件监听器
				EventTrigger trigger = mask.AddComponent<EventTrigger>();

				//添加OnClick事件
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener(OnClickMask);
				trigger.triggers.Add(entry);
			}
		}

		/// <summary>
		/// 蒙版淡入
		/// </summary>
		/// <param name="_time">时间</param>
		public void FadeInMask(float _time = 0.5f)
		{
			if (mask != null)
			{
				mask.GetComponent<UIMask>().FadeIn(_time);
			}
		}

		/// <summary>
		/// 设置蒙版颜色
		/// </summary>
		/// <param name="_color"></param>
		public void SetMaskColor(Color _color)
		{
			if (mask != null) 
			{
				Image img = mask.GetComponent<Image>();
				img.color = _color;
			}
		}

		public void InitCallback(Action<object> _callback)
		{
			_closedCallback = _callback;
		}

		public virtual void Close(bool _anim = true) 
		{
			if (_anim)
			{
				if (!CheckShowHideEffect())
				{
					DestaroyPanel();
				}
			}
			else
			{
				DestaroyPanel();
			}
		}

		void OnCloseBtnClick()
		{
			Close(true);
		}

		void OnClickMask(BaseEventData baseEventData)
		{
			if (touchEnable)
			{
				Close();
			}
		}

		public void DestaroyPanel()
		{
			GameObject.Destroy(gameObject);
			//GI.DialogMgr.ReleaseMask(mask);
			_closedCallback?.Invoke(Result);

			//GI.DialogMgr.DelOpenData();
			//GI.DialogMgr.TryOpen();
		}

		//判定是否挂载面板关闭过渡动画
		public bool CheckShowHideEffect()
		{
			if (_showHideEffects == null || _showHideEffects.Length == 0) { return false; }
			DialogShowHideBaseAnim maxTimeEffect = _showHideEffects[0];
			for (int i = 1; i < _showHideEffects.Length; ++i)
			{
				if (maxTimeEffect.durationOut < _showHideEffects[i].durationOut)
				{
					maxTimeEffect = _showHideEffects[i];
				}
			}
			for (int i = 0; i < _showHideEffects.Length; ++i)
			{
				if (maxTimeEffect == _showHideEffects[i])
				{
					_showHideEffects[i].CloseWithAnim(DestaroyPanel);
				}
				else
				{
					_showHideEffects[i].CloseWithAnim();
				}
			}
			return true;
		}
	}
}
