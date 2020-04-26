using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    public enum DestroyOption
    {
        DestroySelf,
        DoNotDestroySelf
    }

    public DestroyOption option;
    public GameObject parent;
    public SimpleDemos.DeleteObject_Example deleteScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent)
        {
            return;
        }

        if (option == DestroyOption.DestroySelf)
        {
            if (other.tag == "Grabbable")
            {
                if (other.GetComponent<SimpleDemos.DeleteObject_Example>() != null)
                {
                    other.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
                }
                deleteScript.m_Delete = true;
            }
        } else if (option == DestroyOption.DoNotDestroySelf)
        {
            if (other.tag == "Grabbable")
            {
                if (other.GetComponent<SimpleDemos.DeleteObject_Example>() != null)
                {
                    other.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
                }
            }
        }
    }
}
