using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityView : UIAction
{
    GameObject o_Tog1;
    GameObject o_Tog2;
    GameObject o_Tog3;

    void Start()
    {
        OnCompletes();
        OnUserClick();
    }

    void OnCompletes()
    {
        o_Tog1 = this.gameObject.GetDeepGameObject("P_Tog1");
        o_Tog2 = this.gameObject.GetDeepGameObject("P_Tog2");
        o_Tog3 = this.gameObject.GetDeepGameObject("P_Tog3");

    }

    void OnUserClick()
    {
        this.transform.SetDeepButtonAction("B_Select1", () =>
        {
            TogleAction(1);
        });
        this.transform.SetDeepButtonAction("B_Select2", () =>
        {
            TogleAction(2);
        });
        this.transform.SetDeepButtonAction("B_Select3", () =>
        {
            TogleAction(3);
        });
    }

    void TogleAction(int index)
    {
        o_Tog1.SetActive(index == 1);
        o_Tog2.SetActive(index == 2);
        o_Tog3.SetActive(index == 3);
    }
}
