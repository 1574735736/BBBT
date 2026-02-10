using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallView : MonoBehaviour
{
    private GameObject o_MorePanel;
    private GameObject o_HideTop;
    private GameObject o_ShowTop;
    private RectTransform r_TopScaling;
    private GameObject o_LeftTalk;
    private CanvasGroup cv_LeftTalk;
    void Start()
    {
        AudioManager.Instance.PlayBackgroundMusic(AudioConfig.GameBgm);

        OnResetData();
        OnCompletes();
        OnUserClick();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    void OnResetData()
    {
        
    }

   
    /// <summary>
    /// 获取组件
    /// </summary>
    void OnCompletes()
    {
        o_MorePanel = this.gameObject.GetDeepGameObject("I_MorePanel");
        r_TopScaling = this.transform.FindDeepChild("P_TopScaling") as RectTransform;
        o_HideTop = this.gameObject.GetDeepGameObject("B_HideTop");
        o_ShowTop = this.gameObject.GetDeepGameObject("B_ShowTop");
        o_LeftTalk = this.transform.parent.FindDeepChild("P_LeftTalk").gameObject;
        cv_LeftTalk = o_LeftTalk.GetComponent<CanvasGroup>();

        o_ShowTop.gameObject.SetActive(false);
    }

    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    void OnUserClick()
    {
        this.transform.SetDeepButtonAction("B_Remember", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.MemoirsView);
        });
        this.transform.SetDeepButtonAction("B_Master", () =>
        {
            //UnBuildView();
            UIManager.Instance.OpenUI(UIConfig.CityLordView);
        });
        this.transform.SetDeepButtonAction("B_Tasks", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.ActivityView);
        });
        this.transform.SetDeepButtonAction("B_General", () =>
        {
            //UnBuildView();
            UIManager.Instance.OpenUI(UIConfig.GeneralView);
        });
        this.transform.SetDeepButtonAction("B_Map", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.HomelandView);
        });
        this.transform.SetDeepButtonAction("B_Recruit", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.RecruitmentView);
        });
        this.transform.SetDeepButtonAction("B_Backpack", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.BackpackView);
        });
        this.transform.SetDeepButtonAction("B_Shop", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.ShopView);            
        });
        this.transform.SetDeepButtonAction("B_More", () =>
        {
            if (o_MorePanel != null)
            {
                if (o_MorePanel.activeSelf)
                {
                    o_MorePanel.SetActive(false);
                }
                else
                {
                    o_MorePanel.SetActive(true);
                }
            }          
        });
        this.transform.SetDeepButtonAction("B_Setting", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.SettingView);
        });
        this.transform.SetDeepButtonAction("B_HideTop", () =>
        {
            r_TopScaling.DOScaleX(0, 0.3f);
            o_HideTop.SetActive(false);
            o_ShowTop.SetActive(true);
        });
        this.transform.SetDeepButtonAction("B_ShowTop", () =>
        {
            r_TopScaling.DOScaleX(1, 0.3f);
            o_HideTop.SetActive(true);
            o_ShowTop.SetActive(false);
        });
        this.transform.SetDeepButtonAction("B_Smoke", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.LotteryView);
        });
        this.transform.SetDeepButtonAction("B_Exchange", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.ExchangeView);
        });
        this.transform.SetDeepButtonAction("B_Activity", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.ActivityView);
        });
        this.transform.SetDeepButtonAction("B_RankList", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.RankView);
        });
        this.transform.SetDeepButtonAction("B_Bulletin", () =>
        {
            //UnBuildView();
            UIManager.Instance.OpenUI(UIConfig.NoticeView);
        });
        this.transform.SetDeepButtonAction("B_Email", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.EmailView);
            //UnBuildView();
        });
        this.transform.SetDeepButtonAction("B_MoneyShop", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.BankView);
        });
        this.transform.SetDeepButtonAction("B_Auction", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.AuctionView);
        });
        this.transform.SetDeepButtonAction("B_MainCity", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.CasinoView);
        });
        this.transform.SetDeepButtonAction("B_Weapon", () =>
        {
            //UnBuildView();
            
            UIManager.Instance.OpenUI(UIConfig.BlacksmithView);
        });
        this.transform.SetDeepButtonAction("B_PostStation", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.PostStationView);
        });
        this.transform.SetDeepButtonAction("B_Gangs", () =>
        {
            //UnBuildView();
            UIManager.Instance.OpenUI(UIConfig.FactionView);
        });
        this.transform.SetDeepButtonAction("B_Rebirth", () =>
        {
            //UnBuildView();
            
            UIManager.Instance.OpenUI(UIConfig.SamsaraView);
        });
        this.transform.SetDeepButtonAction("B_Ring", () =>
        {
            //UnBuildView();
            UIManager.Instance.OpenUI(UIConfig.PVPView);
        });
        this.transform.SetDeepButtonAction("B_Apprentice", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.ApprenticeshipView);
            //UnBuildView();
        });
        this.transform.SetDeepButtonAction("B_Stalls", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.StallView);
        });
        this.transform.SetDeepButtonAction("B_Teleport", () =>
        {
            UnBuildView();
        });

        this.o_LeftTalk.transform.SetDeepButtonAction("B_CloseTalk", () =>
        {            
            ShowLeftTalk(false);
        });
    }

    void DefaultAudio()
    {
        AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
    }

    void UnBuildView()
    {
        DefaultAudio();
        EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "当前未开发");
    }

    public void ShowLeftTalk(bool isShow)
    {
        if (isShow)
        {
            if (cv_LeftTalk == null)
            {
                Debug.Log("cv_LeftTalk is null !!!");
            }
            cv_LeftTalk.DOKill();
            cv_LeftTalk.alpha = 0;
            o_LeftTalk.SetActive(true);
            cv_LeftTalk.DOFade(1, 0.5f);
        }
        else
        {
            cv_LeftTalk.DOKill();
            cv_LeftTalk.alpha = 1;
            o_LeftTalk.SetActive(true);
            cv_LeftTalk.DOFade(0, 0.5f).OnComplete(() => {
                o_LeftTalk.SetActive(false);
            });
        }
      
    }
}
