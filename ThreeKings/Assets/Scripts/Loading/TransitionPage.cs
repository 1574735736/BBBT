using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using DG.Tweening;
using Spine;
using Spine.Unity;

public class TransitionPage : Singleton<TransitionPage>
{
    [SerializeField] Transform _panel;
    [SerializeField] private RectTransform _mask;

    private readonly Vector2 Max = new Vector2(20, 20);
    private readonly Vector2 Min = new Vector2(0, 0);

    private float _scaleTimer = 0.5f;

    void Start()
    {
        _panel.gameObject.SetActive(false);
        _mask.gameObject.SetActive(false);
    }

    public void StartToNewScene(Action ac)
    {
        _panel.gameObject.SetActive(true);
        _mask.gameObject.SetActive(true);

        _mask.localScale = Max;
        _mask.DOScale(Min, _scaleTimer).OnComplete(() =>
        {
            ac?.Invoke();
        });
        //Timer.Instance.AddDelayedAction(0.5F, () => {
        //    ac?.Invoke();
        //});
        //AudioManager.Instance.PlaySoundEffect(AudioConfig.SceneTurn);
        //_skeletonAnimation.gameObject.SetActive(true);
        //SpineAnimManager.Instance.PlayAnim(_skeletonAnimation, 0, "animation", false);
        //Timer.Instance.AddDelayedAction(3F, () => {

        //    _skeletonAnimation.gameObject.SetActive(false);
        //});
    }

    public void EndToNewScene(Action ac)
    {
        Timer.Instance.AddDelayedAction(0.3F, () =>
        {
            _mask.DOScale(Max, _scaleTimer).OnComplete(() =>
            {
                _panel.gameObject.SetActive(false);
                _mask.gameObject.SetActive(false);
                ac?.Invoke();
            });
        });
        //Timer.Instance.AddDelayedAction(0.5F, () => {
        //    ac?.Invoke();
        //});

        //_skeletonAnimation.gameObject.SetActive(true);
        //SpineAnimManager.Instance.PlayAnim(_skeletonAnimation, 0, "animation", false);
        //Timer.Instance.AddDelayedAction(3F, () => {
        //    //ac?.Invoke();
        //    _skeletonAnimation.gameObject.SetActive(false);
        //});
    }

}
