using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecruitmentView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;
    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("I_Page1");
        o_Pag2 = this.gameObject.GetDeepGameObject("I_Page2");

        this.transform.SetDeepButtonAction("B_Interaction1", OnPageOneClick);
        this.transform.SetDeepButtonAction("B_Interaction2", OnPageOneClick);
        this.transform.SetDeepButtonAction("B_Interaction3", OnPageOneClick);
        this.transform.SetDeepButtonAction("B_Interaction4", OnPageOneClick);
        this.transform.SetDeepButtonAction("B_Interaction5", OnPageTwoClick);
        this.transform.SetDeepButtonAction("B_Interaction6", OnPageTwoClick);

        o_Pag2.SetActive(false);
    }

    void OnPageOneClick()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        UIBase.PageDoFade(true, o_Pag2);
    }

    void OnPageTwoClick()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        UIBase.PageDoFade(false, o_Pag2);
    }
}
