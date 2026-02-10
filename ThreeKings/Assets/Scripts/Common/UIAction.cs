using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAction : MonoBehaviour
{
    [HideInInspector]
    public Transform _mainPanel;

    public virtual void Awake()
    {
        _mainPanel = this.transform.FindDeepChild("Main");
        if (_mainPanel != null)
        {
            _mainPanel.DoScaleOpenBounce(Vector3.one,0.3f, OnCompleteAction);

            _mainPanel.SetDeepButtonAction("B_Close", () => {
                AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
                OnClose();
            });

            _mainPanel.SetDeepButtonAction("B_Back", () => {
                AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
                GoBack();
            });

        }

    }

    public virtual void OnCompleteAction()
    {

    }

    public virtual void InitData(object obj)
    {
        
    }

    public virtual void OnClose()
    {
        _mainPanel.DoScaleHideBounce(Vector3.zero, 0.3f, () => {
            UIManager.Instance.CloseUI();
        });
    }

    public virtual void GoBack()
    {
        UIManager.Instance.RemoveUI(this.transform.gameObject);
    }

}
