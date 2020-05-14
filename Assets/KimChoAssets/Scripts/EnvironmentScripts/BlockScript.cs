using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{

    public int hasBeenHit = 0;
    private float timer = 0.0f;

    private SimpleDemos.DeleteObject_Example deleteScript;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenHit = 0;
        timer = 0.0f;

        deleteScript = this.gameObject.GetComponent<SimpleDemos.DeleteObject_Example>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenHit >= 3)
        {
            deleteScript.m_Delete = true;
        }

        if (hasBeenHit > 0)
        {
            timer += Time.fixedDeltaTime;
        }

        if (timer >= 4.5f)
        {
            hasBeenHit = 0;
            timer = 0.0f;
        }
    }
}
