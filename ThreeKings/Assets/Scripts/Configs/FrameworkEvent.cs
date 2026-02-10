
//---------------------------------------------------------------------
// FrameworkEvent
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2023-08-07 10:55:17
// Feedback: yang2686022430@163.com
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LieyouFramework
{
	public class FrameworkEvent
	{
		public static Action MonoUpdate;
		public static Action MonoFixedUpdate;
		public static Action MonoLateUpdate;
		public static Action<bool> MonoApplicationPause;
		public static Action MonoApplicationQuit;
	}
}
