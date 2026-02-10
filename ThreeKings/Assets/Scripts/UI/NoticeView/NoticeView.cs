using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static NoticeDb;


public class NoticeView : UIAction
{
    GameObject o_Pag1;
    GameObject o_Pag2;

    GameObject item;
    Transform content;

    List<NoticeSheetItem> noticeSheets = new List<NoticeSheetItem>();
    void Start()
    {
        o_Pag1 = this.gameObject.GetDeepGameObject("P_NoticeList");
        o_Pag2 = this.gameObject.GetDeepGameObject("P_NoticeDetail");
        
        this.transform.SetDeepButtonAction("B_ReturnList", () => { ChangePage(1); });

        item = this.transform.FindDeepChild("Item").gameObject;
        content = this.transform.FindDeepChild("Content");

        InitNet();
    }

    void InitNet()
    {
        NoticeDb.GetNoticeAttr((response) => {
            List<NoticeAttrItem> noticeAttrItems = response.data.data;
            OnLimitNet(noticeAttrItems, 0);
        }, (error) => {
            Debug.Log(error);
        });
    }

    void OnLimitNet(List<NoticeAttrItem> noticeAttrs,int addIndex = 0)
    {
        if (addIndex >= noticeAttrs.Count)
        {
            CreateItem();
            return;
        }
        NoticeAttrItem  noticeAttrItem = noticeAttrs[addIndex];
        NoticeDb.GetNoticeSheet(noticeAttrItem.id, 1, 100, (sheetResponse) => {
            noticeSheets.AddRange(sheetResponse.data.list);
            addIndex++;
            OnLimitNet(noticeAttrs, addIndex);
        }, (error) => {
            Debug.LogError(error);
        });
    }

    void CreateItem()
    {
        if (noticeSheets == null || noticeSheets.Count == 0)
        {
            return;
        }
        foreach (var sheet in noticeSheets)
        {
            GameObject go = UnityEngine.Object.Instantiate(item, content);
            go.SetActive(true);
            NoticeItem notice = go.GetComponent<NoticeItem>();
            notice.InitData(this,sheet);
        }
    }

    //void OnItemList()
    //{
    //    for (int i = 1; i < 6; i++) { 
    //        int m = i;
    //        Transform gp = this.transform.FindDeepChild("Item" + m);
    //        gp.SetDeepButtonAction("B_ShowDetail", () => { ChangePage(2); });
    //    }
    //}

    public void OpenDetail(int noticeId)
    {
        ChangePage(2);
        NoticeDb.GetNoticeInfo(noticeId, (noticeInfo) => {
            this.transform.SetDeepText("T_DetailTitle", noticeInfo.data.data.title);
            this.transform.SetDeepText("T_DetailImfo", noticeInfo.data.data.content);
            this.transform.SetDeepText("T_DetailTime", noticeInfo.data.data.create_time_text);
        },(error) => {
            Debug.LogError(error);
        });

    }

    void ChangePage(int pagIndex)
    {
        o_Pag1.SetActive(pagIndex == 1);
        o_Pag2.SetActive(pagIndex == 2);
    }
}
