using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EmailDb;

public class EmailAwardItem  : MonoBehaviour
{
    EmailInfoArticleItem emailInfoArticle;
    Image i_AwardBg;
    TextMeshProUGUI t_AwardNum;
    TextMeshProUGUI t_AwardName;
    void Awake()
    {
        
    }

    void InitComplete()
    {
        if (i_AwardBg == null)
        {
            i_AwardBg = this.transform.GetDeepComponent<Image>("I_AwardBg");
            t_AwardNum = this.transform.GetDeepComponent<TextMeshProUGUI>("I_AwardNum");
            t_AwardName = this.transform.GetDeepComponent<TextMeshProUGUI>("I_AwardName");
        }
    }

    public void InitData(EmailInfoArticleItem emailInfo)
    {
        emailInfoArticle = emailInfo;
        InitComplete();
        t_AwardNum.text = emailInfo.num.ToString();
        t_AwardName.text = emailInfo.goods_info.name;
    }
   
}
