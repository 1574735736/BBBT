using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoView : UIAction
{
    List<GameObject> o_Pags = new List<GameObject>();
    
    void Start()
    {
        o_Pags = this.gameObject.GetObjectsWithPrefix("I_Pag");
        for (int i = 0; i < 4; i++)
        {
            int m = i;
            this.transform.SetDeepButtonAction("B_Pag" + m, () => { ChangeView(m); });
        }
        this.transform.SetDeepButtonAction("B_P1B1", () => { ChangeView(6); });
        this.transform.SetDeepButtonAction("B_P1B2", () => { ChangeView(5); });
        this.transform.SetDeepButtonAction("B_P1B3", () => { ChangeView(4); });

        this.transform.SetDeepButtonAction("B_P4B1", () => { ChangeView(0); });
        this.transform.SetDeepButtonAction("B_P5B1", () => { ChangeView(0); });
        this.transform.SetDeepButtonAction("B_P6B1", () => { ChangeView(0); });

        ChangeView(0);
    }



    void ChangeView(int pagIndex)
    {
        o_Pags[0].SetActive(true);
        for (int i = 1; i < o_Pags.Count; i++) {

            int m = i;
            if (pagIndex < 4)
            {
                if (m == pagIndex)
                {
                    o_Pags[m].SetActive(true);
                }
                else
                {
                    o_Pags[m].SetActive(false);
                }
            }
            else
            {
                if (m == pagIndex)
                {
                    UIBase.PageDoFade(true,o_Pags[m]);
                }
                else
                {
                    o_Pags[m].SetActive(false);
                }
            }

        }
    }
}

