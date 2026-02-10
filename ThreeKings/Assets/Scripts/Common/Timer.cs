using System;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    private class DelayedAction
    {
        public Action Action;
        public float Delay;
        public bool IsPaused;
        public bool IsCancelled;
    }

    private List<DelayedAction> delayedActions = new List<DelayedAction>();

    public void AddDelayedAction(float delay, Action action)
    {
        if (action == null)
        {
            Debug.LogError("添加的定时任务不能为null！");
            return;
        }
        // 如果有正在执行的相同的方法，就重新延时执行
        foreach (var delayedAction in delayedActions)
        {
            if (delayedAction.Action == action)
            {
                delayedAction.Delay = delay;
                return;
            }
        }

        // 否则添加新的延时动作
        delayedActions.Add(new DelayedAction { Action = action, Delay = delay });
    }

    public void PauseAction(Action action)
    {
        if (action == null) return;
        foreach (var delayedAction in delayedActions)
        {
            if (delayedAction.Action == action)
            {
                delayedAction.IsPaused = true;
                return;
            }
        }
    }

    public void ResumeAction(Action action)
    {
        if (action == null) return;

        foreach (var delayedAction in delayedActions)
        {
            if (delayedAction.Action == action)
            {
                delayedAction.IsPaused = false;
                return;
            }
        }
    }

    public void CancelAction(Action action)
    {
        if (action == null) return;
        delayedActions.RemoveAll(da => da.Action == action);
    }

    private void Update()
    {
        for (int i = delayedActions.Count - 1; i >= 0; i--)
        {
            var delayedAction = delayedActions[i];
            if (delayedAction.IsCancelled)
            {
                delayedActions.RemoveAt(i);
            }
            if (delayedAction.IsPaused) continue;

            delayedAction.Delay -= Time.deltaTime;
            if (delayedAction.Delay <= 0)
            {
                // 执行前先检查方法有效性
                if (IsActionValid(delayedAction.Action))
                {
                    try
                    {
                        delayedAction.Action(); // 尝试执行方法
                    }
                    catch (Exception e)
                    {
                        // 捕获执行中的异常，避免崩溃或循环
                        Debug.LogError($"定时任务执行出错: {e.Message}\n堆栈跟踪: {e.StackTrace}");
                    }
                }
                else
                {
                    Debug.LogWarning("定时任务的方法已无效（可能已被销毁），跳过执行");
                }

                // 无论执行成功/失败/无效，都移除任务，避免重复处理
                delayedActions.RemoveAt(i);
            }

        }
    }

    // 检查Action是否有效（防止目标对象已销毁）
    private bool IsActionValid(Action action)
    {
        if (action == null) return false;

        // 检查委托的目标是否是MonoBehaviour且已被销毁（Unity特殊处理）
        if (action.Target is MonoBehaviour monoTarget)
        {
            // Unity中被销毁的MonoBehaviour用==null判断会返回true
            return monoTarget != null;
        }

        // 非MonoBehaviour目标直接判断是否为null
        return action.Target != null;
    }


    // 将秒数转换为小时：分钟：秒格式
    public string FormatTime(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
