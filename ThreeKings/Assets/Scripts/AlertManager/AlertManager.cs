
//---------------------------------------------------------------------
// AlertManager
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-16 11:16:43
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LieyouFramework
{
	public class AlertManager
	{
		public static readonly AlertManager Instance = new AlertManager();
		public Canvas AlertCanvas { get; private set; }

		private AlertLoading _loadingPanel;
		private Coroutine _loadingCoroutine;

		AlertManager()
		{
			Utility.Debug.LogFM("AlertManager init");
			CreateAlertCanvas();
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		}

		public Alert Show(string _str, float _time = 1.6f)
		{
			GameObject a = Resources.Load<GameObject>("Prefabs/CW_Alert");
			GameObject go = GameObject.Instantiate(a, AlertCanvas.transform, false);
			Alert alert = go.GetComponent<Alert>();
			alert.Init(_str, _time);
			return alert;
		}

		public Alert ShowLan(string _lanKey, float _time = 1.6f)
		{
			//string str = GI.Language.GetText(_lanKey);
			//return Show(str, _time);
			return Show(_lanKey, _time);
		}


		public AlertOK AlertOK(string message, Action okBtnClick = null)
		{
			string path = GetPath();
			//Dialog dialog = GI.DialogMgr.Create(path, null, AlertCanvas.transform, (r) =>
			//{
			//	UIResult result = (UIResult)r;
			//	if (result == UIResult.OK)
			//	{
			//		okBtnClick?.Invoke();
			//	}
			//});
			//AlertOK alert = dialog as AlertOK;
			//alert.ShowAlertOK(message);
			//return alert;
			return null;
		}

		public AlertOK AlertOkAndCancel(string message, Action okBtnClick = null, Action cancelBtnClick = null)
		{
			string path = GetPath();
			//Dialog dialog = GI.DialogMgr.Create(path,null, AlertCanvas.transform, (r) =>
			//{
			//	UIResult result = (UIResult)r;
			//	if (result == UIResult.OK)
			//	{
			//		okBtnClick?.Invoke();
			//	}
			//	else
			//	{
			//		cancelBtnClick?.Invoke();
			//	}
			//});
			//AlertOK alert = dialog as AlertOK;
			//alert.ShowAlertAndCancel(message);
			//return alert;
			return null;
		}

		public AlertLoading ShowLoading(float timeut = 5, string msg = default)
		{
			CoroutineManager.Instance.Stop(_loadingCoroutine);
			_loadingCoroutine = null;
			if (_loadingPanel == null)
			{
				string path = FrameworkConfig.FMLoadingPanelPath;
				GameObject loadingObj = Resources.Load<GameObject>(path);
				GameObject a = GameObject.Instantiate(loadingObj, AlertCanvas.transform, false);
				_loadingPanel = a.GetComponent<AlertLoading>();
				_loadingPanel.Show(msg);
			}
			else
			{
				_loadingPanel.Show(msg);
			}

			_loadingCoroutine = CoroutineManager.Instance.Delay(timeut, () =>
			{
				HideLoading();
			});

			return _loadingPanel;
		}

		public void HideLoading()
		{
			CoroutineManager.Instance.Stop(_loadingCoroutine);
			_loadingCoroutine = null;
			if (_loadingPanel != null)
			{
				_loadingPanel.Hide();
			}
		}

		public void SetCamera(Camera camera)
		{
			AlertCanvas.worldCamera = camera;
		}

		private void CreateAlertCanvas()
		{
			GameObject obj = new GameObject("AlertCanvas");
			obj.transform.position = new Vector3(0, 0, 90);
			GameObject.DontDestroyOnLoad(obj);
			AlertCanvas = obj.AddComponent<Canvas>();
			AlertCanvas.renderMode = FrameworkConfig.RenderMode;

			//if (FrameworkConfig.UseFMCamera)
			//{
			//	AlertCanvas.worldCamera = GI.Camera;
			//}
			//else
			//{
				AlertCanvas.worldCamera = Camera.main;
			//}

			AlertCanvas.planeDistance = FrameworkConfig.AlertPlaneDistance;
			AlertCanvas.gameObject.layer = 5;
			AlertCanvas.sortingOrder = FrameworkConfig.AlertSortingOrder;
			AlertCanvas.gameObject.AddComponent<CanvasScaler>();
			AlertCanvas.gameObject.AddComponent<GraphicRaycaster>();
			AlertCanvas.gameObject.AddComponent<CanvasFix>();
		}

		private string GetPath()
		{
			string path = FrameworkConfig.FMAlertOKPath;
			if (string.IsNullOrEmpty(FrameworkConfig.FMAlertOKPath))
			{
				if (FrameworkConfig.ReferenceResolution.x > FrameworkConfig.ReferenceResolution.y) //横屏
				{
					path = "Prefabs/CW_LSAlertOK";
				}
				else //竖屏
				{
					path = "Prefabs/CW_PAlertOK";
				}
			}
			return path;
		}

		void OnSceneLoaded(Scene s, LoadSceneMode m)
		{
			//if (FrameworkConfig.UseFMCamera)
			//{
			//	AlertCanvas.worldCamera = GI.Camera;
			//}
			//else
			//{
				AlertCanvas.worldCamera = Camera.main;
			//}
		}
	}
}
