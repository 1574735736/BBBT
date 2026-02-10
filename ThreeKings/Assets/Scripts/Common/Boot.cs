using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    void Awake()
    {
        Debug.unityLogger.logEnabled = true;
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        FrameworkConfig.FreeBuy = false;
    }

}
