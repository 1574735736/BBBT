using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardContentView : UIAction
{
    Transform content;
    GameObject item;
    void Start()
    {
        content = this.transform.FindDeepChild("Content");
        item = this.gameObject.GetDeepGameObject("AwardItem");
    }


    void CreateItem()
    {
        
    }
}
