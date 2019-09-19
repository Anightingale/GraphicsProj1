using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based off answer from Brogan89 https://answers.unity.com/questions/1366716/how-to-liimit-fps.html
public class TargetFrameRate : MonoBehaviour
{

    public int targetFPS = 30;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }

    void Update()
    {
        if (Application.targetFrameRate != targetFPS)
        {
            Application.targetFrameRate = targetFPS;
        }  
    }

}
