using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHandScript : MonoBehaviour
{
    public GameObject handToFollow;

    // Start is called before the first frame update
    void Start()
    {
        handToFollow = GameObject.FindGameObjectWithTag("LeftController");
    }

    // Update is called once per frame
    void Update()
    {
        if (handToFollow == null)
        {
            handToFollow = GameObject.FindGameObjectWithTag("LeftController");
            return;
        }

        this.transform.position = handToFollow.transform.position;
        this.transform.rotation = handToFollow.transform.rotation;
    }
}
