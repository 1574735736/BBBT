using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApprenticeshipView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;
    GameObject o_Pag3;
    GameObject o_Pag4;
    GameObject o_Pag5;
    GameObject o_Pag6;
    GameObject o_Pag7;
    GameObject o_Pag8;
    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("I_Pag1");
        o_Pag2 = this.gameObject.GetDeepGameObject("I_Pag2");
        o_Pag3 = this.gameObject.GetDeepGameObject("I_Pag3");
        o_Pag4 = this.gameObject.GetDeepGameObject("I_Pag4");
        o_Pag5 = this.gameObject.GetDeepGameObject("I_Pag5");
        o_Pag6 = this.gameObject.GetDeepGameObject("I_Pag6");
        o_Pag7 = this.gameObject.GetDeepGameObject("I_Pag7");
        o_Pag8 = this.gameObject.GetDeepGameObject("I_Pag8");

        this.transform.SetDeepButtonAction("B_P1B1", () => { OnPageChage(2); });
        this.transform.SetDeepButtonAction("B_P1B2", () => { OnPageChage(3); });
        this.transform.SetDeepButtonAction("B_P1B3", () => { OnPageChage(8); });

        this.transform.SetDeepButtonAction("B_P2B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P2B2", () => { OnPageChage(7); });
        this.transform.SetDeepButtonAction("B_P2B3", () => { OnPageChage(7); });
        this.transform.SetDeepButtonAction("B_P2B4", () => { OnPageChage(7); });

        this.transform.SetDeepButtonAction("B_P3B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P3B2", () => { OnPageChage(4); });
        this.transform.SetDeepButtonAction("B_P3B3", () => { OnPageChage(4); });
        this.transform.SetDeepButtonAction("B_P3B4", () => { OnPageChage(4); });

        this.transform.SetDeepButtonAction("B_P4B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P4B2", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P4B3", () => { OnPageChage(5); });

        this.transform.SetDeepButtonAction("B_P5B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P5B2", () => { OnPageChage(1); });

        this.transform.SetDeepButtonAction("B_P6B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P6B2", () => { OnPageChage(1); });

        this.transform.SetDeepButtonAction("B_P7B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P7B2", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P7B3", () => { OnPageChage(6); });

        this.transform.SetDeepButtonAction("B_P8B1", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P8B2", () => { OnPageChage(1); });
        this.transform.SetDeepButtonAction("B_P8B3", () => { OnPageChage(1); });

        OnPageChage(1);
    }


    void OnPageChage(int pagIndex)
    {
        o_Pag1.SetActive(true);
       
        if (pagIndex == 2)
        {
            if (!o_Pag2.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag2);
            }
        }
        if (pagIndex == 3)
        {
            if (!o_Pag3.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag3);
            }
        }
        if (pagIndex == 4)
        {
            if (!o_Pag4.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag4);
            }
        }
        if (pagIndex == 5)
        {
            if (!o_Pag5.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag5);
            }
        }
        if (pagIndex == 6)
        {
            if (!o_Pag6.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag6);
            }
        }
        if (pagIndex == 7)
        {
            if (!o_Pag7.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag7);
            }
        }
        if (pagIndex == 8)
        {
            if (!o_Pag8.activeSelf)
            {
                UIBase.PageDoFade(true, o_Pag8);
            }
        }

        o_Pag2.SetActive(pagIndex == 2);
        o_Pag3.SetActive(pagIndex == 3);
        o_Pag4.SetActive(pagIndex == 4);
        o_Pag5.SetActive(pagIndex == 5);
        o_Pag6.SetActive(pagIndex == 6);
        o_Pag7.SetActive(pagIndex == 7);
        o_Pag8.SetActive(pagIndex == 8);
    }
   
}
