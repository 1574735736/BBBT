using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EmailDb;

public class EmailSystemItem : MonoBehaviour
{
    private EmailSheetItem emailSheet;
    private GameObject o_OffEmail;
    private GameObject o_OnEmail;
    private EmailPageOne emailPageOne;
    private EmailPageTwo emailPageTwo;
    private EmailPageThree emailPageThree;

    void Awake()
    {
        o_OffEmail = this.gameObject.GetDeepGameObject("I_OffEmail");
        o_OnEmail = this.gameObject.GetDeepGameObject("I_OnEmail");
        this.transform.GetComponent<Button>().onClick.AddListener(() => { OnClickSelf(); });
    }

    /// <summary>
    /// 数据初始化
    /// </summary>    
    public void InitData(EmailPageOne pageOne, EmailSheetItem sheetItem)
    {
        emailSheet = sheetItem;
        emailPageOne = pageOne;
        this.transform.SetDeepText("T_EmailTimeOff", sheetItem.create_time_text);
        this.transform.SetDeepText("T_EmailTimeOn", sheetItem.create_time_text);
    }

    public void InitDataTwo(EmailPageTwo pageOne, EmailSheetItem sheetItem)
    {
        emailSheet = sheetItem;
        emailPageTwo = pageOne;
        this.transform.SetDeepText("T_EmailTimeOff", sheetItem.create_time_text);
        this.transform.SetDeepText("T_EmailTimeOn", sheetItem.create_time_text);
        if (sheetItem.system != null)
        {
            this.transform.SetDeepText("T_EmailTitleOff", string.Format("发送给{0}的邮件", sheetItem.system.nickname));
            this.transform.SetDeepText("T_EmailTitleOn", string.Format("发送给{0}的邮件", sheetItem.system.nickname));
        }       
    }

    public void InitDataThree(EmailPageThree pageOne, EmailSheetItem sheetItem)
    {
        emailSheet = sheetItem;
        emailPageThree = pageOne;
        this.transform.SetDeepText("T_EmailTimeOff", sheetItem.create_time_text);
        this.transform.SetDeepText("T_EmailTimeOn", sheetItem.create_time_text);
        if (sheetItem.system != null)
        {
            this.transform.SetDeepText("T_EmailTitleOff", string.Format("来自{0}的礼物", sheetItem.system.nickname));
            this.transform.SetDeepText("T_EmailTitleOn", string.Format("来自{0}的礼物", sheetItem.system.nickname));
        }
    }

    private void OnClickSelf()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        if (emailPageOne != null && emailSheet != null)
        {
            emailPageOne.ChangeSelect(emailSheet.id);
        }
        else if (emailPageTwo != null && emailSheet != null)
        {
            emailPageTwo.ChangeSelect(emailSheet.id);
        }
        else if (emailPageThree != null && emailSheet != null)
        {
            emailPageThree.ChangeSelect(emailSheet.id);
        }
    }

    public void SelectItem(int id)
    {
        o_OffEmail.SetActive(id != emailSheet.id);
        o_OnEmail.SetActive(id == emailSheet.id);
    }

    public EmailSheetItem EmailSheet
    {
        get { return emailSheet; }
    }

}
