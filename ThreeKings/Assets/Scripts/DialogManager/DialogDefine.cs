
//---------------------------------------------------------------------
// XRLGame
// Copyright © 2019-2022 XRLGame Co., Ltd. All rights reserved.
// Author: JerryYang
// Time: 2022-10-28 10:32:02
// Feedback: yang2686022430@163.com.
//---------------------------------------------------------------------


namespace LieyouFramework
{
	public enum PanelMaskMode
	{
		None = 0,           //无遮罩层（空闲区域可以点击）
		Block,              //阻挡（空闲区域无法点击）
		ClickDisappear,     //点击蒙层面板面板消失
	}

	public enum UIResult
	{
		None,
		OK,
		Cancel,
		Yes,
		No,
		Replay,
		Next,
		GOHome
	}
}
