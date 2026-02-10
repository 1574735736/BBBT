using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TeamDb;

public class MultiplayerItem : MonoBehaviour
{
    MultiplayerTeamView multiplayerTeam;
    GameObject o_Check;
    bool userIsCaptain = false;
    UserRoleInfo userRoleInfo = null;
    void Start()
    {
        o_Check = this.gameObject.GetDeepGameObject("I_Check");
        this.transform.SetDeepButtonAction("B_SelectBg", OnCheckSelect);
    }

    public void InitData(UserRoleInfo userRole,bool isCaptain = false)
    {
        userIsCaptain = isCaptain;
        userRoleInfo = userRole;
        this.gameObject.GetDeepGameObject("I_PlayerRow").SetActive(false);
        this.transform.SetDeepText("T_PlayerName", userRole.nickname);
        if (isCaptain)
        {
            this.gameObject.GetDeepGameObject("B_SelectBg").SetActive(false);
        }
    }

    public void InitReset(MultiplayerTeamView teamView)
    {
        teamView = multiplayerTeam;
        this.transform.SetDeepText("T_PlayerName", "");
        this.transform.SetDeepText("T_PlayerInfo", "");
        this.gameObject.GetDeepGameObject("I_PlayerRow").SetActive(true);
        if (o_Check != null)
        {
            o_Check.SetActive(false);
        }
        this.gameObject.GetDeepGameObject("B_SelectBg").SetActive(true);
    }


    private void OnCheckSelect()
    {
        if (userRoleInfo != null)
        {
            multiplayerTeam.SelectItem(userRoleInfo.id);
        }
    }

    public void OnSelectItem(int id)
    {
        if (o_Check != null)
        {
            o_Check.SetActive(userRoleInfo.id == id);
        }
    }

}
