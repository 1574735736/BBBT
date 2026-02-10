using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonDb;

public class ChangeServerView : UIAction
{
    int selectIdx = 0;

    GameObject item;
    Transform newContent;
    Transform oldContent;

    List<ChangeServerItem> serverItems = new List<ChangeServerItem>();

    int createIndex = 0;
    int userIndexStart = 0;     //用户服务器起始索引
    
    void Start()
    {
        item = this.gameObject.GetDeepGameObject("Item");
        newContent = this.transform.FindDeepChild("NewContent");
        oldContent = this.transform.FindDeepChild("OldContent");

        this.transform.SetDeepButtonAction("B_GoInGame", OnGoInGame);

        ServerNet();
    }

    void ServerNet()
    {
        CommonDb.GetServerSheet(1, 100, (data) => {
            List<ServerInfo> serverInfos = data.data.data;
            foreach (var item in serverInfos)
            {
                CreateItem(newContent,null,item);
            }
            UserServerNet();
        }, (error) => { 
            Debug.LogError(error);
            UserServerNet();
        });
    }

    void UserServerNet()
    {
        UserDb.GetMyServer((data) => {
            List< UserDb.ServerInfo> serverInfos = data.data.data;
            if (serverInfos != null && serverInfos.Count > 0)
            {
                userIndexStart = createIndex;
                foreach (var item in serverInfos)
                {
                    CreateItem(oldContent, item, null);
                }
                Selectitem();
            }         
        }, (error) => {
            Debug.LogError(error);
            Selectitem();
        });
    }

    public override void OnClose()
    {
        _mainPanel.DoScaleHideBounce(Vector3.zero, 0.3f, () => {
            UIManager.Instance.CloseUI(() => {
                if (!string.IsNullOrEmpty(UserDb.UserToken))
                {
                    LoginPanel loginPanel = FindObjectOfType<LoginPanel>();
                    loginPanel.OnGoToGame();
                }
            });
        });
    }

    private void OnGoInGame()
    {
        foreach (var item in serverItems)
        {
            if (selectIdx == item.GetId())
            {
                EventManager.Instance.TriggerEvent("ServerSelect", item.GetServerName());
                DataManager.Instance.ServerId = item.GetServerId();
                DataManager.Instance.RoleID = item.GetRoleID();
                Debug.Log(" DataManager.Instance.RoleID  :" + DataManager.Instance.RoleID);
                break;
            }
        }
        OnClose();        
    }

    void Selectitem()
    {
        StartCoroutine(WaitAction(0.1f, () => { ChangeItem(userIndexStart); }));        
    }

    IEnumerator WaitAction(float timer,Action action)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }


    void CreateItem(Transform parentContent,UserDb.ServerInfo info , CommonDb.ServerInfo serverInfo)
    {
        GameObject go = Instantiate(item, parentContent);
        go.SetActive(true);
        ChangeServerItem serverItem = go.GetComponent<ChangeServerItem>();
        serverItems.Add(serverItem);
        if (info == null)
        {
            serverItem.InitData(this,createIndex, serverInfo);
        }
        else
        {
            serverItem.InitUserDbData(this,createIndex, info);
        }

        createIndex++;
    }



    public void ChangeItem(int index)
    {
        selectIdx = index;
        if (serverItems == null || serverItems.Count == 0)
        {
            return;
        }
        foreach (var item in serverItems)
        {
            item.SelectChange(index);
        }
    }

}
