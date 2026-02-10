using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccountLoginView : UIAction
{
    TMP_InputField ipt_Account;
    TMP_InputField ipt_Password;

    Button b_EyeClose;
    Button b_EyeOpen;

    void Start()
    {
        ipt_Account = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Account");
        ipt_Password = this.transform.GetDeepComponent<TMP_InputField>("Ipt_Password");
        b_EyeClose = this.transform.GetDeepComponent<Button>("B_EyeClose");
        b_EyeOpen = this.transform.GetDeepComponent<Button>("B_EyeOpen");
        this.transform.SetDeepButtonAction("B_Login", OnLogin);
        this.transform.SetDeepButtonAction("T_Register", OnRegister);
        this.transform.SetDeepButtonAction("T_ChangePassword", OnPassword);
        b_EyeClose.onClick.AddListener(OnCloseEye);
        b_EyeOpen.onClick.AddListener(OnOpenEye);
        b_EyeOpen.gameObject.SetActive(false);
    }

    void OnLogin()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        if (string.IsNullOrEmpty(ipt_Account.text) || string.IsNullOrEmpty(ipt_Password.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÇëÊäÈëÕËºÅÃÜÂë");
            return;
        }
        //if (!DataManager.Instance.CanLogin(ipt_Account.text, ipt_Password.text))
        //{
        //    EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "ÕËºÅÃÜÂë²»ÕýÈ·");
        //    return;
        //}

        UserDb.Login(ipt_Account.text, ipt_Password.text, (LoginResponse) => {
            OnClose();           
        }, (error) => { Debug.LogError(error); });
    }

    private void OnOpenEye()
    {
        ipt_Password.contentType = TMP_InputField.ContentType.Password;
        b_EyeClose.gameObject.SetActive(true);
        b_EyeOpen.gameObject.SetActive(false);
        ipt_Password.gameObject.SetActive(false);
        ipt_Password.gameObject.SetActive(true);
    }

    private void OnCloseEye()
    {
        ipt_Password.contentType = TMP_InputField.ContentType.Standard;
        b_EyeClose.gameObject.SetActive(false);
        b_EyeOpen.gameObject.SetActive(true);
        ipt_Password.gameObject.SetActive(false);
        ipt_Password.gameObject.SetActive(true);
    }

    public override void OnClose()
    {
        _mainPanel.DoScaleHideBounce(Vector3.zero, 0.3f, () => {
            UIManager.Instance.CloseUI(() => {

                if (!string.IsNullOrEmpty(UserDb.UserToken))
                {
                    LoginPanel loginPanel = FindObjectOfType<LoginPanel>();
                    //loginPanel.OnGoToGame();
                    loginPanel.OnChangeServer();
                }               
            });
        });
    }

    void OnRegister()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        UIManager.Instance.OpenUI(UIConfig.RegisterView);
    }

    void OnPassword()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        UIManager.Instance.OpenUI(UIConfig.ChangePasswordView);
    }
}
