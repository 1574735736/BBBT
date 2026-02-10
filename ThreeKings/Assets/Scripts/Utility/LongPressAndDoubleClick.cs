using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongPressAndDoubleClick : MonoBehaviour,IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private float longPressDuration = 1f;
    [SerializeField] private float doubleClickTime = 0.5f;
    [SerializeField] private int requiredClickCount = 3;

    private float pointerDownTime;
    private int clickCount;
    private float lastClickTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownTime = Time.time;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        float pressDuration = Time.time - pointerDownTime;

        // 检测长按
        if (pressDuration >= longPressDuration)
        {
            Debug.Log("长按事件触发");
            clickCount = 0; // 重置点击计数
            return;
        }

        // 检测多击
        if (Time.time - lastClickTime < doubleClickTime)
        {
            clickCount++;
        }
        else
        {
            clickCount = 1;
        }

        lastClickTime = Time.time;

        if (clickCount >= requiredClickCount)
        {
            Debug.Log(requiredClickCount + "击事件触发");
            clickCount = 0;
        }
    }
}
