using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactionCreateView : UIAction
{
    TMP_InputField ipt_Name;
    TMP_InputField ipt_FactionInfo;


    void Start()
    {
        ipt_Name = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Name");
        ipt_FactionInfo = this.transform.GetDeepComponent<TMP_InputField>("Ipt_FactionInfo");

        this.transform.SetDeepButtonAction("B_BuildFaction", () => {

            if (string.IsNullOrEmpty(ipt_Name.text))
            {
                EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "请输入名称");
                return;
            }
            if (string.IsNullOrEmpty(ipt_FactionInfo.text))
            {
                EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "请输入教义");
                return;
            }
            UIManager.Instance.OpenUI(UIConfig.FactionCreateTip);
        });
    }

   
}
