using DG.Tweening;
using LieyouFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoShackAni : MonoBehaviour
{
    //[Header("Animation Settings")]
    private float rotationAngle = 30.0f; // 摆动的角度
    private float shakeDuration = 0.8f; // 每次摆动的持续时间
    private float pauseDuration = 1.0f; // 两次摆动之间的暂停时间

    private bool isShaking = false;
    private Vector3 originalRotation;

    public bool StartPlay = true;
    public bool WaitPlay = false;

    void Start()
    {
        originalRotation = transform.localEulerAngles;

        if (StartPlay)
        {
            if (WaitPlay)
            {
                CoroutineManager.Instance.Delay(1F, () => {
                    StartShaking();
                });
            }
            else
            {
                StartShaking();
            }
        }
    }

    public void StartShaking()
    {
        if (!isShaking)
        {
            isShaking = true;
            transform.DOKill(); // 停止所有动画
            transform.localEulerAngles = originalRotation; // 恢复到原始角度
            StartCoroutine(ShakeCoroutine());
        }
    }

    public void StopShaking()
    {
        if (isShaking)
        {
            isShaking = false;
            transform.DOKill(); // 停止所有动画
            transform.localEulerAngles = originalRotation; // 恢复到原始角度
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        while (isShaking)
        {
            for (int i = 0; i < 2; i++)
            {
                // 向左摆动到 rotationAngle
                transform.DOLocalRotate(new Vector3(0, 0, rotationAngle), shakeDuration / 4).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // 向右摆动到 -rotationAngle
                        transform.DOLocalRotate(new Vector3(0, 0, -rotationAngle), shakeDuration / 2).SetEase(Ease.Linear)
                            .OnComplete(() =>
                            {
                                // 恢复到原始角度
                                transform.DOLocalRotate(originalRotation, shakeDuration / 4).SetEase(Ease.Linear);
                                     // 使用InOutSine缓动效果，使摆动更自然
                            });
                    });
                yield return new WaitForSeconds(shakeDuration);
            }

            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        transform.DOKill();
    }
}
