using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;
    GameObject o_Pag3; //称号
    GameObject o_Pag4; //属性点分配
    GameObject o_Pag5; //战斗能力
    GameObject o_Pag6; //技能学校
    GameObject o_Pag7; //查看技能
    GameObject o_Pag8; //改名
    GameObject o_Pag9; //战斗录像
    GameObject o_Pag10; //装备
    //GameObject o_Pag11; //装备详情
    GameObject o_Pag12; //性格
    GameObject o_Pag13; //坐骑
    GameObject o_Pag14; //天赋石
    GameObject o_Pag15; //技能研习
    GameObject o_Pag16; //生平资料
    GameObject o_Pag17; //装备详情


    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("I_Page1");
        o_Pag2 = this.gameObject.GetDeepGameObject("I_Page2");
        o_Pag3 = this.gameObject.GetDeepGameObject("I_Page3");
        o_Pag4 = this.gameObject.GetDeepGameObject("I_Page4");
        o_Pag5 = this.gameObject.GetDeepGameObject("I_Page5");
        o_Pag6 = this.gameObject.GetDeepGameObject("I_Page6");
        o_Pag7 = this.gameObject.GetDeepGameObject("I_Page7");
        o_Pag8 = this.gameObject.GetDeepGameObject("I_Page8");
        o_Pag9 = this.gameObject.GetDeepGameObject("I_Page9");
        o_Pag10 = this.gameObject.GetDeepGameObject("I_Page10");
        //o_Pag11 = this.gameObject.GetDeepGameObject("I_Page11");
        o_Pag12 = this.gameObject.GetDeepGameObject("I_Page12");
        o_Pag13 = this.gameObject.GetDeepGameObject("I_Page13");
        o_Pag14 = this.gameObject.GetDeepGameObject("I_Page14");
        o_Pag15 = this.gameObject.GetDeepGameObject("I_Page15");
        o_Pag16 = this.gameObject.GetDeepGameObject("I_Page16");
        o_Pag17 = this.gameObject.GetDeepGameObject("I_Page17");


        this.transform.SetDeepButtonAction("B_ClosePanel", () => { OnClose(); });
        this.transform.SetDeepButtonAction("B_LeftTog1", () => { OnDefault(); o_Pag1.gameObject.SetActive(true); o_Pag2.gameObject.SetActive(false); });
        this.transform.SetDeepButtonAction("B_LeftTog2", () => { OnDefault(); o_Pag1.gameObject.SetActive(false); o_Pag2.gameObject.SetActive(true); });
        this.transform.SetDeepButtonAction("B_P1b1", () => { OnJobTransfer(); });
        this.transform.SetDeepButtonAction("B_P1b2", () => { OnDefault(); PageDoFade(true, o_Pag3); });
        this.transform.SetDeepButtonAction("B_P1b3", () => { OnDefault(); PageDoFade(true, o_Pag4); });
        this.transform.SetDeepButtonAction("B_P1b4", () => { OnDefault(); PageDoFade(true, o_Pag16); });
        this.transform.SetDeepButtonAction("B_P1b5", () => { OnDefault(); PageDoFade(true, o_Pag12); });
        this.transform.SetDeepButtonAction("B_P1b6", () => { OnDefault(); PageDoFade(true, o_Pag7); });
        this.transform.SetDeepButtonAction("B_P1b7", () => { OnDefault(); PageDoFade(true, o_Pag6); });
        this.transform.SetDeepButtonAction("B_P1b8", () => { OnDefault(); PageDoFade(true, o_Pag9); });
        this.transform.SetDeepButtonAction("B_P1b9", () => { OnDefault(); PageDoFade(true, o_Pag5); });
        for (int i = 10; i < 16; i++)
        {
            int m = i;
            this.transform.SetDeepButtonAction("B_P1b" + m, () => { OnDefault(); PageDoFade(true, o_Pag10); });
        }

        this.transform.SetDeepButtonAction("B_P2b1", () => { OnDefault(); PageDoFade(true, o_Pag13); });

        this.transform.SetDeepButtonAction("B_P3b1", () => { OnDefault(); PageDoFade(false, o_Pag3); });
        this.transform.SetDeepButtonAction("B_P3b2", () => { OnDefault(); PageDoFade(false, o_Pag3); });

        this.transform.SetDeepButtonAction("B_P4b1", () => { OnDefault(); PageDoFade(false, o_Pag4); });
        for (int i = 2; i < 8; i++)
        {
            int m = i;
            this.transform.SetDeepButtonAction("B_P4b"+ m, () => { OnBuildDefault(); });
        }

        this.transform.SetDeepButtonAction("B_P5b1", () => { OnDefault(); PageDoFade(false, o_Pag5); });

        this.transform.SetDeepButtonAction("B_P6b1", () => { OnDefault(); PageDoFade(false, o_Pag6); });

        this.transform.SetDeepButtonAction("B_P7b1", () => { OnDefault(); PageDoFade(false, o_Pag7); });

        this.transform.SetDeepButtonAction("B_P8b1", () => { OnDefault(); PageDoFade(false, o_Pag8); });
        this.transform.SetDeepButtonAction("B_P8b2", () => { OnDefault(); PageDoFade(false, o_Pag8); });

        this.transform.SetDeepButtonAction("B_P9b1", () => { OnDefault(); PageDoFade(false, o_Pag9); });

        this.transform.SetDeepButtonAction("B_P10b1", () => { OnDefault(); PageDoFade(false, o_Pag10); });
        this.transform.SetDeepButtonAction("B_P10b2", () => { OnDefault(); PageDoFade(true, o_Pag17); });
        this.transform.SetDeepButtonAction("B_P10b3", () => { OnDefault(); PageDoFade(true, o_Pag17); });

        this.transform.SetDeepButtonAction("B_P12b1", () => { OnDefault(); PageDoFade(false, o_Pag12); });
        this.transform.SetDeepButtonAction("B_P12b2", () => { OnDefault(); PageDoFade(false, o_Pag12); });

        this.transform.SetDeepButtonAction("B_P13b1", () => { OnDefault(); PageDoFade(false, o_Pag13); });

        this.transform.SetDeepButtonAction("B_P16b1", () => { OnDefault(); PageDoFade(false, o_Pag16); });

        this.transform.SetDeepButtonAction("B_P17b1", () => { OnDefault(); PageDoFade(false, o_Pag17); });
        this.transform.SetDeepButtonAction("B_P17b2", () => { OnBuildDefault(); });
        this.transform.SetDeepButtonAction("B_P17b3", () => { OnBuildDefault(); });

        OnInitShow();
    }

    private void OnInitShow()
    {
        o_Pag1.SetActive(true);
        o_Pag2.SetActive(false);
        o_Pag3.SetActive(false);
        o_Pag4.SetActive(false);
        o_Pag5.SetActive(false);
        o_Pag6.SetActive(false);
        o_Pag7.SetActive(false);
        o_Pag8.SetActive(false);
        o_Pag9.SetActive(false);
        o_Pag10.SetActive(false);
        o_Pag12.SetActive(false);
        o_Pag13.SetActive(false);
        o_Pag14.SetActive(false);
        o_Pag15.SetActive(false);
        o_Pag16.SetActive(false);
        o_Pag17.SetActive(false);
    }

    /// <summary>
    /// 默认点击
    /// </summary>
    private void OnDefault()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
    }

    private void OnJobTransfer()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "提示：您的等级需要达到<color=red>100级</color>才能转职！");
    }

    private void OnBuildDefault()
    {
        OnDefault();
        EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "提示：当前模块正开发中！");
    }

    private void PageDoFade(bool isShow , GameObject go)
    {
        Image image = go.GetComponent<Image>();
        if (image == null) return;
        go.SetActive(true);
        image.DOKill();
        if (isShow)
        {
            image.color = new Color(1, 1, 1, 0);
            image.DOFade(1, 0.5f);
        }
        else
        {
            image.DOFade(0, 0.5f).OnComplete(() =>{
                image.gameObject.SetActive(false);
            });
        }
    }
}




