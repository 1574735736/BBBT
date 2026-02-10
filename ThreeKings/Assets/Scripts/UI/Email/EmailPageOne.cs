using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EmailDb;
using static UnityEngine.ParticleSystem;

public class EmailPageOne : MonoBehaviour
{
    private GameObject systemItem;
    private Transform systemContent;

    private GameObject awardItem;
    private Transform awardContent;

    private TextMeshProUGUI t_DetailTitle;
    private TextMeshProUGUI t_Detail1;
    private TextMeshProUGUI t_Detail2;

    private GameObject o_GetSingle;
    private GameObject o_EmailDetail;

    List<GameObject> sysItems = new List<GameObject>();
    List<EmailSystemItem> emails = new List<EmailSystemItem>();
    List<GameObject> awardItems = new List<GameObject>();

    private int CurEmailId = 0;

    void Start()
    {
        systemItem = this.gameObject.GetDeepGameObject("SystemItem");
        systemContent = this.transform.FindDeepChild("Content");

        awardItem = this.gameObject.GetDeepGameObject("AwardItem");
        awardContent = this.transform.FindDeepChild("AwardContent");

        t_DetailTitle = this.transform.GetDeepComponent<TextMeshProUGUI>("T_DetailTitle");
        t_Detail1 = this.transform.GetDeepComponent<TextMeshProUGUI>("T_Detail1");
        t_Detail2 = this.transform.GetDeepComponent<TextMeshProUGUI>("T_Detail2");

        o_GetSingle = this.gameObject.GetDeepGameObject("B_GetSingle");
        o_EmailDetail = this.gameObject.GetDeepGameObject("I_EmailDetail");

        this.transform.SetDeepButtonAction("B_GetAll", OnGetAll);
        this.transform.SetDeepButtonAction("B_GetSingle", OnGetSingle);
        this.transform.SetDeepButtonAction("B_DeleteAll", OnDeleteAll);        

        InitNetData();
    }


    void InitNetData()
    {
        EmailDb.GetEmailSheet(1, DataManager.Instance.RoleID, (SheetData) => {
            List<EmailSheetItem> sheetItems = SheetData.data.data;
           
            OnCreateItem(sheetItems);
        }, (error) => {
            Debug.LogError(error);
        });
    }

    void OnGetAll()
    {
        EmailDb.DrawAllEmail(DataManager.Instance.RoleID, (emailData) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "奖励领取成功！");
            EventManager.Instance.TriggerEvent(Enums.Lisiner.resourceUpdate, null);
            InitNetData();
        }, (error) => {
            Debug.LogError(error);
        });
    }

    void OnGetSingle()
    {
        EmailDb.DrawSingleEmail(CurEmailId, (emailData) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow,"奖励领取成功！");
            EventManager.Instance.TriggerEvent(Enums.Lisiner.resourceUpdate, null);
        }, (error) => { 
            Debug.LogError(error);
        });
    }

    void OnDeleteAll()
    {
        EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "没用的");
    }

    void OnCreateItem(List<EmailSheetItem> sheetItems)
    {
        if (sheetItems.Count <= 0)
        {
            o_EmailDetail.gameObject.SetActive(false);
        }
        o_EmailDetail.gameObject.SetActive(true);
        foreach (var item in sysItems)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < sheetItems.Count; i++)
        {
            int m = i;
            GameObject go = null;
            if (m < sysItems.Count)
            {
                go = sysItems[m];
            }
            else
            {
                go = Instantiate(systemItem, systemContent);
                sysItems.Add(go);
            }
            go.SetActive(true);
            EmailSystemItem emailSheet = go.GetComponent<EmailSystemItem>();
            if (!emails.Contains(emailSheet))
            {
                emails.Add(emailSheet);
            }            
            emailSheet.InitData(this,sheetItems[m]);
        }
        if (sheetItems.Count > 0)
        {
            StartCoroutine(WaitAction(0.1f, () => {
                ChangeSelect(sheetItems[0].id);
            }));
        }
       
    }

    IEnumerator WaitAction(float timer,Action action)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    void CreateAwardItem(List<EmailInfoArticleItem> infoArticleItems)
    {
        foreach (var item in awardItems)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < infoArticleItems.Count; i++)
        {
            int m = i;
            GameObject go = null;
            if (m < awardItems.Count)
            {
                go = awardItems[m];
            }
            else
            {
                go = Instantiate(awardItem, awardContent);
                awardItems.Add(go);
            }
            go.SetActive(true);
            EmailAwardItem emailAward = go.GetComponent<EmailAwardItem>();
            emailAward.InitData(infoArticleItems[m]);
        }
    }

    public void ChangeSelect(int id)
    {
        foreach (var item in emails)
        {
            item.SelectItem(id);
        }
        CurEmailId = id;
        SystemDetail(id);
    }

    /// <summary>
    /// 详情
    /// </summary>
    void SystemDetail(int id)
    {
        EmailDb.GetEmailInfo(id, (infoData) => {
            if (infoData != null)
            {
                this.transform.SetDeepText("T_Detail2", infoData.data.info.content);
                List<EmailInfoArticleItem> emailInfos = infoData.data.info.article_info;
                CreateAwardItem(emailInfos);                
                if (infoData.data.info.status == 2)
                {
                    o_GetSingle.SetActive(false);
                }
                else
                {
                    o_GetSingle.SetActive(true);
                }
            }
        }, (error) => {
            o_GetSingle.SetActive(false);
            Debug.LogError(error);
        });
    }
}
