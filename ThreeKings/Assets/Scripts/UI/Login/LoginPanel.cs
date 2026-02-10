using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class LoginPanel : MonoBehaviour
{
    private GameObject o_GoToGame;
    private GameObject o_Loading;

    private TextMeshProUGUI loadingText;
    private TextMeshProUGUI progressText;
    private Image progressSlider;

    private AsyncOperation loadOperation;
    private int dotCount;
    private float dotTimer;
    private float loadingStartTime;
    private float currentProgress;

    private float minLoadingTime = 2.0f;
    private float progressSmoothingSpeed = 5f;
    private float dotUpdateInterval = 0.3f;

    private bool isLoading = false;

    [SerializeField] private CharacterRoleView characterRoleView;
    [SerializeField] private CreateRoleView createRoleView;

    void Start()
    {
        o_GoToGame = this.gameObject.GetDeepGameObject("B_Login");
        o_Loading = this.gameObject.GetDeepGameObject("I_Loading");

        loadingText = this.transform.GetDeepComponent<TextMeshProUGUI>("T_Loading");
        progressText = this.transform.GetDeepComponent<TextMeshProUGUI>("T_Progress");
        progressSlider = this.transform.GetDeepComponent<Image>("I_GameProgress");

        this.transform.SetDeepButtonAction("B_ChangeServer", OnChangeServer);

        this.transform.SetDeepButtonAction("B_Login", () => {
            UIManager.Instance.OpenUI(UIConfig.AccountLoginView);
        });// OnGoToGame);

        o_GoToGame.SetActive(true);
        o_Loading.SetActive(false);
        ServerNet();

        EventManager.Instance.StartListening("ServerSelect", OnServerName);
    }

    void ServerNet()
    {
        CommonDb.GetServerSheet(1, 100, (data) => {
            List<CommonDb.ServerInfo> serverInfos = data.data.data;
            this.transform.SetDeepText("T_ServerInfo", serverInfos[0].name);
        }, (error) => {
            Debug.LogError(error);
        });
    }

    private void OnServerName(object obj)
    {
        string name = (string)obj;
        this.transform.SetDeepText("T_ServerInfo", name);
    }

    void Update()
    {
        if (isLoading)
        {
            UpdateLoadingDots();
            UpdateProgressSlider();
        }
    }

    /// <summary>
    /// 点击换区
    /// </summary>
    public void OnChangeServer()
    {
        //AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        UIManager.Instance.OpenUI(UIConfig.ChangeServerView);
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void OnGoToGame()
    {
        //AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
        
        ShowLoadingPanel();
        StartCoroutine(LoadGameScene());
    }

    private void ShowLoadingPanel()
    {
        o_GoToGame.SetActive(false);
        o_Loading.SetActive(true);
        isLoading = true;
        // 初始化加载界面
        loadingText.text = "游戏加载中";
        progressText.text = "0%";
        progressSlider.fillAmount = 0f;
        currentProgress = 0f;
        dotCount = 0;
        dotTimer = 0f;
    }

    private IEnumerator LoadGameScene()
    {
        loadingStartTime = Time.time;

        // 异步加载场景
        loadOperation = SceneManager.LoadSceneAsync("Hall");
        loadOperation.allowSceneActivation = false;

        // 等待加载到90%
        while (loadOperation.progress < 0.9f)
        {
            currentProgress = loadOperation.progress;
            yield return null;
        }

        // 模拟剩余10%的加载
        float virtualLoadStartTime = Time.time;
        while (Time.time - virtualLoadStartTime < 0.5f)
        {
            float virtualProgress = Mathf.Clamp01((Time.time - virtualLoadStartTime) / 0.5f);
            currentProgress = 0.9f + 0.1f * virtualProgress;
            yield return null;
        }

        currentProgress = 1.0f;

        // 确保最小加载时间
        while (Time.time - loadingStartTime < minLoadingTime)
        {
            yield return null;
        }

        if (DataManager.Instance.RoleID > 0)
        {
            characterRoleView.gameObject.SetActive(true);
            characterRoleView.OnShow();
        }
        else
        {
            createRoleView.gameObject.SetActive(true);
            createRoleView.OnShow();
        }

        //ChangeScene();
    }

    public void ChangeScene()
    {
        // 允许场景切换
        UIManager.Instance.GoToScene(Enums.SceneType.Hall, () =>
        {
            loadOperation.allowSceneActivation = true;
        }, null);
    }

    private void UpdateProgressSlider()
    {
        progressSlider.fillAmount = Mathf.Lerp(progressSlider.fillAmount, currentProgress, Time.deltaTime * progressSmoothingSpeed);

        // 更新进度文本显示
        int displayProgress = Mathf.RoundToInt(progressSlider.fillAmount * 100);
        progressText.text = $"{displayProgress}%";
    }

    private void UpdateLoadingDots()
    {
        dotTimer += Time.deltaTime;

        if (dotTimer >= dotUpdateInterval)
        {
            dotTimer = 0f;
            dotCount = (dotCount + 1) % 4;
            loadingText.text = "游戏加载中" + new string('.', dotCount);
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening("ServerSelect", OnServerName);
    }
}
