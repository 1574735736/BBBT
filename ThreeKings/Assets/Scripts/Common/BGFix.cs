using LieyouFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFix : MonoBehaviour
{
    [SerializeField] FixLifeCycle _fixLifeCycle = FixLifeCycle.Awake;

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

    void Fix()
    {

        Rect realSize = Utility.UGUI.GetSize(transform.parent.gameObject); //UnityEngine.Screen.currentResolution; 

        Debug.Log("当前手机宽度:" + realSize.width + "当前手机高度:" + realSize.height);

        float fixResolution = (1280f / 720f) > ((float)realSize.height / (float)realSize.width) ? ((float)realSize.width * 1280f / (float)realSize.height) / 720f : ((float)realSize.height * 720f / (float)realSize.width) / 1280f;
        //float fixResolution = (2400f * 720f / 1080f) / 1280f;
        Debug.Log("当前缩放:" + fixResolution);
        this.GetComponent<RectTransform>().localScale = fixResolution * transform.localScale;
        // Rect rect = Utility.UGUI.GetSize(transform.parent.gameObject);
        // float scalex = rect.height / LieyouFramework.FrameworkConfig.ReferenceResolution.y;
        // float scaley = rect.width / LieyouFramework.FrameworkConfig.ReferenceResolution.x;
        // Debug.Log("++++++++++w:" + rect.width + " heiht:" + rect.height);
        // if (scalex > scaley)
        // {
        // 	transform.localScale = Vector3.one * scalex;
        // }
        // else
        // {
        // 	transform.localScale = Vector3.one * scaley;
        // }
    }

    private enum FixLifeCycle
    {
        Awake,
        OnEnable,
        Start
    }
}
