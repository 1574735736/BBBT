using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;
    GameObject o_Pag3;
    int pagIdx = 1;
    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("I_Pag1");
        o_Pag2 = this.gameObject.GetDeepGameObject("I_Pag2");
        o_Pag3 = this.gameObject.GetDeepGameObject("I_Pag3");

        this.transform.SetDeepButtonAction("B_Right", () => {
            pagIdx++;
            if (pagIdx >= 3)
            {
                pagIdx = 3;
            }
            OnShow();
        });

        this.transform.SetDeepButtonAction("B_Left", () => {
            pagIdx--;
            if (pagIdx <= 1)
            {
                pagIdx = 1;
            }
            OnShow();
        });
        pagIdx = 1;
        OnShow();
    }

    void OnShow()
    {
        o_Pag1.SetActive(pagIdx == 1);
        o_Pag2.SetActive(pagIdx == 2);
        o_Pag3.SetActive(pagIdx == 3);
    }
  
}

