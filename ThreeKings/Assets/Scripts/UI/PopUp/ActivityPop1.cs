using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityPop1 : MonoBehaviour
{
    private List<GameObject> o_Selects = new List<GameObject>();
    private List<GameObject> o_UnSelects = new List<GameObject>();
    void Start()
    {
        o_Selects = this.gameObject.GetObjectsWithPrefix("I_Select");
        o_UnSelects = this.gameObject.GetObjectsWithPrefix("I_NoSelect");
        List<Button> buttons = this.gameObject.GetComponentsWithPrefix<Button>("B_AwardIndex");
        for (int i = 0; i < buttons.Count; i++)
        {
            int j = i;
            buttons[j].onClick.AddListener(() => { OnClickSelect(j);
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
            o_UnSelects[i].SetActive(i != index);
        }
    }

}
