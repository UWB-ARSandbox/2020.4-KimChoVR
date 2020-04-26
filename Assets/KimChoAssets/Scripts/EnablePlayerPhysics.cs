using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayerPhysics : MonoBehaviour
{
    public GameObject playerBody;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerBody == null)
        {
            playerBody = GameObject.FindGameObjectWithTag("PlayerBody");
            return;
        }

        playerBody.GetComponent<Rigidbody>().isKinematic = false;
    }
}
