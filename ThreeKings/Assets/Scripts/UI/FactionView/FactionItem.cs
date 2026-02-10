using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetDeepButtonAction("B_GoIn", () => {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "暂时无法加入帮派");
        });
    }

   
}
