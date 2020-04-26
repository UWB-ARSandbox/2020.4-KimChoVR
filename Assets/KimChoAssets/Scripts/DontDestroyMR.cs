using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMR : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
