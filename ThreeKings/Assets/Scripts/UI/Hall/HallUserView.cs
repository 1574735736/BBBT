using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallUserView : MonoBehaviour
{
    void Start()
    {
        this.transform.SetDeepButtonAction("B_IocnBg", () =>
        {
            UIManager.Instance.OpenUI(UIConfig.UserInfoView);
        });

        this.transform.SetDeepButtonAction("B_UserTip", () =>
        {
            EventManager.Instance.TriggerEvent(Enums.Lisiner.TipShow, "´ý¿ª·¢");
        });
    }

}
