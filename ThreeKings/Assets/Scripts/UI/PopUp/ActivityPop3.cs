using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityPop3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetDeepButtonAction("B_Detail1", OnOpenDetail);
        this.transform.SetDeepButtonAction("B_Detail2", OnOpenDetail);
        this.transform.SetDeepButtonAction("B_Detail3", OnOpenDetail);
        this.transform.SetDeepButtonAction("B_Detail4", OnOpenDetail);
    }

    void OnOpenDetail()
    {
        UIManager.Instance.OpenUI(UIConfig.MonthAwardView);
    }
}
