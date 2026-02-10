using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsMallView : UIAction
{
    // Start is called before the first frame update
    void Start()
    {
        SetToggles();
    }


    private List<GameObject> o_Selects = new List<GameObject>();
    void SetToggles()
    {
        o_Selects = this.gameObject.GetObjectsWithPrefix("I_Select");
        List<Button> buttons = this.gameObject.GetComponentsWithPrefix<Button>("B_Status");
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].onClick.AddListener(() => {
                OnClickSelect(j);
                AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
            });
        }
        OnClickSelect(0);
    }

    void OnClickSelect(int index)
    {
        for (int i = 0; i < o_Selects.Count; i++)
        {
            o_Selects[i].SetActive(i == index);
        }
    }
}
