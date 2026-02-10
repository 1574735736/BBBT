
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UIBase
{
    public static Transform FindDeepChild(this Transform parent, string name)
    {
        Transform result = parent.Find(name);
        if (result != null) 
            return result;

        foreach (Transform child in parent)
        {
            result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }

    // 用于深度搜索 GameObject 的所有子对象并获取对应的 GameObject
    public static GameObject GetDeepGameObject(this GameObject obj, string name)
    {
        Transform[] allTransforms = obj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public static T GetDeepComponent<T>(this GameObject obj, string name) where T : Component
    {
        Transform childTransform = obj.transform.FindDeepChild(name);
        if (childTransform != null)
            return childTransform.GetComponent<T>();
        return null;
    }

    public static T GetDeepComponent<T>(this Transform transform, string name) where T : Component
    {
        Transform childTransform = transform.FindDeepChild(name);
        if (childTransform != null)
            return childTransform.GetComponent<T>();
        return null;
    }

    public static void SetDeepColor(this GameObject obj, string name, Color color)
    {
        // 尝试获取 Text 组件并设置颜色
        Text textComponent = obj.GetDeepComponent<Text>(name);
        if (textComponent != null)
        {
            textComponent.color = color;
            return;
        }

        // 尝试获取 Image 组件并设置颜色
        Image imageComponent = obj.GetDeepComponent<Image>(name);
        if (imageComponent != null)
        {
            imageComponent.color = color;
            return;
        }

        // 尝试获取 TextMeshProUGUI 组件并设置颜色
        TextMeshProUGUI tmpComponent = obj.GetDeepComponent<TextMeshProUGUI>(name);
        if (tmpComponent != null)
        {
            tmpComponent.color = color;
            return;
        }
    } 

    public static void SetDeepColor(this Transform transform, string name, Color color)
    {
        // 尝试获取 Text 组件并设置颜色
        Text textComponent = transform.GetDeepComponent<Text>(name);
        if (textComponent != null)
        {
            textComponent.color = color;
            return;
        }

        // 尝试获取 Image 组件并设置颜色
        Image imageComponent = transform.GetDeepComponent<Image>(name);
        if (imageComponent != null)
        {
            imageComponent.color = color;
            return;
        }

        // 尝试获取 TextMeshProUGUI 组件并设置颜色
        TextMeshProUGUI tmpComponent = transform.GetDeepComponent<TextMeshProUGUI>(name);
        if (tmpComponent != null)
        {
            tmpComponent.color = color;
            return;
        }
    }

    public static void SetDeepColor(this Image image,string colorString)
    {
        Color color;
        if (!ColorUtility.TryParseHtmlString(colorString, out color))
        {
            Debug.LogError("Invalid color string: " + colorString);
            return;
        }
        image.color = color;
    }

    public static void ChangeAlpha(this Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

    public static void SetDeepColor(this GameObject obj, string name, string colorString)
    {
        Color color;
        if (!ColorUtility.TryParseHtmlString(colorString, out color))
        {
            Debug.LogError("Invalid color string: " + colorString);
            return;
        }

        obj.SetDeepColor(name, color);
    }

    public static void SetDeepColor(this Transform transform, string name, string colorString)
    {
        Color color;
        if (!ColorUtility.TryParseHtmlString(colorString, out color))
        {
            Debug.LogError("Invalid color string: " + colorString);
            return;
        }

        transform.SetDeepColor(name, color);
    }

    public static void SetDeepText(this GameObject obj, string name, string text)
    {
        // 尝试获取 Text 组件并设置文本
        Text textComponent = obj.GetDeepComponent<Text>(name);
        if (textComponent != null)
        {
            textComponent.text = text;
            return;
        }

        // 尝试获取 TextMeshProUGUI 组件并设置文本
        TextMeshProUGUI tmpComponent = obj.GetDeepComponent<TextMeshProUGUI>(name);
        if (tmpComponent != null)
        {
            tmpComponent.text = text;
            return;
        }
    }

    public static void SetDeepText(this Transform transform, string name, string text)
    {
        // 尝试获取 Text 组件并设置文本
        Text textComponent = transform.GetDeepComponent<Text>(name);
        if (textComponent != null)
        {
            textComponent.text = text;
            return;
        }

        // 尝试获取 TextMeshProUGUI 组件并设置文本
        TMP_Text tmpComponent = transform.GetDeepComponent<TMP_Text>(name);
        if (tmpComponent != null)
        {
            tmpComponent.text = text;
            return;
        }
    }

    public static void SetThrottledClick(this Transform obj, string name, Action newAction, float interval = 0.5f)
    {
        Button buttonComponent = obj.GetDeepComponent<Button>(name);
        if (buttonComponent == null)
        {
            // 如果没有找到 Button 组件，那么找到对应的 GameObject 并添加一个 Button 组件
            GameObject targetObj = obj.FindDeepChild(name).gameObject;
            if (targetObj != null)
            {
                buttonComponent = targetObj.AddComponent<Button>();
            }
        }

        if (buttonComponent != null)
        {
            // 移除所有之前的监听器
            buttonComponent.onClick.RemoveAllListeners();

            // 添加新的监听器
            buttonComponent.SetThrottledClick(newAction, interval);
        }
    }

    public static void SetThrottledClick(this GameObject obj, string name, Action newAction, float interval = 0.5f)
    {
        Button buttonComponent = obj.GetDeepComponent<Button>(name);
        if (buttonComponent == null)
        {
            // 如果没有找到 Button 组件，那么找到对应的 GameObject 并添加一个 Button 组件
            GameObject targetObj = obj.GetDeepGameObject(name);
            if (targetObj != null)
            {
                buttonComponent = targetObj.AddComponent<Button>();
            }
        }

        if (buttonComponent != null)
        {
            // 移除所有之前的监听器
            buttonComponent.onClick.RemoveAllListeners();

            // 添加新的监听器
            buttonComponent.SetThrottledClick(newAction, interval);
        }
    }

    // 带间隔的点击事件
    public static IDisposable SetThrottledClick(this Button button, Action onClick, float interval = 0.5f)
    {
        return button.OnClickAsObservable()
            .Throttle(TimeSpan.FromSeconds(interval))
            .Subscribe(_ => onClick?.Invoke())
            .AddTo(button);
    }

    // 长按检测
    public static IDisposable SetLongPress(this Button button, Action onLongPress, float duration = 2f)
    {
        return button.OnPointerDownAsObservable()
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(duration)))
            .TakeUntil(button.OnPointerUpAsObservable())
            .RepeatUntilDestroy(button)
            .Subscribe(_ => onLongPress?.Invoke())
            .AddTo(button);
    }

    // 连续点击检测
    public static IDisposable SetMultiClick(this Button button, Action<int> onMultiClick, int count = 2, float interval = 0.5f)
    {
        return button.OnClickAsObservable()
            .Buffer(TimeSpan.FromSeconds(interval))
            .Where(x => x.Count >= count)
            .Subscribe(clicks => onMultiClick?.Invoke(clicks.Count))
            .AddTo(button);
    }


    public static void SetDeepButtonAction(this GameObject obj, string name, UnityAction newAction)
    {
        // 尝试获取 Button 组件
        Button buttonComponent = obj.GetDeepComponent<Button>(name);
        if (buttonComponent == null)
        {
            // 如果没有找到 Button 组件，那么找到对应的 GameObject 并添加一个 Button 组件
            GameObject targetObj = obj.GetDeepGameObject(name);
            if (targetObj != null)
            {
                buttonComponent = targetObj.AddComponent<Button>();
            }
        }

        if (buttonComponent != null)
        {
            // 移除所有之前的监听器
            buttonComponent.onClick.RemoveAllListeners();

            // 添加新的监听器
            buttonComponent.onClick.AddListener(newAction);
        }
    }

    public static void SetDeepButtonAction(this Transform obj, string name, UnityAction newAction)
    {
        // 尝试获取 Button 组件
        Button buttonComponent = obj.GetDeepComponent<Button>(name);
        if (buttonComponent == null)
        {
            // 如果没有找到 Button 组件，那么找到对应的 GameObject 并添加一个 Button 组件
            GameObject targetObj = obj.gameObject.GetDeepGameObject(name);
            if (targetObj != null)
            {
                buttonComponent = targetObj.AddComponent<Button>();
            }
        }

        if (buttonComponent != null)
        {
            // 移除所有之前的监听器
            buttonComponent.onClick.RemoveAllListeners();

            // 添加新的监听器
            buttonComponent.onClick.AddListener(
                () => {
                    AudioManager.Instance.PlaySoundEffect(AudioConfig.BtnClick);
                    newAction?.Invoke(); }
                );
        }
    }

    public static List<GameObject> GetGameObjectsWithPrefix(this GameObject obj, string prefix)
    {
        List<GameObject> results = new List<GameObject>();

        Transform[] allTransforms = obj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            if (t.name.StartsWith(prefix))
            {
                results.Add(t.gameObject);
            }
        }

        // 对结果进行排序
        results.Sort((a, b) => {
            int suffixA = int.Parse(a.name.Substring(prefix.Length));
            int suffixB = int.Parse(b.name.Substring(prefix.Length));
            return suffixA.CompareTo(suffixB);
        });

        return results;
    }


    public static List<T> GetComponentsWithPrefix<T>(this GameObject obj, string prefix)
    where T : Component
    {
        List<T> results = new List<T>();

        Transform[] allTransforms = obj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            T component = t.GetComponent<T>();
            if (t.name.StartsWith(prefix) && component != null)
            {
                results.Add(component);
            }
        }

        // 对结果进行排序
        results.Sort((a, b) => {
            int suffixA = int.Parse(a.gameObject.name.Substring(prefix.Length));
            int suffixB = int.Parse(b.gameObject.name.Substring(prefix.Length));
            return suffixA.CompareTo(suffixB);
        });

        return results;
    }

    public static List<GameObject> GetObjectsWithPrefix(this GameObject obj, string prefix)
    {
        List<GameObject> results = new List<GameObject>();

        Transform[] allTransforms = obj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            if (t.name.StartsWith(prefix))
            {
                results.Add(t.gameObject);
            }
        }

        // 对结果进行排序
        results.Sort((a, b) => {
            int suffixA = int.Parse(a.gameObject.name.Substring(prefix.Length));
            int suffixB = int.Parse(b.gameObject.name.Substring(prefix.Length));
            return suffixA.CompareTo(suffixB);
        });

        return results;
    }

    public static List<Transform> GetTransWithPrefix(this Transform obj, string prefix)
    {
        List<Transform> results = new List<Transform>();

        Transform[] allTransforms = obj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            if (t.name.StartsWith(prefix))
            {
                results.Add(t);
            }
        }

        // 对结果进行排序
        results.Sort((a, b) => {
            int suffixA = int.Parse(a.gameObject.name.Substring(prefix.Length));
            int suffixB = int.Parse(b.gameObject.name.Substring(prefix.Length));
            return suffixA.CompareTo(suffixB);
        });

        return results;
    }


    public static void DoScaleOpenBounce(this Transform target, Vector3 endValue, float duration = 0.3f,Action action = null) 
    {
        target.transform.localScale = Vector3.zero;
        target.DOScale(endValue, duration).SetEase(Ease.OutBack).OnComplete(()=>action?.Invoke());
    }

    public static void DoScaleHideBounce(this Transform target, Vector3 endValue, float duration = 0.3f,Action action = null)
    {
        target.transform.localScale = Vector3.one;
        target.DOScale(endValue, duration).SetEase(Ease.InBack).OnComplete(()=>action?.Invoke());
    }

    /// <summary>
    /// 根据tag获取子物体
    /// </summary>
    public static GameObject FindChildWithCityTag(this GameObject obj, string tag)
    {
        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null; // 如果没有找到，则返回null
    }

    public static void PageDoFade(bool isShow, GameObject go)
    {
        Image image = go.GetComponent<Image>();
        if (image == null) return;
        go.SetActive(true);
        image.DOKill();
        if (isShow)
        {
            image.color = new Color(1, 1, 1, 0);
            image.DOFade(1, 0.5f);
        }
        else
        {
            image.DOFade(0, 0.5f).OnComplete(() => {
                image.gameObject.SetActive(false);
            });
        }
    }
}

