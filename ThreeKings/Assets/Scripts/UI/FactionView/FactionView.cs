using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionView : UIAction
{

    void Start()
    {
        this.transform.SetDeepButtonAction("B_CreateFaction", () => {
            UIManager.Instance.OpenUI(UIConfig.FactionCreateView);
        });

    }

}
