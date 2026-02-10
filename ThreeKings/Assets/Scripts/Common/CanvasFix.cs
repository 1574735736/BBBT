
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


public class CanvasFix : MonoBehaviour
{
    [SerializeField] FixLifeCycle _fixLifeCycle = FixLifeCycle.Awake;
    private CanvasScaler _canvasScaler;

    public Vector2 ReferenceResolution = new Vector2(1080, 1920);

    private void Awake()
    {
        if (_fixLifeCycle == FixLifeCycle.Awake)
        {
            Fix();
        }
    }

    private void OnEnable()
    {
        if (_fixLifeCycle == FixLifeCycle.OnEnable)
        {
            Fix();
        }
    }

    private void Start()
    {
        if (_fixLifeCycle == FixLifeCycle.Start)
        {
            Fix();
        }
    }

    private void Fix()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = ReferenceResolution;

        _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        float r = (float)Screen.width / Screen.height;

        if (Screen.height > Screen.width)
        {
            r = (float)Screen.height / Screen.width;
        }

        if (r < 1.7f)
        {
            _canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            _canvasScaler.matchWidthOrHeight = 0;
        }
    }

    private enum FixLifeCycle
    {
        Awake,
        OnEnable,
        Start
    }
}

