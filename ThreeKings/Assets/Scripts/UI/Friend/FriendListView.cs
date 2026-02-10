using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FriendDb;

public class FriendListView : UIAction
{
    GameObject o_FriendItem;
    Transform friendContent;
    TMP_InputField ipt_SerchID;     //输入名称或ID
    TMP_InputField ipt_UserTalk;        //用户聊天输入

    List<GameObject> o_Items = new List<GameObject>();

    void Start()
    {
        o_FriendItem = this.gameObject.GetDeepGameObject("FriendItem");
        friendContent = this.transform.FindDeepChild("FriendContent");
        ipt_SerchID = this.transform.GetDeepComponent<TMP_InputField>("Ipt_BrotherId");
        ipt_UserTalk = this.transform.GetDeepComponent<TMP_InputField>("Ipt_UserTalk");

        this.transform.SetDeepButtonAction("B_BadBrother", OnBadBrother);
        this.transform.SetDeepButtonAction("B_GoodBrother", OnGoodBrother);
        this.transform.SetDeepButtonAction("B_Serch", OnSerchBrother);
        this.transform.SetDeepButtonAction("B_UserSend", OnSendTalk);
    }


    /// <summary>
    /// 黑名单
    /// </summary>
    void OnBadBrother()
    {
        FriendDb.GetMyHailList(DataManager.Instance.RoleID, 1, 1, 100, (data) => {
            List<HailFriendItem> hailFriendItems = data.data.data;
            CreateItem(hailFriendItems);
        }, (error) => {
            Debug.LogError(error);
        });
    }



    /// <summary>
    /// 好友
    /// </summary>
    void OnGoodBrother()
    {
        FriendDb.GetMyHailList(DataManager.Instance.RoleID, 0, 1, 100, (data) => {
            List<HailFriendItem> hailFriendItems = data.data.data;
            CreateItem(hailFriendItems);
        }, (error) => {
            Debug.LogError(error);
        });
    }
    
    /// <summary>
    /// 查找好友
    /// </summary>
    void OnSerchBrother()
    {
        if (string.IsNullOrEmpty(ipt_SerchID.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "请输入名称或ID");
            return;
        }
    }

    /// <summary>
    /// 发送聊天内容
    /// </summary>
    void OnSendTalk()
    {
        if (string.IsNullOrEmpty(ipt_UserTalk.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "请输入内容");
            return;
        }
    }

    void CreateItem(List<HailFriendItem> friendItems)
    {
        foreach (var item in o_Items)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < friendItems.Count; i++)
        {
            int m = i;
            if (m < o_Items.Count)
            {
                o_Items[m].SetActive(true);
                FriendListItem listItem = o_Items[m].GetComponent<FriendListItem>();
                listItem.InitData(friendItems[m]);
            }
            else
            {
                GameObject go = Instantiate(o_FriendItem, friendContent);
                go.SetActive(true);
                o_Items.Add(go);
                FriendListItem listItem = go.GetComponent<FriendListItem>();
                listItem.InitData(friendItems[m]);
            }
        }
    }
}

