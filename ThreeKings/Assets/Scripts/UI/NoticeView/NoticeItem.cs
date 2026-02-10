using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NoticeDb;

public class NoticeItem : MonoBehaviour
{
    NoticeSheetItem noticeSheet;    
    NoticeView view;
    void Start()
    {
        this.transform.SetDeepButtonAction("B_ShowDetail", OnClickDetail);
    }

    public void InitData(NoticeView noticeView,NoticeSheetItem sheetItem)
    {
        noticeSheet = sheetItem;
        view = noticeView;
        this.transform.SetDeepText("T_ItemDetail", sheetItem.title);
    }

    private void OnClickDetail()
    {
        view.OpenDetail(noticeSheet.id);
    }

}
