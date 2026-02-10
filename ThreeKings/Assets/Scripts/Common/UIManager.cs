using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private Transform uicanvas;
    public string quitGamePanelName = "SettingPanel";
    private Stack<GameObject> uiStack = new Stack<GameObject>();
    private Canvas UICanvas;

    private HashSet<string> loadingUIs = new HashSet<string>();
    private Dictionary<string, GameObject> openedUIs = new Dictionary<string, GameObject>();
    
    protected override void Init()
    {
        EventManager.Instance.StartListening(Enums.Lisiner.TutorialNotice, OnTutorialOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiStack.Count > 0)
            {
                CloseUI();
            }
            else
            {
                OpenUI(quitGamePanelName);
            }
        }
    }

    public void LoadScene(string name, LoadSceneMode model = LoadSceneMode.Single)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name, model);
    }

    public AsyncOperation LoadSceneAsync(string name, LoadSceneMode model = LoadSceneMode.Single)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, model);
        return operation;
    }

    public string GetCurSceneName()
    {
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        return scene.name;
    }

    public void GoToScene(Enums.SceneType sceneType,Action sAction = null,Action eAction = null)
    {
        while (uiStack.Count > 0 )
        {
            CloseUI();
        }
        DataManager.Instance.CurScene = sceneType;

        TransitionPage.Instance.StartToNewScene(() => {
            sAction?.Invoke();
            TransitionPage.Instance.EndToNewScene(() => { 
                eAction?.Invoke();  
            });
        });
    }

    public void RemoveAllUI()
    {
        while (uiStack.Count > 0)
        {
            CloseUI();
        }
    }

    public void ChangeScene(Enums.SceneType sceneType, Action sAction = null, Action eAction = null)
    {
        while (uiStack.Count > 0)
        {
            CloseUI();
        }
        DataManager.Instance.CurScene = sceneType;

        TransitionPage.Instance.StartToNewScene(() => {
            LoadScene(sceneType.ToString());
            sAction?.Invoke();
            TransitionPage.Instance.EndToNewScene(() => {
                eAction?.Invoke();
            });
        });
    }


    public bool IsInUI()
    {
        return uiStack.Count > 0;
    }

    public void OpenUI(string uiName, Action onUIChanged = null,object initData = null)
    {
        if (loadingUIs.Contains(uiName))
        {
            Debug.LogWarning($"UI {uiName} is already loading");
            return;
        }

        //if (uiName == UIConfig.ID_Panel)
        //{
        //    OpenQuitGame();
        //}

        if (openedUIs.ContainsKey(uiName))
        {
            Debug.LogWarning($"UI {uiName} is already open");
            // 将已打开的UI置顶
            BringUIToTop(uiName);
            return;
        }

        // 标记为正在加载
        loadingUIs.Add(uiName);

        ResManager.Instance.LoadPrefabByAa(uiName, UICanvaces(), (obj) => {
            loadingUIs.Remove(uiName);
            if (obj == null)
            {
                Debug.LogError("UI prefab not found for name: " + uiName);
                return;
            }
            GameObject uiInstance = obj;//Instantiate(obj, UICanvaces());
            uiInstance.transform.SetParent(UICanvaces());
            uiInstance.transform.localScale = Vector3.one;
            uiInstance.transform.localPosition = Vector3.zero;
            uiStack.Push(uiInstance);
            openedUIs[uiName] = uiInstance;

            if (initData != null)
            {
                var uiScript = uiInstance.GetComponent<UIAction>();

                if (uiScript != null)
                {
                    uiScript.InitData(initData);
                }
            }


            onUIChanged?.Invoke();

        }, null);

        //GameObject uiPrefab = ResManager.Instance.LoadResource<GameObject>(uiName);
      
    }

    public void CloseUI(Action onUIChanged = null)
    {
        if (uiStack.Count > 0)
        {
            GameObject topUI = uiStack.Pop();
            foreach (var kvp in openedUIs)
            {
                if (kvp.Value == topUI)
                {
                    openedUIs.Remove(kvp.Key);
                    break;
                }
            }
            Destroy(topUI);
            onUIChanged?.Invoke();
        }
    }

    private void BringUIToTop(string uiName)
    {
        if (openedUIs.TryGetValue(uiName, out GameObject uiInstance))
        {
            // 从栈中移除
            Stack<GameObject> tempStack = new Stack<GameObject>();
            while (uiStack.Count > 0)
            {
                GameObject top = uiStack.Pop();
                if (top != uiInstance)
                {
                    tempStack.Push(top);
                }
                else
                {
                    break;
                }
            }

            // 重新入栈（置顶）
            uiStack.Push(uiInstance);

            // 恢复其他UI的顺序
            while (tempStack.Count > 0)
            {
                uiStack.Push(tempStack.Pop());
            }

            // 设置为最上层
            uiInstance.transform.SetAsLastSibling();
        }
    }

    public void InsertUI(GameObject uiInstance)
    {
        uiStack.Push(uiInstance);
    }

    public void RemoveUI(GameObject uiToRemove, Action onUIChanged = null)
    {
        Stack<GameObject> tempStack = new Stack<GameObject>();
        bool found = false;
        while (uiStack.Count > 0)
        {
            GameObject topUI = uiStack.Pop();

            if (topUI == uiToRemove)
            {
                foreach (var kvp in openedUIs)
                {
                    if (kvp.Value == topUI)
                    {
                        openedUIs.Remove(kvp.Key);
                        break;
                    }
                }
                Destroy(topUI);
                found = true;
                break;
            }
            else
            {
                tempStack.Push(topUI);
            }
        }

        while (tempStack.Count > 0)
        {
            uiStack.Push(tempStack.Pop());
        }
        if (found)
        {
            onUIChanged?.Invoke();
        }
    }

    /// <summary>
    /// 关闭现有UI，打开新的UI
    /// </summary>
    public void CloseCurrentAndOpenUI(string uiName, Action onUIChanged = null, object initData = null)
    {
        if (uiStack.Count == 0)
        {
            Debug.LogWarning("没有打开的UI可以关闭，直接打开新UI");
            OpenUI(uiName, onUIChanged, initData);
            return;
        }

        // 关闭当前UI
        CloseUI(() => {
            // 打开新UI
            OpenUI(uiName, () => {
                onUIChanged?.Invoke();
            }, initData);
        });
    }

    void OpenQuitGame()
    {
        Timer.Instance.AddDelayedAction(1800f, () => {
            //if (!LoginDb.GetUserAuthStatus())
            //{
            //    OpenUI(UIConfig.CommonTip, () => {
            //        CommonTipView tipView = FindObjectOfType<CommonTipView>();
            //        tipView.InitInfo("未实名不能继续游戏","退出游戏","返回登录", QuitGame, ReturnLoading,
            //            () => {
            //                LoginDb.QuitGameAuth((success, msg) => { });
            //            });
            //    }, null);
            //}           
        });
    }

    public void OpenNetontinue()
    {
        //OpenUI(UIConfig.CommonTip, () => {
        //    //CommonTipView tipView = FindObjectOfType<CommonTipView>();
        //    //tipView.InitInfo("网络异常，链接中断", "退出游戏", "返回登录", QuitGame, ReturnLoading,
        //    //    () => {
        //    //        LoginDb.QuitGameAuth((success, msg) => { });
        //    //    });
        //}, null);
    }


    private void OnTutorialOpen(object obj)
    {
        //if (DataManager.Instance.IsTutorialed)
        //{
        //    return;
        //}
        //if (!LoginDb.GetUserAuthStatus() && (DataManager.Instance.TutorialLevel == 1 || DataManager.Instance.TutorialLevel == 2))
        //{
        //    DataManager.Instance.TutorialLevel = 3;
        //}

        //switch (DataManager.Instance.TutorialLevel)
        //{
        //    case 0:
        //    case 1:

        //        List<MailData> hasEmails = EmailDb.AllMails.FindAll(m => EmailDb.HasAward(m) && !EmailDb.IsAwardDrawn(m));
        //        if (hasEmails.Count == 0)
        //        {
        //            EmailDb.GetMailList((success, str) => {
        //                if (success)
        //                {
        //                    hasEmails = EmailDb.AllMails.FindAll(m => EmailDb.HasAward(m) && !EmailDb.IsAwardDrawn(m));
        //                    if (hasEmails.Count > 0)
        //                    {
        //                        UIManager.Instance.OpenUI(UIConfig.TutorialPanel);
        //                    }
        //                }
        //            });
        //        }
        //        else
        //        {
        //            UIManager.Instance.OpenUI(UIConfig.TutorialPanel);
        //        }
        //        break;
        //    case 2:
        //        if (EmailDb.AllMails.Count > 0)
        //        {
        //            List<MailData> unclaimedMails = EmailDb.AllMails.FindAll(m => EmailDb.HasAward(m) && !EmailDb.IsAwardDrawn(m));
        //            if (unclaimedMails.Count == 0)
        //            {
        //                return;
        //            }
        //            else
        //            {
        //                UIManager.Instance.OpenUI(UIConfig.TutorialPanel);
        //            }
        //        }                
        //        break;
        //    case 3:
        //        UIManager.Instance.OpenUI(UIConfig.TutorialPanel);
        //        break;
        //    default:
        //        break;
        //}
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void ReturnLoading()
    {
        ChangeScene(Enums.SceneType.Loading);
    }

    private Transform UICanvaces()
    {
        if (UICanvas == null)
        {
            if (GameObject.Find("UICanvas") != null)
            {
                UICanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            }
            else
            {
                CreateAlertCanvas(100);
            }                
        }
        if (uicanvas != null)
        {
            return uicanvas;
        }
        if (UICanvas != null)
        {
            uicanvas = UICanvas.transform;
            return uicanvas;
        }
        uicanvas = GameObject.Find("UICanvas").transform;
        return uicanvas;
    }

    private void CreateAlertCanvas(int sortOrder)
    {
        GameObject obj = new GameObject("UICanvas");
        obj.transform.position = new Vector3(0, 0, 90);
        GameObject.DontDestroyOnLoad(obj);
        UICanvas = obj.AddComponent<Canvas>();
        UICanvas.renderMode = FrameworkConfig.RenderMode;

        UICanvas.worldCamera = Camera.main;

        UICanvas.planeDistance = FrameworkConfig.AlertPlaneDistance;
        UICanvas.gameObject.layer = 5;
        UICanvas.sortingOrder = sortOrder;
        UICanvas.gameObject.AddComponent<CanvasScaler>();
        UICanvas.gameObject.AddComponent<GraphicRaycaster>();
        UICanvas.gameObject.AddComponent<CanvasFix>();
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening(Enums.Lisiner.TutorialNotice, OnTutorialOpen);
    }

}
