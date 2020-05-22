using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public GameObject InventoryCanvas;

    public TextMeshProUGUI InventoryText;

    public GameObject ItemPanel;
    public GameObject BlockPanel;
    public GameObject[] itemSlots;
    public GameObject[] blockSlots;

    public GameObject currentlySelectedBlock;

    public List<GameObject> Items;
    public SimpleDemos.CreateObject_Example createScript;

    // Start is called before the first frame update
    void Start()
    {
        itemSlots = new GameObject[16];
        blockSlots = new GameObject[16];
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameObject.GetComponent<ASL.ASLObject>().m_Mine)
        {
            this.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            if (InventoryCanvas.activeSelf)
            {
                InventoryCanvas.SetActive(false);
            } else
            {
                InventoryCanvas.SetActive(true);
            }
        }

        placeBlock();
    }

    public void placeBlock() 
    {
    }

    public bool addItem(string itemName)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null)
            {
                GameObject newObj = Instantiate(Resources.Load<GameObject>("InventoryResources/ItemSlot"), ItemPanel.transform);
                newObj.GetComponent<ItemSlotIdentifier>().itemID = itemName;
                newObj.GetComponent<Button>().onClick.AddListener(delegate { ItemSlotPress(); });
                newObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("InventoryResources/" + itemName);
                itemSlots[i] = newObj;
                return true;
            }
        }
        return false;
    }

    public bool addBlock(string blockName)
    {
        bool alreadyContainsBlock = false;
        int blockIndex = -1;
        int nextOpenIndex = -1;

        for (int i = 0; i < blockSlots.Length; i++)
        {
            if (blockSlots[i] == null)
            {
                continue;
            }

            if (blockSlots[i].GetComponent<ItemSlotIdentifier>().itemID == blockName || blockSlots[i].GetComponent<ItemSlotIdentifier>().itemID == blockName + "(Clone)")
            {
                alreadyContainsBlock = true;
                blockIndex = i;
            }
        }

        for (int i = 0; i < blockSlots.Length; i++)
        {
            if (blockSlots[i] == null)
            {
                nextOpenIndex = i;
                break;
            }
        }

        if (alreadyContainsBlock)
        {
            GameObject temp = blockSlots[blockIndex].gameObject;
            temp.GetComponent<ItemSlotIdentifier>().quantity++;
            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = temp.GetComponent<ItemSlotIdentifier>().quantity + "";
            return true;
        } else if (nextOpenIndex >= 0)
        {
            GameObject newObj = Instantiate(Resources.Load<GameObject>("InventoryBlockResources/BlockSlot"), BlockPanel.transform);
            newObj.GetComponent<ItemSlotIdentifier>().itemID = blockName;
            newObj.GetComponent<ItemSlotIdentifier>().itemSlotIndex = nextOpenIndex;
            newObj.GetComponent<ItemSlotIdentifier>().quantity = 1;
            newObj.GetComponent<Button>().onClick.AddListener(delegate { BlockSlotPress(); });
            newObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("InventoryBlockResources/" + blockName);
            blockSlots[nextOpenIndex] = newObj;
            return true;
        }

        return false;
    }

    public void ItemSlotPress()
    {
        string itemID = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlotIdentifier>().itemID;

        for (int i = 0; i < StaticItemList.itemIconList.Length; i++)
        {
            if (itemID == StaticItemList.itemIconList[i])
            {
                createScript.m_CreateObject = (SimpleDemos.CreateObject_Example.ObjectToCreate) i;
                createScript.m_SpawnObject = true;
            }
        }

        Destroy(EventSystem.current.currentSelectedGameObject);
    }

    public void BlockSlotPress()
    {
        int slotIndex = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlotIdentifier>().itemSlotIndex;
        currentlySelectedBlock = blockSlots[slotIndex];
    }

    public void OnBlockTabPress()
    {
        BlockPanel.SetActive(true);
        ItemPanel.SetActive(false);
        InventoryText.text = "Blocks";
    }

    public void OnItemTabPress()
    {
        ItemPanel.SetActive(true);
        BlockPanel.SetActive(false);
        InventoryText.text = "Items";
    }
}
