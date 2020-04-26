using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Physics;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class PlayerHandScript : MonoBehaviour
{
    public GameObject selectedObject;
    public GameObject grabbedObject;
    public Handedness handedness;

    // Flags
    private bool isTriggerDown;

    private void Update()
    {
        if (handedness == Handedness.Left && Input.GetKey(KeyCode.JoystickButton4))
        {
            isTriggerDown = true;
        }
        else if (handedness == Handedness.Right && Input.GetKey(KeyCode.JoystickButton5))
        {
            isTriggerDown = true;
        } 
        else
        {
            isTriggerDown = false;
        }

        if (grabbedObject == null)
        {
            return;
        }

        if (grabbedObject.GetComponent<ObjectController>() != null)
        {
            grabbedObject.transform.position = this.transform.position;
            grabbedObject.transform.rotation = this.transform.rotation;
        }

        if (!isTriggerDown)
        {
            if (grabbedObject.GetComponent<ObjectController>() != null)
            {
                grabbedObject.GetComponent<ObjectController>().releaseKinematic();
                if (grabbedObject.GetComponent<ThrowingScript>() != null)
                {
                    grabbedObject.GetComponent<ThrowingScript>().simulateThrow();
                }
            }
            grabbedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ENTERED: " + other.gameObject);

        if (other.tag == "Grabbable")
        {
            if (other.GetComponent<ObjectController>() != null)
            {
                selectedObject = other.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("STAY: " + other.gameObject);

        if (other.gameObject == null) {
            selectedObject = null;
        }

        if (selectedObject == null)
        {
            return;
        }

        if (handedness == Handedness.Left)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                grabbedObject = selectedObject;
                grabbedObject.GetComponent<ObjectController>().ClaimMe();
                grabbedObject.GetComponent<ObjectController>().makeKinematic();
            }
        }
        else if (handedness == Handedness.Right)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                grabbedObject = selectedObject;
                grabbedObject.GetComponent<ObjectController>().ClaimMe();
                grabbedObject.GetComponent<ObjectController>().makeKinematic();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("EXIT: " + other.gameObject);

        if (other.gameObject == selectedObject)
        {
            selectedObject = null;
        }
    }
}
