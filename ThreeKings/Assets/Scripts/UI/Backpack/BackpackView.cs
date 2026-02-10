using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackView : UIAction
{
    List<Toggle> toggles = new List<Toggle>();
    void Start()
    {
        InitCompletes();
    }

    void InitCompletes()
    {
        toggles = this.gameObject.GetComponentsWithPrefix<Toggle>("Tog_Select");
        for (int i = 0; i < toggles.Count; i++) {
            int m = i;
            Toggle toggle = toggles[m];
            toggle.enabled = false;
            toggle.onValueChanged.AddListener((isOn) => {
                OnToggleChange(m, isOn);
            });
        }
    }

    void OnToggleChange(int index, bool isShow)
    {
        
    }
}
