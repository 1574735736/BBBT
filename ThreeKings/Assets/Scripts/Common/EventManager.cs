using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, Action<object>> eventDictionary;

    protected override void Init()
    {
        eventDictionary = new Dictionary<string, Action<object>>();
    }

    public void StartListening(string eventName, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += listener;
        }
        else
        {
            eventDictionary[eventName] = listener;
        }
    }

    public void StopListening(string eventName, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName, object eventParam)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(eventParam);
        }
    }

    public void StartListening(Enums.Lisiner eventName, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventName.ToString()))
        {
            eventDictionary[eventName.ToString()] += listener;
        }
        else
        {
            eventDictionary[eventName.ToString()] = listener;
        }
    }

    public void StopListening(Enums.Lisiner eventName, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventName.ToString()))
        {
            eventDictionary[eventName.ToString()] -= listener;
        }
    }

    public void TriggerEvent(Enums.Lisiner eventName, object eventParam)
    {
        if (eventDictionary.ContainsKey(eventName.ToString()))
        {
            eventDictionary[eventName.ToString()]?.Invoke(eventParam);
        }
    }
}
