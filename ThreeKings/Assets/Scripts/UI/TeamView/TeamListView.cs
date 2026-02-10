using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TeamDb;

public class TeamListView : UIAction
{
    GameObject teamItem;
    Transform content;
    GameObject createItem;
    GameObject scrollView;

    MyTeamData myTeamData;

    List<TeamListItem> teamListItems = new List<TeamListItem>();
    void Start()
    {
        teamItem = this.gameObject.GetDeepGameObject("Item");
        createItem = this.gameObject.GetDeepGameObject("CreateItem");
        content = this.transform.FindDeepChild("Content");
        scrollView = this.gameObject.GetDeepGameObject("ScrollView");

        this.transform.SetDeepButtonAction("B_TeamUp", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.TeamApplicationView);
        });
        this.transform.SetDeepButtonAction("B_Self", () =>
        {
            OnOneTeam();
            this.transform.SetDeepText("T_ListInfo", "我的队伍");
        });
        this.transform.SetDeepButtonAction("B_Other", () =>
        {
            OnOtherTeam();
            this.transform.SetDeepText("T_ListInfo", "其他队伍");
        });
        teamItem.AddComponent<Button>().onClick.AddListener(() => {
            UIManager.Instance.OpenUI(UIConfig.MultiplayerTeamView);
        });

        this.transform.SetDeepButtonAction("CreateItem", OnCreateMyTeam);
        this.transform.SetDeepText("T_ListInfo", "我的队伍");
        InitNet();        
    }

    void InitNet()
    {
        scrollView.SetActive(false);    
        TeamDb.GetMyTeam(DataManager.Instance.RoleID, (response) => {
            if (response.data.type == 0)
            {
                teamItem.SetActive(false);
                createItem.SetActive(true);
            }
            else {
                OnUserTeamShow(response.data);
            }               
        }, (error) => {
            Debug.LogError(error);
            teamItem.SetActive(false);
            createItem.SetActive(true);            
        });
    }

    void OnOtherTeam()
    {
        teamItem.SetActive(false);
        createItem.SetActive(false);
        scrollView.SetActive(true);
        TeamDb.GetTeamSheet(1, 100, (response) => {
            List<TeamSimpleInfo> teamSimples = response.data.data;
            CreateTeamListItem(teamSimples);
        }, (error) => {
            Debug.LogError(error);
        });
    }

    void OnOneTeam()
    {
        scrollView.gameObject.SetActive(false);
        InitNet();
    }

    /// <summary>
    /// 创建我的队伍
    /// </summary>
    void OnCreateMyTeam()
    {
        TeamDb.CreateTeam(DataManager.Instance.RoleID, "牛逼的车队", 10, "", (response) => {
            OnOneTeam();
        }, (error) => {
            Debug.LogError(error);
            teamItem.SetActive(false);
            createItem.SetActive(true);
        });
    }

    /// <summary>
    /// 显示我的队伍
    /// </summary>
    void OnUserTeamShow(MyTeamData myTeam)
    {
        myTeamData = myTeam;
        teamItem.SetActive(true);
        createItem.SetActive(false);
        teamItem.transform.SetDeepText("T_TeamStatus", "离队");
        teamItem.transform.SetDeepText("T_TeamName", myTeam.GetTeamInfo().name);
        teamItem.transform.SetDeepText("T_TeamNum", string.Format("最低等级 {0}" , myTeam.GetTeamInfo().min_level));
        teamItem.SetDeepButtonAction("B_Join", () => { QuitTeam(); });
    }

    /// <summary>
    /// 退队
    /// </summary>
    void QuitTeam()
    {
        if (myTeamData == null)
        {
            return;
        }
        TeamDb.BackTeam(DataManager.Instance.RoleID, myTeamData.GetTeamInfo().id, (myTeamData) => {
            teamItem.SetActive(false);
            createItem.SetActive(true);
        }, (error) => {
            Debug.LogError(error);
        });
    }

    void CreateTeamListItem(List<TeamSimpleInfo> simpleInfos)
    {
        for (int i = 0; i < teamListItems.Count; i++) {
            teamListItems[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < simpleInfos.Count; i++)
        {
            int m = i;
            GameObject go = null;
            if (m < teamListItems.Count)
            {
                go = teamListItems[m].gameObject;
            }
            else
            {
                go = Instantiate(teamItem, content);                           
            }
            go.SetActive(true);
            TeamListItem teamList = go.GetComponent<TeamListItem>() ?? go.AddComponent<TeamListItem>();
            teamListItems.Add(teamList);
            teamList.InitData(simpleInfos[m]);
        }
    }

}
