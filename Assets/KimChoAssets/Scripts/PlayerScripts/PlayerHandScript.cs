using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Physics;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class PlayerHandScript : MonoBehaviour
{
    public GameObject selectedObject;
    public GameObject grabbedObject;
    public Handedness handedness;

    public GameObject inventoryObject;

    public IMixedRealityController source;

    // Flags
    private bool isTriggerDown;
    private bool isBlockTriggerDown;

    private void Update()
    {
        findInventory();
        findPointers();

        handleControllerTrigger();

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

        if (grabbedObject.GetComponent<ASL.ASLObject>() != null)
        {
            if (!grabbedObject.GetComponent<ASL.ASLObject>().m_Mine)
            {
                selectedObject = null;
                grabbedObject = null;
                return;
            }
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

    private void findPointers()
    {
        if (source != null)
        {
            return;
        }

        foreach (var inputSource in CoreServices.InputSystem.DetectedInputSources)
        {
            foreach (var pointer in inputSource.Pointers)
            {
                if (pointer.Controller != null)
                {
                    if (pointer.Controller.ControllerHandedness == handedness)
                    {
                        source = pointer.Controller;
                    }
                }
            }
        }
    }

    private void findInventory()
    {
        if (inventoryObject == null)
        {
            inventoryObject = GameObject.FindGameObjectWithTag("InventoryObject");
        }
    }

    private void handleControllerTrigger()
    {
        if (source == null)
        {
            return;
        }

        if (source.ControllerHandedness == Handedness.Right)
        {
            if (Input.GetAxis("AXIS_10") > 0.2f && !isBlockTriggerDown)
            {

                isBlockTriggerDown = true;

                if (source.InputSource.Pointers[0].Result.CurrentPointerTarget != null)
                {
                    if (source.InputSource.Pointers[0].Result.CurrentPointerTarget.tag == "Block")
                    {
                        handleBlockPlacement();
                    }
                }
            } else if (Input.GetAxis("AXIS_10") < 0.1f)
            {
                isBlockTriggerDown = false;
            }
        }

        if (source.ControllerHandedness == Handedness.Left)
        {
            if (Input.GetAxis("AXIS_9") > 0.2f)
            {

            }
        }
    }

    void handleBlockPlacement()
    {
        Vector3 ptrPos = source.InputSource.Pointers[0].Result.Details.Point;
        Vector3 blockPos = source.InputSource.Pointers[0].Result.CurrentPointerTarget.transform.position;
        float offset = BlockStaticScript.blockScaleX / 2.0f;

        if (inventoryObject.GetComponent<InventoryScript>().currentlySelectedBlock == null)
        {
            return;
        }

        ItemSlotIdentifier blockInfo = inventoryObject.GetComponent<InventoryScript>().currentlySelectedBlock.GetComponent<ItemSlotIdentifier>();
        string blockName = blockInfo.itemID;

        if (ptrPos.x >= blockPos.x + offset)
        {
            Vector3 newPos = blockPos + new Vector3(BlockStaticScript.blockScaleX, 0, 0);
            spawnBlock(newPos, blockName);
        }
        else if (ptrPos.x <= blockPos.x - offset)
        {
            Vector3 newPos = blockPos - new Vector3(BlockStaticScript.blockScaleX, 0, 0);
            spawnBlock(newPos, blockName);
        }
        else if (ptrPos.y >= blockPos.y + offset)
        {
            Vector3 newPos = blockPos + new Vector3(0, BlockStaticScript.blockScaleX, 0);
            spawnBlock(newPos, blockName);
        }
        else if (ptrPos.y <= blockPos.y - offset)
        {
            Vector3 newPos = blockPos - new Vector3(0, BlockStaticScript.blockScaleX, 0);
            spawnBlock(newPos, blockName);
        }
        else if (ptrPos.z >= blockPos.z + offset)
        {
            Vector3 newPos = blockPos + new Vector3(0, 0, BlockStaticScript.blockScaleX);
            spawnBlock(newPos, blockName);
        }
        else if (ptrPos.z <= blockPos.z - offset)
        {
            Vector3 newPos = blockPos - new Vector3(0, 0, BlockStaticScript.blockScaleX);
            spawnBlock(newPos, blockName);
        } else
        {
            return;
        }

        GameObject temp = inventoryObject.GetComponent<InventoryScript>().blockSlots[blockInfo.itemSlotIndex].gameObject;
        temp.GetComponent<ItemSlotIdentifier>().quantity--;
        temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = temp.GetComponent<ItemSlotIdentifier>().quantity + "";

        if (temp.GetComponent<ItemSlotIdentifier>().quantity <= 0)
        {
            Destroy(temp.gameObject);
        }
    }

    void spawnBlock(Vector3 pos, string blockName)
    {
        GameObject outBlock = null;

        if (GenerateTerrain.blockDictionary.ContainsKey(pos))
        {
            GenerateTerrain.blockDictionary.TryGetValue(pos, out outBlock);
            if (outBlock.gameObject.name == "Air" || outBlock.gameObject.name == "Air(Clone)")
            {
                outBlock.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
                GenerateTerrain.blockDictionary.Remove(pos);
            }
            else
            {
                return;
            }
        }

        ASL.ASLHelper.InstanitateASLObject("BlockPrefabs/" + blockName,
            new Vector3(pos.x,
                        pos.y,
                        pos.z), Quaternion.identity, "Environment", "",
            GenerateTerrain.addObjectToList,
            null,
            null);
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

        if (other.tag == "Block" && isTriggerDown)
        {
            if (other.GetComponent<BlockScript>() != null)
            {
                if (other.GetComponent<BlockScript>().isBreakable)
                {
                    other.GetComponent<BlockScript>().hasBeenHit++;
                    other.GetComponent<BlockScript>().timer = 0;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("STAY: " + other.gameObject);

        if (other.gameObject == null) {
            selectedObject = null;
        }

        if (other.tag == "Grabbable")
        {
            if (other.GetComponent<ObjectController>() != null)
            {
                selectedObject = other.gameObject;
            }
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
