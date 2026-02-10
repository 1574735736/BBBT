
//---------------------------------------------------------------------
// DalaranFramework
// Copyright © 2013-2020 Dalaran Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2020/12/17 14:45:15
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace LieyouFramework
{
	public static partial class Utility
	{
		/// <summary>
		/// UGUI辅助类
		/// </summary>
		public static class UGUI
		{
			/// <summary>
			/// 获取UGUI对象的宽
			/// </summary>
			/// <param name="_UI">要获取的对象</param>
			/// <returns></returns>
			public static float GetWidth(GameObject _UI)
			{
				RectTransform rt = _UI.GetComponent<RectTransform>();
				return rt.rect.width;
			}

			/// <summary>
			/// 获取UGUI对象的高
			/// </summary>
			/// <param name="_UI">要获取的对象</param>
			/// <returns></returns>
			public static float GetHeight(GameObject _UI)
			{
				RectTransform rt = _UI.GetComponent<RectTransform>();
				return rt.rect.height;
			}

			/// <summary>
			/// 获取UGUI对象的尺寸
			/// </summary>
			/// <param name="_UI">要获取的对象</param>
			/// <returns></returns>
			public static Rect GetSize(GameObject _UI)
			{
				RectTransform rt = _UI.GetComponent<RectTransform>();
				return rt.rect;
			}

			/// <summary>
			/// 设置UGUI对象的宽
			/// </summary>
			/// <param name="_UI">要设置的对象</param>
			/// <param name="_width">要设置的宽度</param>
			public static void SetWidth(GameObject _UI, float _width)
			{
				RectTransform trn = _UI.GetComponent<RectTransform>();
				trn.sizeDelta = new Vector2(_width, trn.sizeDelta.y);
			}

			/// <summary>
			/// 设置UGUI对象的高
			/// </summary>
			/// <param name="_UI">要设置的对象</param>
			/// <param name="_height">要设置的高度</param>
			public static void SetHeight(GameObject _UI, float _height)
			{
				RectTransform trn = _UI.GetComponent<RectTransform>();
				trn.sizeDelta = new Vector2(trn.sizeDelta.x, _height);
			}

			/// <summary>
			/// 设置UGUI对象的Size
			/// </summary>
			/// <param name="_UI">要设置的对象</param>
			/// <param name="_size">要设置的 Size</param>
			public static void SetSize(GameObject _UI, Vector2 _size)
			{
				RectTransform trn = _UI.GetComponent<RectTransform>();
				trn.sizeDelta = _size;
			}

			/// <summary>
			/// 设置透明度
			/// </summary>
			/// <param name="_UI">要设置的UI对象</param>
			/// <param name="alpha">透明度</param>
			/// <returns>CanvasGroup</returns>
			public static CanvasGroup SetAlpha(GameObject _UI, float alpha)
			{
				CanvasGroup canvasGroup = _UI.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = _UI.AddComponent<CanvasGroup>();
				}
				canvasGroup.alpha = alpha;
				return canvasGroup;
			}

			/// <summary>
			/// 设置层级
			/// </summary>
			/// <param name="_UI">要设置的UI对象</param>
			/// <param name="_order">层级</param>
			/// <returns>Canvas</returns>
			public static Canvas SetOrder(GameObject _UI, int _order)
			{
				Canvas canvas = _UI.GetComponent<Canvas>();
				if (canvas == null)
				{
					canvas = _UI.AddComponent<Canvas>();
				}
				canvas.overrideSorting = true;
				canvas.sortingOrder = _order;
				return canvas;
			}

			/// <summary>
			/// 判断点击不在UI上
			/// </summary>
			/// <returns></returns>
			public static bool IsTouchNotOnUI()
			{
				return EventSystem.current.currentSelectedGameObject == null;
			}

			/// <summary>
			/// 获取颜色值
			/// </summary>
			/// <param name="r">红色通道值，范围0-255</param>
			/// <param name="g">绿色通道值，范围0-255</param>
			/// <param name="b">蓝色通道值，范围0-255</param>
			/// <param name="a">透明通道值，范围0-255</param>
			/// <returns></returns>
			public static Color NewColor(int r, int g, int b, int a = 255)
			{
				return new Color((float)r / 255, (float)g / 255, (float)b / 255, (float)a / 255);
			}
		}
	}
}
