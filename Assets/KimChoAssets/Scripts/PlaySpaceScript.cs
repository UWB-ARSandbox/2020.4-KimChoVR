using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpaceScript : MonoBehaviour
{
    public GameObject body;
    public Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(transform.position.x - offSet.x, body.transform.position.y - offSet.y, transform.position.z - offSet.z);

        this.transform.position = newPos;
    }
}
