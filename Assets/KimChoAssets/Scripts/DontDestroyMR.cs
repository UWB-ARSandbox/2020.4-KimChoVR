using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * DontDestroyMR
 * -------------
 * Any object with this script attached will not be destroyed
 * upon changing scenes.
 */
public class DontDestroyMR : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
