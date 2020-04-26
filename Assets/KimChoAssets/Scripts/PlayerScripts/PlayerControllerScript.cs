using Oculus.Platform.Samples.VrHoops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public GameObject playerView;
    public GameObject playSpace;
    public Rigidbody rb;

    public Vector3 offSet;

    public SimpleDemos.TransformObjectViaLocalSpace_Example onlineController;
    public PlayerStatsScript playerStats;

    private bool jumped;
    private float timer;

    void Start()
    {
        playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playSpace = GameObject.FindGameObjectWithTag("Player");
        onlineController = this.GetComponent<SimpleDemos.TransformObjectViaLocalSpace_Example>();
        playerStats = this.GetComponent<PlayerStatsScript>();

        rb = this.gameObject.GetComponent<Rigidbody>();
        jumped = false;

        this.transform.position = playerView.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.GetComponent<ASL.ASLObject>().m_Mine)
        {
            return;
        }

        timer += Time.fixedDeltaTime;

        handleMovementInput();
    }

    void Update()
    {
        if (!this.GetComponent<ASL.ASLObject>().m_Mine)
        {
            return;
        }

        handleJumpInput();
    }

    private void handleMovementInput()
    {
        //Vector3 playerViewOffset = this.transform.position - playerView.transform.position;
        Vector3 playSpaceOffset = playerView.transform.position - playSpace.transform.position;
        Vector3 additativeAmount = new Vector3(playerView.transform.forward.x, 0, playerView.transform.forward.z) * -Input.GetAxis("AXIS_18") * playerStats.floatScript.m_MyFloats[2] * Time.fixedDeltaTime;
        float rotateAmount = Input.GetAxis("AXIS_19") * playerStats.floatScript.m_MyFloats[3];

        // Handle Online Positioning
        onlineController.m_AdditiveMovementAmount = additativeAmount;
        onlineController.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.y;
        onlineController.m_Angle = rotateAmount;
        onlineController.m_SendAdditiveTransform = true;
        playSpace.transform.position = this.transform.position - playSpaceOffset;
        playSpace.transform.rotation = this.transform.rotation;

        if (timer >= 5)
        {
            // Handle Position
            this.onlineController.m_MoveToPosition = this.transform.position;

            // Handle Scale
            this.onlineController.m_ScaleToAmount = new Vector3(0.2f, 0.4f, 0.2f);

            // Handle Rotation
            this.onlineController.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.custom;
            this.onlineController.m_MyCustomAxis = this.transform.localEulerAngles;

            this.onlineController.m_SendTransform = true;
            timer = 0;
        }

        //playSpace.transform.position += new Vector3(playerView.transform.forward.x, 0, playerView.transform.forward.z) * -Input.GetAxis("AXIS_18") * playerStats.floatScript.m_MyFloats[2] * Time.fixedDeltaTime;
        //playSpace.transform.Translate(Input.GetAxis("AXIS_17") * playerStats.floatScript.m_MyFloats[2] * Time.fixedDeltaTime, 0, 0);
        //playSpace.transform.Rotate(0, Input.GetAxis("AXIS_19") * playerStats.floatScript.m_MyFloats[3], 0);

        //this.onlineController.m_MoveToPosition = new Vector3(playerView.transform.position.x, this.transform.position.y, playerView.transform.position.z);
        //this.onlineController.m_MyCustomAxis = new Vector3(this.transform.localEulerAngles.x, playerView.transform.localEulerAngles.y, this.transform.localEulerAngles.z);

        //playSpace.transform.position = this.transform.position;
        //playSpace.transform.rotation = this.transform.rotation;

        //Vector3 additativeAmount = new Vector3(playerView.transform.forward.x, 0, playerView.transform.forward.z) * -Input.GetAxis("AXIS_18") * playerStats.floatScript.m_MyFloats[2] * Time.fixedDeltaTime;
        //float rotateAmount = Input.GetAxis("AXIS_19") * playerStats.floatScript.m_MyFloats[3];

        //onlineController.m_AdditiveMovementAmount = additativeAmount;
        //onlineController.m_MyRotationAxis = SimpleDemos.TransformObjectViaLocalSpace_Example.RotationAxis.y;
        //onlineController.m_Angle = rotateAmount;

        //onlineController.m_SendAdditiveTransform = true;
    }

    private void handleJumpInput()
    {
        if (rb.velocity == Vector3.zero)
        {
            playerStats.floatScript.m_MyFloats[0] = 0;
            jumped = false;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton8) && !jumped || Input.GetKeyDown(KeyCode.JoystickButton9) && !jumped)
        {
            playerStats.floatScript.m_MyFloats[0] = 1.0f;
            jumped = true;
        }

        playerStats.floatScript.m_SendFloat = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        handleOnlinePlayerCollisions(collision);
    }

    private void handleOnlinePlayerCollisions(Collision collision)
    {
        if (playerStats != null)
        {
            if (collision.gameObject.tag == "Floor")
            {
                playerStats.floatScript.m_MyFloats[0] = 0.0f;
                jumped = false;
            }
        }

        playerStats.floatScript.m_SendFloat = true;
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
