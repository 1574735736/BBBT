using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trigger : MonoBehaviour
{
    EventTrigger eventTrigger;
    // Start is called before the first frame update
    void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();

        AddPointerEnterEvent();
        AddPointerExitEvent();
        AddPointerClickEvent();
    }

    void AddPointerEnterEvent()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(PointerEnter);
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
        Debug.Log("AddPointerEnterEvent");
    }

    void AddPointerExitEvent()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(PointerExit);
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
        Debug.Log("AddPointerExitEvent");
    }

    void AddPointerClickEvent()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(PointerClick);
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
        Debug.Log("AddPointerClickEvent");
    }

    void PointerEnter(BaseEventData baseEventData)
    {
        Debug.Log("enter");
    }

    void PointerExit(BaseEventData baseEventData)
    {
        Debug.Log("exit");
    }

    void PointerClick(BaseEventData baseEventData)
    {
        Debug.Log("click");
    }
}
