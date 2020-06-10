using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ASLObjectSync
 * -------------
 * This script will sync an asl object with an asl controller object.
 */
public class ASLObjectSync : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent.GetChild(this.transform.GetSiblingIndex() - 1).GetComponent<ObjectController>().objectToSyncWith = this.gameObject;
        this.transform.parent.GetChild(this.transform.GetSiblingIndex() - 1).GetComponent<SimpleDemos.TransformObjectViaLocalSpace_Example>().m_ObjectToManipulate = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
