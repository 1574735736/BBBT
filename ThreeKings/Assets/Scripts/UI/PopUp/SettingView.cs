using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelView : UIAction
{
    // UI组件引用
    [Header("音乐设置")]
    public Toggle musicToggle; // 音乐开关Toggle

    [Header("音效设置")]
    public Toggle soundEffectToggle; // 音效开关Toggle

    private TextMeshProUGUI t_Server;   //所处服务器
    private TextMeshProUGUI t_Name;     //用户名称
    private TextMeshProUGUI t_UserId;   //用户ID
    private TMP_InputField ipt_code;    //兑换码



    void Start()
    {
        InitializeSettings();
        SetupEventListeners();

        t_Server = this.transform.GetDeepComponent<TextMeshProUGUI>("T_Server");
        t_Name = this.transform.GetDeepComponent<TextMeshProUGUI>("T_Name");
        t_UserId = this.transform.GetDeepComponent<TextMeshProUGUI>("T_UserId");
        ipt_code = this.transform.GetDeepComponent<TMP_InputField>("Ipt_ExchangeCode");

        this.transform.SetDeepButtonAction("B_ChangeUser", OnChangeServer);
        this.transform.SetDeepButtonAction("B_Quit", OnQuitGame);
        this.transform.SetDeepButtonAction("B_ExchangeCode", OnExchangeCode);
    }

    // 初始化设置
    private void InitializeSettings()
    {
        musicToggle.isOn = AudioManager.Instance.IsBackgroundMusicEnabled;
        soundEffectToggle.isOn = AudioManager.Instance.IsSoundEffectsEnabled;
    }

    // 设置事件监听
    private void SetupEventListeners()
    {
        musicToggle.onValueChanged.AddListener(isOn =>
        {
            AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
            AudioManager.Instance.ToggleBackgroundMusic(isOn);
        });
       
        // 音效开关Toggle事件
        soundEffectToggle.onValueChanged.AddListener(isOn =>
        {
            AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
            AudioManager.Instance.ToggleSoundEffects(isOn);
        });
    }

    private void OnChangeServer()
    {
        
    }

    private void OnQuitGame()
    {
        
    }

    private void OnExchangeCode()
    {
        if (string.IsNullOrEmpty(ipt_code.text))
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "请输入兑换码");
            return;
        }
    }


}
