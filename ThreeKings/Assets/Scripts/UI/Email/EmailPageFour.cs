using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmailPageFour : MonoBehaviour
{
    TMP_InputField ipt_Recipient;
    TMP_InputField ipt_SendGold;
    TMP_InputField ipt_DetailInfo;
    TMP_InputField ipt_NeedGold;

    Transform awardContent;
    GameObject awardItem;

    void Start()
    {
        ipt_Recipient = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Recipient");
        ipt_SendGold = this.transform.GetDeepComponent<TMP_InputField>("Ipt_SendGold");
        ipt_DetailInfo = this.transform.GetDeepComponent<TMP_InputField>("Ipt_DetailInfo");
        ipt_NeedGold = this.transform.GetDeepComponent<TMP_InputField>("Ipt_NeedGold");

        awardContent = this.transform.FindDeepChild("B_AddAward");
        awardItem = this.gameObject.GetDeepGameObject("AwardItem");

        this.transform.SetDeepButtonAction("B_AddAward", AddAward);
        this.transform.SetDeepButtonAction("B_Send", EmailSend);
    }


    void AddAward()
    {
        UIManager.Instance.OpenUI(UIConfig.EamilBackPackView);
    }

    void EmailSend()
    {
        Dictionary<string,string> valuePairs = new Dictionary<string,string>();
        valuePairs.Add("number", ipt_Recipient.text);
        valuePairs.Add("sliver", ipt_SendGold.text);
        valuePairs.Add("content", ipt_DetailInfo.text);
        valuePairs.Add("extract", ipt_NeedGold.text);
        UIManager.Instance.OpenUI(UIConfig.EmailSendView,null, valuePairs);
    }
    
}
