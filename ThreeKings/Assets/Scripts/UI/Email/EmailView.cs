using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;
    GameObject o_Pag3;
    GameObject o_Pag4;

    private List<GameObject> o_Selects = new List<GameObject>();
    private List<GameObject> o_UnSelects = new List<GameObject>();
    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("I_Pag1");
        o_Pag2 = this.gameObject.GetDeepGameObject("I_Pag2");
        o_Pag3 = this.gameObject.GetDeepGameObject("I_Pag3");
        o_Pag4 = this.gameObject.GetDeepGameObject("I_Pag4");

        SetToggles();
    }

    void OnChangePage(int index)
    {
        o_Pag1.SetActive(index == 0);
        o_Pag2.SetActive(index == 1);
        o_Pag3.SetActive(index == 2);
        o_Pag4.SetActive(index == 3);
    }

    void SetToggles()
    {
        o_Selects = this.gameObject.GetObjectsWithPrefix("I_OnTog");
        o_UnSelects = this.gameObject.GetObjectsWithPrefix("I_OffTog");
        List<Button> buttons = this.gameObject.GetComponentsWithPrefix<Button>("B_Tog");

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
            o_UnSelects[i].SetActive(i != index);
        }
        OnChangePage(index);
    }

}

