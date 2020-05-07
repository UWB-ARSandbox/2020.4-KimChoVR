using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // Scripts
    public SimpleDemos.SendFloatArray_Example floatObject;
    public SimpleDemos.TransformObjectViaLocalSpace_Example ASLTransformScript;

    public GameObject objectToSyncWith;

    // Flags
    public bool leftGrab;
    public bool rightGrab;
    public bool attachToHand;

    // Private Data
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    
    void Start()
    {
        this.previousPosition = this.transform.position;
        this.previousRotation = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        handleObjectKinematic();

        if (!this.gameObject.GetComponent<ASL.ASLObject>().m_Mine)
        {
            SyncLocally();
            return;
        }
        
        sendTransformUpdates();
    }

    public void sendTransformUpdates()
    {
        if (!this.previousPosition.Equals(this.transform.position) ||
            !this.previousRotation.Equals(this.transform.localEulerAngles))
        {
            this.objectToSyncWith.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                Debug.Log("Successfully claimed object! " + this.gameObject.name);
            });

            //Debug.Log(this.objectToSyncWith.GetComponent<ASL.ASLObject>().m_Id);
            //Debug.Log(this.gameObject.GetComponent<ASL.ASLObject>().m_Id);

            // Handle Position
            this.ASLTransformScript.m_MoveToPosition = this.transform.position;

            // Handle Scale
            this.ASLTransformScript.m_ScaleToAmount = this.transform.localScale;

            // Handle Rotation
            this.ASLTransformScript.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.custom;
            this.ASLTransformScript.m_MyCustomAxis = this.transform.eulerAngles;

            this.previousPosition = this.transform.position;
            this.previousRotation = this.transform.localEulerAngles;

            this.ASLTransformScript.m_SendTransform = true;
        }
    }

    public void makeKinematic()
    {
        if (this.floatObject == null)
        {
            return;
        }

        this.floatObject.m_MyFloats[0] = 1.0f;
        this.floatObject.m_SendFloat = true;
    }

    public void releaseKinematic()
    {
        if (this.floatObject == null)
        {
            return;
        }

        this.floatObject.m_MyFloats[0] = 0.0f;
        this.floatObject.m_SendFloat = true;
    }

    public void handleObjectKinematic()
    {
        if (floatObject != null)
        {
            if (this.gameObject.GetComponent<Collider>() != null)
            {
                if (this.gameObject.GetComponent<Rigidbody>() != null)
                {
                    if (floatObject.m_MyFloats[0] > 0.9 && floatObject.m_MyFloats[0] < 1.1)
                    {
                        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    } else
                    {
                        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
            }
        }
    }

    public void ClaimMe()
    {
        if (this.gameObject.GetComponent<ASL.ASLObject>() != null)
        {
            if (!this.gameObject.gameObject.GetComponent<ASL.ASLObject>().m_Mine)
            {
                this.gameObject.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    Debug.Log("Successfully claimed object! " + this.gameObject.name);
                });
            }
            else
            {
                Debug.Log("Already own this object");
            }
        }
    }

    public void SyncLocally()
    {
        if (objectToSyncWith == null)
        {
            return;
        }
        this.transform.position = objectToSyncWith.transform.position;
        this.transform.rotation = objectToSyncWith.transform.rotation;
    }

    private void OnDestroy()
    {
        objectToSyncWith.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
    }
}
