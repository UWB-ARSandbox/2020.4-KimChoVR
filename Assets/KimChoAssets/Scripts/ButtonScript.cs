using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private SimpleDemos.CreateObject_Example createObjectScript;

    private void Start()
    {
        createObjectScript = this.gameObject.GetComponent<SimpleDemos.CreateObject_Example>();
    }

    public void SpawnObject()
    {
        createObjectScript.m_SpawnObject = true;
    }

    public void SpawnRandomObject()
    {
        int randomNumber = Random.Range(6, SimpleDemos.CreateObject_Example.UniqueObjectsCount);
        createObjectScript.m_CreateObject = (SimpleDemos.CreateObject_Example.ObjectToCreate) randomNumber;
        createObjectScript.m_SpawnObject = true;
    }
}
