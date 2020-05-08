using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingScript : MonoBehaviour
{
    public Rigidbody rb;
    private float timer;
    private Vector3 prevPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        timer = 0.0f;
        prevPosition = rb.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= 0.05f)
        {
            prevPosition = rb.position;
        }
    }

    public void simulateThrow()
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        Vector3 offSet = rb.transform.position - prevPosition;
        rb.AddForce(offSet * 3000);
    }
}
