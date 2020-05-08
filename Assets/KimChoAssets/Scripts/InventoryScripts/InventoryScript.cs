using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public GameObject InventoryCanvas;
    public GameObject InventoryPanel;
    public GameObject[] itemSlots;
    public List<GameObject> Items;
    public SimpleDemos.CreateObject_Example createScript;

    // Start is called before the first frame update
    void Start()
    {
        itemSlots = new GameObject[16];
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public bool addItem(string itemName)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null)
            {
                GameObject newObj = Instantiate(Resources.Load<GameObject>("InventoryResources/ItemSlot"), InventoryPanel.transform);
                newObj.GetComponent<ItemSlotIdentifier>().itemID = itemName;
                newObj.GetComponent<Button>().onClick.AddListener(delegate { ItemSlotPress(); });
                newObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("InventoryResources/" + itemName);
                itemSlots[i] = newObj;
                return true;
            }
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
                createScript.m_CreateObject = (SimpleDemos.CreateObject_Example.ObjectToCreate) i + SimpleDemos.CreateObject_Example.GrabbableItemStartIndex;
                createScript.m_SpawnObject = true;
            }
        }

        Destroy(EventSystem.current.currentSelectedGameObject);
    }
}
