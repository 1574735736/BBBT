using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserDb;

public class CreateRoleView : MonoBehaviour
{
    List<RoleSheetItem> roleSheetItems;
    [SerializeField]
    private CharacterRoleView characterRoleView;
    void Start()
    {
        this.transform.SetDeepButtonAction("B_GoinGame", OnGoInGame);
        
    }

    public void OnShow()
    {
        this.transform.DoScaleOpenBounce(Vector3.one);
        InitRoleList();
    }

    public void OnHide()
    {
        this.transform.DoScaleHideBounce(Vector3.zero, 0.3f, () => { 
            this.gameObject.SetActive(false);
            characterRoleView.gameObject.SetActive(true);
            characterRoleView.OnShow();
        });
    }

    private void OnGoInGame()
    {
        UserDb.DrawRole(DataManager.Instance.ServerId, roleSheetItems[0].id, "ß´ß´ÍáÍá", "error", (response) => {
            OnHide();
        }, (error) => {
            Debug.LogError(error);
        });
    }

    private void InitRoleList()
    {
        if (roleSheetItems != null)
        {
            roleSheetItems.Clear();
        }
        
        UserDb.GetRoleSheet((response) => {
            roleSheetItems = response.data.data;

        }, (error) => {
            Debug.LogError(error);
        });
    }


}
