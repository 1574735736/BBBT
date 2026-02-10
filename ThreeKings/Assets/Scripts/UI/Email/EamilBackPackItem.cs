using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BackpackDb;
using TMPro;

public class EamilBackPackItem : MonoBehaviour
{
    GameObject o_Select;
    Image i_AwardBg;
    TextMeshProUGUI t_AwardNum;
    TextMeshProUGUI t_AwardName;

    void Start()
    {
        
    }

    void InitCompletes()
    {
        if (o_Select == null)
        {
            o_Select = this.transform.FindDeepChild("I_Select").gameObject;
            i_AwardBg = this.transform.GetDeepComponent<Image>("I_AwardBg");
            t_AwardNum = this.transform.GetDeepComponent<TextMeshProUGUI>("T_AwardNum");
            t_AwardName = this.transform.GetDeepComponent<TextMeshProUGUI>("T_AwardName");
        }
    }

    public void InitData(BackpackItem backpack)
    {
        InitCompletes();
        t_AwardNum.text = UtilityBase.FormatWealth(backpack.num);
        t_AwardName.text = backpack.goods.name;
    }
}
