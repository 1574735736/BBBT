using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TeamDb;

public class TeamApplicationView : UIAction
{
    GameObject item;
    Transform content;
    MyTeamData teamData;
    List<TeamApplicationItem> applicationItems = new List<TeamApplicationItem>();
    void Start()
    {
        item = this.gameObject.GetDeepGameObject("Item");
        content = this.transform.FindDeepChild("Content");
        InitNet();

        
    }

    void InitNet()
    {
        TeamDb.GetMyTeam(DataManager.Instance.RoleID, (response) => {
            teamData = response.data;
            InitApplicationList();
        }, (error) => {
            Debug.LogError(error);
            foreach (var item in applicationItems)
            {
                item.gameObject.SetActive(false);
            }
        });


    }

    void InitApplicationList()
    {
        TeamDb.GetTeamApplySheet(teamData.GetTeamInfo().id, (response) => {
            List<TeamApplyInfo> teamApplyInfos = response.data.data;
            OnCreateItem(teamApplyInfos);
        }, (error) => {
            Debug.LogError(error);
            foreach (var item in applicationItems)
            {
                item.gameObject.SetActive(false);
            }
        });
    }

    void OnCreateItem(List<TeamApplyInfo> teamApplies)
    {
        for (int i = 0; i < applicationItems.Count; i++)
        {
            applicationItems[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < teamApplies.Count; i++)
        {
            int m = i;
            GameObject go = null;
            if (m < applicationItems.Count)
            {
                go = applicationItems[m].gameObject;
            }
            else
            {
                go = Instantiate(item, content);
            }
            go.SetActive(true);
            TeamApplicationItem teamList = go.GetComponent<TeamApplicationItem>() ?? go.AddComponent<TeamApplicationItem>();
            applicationItems.Add(teamList);
            teamList.InitData(this,teamApplies[m]);
        }
    }

    /// <summary>
    /// Ë¢ÐÂ½çÃæ 
    /// </summary>
    public void OnRefreshView()
    {
        InitNet();
    }
}
