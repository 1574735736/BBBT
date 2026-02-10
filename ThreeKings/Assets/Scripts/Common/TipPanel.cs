using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanel : MonoBehaviour
{
    private Transform main;
    
    void Start()
    {
        main = this.transform.FindDeepChild("I_TipBg");
        main.transform.localScale = new Vector3(1, 0, 1);
        main.gameObject.SetActive(false);
        EventManager.Instance.StartListening(Enums.Lisiner.TipShow.ToString(), ShowTipData);
    }

    private void ShowTipData(object obj)
    {
        string str = (string)obj;
        if (string.IsNullOrEmpty(str))
        {
            return;
        }
        main.DOKill();
        main.gameObject.SetActive(true);
        if (main.transform.localScale.y != 1) 
        {
            main.DOScaleY(1, 0.3f);
        }        
        this.transform.SetDeepText("T_TipInfo", str);
        Timer.Instance.AddDelayedAction(1.2f, () => {
            main.DOScaleY(0, 0.3f).OnComplete(() => { 
                main.gameObject.SetActive(false);
            });
            });
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(Enums.Lisiner.TipShow.ToString(), ShowTipData);
    }
}
