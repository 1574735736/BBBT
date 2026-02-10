using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    private List<GameObject> o_Selects = new List<GameObject>();
    private List<GameObject> o_UnSelects = new List<GameObject>();
    private List<GameObject> o_Pags = new List<GameObject>();
    void Start()
    {
        this.transform.SetDeepButtonAction("B_Parcel1", () => { OnParcelClick(0); });
        this.transform.SetDeepButtonAction("B_Parcel2", () => { OnParcelClick(1); });
        this.transform.SetDeepButtonAction("B_Parcel3", () => { OnParcelClick(2); });
        SetToggles();
        OnParcelClick(0);
    }
    void SetToggles()
    {
        o_Selects = this.gameObject.GetObjectsWithPrefix("I_OnTog");
        o_UnSelects = this.gameObject.GetObjectsWithPrefix("I_OffTog");
        o_Pags = this.gameObject.GetObjectsWithPrefix("P_Pag");
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
            if (o_Pags.Count > i)
            {
                o_Pags[i].SetActive(i == index);
            }
        }
    }

    void OnParcelClick(int index)
    {
        this.transform.FindDeepChild("I_OnParcel1").gameObject.SetActive(index == 0);
        this.transform.FindDeepChild("I_OffParcel1").gameObject.SetActive(index != 0);
        this.transform.FindDeepChild("I_OnParcel2").gameObject.SetActive(index == 1);
        this.transform.FindDeepChild("I_OffParcel2").gameObject.SetActive(index != 1);
        this.transform.FindDeepChild("I_OnParcel3").gameObject.SetActive(index == 2);
        this.transform.FindDeepChild("I_OffParcel3").gameObject.SetActive(index != 2);
    }

}
