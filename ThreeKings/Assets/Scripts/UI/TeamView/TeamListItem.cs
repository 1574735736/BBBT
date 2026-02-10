using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TeamDb;

public class TeamListItem : MonoBehaviour
{
    TeamSimpleInfo teamInfo;
    void Start()
    {
        this.transform.SetDeepButtonAction("B_Join", OnJoinTeam);
    }

    public void InitData(TeamSimpleInfo simpleInfo)
    {
        teamInfo = simpleInfo;
        this.transform.SetDeepText("T_TeamName", simpleInfo.name);
        this.transform.SetDeepText("T_TeamNum", "最低等级 " + simpleInfo.min_level);
        this.transform.SetDeepText("T_TeamStatus", "加入");        
    }

    void OnJoinTeam()
    {
        if (teamInfo == null)
        {
            return;
        }
        TeamDb.JoinTeam(DataManager.Instance.RoleID, teamInfo.id, "", (response) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "加入成功");
        }, (error) => {
            Debug.LogError(error);
        });
    }
}
