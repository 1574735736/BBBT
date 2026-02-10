using DG.Tweening;
using LieyouFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoJumpAni : MonoBehaviour
{
    private RectTransform redDot;   // UI 红点
    private float jumpHeight = 15f;  // 跳动的高度
    private float jumpDuration = 2f;  // 跳动持续时间
    private Vector3 originalPosition;  // 红点的初始位置
    private Sequence jumpSequence;  // 跳动的动画序列
    private bool isJumping = false;  // 控制动画是否继续 

    public bool StartPlay = true;
    public bool WaitPlay = false;

    void Start()
    {
        redDot = (RectTransform)this.transform;
        originalPosition = redDot.anchoredPosition;
        if (StartPlay)
        {
            if (WaitPlay)
            {
                CoroutineManager.Instance.Delay(1F, () => {                   
                    StartJumping();
                });
            }
            else
            {
                StartJumping();
            }            
        }
    }
    public void StartJumping()
    {
        if (isJumping) return;

        isJumping = true;

        // 创建跳动的动画序列
        jumpSequence = DOTween.Sequence();

        // 跳动的动画：先上升再下降
        jumpSequence.Append(redDot.DOAnchorPosY(originalPosition.y + jumpHeight, jumpDuration / 2).SetEase(Ease.Linear));//SetEase(Ease.OutQuad));
        jumpSequence.Append(redDot.DOAnchorPosY(originalPosition.y, jumpDuration / 2).SetEase(Ease.Linear));//SetEase(Ease.InQuad));

        // 每0.5秒重复一次
        jumpSequence.SetLoops(-1, LoopType.Restart);

        // 启动动画
        jumpSequence.Play();
    }

    public void StopJumping()
    {
        if (!isJumping) return;

        isJumping = false;

        // 停止动画
        jumpSequence.Kill();

        // 返回初始位置
        redDot.anchoredPosition = originalPosition;
    }

    private void OnDestroy()
    {
        StopJumping();

    }
}
