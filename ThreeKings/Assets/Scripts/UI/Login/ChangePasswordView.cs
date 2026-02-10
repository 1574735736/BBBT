using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangePasswordView : UIAction
{
    TMP_InputField ipt_Account;
    TMP_InputField ipt_OldPassword;
    TMP_InputField ipt_Password;
    TMP_InputField ipt_RePassword;

    void Start()
    {
        ipt_Account = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Account");
        ipt_OldPassword = this.transform.GetDeepComponent<TMP_InputField>("Ipt_OldPassword");
        ipt_Password = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Password");
        ipt_RePassword = this.transform.GetDeepComponent<TMP_InputField>("Ipt_RePassword");

        this.transform.SetDeepButtonAction("B_Change", OnChangeClick);
    }

    void OnChangeClick()
    {
        if (string.IsNullOrEmpty(ipt_Account.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÇëÊäÈëÕËºÅ");
            return;
        }
        if (string.IsNullOrEmpty(ipt_OldPassword.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÇëÊäÈë¾ÉÃÜÂë");
            return;
        }
        if (string.IsNullOrEmpty(ipt_Password.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÇëÊäÈëÃÜÂë");
            return;
        }
        if (string.IsNullOrEmpty(ipt_RePassword.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÇëÔÙ´ÎÊäÈëÃÜÂë");
            return;
        }
        if (ipt_RePassword.text != ipt_Password.text)
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "Á½´ÎÃÜÂë²»Ò»ÖÂ");
            return;
        }
        if (!DataManager.Instance.HasLoginInfo(ipt_Account.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "µ±Ç°ÕËºÅ²»´æÔÚ");
            return;
        }
        //DataManager.Instance.ChangeLoginData(ipt_Account.text, ipt_Password.text);
        UserDb.ChangeRoleAvatar(ipt_Account.text, ipt_OldPassword.text, ipt_Password.text, (response) => {
            OnClose();
        }, (error) => {
            Debug.LogError(error);
        });        
    }
}
