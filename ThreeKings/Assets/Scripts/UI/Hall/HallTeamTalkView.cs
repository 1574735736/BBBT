using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallTeamTalkView : MonoBehaviour
{
    GameObject o_TalkBg;
    GameObject o_TeamBg;
    HallView hallView;
    void Start()
    {
        o_TalkBg = this.gameObject.GetDeepGameObject("I_TalkBg");
        o_TeamBg = this.gameObject.GetDeepGameObject("I_TeamBg");
        hallView = FindObjectOfType<HallView>();

        this.transform.SetDeepButtonAction("B_UserTalk", () =>
        {
            o_TalkBg.SetActive(true);
            o_TeamBg.SetActive(false);
            this.transform.FindDeepChild("I_UnUserTalk").gameObject.SetActive(false);
            this.transform.FindDeepChild("I_UnUserTeam").gameObject.SetActive(true);
            this.transform.FindDeepChild("I_UnUserFriends").gameObject.SetActive(true);
        });

        this.transform.SetDeepButtonAction("B_UserTeam", () =>
        {
            o_TalkBg.SetActive(false);
            o_TeamBg.SetActive(true);
            this.transform.FindDeepChild("I_UnUserTalk").gameObject.SetActive(true);
            this.transform.FindDeepChild("I_UnUserTeam").gameObject.SetActive(false);
            this.transform.FindDeepChild("I_UnUserFriends").gameObject.SetActive(true);
        });

        this.transform.SetDeepButtonAction("B_UserFriends", () =>
        {
            o_TalkBg.SetActive(false);
            o_TeamBg.SetActive(false);
            this.transform.FindDeepChild("I_UnUserTalk").gameObject.SetActive(true);
            this.transform.FindDeepChild("I_UnUserTeam").gameObject.SetActive(true);
            this.transform.FindDeepChild("I_UnUserFriends").gameObject.SetActive(false);
            UIManager.Instance.OpenUI(UIConfig.FriendListView);
        });

        this.transform.SetDeepButtonAction("I_TalkBg", () =>
        {
            hallView.ShowLeftTalk(true);
        });

        this.transform.SetDeepButtonAction("B_TeamList", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.TeamListView);
        });

        this.transform.SetDeepButtonAction("B_CreateTeam", () =>
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow,"正在制作");
        });

        o_TalkBg.SetActive(true);
        o_TeamBg.SetActive(false);
    }

}
