using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserDb;

public class CharacterRoleView : MonoBehaviour
{
    [SerializeField] private LoginPanel loginPanel;
    int roleId = 14;
    void Start()
    {
        this.transform.SetDeepButtonAction("B_GoinGame", OnGoInGame);
        
    }

    public void OnShow()
    {
        this.transform.DoScaleOpenBounce(Vector3.one);
        InitRoleList();
    }

    private void InitRoleList()
    {
        UserDb.GetMyServer((data) => {
            List<UserDb.ServerInfo> serverInfos = data.data.data;
            if (serverInfos != null && serverInfos.Count > 0)
            {
                if (serverInfos[0].role.Count > 0)
                {
                    roleId = serverInfos[0].role[0].id;
                }
            }
        }, (error) => {
            Debug.LogError(error);
        });
    }

    public void OnHide()
    {
        this.transform.DoScaleHideBounce(Vector3.zero, 0.3f, () => {
            loginPanel.ChangeScene();
        });
    }

    private void OnGoInGame()
    {
        UserDb.JoinServer(roleId, (response) => {
            OnHide();
        }, (error) => {
            Debug.LogError(error);
            //OnHide();
        });
    }
}
