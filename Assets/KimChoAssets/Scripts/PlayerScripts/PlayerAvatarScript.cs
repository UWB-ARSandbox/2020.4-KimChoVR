using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarScript : MonoBehaviour
{
    public enum AvatarType
    {
        Head,
        Body,
        Left,
        Right
    }

    public ASL.ASLObject playerASL;
    public SimpleDemos.TransformObjectViaLocalSpace_Example sendPositionScript;
    public GameObject avatarPartToSyncWith;
    public AvatarType avatar;
    public MeshRenderer mesh;

    // Private Data
    private Vector3 previousPosition;
    private Vector3 previousRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerASL = this.gameObject.GetComponent<ASL.ASLObject>();
        sendPositionScript = this.gameObject.GetComponent<SimpleDemos.TransformObjectViaLocalSpace_Example>();

        if (avatar == AvatarType.Head)
        {
            avatarPartToSyncWith = GameObject.FindGameObjectWithTag("MainCamera");
        }
        else if (avatar == AvatarType.Left)
        {
            avatarPartToSyncWith = GameObject.FindGameObjectWithTag("LeftController");
        }
        else if (avatar == AvatarType.Right)
        {
            avatarPartToSyncWith = GameObject.FindGameObjectWithTag("RightController");
        }
        else if (avatar == AvatarType.Body)
        {
            avatarPartToSyncWith = GameObject.FindGameObjectWithTag("PlayerBody");
        }

        sendPositionScript.m_ObjectToManipulate = this.gameObject;

        this.previousPosition = avatarPartToSyncWith.transform.position;
        this.previousRotation = avatarPartToSyncWith.transform.localEulerAngles;

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        // ClaimMe();
    }

    // Update is called once per frame
    void Update()
    {

        if (!playerASL.m_Mine)
        {
            mesh.gameObject.SetActive(true);
            return;
        }

        if (avatarPartToSyncWith == null)
        {
            if (avatar == AvatarType.Head)
            {
                avatarPartToSyncWith = GameObject.FindGameObjectWithTag("MainCamera");
            }
            else if (avatar == AvatarType.Left)
            {
                avatarPartToSyncWith = GameObject.FindGameObjectWithTag("LeftController");
            }
            else if (avatar == AvatarType.Right)
            {
                avatarPartToSyncWith = GameObject.FindGameObjectWithTag("RightController");
            }
            else if (avatar == AvatarType.Body)
            {
                avatarPartToSyncWith = GameObject.FindGameObjectWithTag("PlayerBody");
            }
            return;
        }

        if (avatar == AvatarType.Body)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            mesh.enabled = false;
        }

        if (avatar == AvatarType.Left || avatar == AvatarType.Right)
        {
            mesh.enabled = false;
        }

        if (avatar == AvatarType.Head)
        {
            mesh.enabled = false;
        }

        if (!this.previousPosition.Equals(avatarPartToSyncWith.transform.position) ||
            !this.previousRotation.Equals(avatarPartToSyncWith.transform.localEulerAngles))
        {
            if (avatar == AvatarType.Head)
            {
                SendHeadUpdates();
            } else if (avatar == AvatarType.Left || avatar == AvatarType.Right)
            {
                SendControllerUpdates();
            } else if (avatar == AvatarType.Body)
            {
                SendBodyUpdates();
            }
        }
    }

    void SendBodyUpdates()
    {

        // Handle Position
        this.sendPositionScript.m_MoveToPosition = avatarPartToSyncWith.transform.position;

        // Handle Scale
        this.sendPositionScript.m_ScaleToAmount = new Vector3(0.2f, 0.4f, 0.2f);

        // Handle Rotation
        this.sendPositionScript.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.custom;
        this.sendPositionScript.m_MyCustomAxis = avatarPartToSyncWith.transform.eulerAngles;

        this.previousPosition = avatarPartToSyncWith.transform.position;
        this.previousRotation = avatarPartToSyncWith.transform.localEulerAngles;

        this.sendPositionScript.m_SendTransform = true;
    }

    void SendHeadUpdates()
    {

        // Handle Position
        this.sendPositionScript.m_MoveToPosition = avatarPartToSyncWith.transform.position;

        // Handle Scale
        this.sendPositionScript.m_ScaleToAmount = new Vector3(0.2f, 0.2f, 0.2f);

        // Handle Rotation
        this.sendPositionScript.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.custom;
        this.sendPositionScript.m_MyCustomAxis = avatarPartToSyncWith.transform.eulerAngles;

        this.previousPosition = avatarPartToSyncWith.transform.position;
        this.previousRotation = avatarPartToSyncWith.transform.localEulerAngles;

        this.sendPositionScript.m_SendTransform = true;
    }

    void SendControllerUpdates()
    {
        // Handle Position
        this.sendPositionScript.m_MoveToPosition = avatarPartToSyncWith.transform.position;

        // Handle Rotation
        this.sendPositionScript.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.custom;
        this.sendPositionScript.m_MyCustomAxis = avatarPartToSyncWith.transform.eulerAngles;

        this.previousPosition = avatarPartToSyncWith.transform.position;
        this.previousRotation = avatarPartToSyncWith.transform.localEulerAngles;

        this.sendPositionScript.m_SendTransform = true;
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
}
