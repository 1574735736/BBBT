using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerTeamView : UIAction
{
    private GameObject o_AppointCaptain;            //任命队长
    private GameObject o_KickOut;                   //踢出队伍
    private GameObject o_RecruitingTeam;            //招募队员
    private GameObject o_DissolveTeam;              //解散队伍
    private GameObject o_QuitTeam;                  //离队
    List<MultiplayerItem> multiplayerItems = new List<MultiplayerItem>();
    void Start()
    {
        InitComplete();
        InitNet();
    }

    void InitComplete()
    {
        if (o_AppointCaptain != null)
        {
            return;
        }
        o_AppointCaptain = this.gameObject.GetDeepGameObject("B_AppointCaptain");
        o_KickOut = this.gameObject.GetDeepGameObject("B_KickOut");
        o_RecruitingTeam = this.gameObject.GetDeepGameObject("B_RecruitingTeam");
        o_DissolveTeam = this.gameObject.GetDeepGameObject("B_DissolveTeam");
        o_QuitTeam = this.gameObject.GetDeepGameObject("B_QuitTeam");
        multiplayerItems = this.gameObject.GetComponentsWithPrefix<MultiplayerItem>("Item");
    }

    void InitNet()
    {
        for (int i = 0; i < multiplayerItems.Count; i++)
        {
            multiplayerItems[i].InitReset(this);
        }
        TeamDb.GetMyTeam(DataManager.Instance.RoleID, (teamData) => {                      
            o_AppointCaptain.SetActive(teamData.data.type == 1);
            o_KickOut.SetActive(teamData.data.type == 1);
            o_RecruitingTeam.SetActive(teamData.data.type == 1);
            o_DissolveTeam.SetActive(teamData.data.type == 1);
            o_QuitTeam.SetActive(teamData.data.type == 2);

            var teamInfo = teamData.data.GetTeamInfo();
            if (teamInfo == null)
            {
                Debug.LogWarning("当前无有效车队信息");
                return;
            }

            if (teamData.data.GetTeamInfo().head_info != null)
            {
                multiplayerItems[0].InitData(teamData.data.GetTeamInfo().head_info,true);
            }         
            if (teamData.data.GetTeamInfo().one_info != null)
            {
                multiplayerItems[1].InitData(teamData.data.GetTeamInfo().one_info);
            }
            if (teamData.data.GetTeamInfo().two_info != null)
            {
                multiplayerItems[2].InitData(teamData.data.GetTeamInfo().two_info);
            }
            if (teamData.data.GetTeamInfo().three_info != null)
            {
                multiplayerItems[3].InitData(teamData.data.GetTeamInfo().three_info);
            }
            if (teamData.data.GetTeamInfo().four_info != null)
            {
                multiplayerItems[4].InitData(teamData.data.GetTeamInfo().four_info);
            }
        }, (error) => {
            Debug.LogError(error);
            HideAll();
        });
    }

    void HideAll()
    {
        for (int i = 0; i < multiplayerItems.Count; i++) {
            multiplayerItems[i].gameObject.SetActive(false);
        }
        o_AppointCaptain.SetActive(false);
        o_KickOut.SetActive(false);
        o_RecruitingTeam.SetActive(false);
        o_DissolveTeam.SetActive(false);
        o_QuitTeam.SetActive(false);
    }

    public void SelectItem(int roleId)
    {
        for (int i = 0; i < multiplayerItems.Count; i++)
        {
            multiplayerItems[i].OnSelectItem(roleId);
        }
    }

}

