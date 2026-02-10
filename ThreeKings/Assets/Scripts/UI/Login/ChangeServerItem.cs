using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChangeServerItem : MonoBehaviour
{

    int itemIndex = 0;
    TextMeshProUGUI t_Info;
    GameObject o_Select;
    CommonDb.ServerInfo server;
    UserDb.ServerInfo serverInfo;
    ChangeServerView changeServerView;

    void Start()
    {
        this.transform.GetComponent<Button>().onClick.AddListener(() => {
            changeServerView.ChangeItem(itemIndex);
        });
    }

    public void InitData(ChangeServerView serverView, int idx, CommonDb.ServerInfo serverInfo)
    {
        server = serverInfo;
        itemIndex = idx;
        changeServerView = serverView;
        if (t_Info == null)
        {
            t_Info = this.transform.GetDeepComponent<TextMeshProUGUI>("T_SelectInfo");
            o_Select = this.gameObject.GetDeepGameObject("I_Select");
        }
        t_Info.text = serverInfo.name;
        o_Select.SetActive(false);
    }

    public void InitUserDbData(ChangeServerView serverView, int idx, UserDb.ServerInfo info)
    {
        serverInfo = info;
        itemIndex = idx;
        changeServerView = serverView;
        if (t_Info == null)
        {
            t_Info = this.transform.GetDeepComponent<TextMeshProUGUI>("T_SelectInfo");
            o_Select = this.gameObject.GetDeepGameObject("I_Select");
        }
        t_Info.text = serverInfo.name;
        o_Select.SetActive(false);
    }

    public void SelectChange(int index)
    {        
        if (o_Select == null) return;
        o_Select.SetActive(index == itemIndex);
    }

    public int GetId()
    {
        return itemIndex;
    }

    public int GetServerId()
    {
        if (serverInfo != null)
        {
            return serverInfo.id;
        }
        if (server != null)
        {
            return server.id;
        }
        return 0;
    }

    public int GetRoleID()
    {
        if (serverInfo != null)
        {
            if (serverInfo.role.Count > 0)
            {
                return serverInfo.role[0].id;
            }            
        }
        return -1;
    }

    public string GetServerName()
    {
        if (serverInfo != null)
        {
            return serverInfo.name;
        }
        if (server != null)
        {
            return server.name;
        }
        return "";
    }
}
