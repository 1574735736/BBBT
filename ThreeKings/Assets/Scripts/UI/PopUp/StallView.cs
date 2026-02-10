using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;
    GameObject o_Pag3;
    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("I_MyStall");
        o_Pag2 = this.gameObject.GetDeepGameObject("I_OtherStall");
        o_Pag3 = this.gameObject.GetDeepGameObject("I_StallRecord");

        this.transform.SetDeepButtonAction("B_MyStall1", () => { OnPageChange(1); });
        this.transform.SetDeepButtonAction("B_CloseMyRecord", () => { OnPageChange(0); });
        this.transform.SetDeepButtonAction("B_MyRecord2", () => { OnPageChange(3); });
        this.transform.SetDeepButtonAction("B_CloseStallRecord", () => { OnPageChange(0); });
    }


    void OnPageChange(int pagIndex)
    {
        o_Pag1.SetActive(pagIndex == 1);
        o_Pag2.SetActive(pagIndex == 2);
        o_Pag3.SetActive(pagIndex == 3);
        if (pagIndex == 1)
        {
            UIBase.PageDoFade(true, o_Pag1);
        }
        else if (pagIndex == 2)
        {
            UIBase.PageDoFade(true, o_Pag2);
        }
        else if (pagIndex == 3)
        {
            UIBase.PageDoFade(true, o_Pag3);
        }

    }
}

