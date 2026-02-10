using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static BackpackDb;
using System;

public class EamilBackPackView : UIAction
{
    Transform togContent;
    GameObject togItem;

    Transform awardContent;
    GameObject awardItem;

    int curIndex = 0;

    List<BackpackAttrItem>  backpackAttrItems = new List<BackpackAttrItem>();
    private List<GameObject> o_Selects = new List<GameObject>();
    private List<GameObject> o_UnSelects = new List<GameObject>();
    private List<EamilBackPackItem> eamilBacks = new List<EamilBackPackItem>();

    void Start()
    {
        togContent = this.transform.FindDeepChild("TogContent");
        togItem = this.gameObject.GetDeepGameObject("B_TogItem");

        awardContent = this.transform.FindDeepChild("Content");
        awardItem = this.gameObject.GetDeepGameObject("Item");

        InitNet();
    }

    void InitNet()
    {
        BackpackDb.GetBackpackAttr((response) => {
            backpackAttrItems = response.data;
            InitTogs();
        }, (error) => { 
            Debug.LogError(error);
        });
    }

    void InitTogs()
    {
        for (int i = 0; i < backpackAttrItems.Count; i++)
        {
            int m = i;
            GameObject go = Instantiate(togItem, togContent);
            o_Selects.Add(go.GetDeepGameObject("I_OnTog"));
            o_UnSelects.Add(go.GetDeepGameObject("I_OffTog"));
            BackpackAttrItem attrItem = backpackAttrItems[m];
            go.SetDeepText("T_OffTog", attrItem.name);
            go.SetDeepText("T_OnTog", attrItem.name);
            go.GetComponent<Button>().onClick.AddListener(() => {
                OnChangeSelect(m);
            });
        }
        StartCoroutine(WaitAction(0.1f, () => {
            OnChangeSelect(0);
        }));
    }

    void InitAwardNet()
    {
        BackpackDb.GetUserBackpackSheet(DataManager.Instance.RoleID, backpackAttrItems[curIndex].id, (sheetData) => {
            List<BackpackItem> backpackItems = sheetData.data.data;
            CreateAwardItem(backpackItems);
        }, (error) => {
            Debug.LogError(error);
        });
    }

    void CreateAwardItem(List<BackpackItem> backpacks)
    {
        for (int i = 0; i < eamilBacks.Count; i++) {
            eamilBacks[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < backpacks.Count; i++) {
            int m = i;
            EamilBackPackItem eamilBack = null;
            if (m < eamilBacks.Count)
            {
                eamilBacks[m].gameObject.SetActive(true);
            }
            else
            {
                GameObject go = Instantiate(awardItem, awardContent);
                go.SetActive(true);
                eamilBack = go.GetComponent<EamilBackPackItem>();
            }
            eamilBack.InitData(backpacks[m]);
        }
    }

    IEnumerator WaitAction(float timer,Action action)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    void OnChangeSelect(int index)
    {
        for (int i = 0; i < o_Selects.Count; i++)
        {
            o_Selects[i].SetActive(i == index);
            o_UnSelects[i].SetActive(i != index);
            curIndex = index;
        }
        InitAwardNet();
    }

}
