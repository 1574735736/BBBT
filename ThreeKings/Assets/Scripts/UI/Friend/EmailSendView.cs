using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailSendView : UIAction
{
    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
    GameObject awardItem;
    Transform awardContent;

    void Start()
    {
        awardItem = this.gameObject.GetDeepGameObject("Item");
        awardContent = this.transform.FindDeepChild("AwardContent");
        this.transform.SetDeepButtonAction("B_Cancle", OnClose);
        this.transform.SetDeepButtonAction("B_Send", OnSendAction);
    }

    private void OnSendAction()
    {
        EmailDb.UserSendEmail(DataManager.Instance.RoleID, valuePairs["number"], valuePairs["sliver"], valuePairs["content"], "", valuePairs["extract"], (response) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.resourceUpdate, null);
            OnClose();
        }, (error) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, error);
        });
    }

    public override void InitData(object obj)
    {
        valuePairs = (Dictionary<string, string>)obj;


    }
}
