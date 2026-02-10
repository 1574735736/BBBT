using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankView : UIAction
{
   List<GameObject> o_Togs = new List<GameObject>();
    List<Toggle> tog_Selects = new List<Toggle>();
    void Start()
    {
        o_Togs = this.gameObject.GetObjectsWithPrefix("P_Tog");
        tog_Selects = this.gameObject.GetComponentsWithPrefix<Toggle>("Tog_Select");
        for (int i = 0; i < tog_Selects.Count; i++)
        {
            int j = i;
            tog_Selects[j].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    OnShowSelf(j);
                }
            } );
        }
        tog_Selects[0].isOn = true;

        this.transform.SetDeepButtonAction("B_OldRecord1", OnOpenRecord);
        this.transform.SetDeepButtonAction("B_OldRecord2", OnOpenRecord);
    }

    void OnShowSelf(int index)
    {
        for (int i = 0; i < o_Togs.Count; i++)
        {
            o_Togs[i].SetActive(i == index);
        }
    }

    void OnOpenRecord()
    {
        UIManager.Instance.OpenUI(UIConfig.BankOldRecordView);
    }
}
