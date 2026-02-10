
//---------------------------------------------------------------------
// AlertOK
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-16 11:17:27
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace LieyouFramework
{
	public class AlertOK : Dialog
	{
		private Image titleBG;
		private TextMeshProUGUI titleText;
		private TextMeshProUGUI messageText;
		private Button okBtn;
		private Button okBtn1;
		private Button cancelBtn;

		private void Awake()
		{
			titleBG = GetCompoentWithPath<Image>("TitleBg");
			titleText = GetCompoentWithPath<TextMeshProUGUI>("TitleBg/TitleText");
			messageText = GetCompoentWithPath<TextMeshProUGUI>("MessageText");
			okBtn = GetCompoentWithPath<Button>("OkBtn");
			okBtn1 = GetCompoentWithPath<Button>("OkBtn1");
			cancelBtn = GetCompoentWithPath<Button>("CancelBtn");
			okBtn.onClick.AddListener(OnOkBtnClick);
			okBtn1.onClick.AddListener(OnOkBtn1Click);
			cancelBtn.onClick.AddListener(OnCancelBtnClick);
		}

		public virtual void ShowAlertOK(string _message)
		{
			okBtn.gameObject.SetActive(true);
			okBtn1.gameObject.SetActive(false);
			cancelBtn.gameObject.SetActive(false);

			messageText.text = _message;
		}

		public virtual void ShowAlertAndCancel(string _message)
		{
			okBtn.gameObject.SetActive(false);
			okBtn1.gameObject.SetActive(true);
			cancelBtn.gameObject.SetActive(true);
			messageText.text = _message;
		}

		//是否显示标题
		public virtual void IsShowTitle(bool _isShow)
		{
			titleBG.gameObject.SetActive(_isShow);
		}

		//设置标题内容
		public virtual void SetTitleText(string _titleStr)
		{
			titleText.text = _titleStr;
		}

		//设置确定按钮文字内容
		public virtual void SetOKBtnTitle(string _titleStr)
		{
			okBtn1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _titleStr;
			okBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _titleStr;
		}

		//设置取消按钮
		public virtual void SetCancelBtnTitle(string _titleStr)
		{
			cancelBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _titleStr;
		}

		void OnOkBtnClick()
		{
			Result = UIResult.OK;
			Close();
		}

		void OnOkBtn1Click()
		{
			Result = UIResult.OK;
			Close();
		}

		void OnCancelBtnClick()
		{
			Result = UIResult.Cancel;
			Close();
		}
	}
}
