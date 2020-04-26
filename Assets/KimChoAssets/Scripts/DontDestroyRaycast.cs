using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyRaycast : MonoBehaviour
{
    private GameObject raycastCamera;
    private bool success;

    void Start()
    {
        success = false;
    }

    void Update()
    {
        if (success)
        {
            return;
        }

        if (raycastCamera == null)
        {
            raycastCamera = GameObject.Find("UIRaycastCamera");
        }
        else
        {
            raycastCamera.AddComponent<DontDestroyMR>();
            success = true;
        }
    }
}