using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterView : UIAction
{
    TMP_InputField ipt_Account;
    TMP_InputField ipt_Password;
    TMP_InputField ipt_RePassword;
    TMP_InputField ipt_ShareCode;
    void Start()
    {
        ipt_Account = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Account");
        ipt_Password = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Password");
        ipt_RePassword = this.transform.GetDeepComponent<TMP_InputField>("Ipt_RePassword");
        ipt_ShareCode = this.transform.GetDeepComponent<TMP_InputField>("Ipt_InvitationCode");

        this.transform.SetDeepButtonAction("B_Register", OnRegisterClick);
    }

    private void OnRegisterClick()
    {
        if (string.IsNullOrEmpty(ipt_Account.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÇëÊäÈëÕËºÅ");
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

        //if (DataManager.Instance.HasLoginInfo(ipt_Account.text))
        //{
        //    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "µ±Ç°ÕËºÅÒÑ´æÔÚ");
        //    return;
        //}
        
        UserDb.Register(ipt_Account.text, ipt_Password.text, "666666", ipt_ShareCode.text, (response) => {
            DataManager.Instance.RegestLoginData(ipt_Account.text, ipt_Password.text);
            UIManager.Instance.RemoveAllUI();
            if (!string.IsNullOrEmpty(UserDb.UserToken))
            {
                LoginPanel loginPanel = FindObjectOfType<LoginPanel>();
                loginPanel.OnChangeServer();
            }

            //loginPanel.OnGoToGame();    
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "×¢²á³É¹¦");
        }, (error) => {
            Debug.LogError("×¢²áÊ§°Ü:" + error);
        });
    }
   
}
