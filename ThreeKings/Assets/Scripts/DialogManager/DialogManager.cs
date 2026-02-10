
//---------------------------------------------------------------------
// XRLGame
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2022-10-28 10:20:40
// Feedback: yang2686022430@163.com.
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LieyouFramework
{
	public class DialogManager
	{
		public static readonly DialogManager Instance = new DialogManager();
		public Canvas DialogCanvas { get; private set; }
		public Dictionary<string, Dialog> Dialogs = new Dictionary<string, Dialog>();

		Queue<OpenData> _panelsQueue = new Queue<OpenData>();
		OpenData _openData = null;

		DialogManager()
		{
			Utility.Debug.LogFM("DialogManager init");
			InitCanvas();
			Init();
		}

		public void Init()
		{
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		}

		public void InitCanvas()
		{
			if (DialogCanvas == null)
			{
				GameObject canvasObj = new GameObject("DialogCanvas");
				DialogCanvas = canvasObj.AddComponent<Canvas>();
				DialogCanvas.renderMode = FrameworkConfig.RenderMode;

				//if (FrameworkConfig.UseFMCamera)
				//{
				//	DialogCanvas.worldCamera = GI.Camera;
				//}
				//else
				//{
				//	DialogCanvas.worldCamera = Camera.main;
				//}
				DialogCanvas.worldCamera = Camera.main;


                DialogCanvas.planeDistance = FrameworkConfig.DialogPlaneDistance;
				DialogCanvas.gameObject.layer = 5;
				DialogCanvas.sortingOrder = FrameworkConfig.DialogSortingOrder;
				CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				GraphicRaycaster graphicRaycaster = canvasObj.AddComponent<GraphicRaycaster>();
				canvasObj.AddComponent<CanvasFix>();
				GameObject.DontDestroyOnLoad(canvasObj);
			}
			else
			{
                //if (FrameworkConfig.UseFMCamera)
                //{
                //	DialogCanvas.worldCamera = GI.Camera;
                //}
                //else
                //{
                //	DialogCanvas.worldCamera = Camera.main;
                //}

                DialogCanvas.worldCamera = Camera.main;
            }
		}

		public Dialog Create(string path, Action<object> cb = null)
		{
			return CreateDialog(path, null, DialogCanvas.transform, PanelMaskMode.Block, cb);
		}

		public Dialog Create(string path, object _param, Action<object> cb = null)
		{
			return CreateDialog(path, _param, DialogCanvas.transform, PanelMaskMode.Block, cb);
		}

		public Dialog Create(string path, object _param, Transform parent, Action<object> cb = null)
		{
			return CreateDialog(path, _param, parent, PanelMaskMode.Block, cb);
		}

		public Dialog Create(string path, object _param, PanelMaskMode _maskMode, Action<object> cb = null)
		{
			return CreateDialog(path, _param, DialogCanvas.transform, _maskMode, cb);
		}

		public void Open(string path, Action<object> cb = null)
		{
			OpenData openData = new OpenData();
			openData.path = path;
			openData.closCallback = cb;
			_panelsQueue.Enqueue(openData);
			TryOpen();
		}

		public void Open(string path, object param, Action<object> cb = null)
		{
			OpenData openData = new OpenData();
			openData.path = path;
			openData.data = param;
			openData.closCallback = cb;
			_panelsQueue.Enqueue(openData);
			TryOpen();
		}

		public void Open(string path, object param, PanelMaskMode maskMode, Action<object> cb = null)
		{
			OpenData openData = new OpenData();
			openData.path = path;
			openData.data = param;
			openData.closCallback = cb;
			openData.maskMode = maskMode;
			_panelsQueue.Enqueue(openData);
			TryOpen();
		}

		public void Add(string path, Action<object> cb = null)
		{
			OpenData openData = new OpenData();
			openData.path = path;
			openData.closCallback = cb;
			_panelsQueue.Enqueue(openData);
		}

		public void Add(string path, object param, Action<object> cb = null)
		{
			OpenData openData = new OpenData();
			openData.path = path;
			openData.data = param;
			openData.closCallback = cb;
			_panelsQueue.Enqueue(openData);
		}

		public void Add(string path, object param, PanelMaskMode maskMode, Action<object> cb = null)
		{
			OpenData openData = new OpenData();
			openData.path = path;
			openData.data = param;
			openData.closCallback = cb;
			openData.maskMode = maskMode;
			_panelsQueue.Enqueue(openData);
		}

		public void TryOpen()
		{
			if (_openData != null) return;
			if (_panelsQueue.Count > 0)
			{
				_openData = _panelsQueue.Dequeue();
				Create(_openData.path, _openData.data, _openData.maskMode, _openData.closCallback);
			}
		}

		public void DelOpenData()
		{
			_openData = null;
		}

		public bool NeedTryOpen(string prefPath)
		{
			foreach (var panel in _panelsQueue)
			{
				if (panel.path.Equals(prefPath))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		public void ReleaseMask(GameObject mask)
		{
			GameObject.Destroy(mask);
		}

		public bool HasDialog(string path)
		{
			return Dialogs.ContainsKey(path);
		}

		public bool HasDialog()
		{
			return Dialogs.Count > 0;
		}

		Dialog CreateDialog(string path, object param, Transform parent, PanelMaskMode _maskMode, Action<object> cb = null)
		{
			if (HasDialog(path))
			{
				Debug.LogError("Dialog already exists，path:" + path);
				return null;
			}

			GameObject uiObj = null;
			GameObject maskObj = null;
			Dialog panel = null;
			bool maskClick = false;

			if (_maskMode == PanelMaskMode.None)
			{

			}
			else if (_maskMode == PanelMaskMode.Block)
			{
				GameObject obj = Resources.Load<GameObject>("Prefabs/CW_UIMask");
				maskObj = GameObject.Instantiate(obj, parent, false);
				maskObj.transform.SetAsLastSibling();
			}
			else if (_maskMode == PanelMaskMode.ClickDisappear)
			{
				GameObject obj = Resources.Load<GameObject>("Prefabs/CW_UIMask");
				maskObj = GameObject.Instantiate(obj, parent, false);
				maskObj.transform.SetAsLastSibling();
				maskClick = true;
			}

			GameObject go = Resources.Load<GameObject>(path);
			if (go == null)
			{
				throw new Exception("not find dialog " + path);
			}
			uiObj = GameObject.Instantiate(go, parent, false);
			panel = uiObj.GetComponent(typeof(Dialog)) as Dialog;
			if (panel == null) { panel = uiObj.AddComponent<Dialog>(); }

			Action<object> action = (obj) =>
			{
				Dialogs.Remove(path);
				cb?.Invoke(obj);
			};

			panel.Init(param);
			panel.InitMask(maskObj, maskClick);
			panel.InitCallback(action);
			panel.DialogPath = path;
			Dialogs.Add(path, panel);

			return panel;
		}

		void OnSceneLoaded(Scene s, LoadSceneMode m)
		{
			InitCanvas();
		}

		class OpenData
		{
			public string path;
			public object data;
			public bool isOpen;
			public Action<object> closCallback;
			public PanelMaskMode maskMode = PanelMaskMode.Block;
		}

	}
}
