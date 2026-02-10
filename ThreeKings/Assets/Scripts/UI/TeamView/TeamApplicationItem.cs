using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TeamDb;

public class TeamApplicationItem : MonoBehaviour
{
    TeamApplyInfo info;
    TeamApplicationView applicationView;
    void Start()
    {
        this.transform.SetDeepButtonAction("B_Agree", OnAgree);
        this.transform.SetDeepButtonAction("B_Refuse", OnRefuse);
    }

    private void OnRefuse()
    {
        TeamDb.TeamApplyPro(info.id, -1, (response) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "拒绝成功");
            applicationView.OnRefreshView();
        }, (error) => {
            Debug.LogError(error);
        });
    }

    private void OnAgree()
    {
        TeamDb.TeamApplyPro(info.id, 1, (response) => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "申请通过");
            applicationView.OnRefreshView();
        }, (error) => {
            Debug.LogError(error);
        });
    }

    public void InitData(TeamApplicationView team, TeamApplyInfo applyInfo)
    {
        info = applyInfo;
        applicationView = team;
        this.transform.SetDeepText("T_RepairInfo", applyInfo.user_role.nickname);
    }



}
