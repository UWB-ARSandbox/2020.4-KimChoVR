using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public SimpleDemos.CreateObject_Example spawnScript;
    public SimpleDemos.CreateObject_Example.ObjectToCreate objectToSpawn;

    // On start, creates players avatar
    void Start()
    {
        spawnScript.m_CreateObject = objectToSpawn;
        spawnScript.m_SpawnObject = true;
    }
}
