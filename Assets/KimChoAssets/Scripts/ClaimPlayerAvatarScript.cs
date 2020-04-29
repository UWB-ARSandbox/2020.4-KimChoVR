using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimPlayerAvatarScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject gameObject in SimpleDemos.CreateObject_Example.m_HandleToFreshObjects)
        {
            if (!gameObject.GetComponent<ASL.ASLObject>().m_Mine)
            {
                gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    Debug.Log("Claimed Avatar!!!");
                });
            }
        }
    }
}
