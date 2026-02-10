using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionView : UIAction
{

    void Start()
    {
        this.transform.SetDeepButtonAction("B_Parcel1", () => { OnParcelClick(0); });
        this.transform.SetDeepButtonAction("B_Parcel2", () => { OnParcelClick(1); });
        SetToggles();
        OnParcelClick(0);
    }

    private List<GameObject> o_Selects = new List<GameObject>();
    private List<GameObject> o_UnSelects = new List<GameObject>();
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
    }

    void OnParcelClick(int index)
    {
        this.transform.FindDeepChild("I_OnParcel1").gameObject.SetActive(index == 0);
        this.transform.FindDeepChild("I_OffParcel1").gameObject.SetActive(index != 0);
        this.transform.FindDeepChild("I_OnParcel2").gameObject.SetActive(index == 1);
        this.transform.FindDeepChild("I_OffParcel2").gameObject.SetActive(index != 1);
        this.transform.FindDeepChild("B_PlayerAuction2").gameObject.SetActive(index == 1);
        this.transform.FindDeepChild("B_PlayerAuction1").gameObject.SetActive(index == 0);
    }

}
