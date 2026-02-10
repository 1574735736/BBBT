using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GM : MonoBehaviour
{
    public void ChangeVaume()
    {
       
    }

    public void NextGame()
    {
 
    }

    public void CustomsMap()
    {
       
    }

    public void CustomsCom()
    {

    }

    public void AddGold(int Count)
    {
        Debug.Log("Ôö¼Ó½ð±Ò £º" + Count);
        DataManager.Instance.Gold += Count;
    }
}
