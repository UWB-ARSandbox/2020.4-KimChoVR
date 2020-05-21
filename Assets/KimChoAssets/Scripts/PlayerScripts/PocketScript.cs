using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketScript : MonoBehaviour
{
    public GameObject InventoryObject;
    public InventoryScript InventoryScript;

    public GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("InventoryObject").Length; i++)
        {
            if (GameObject.FindGameObjectsWithTag("InventoryObject")[i].GetComponent<ASL.ASLObject>() != null &&
                GameObject.FindGameObjectsWithTag("InventoryObject")[i].GetComponent<ASL.ASLObject>().m_Mine)
            {
                InventoryObject = GameObject.FindGameObjectsWithTag("InventoryObject")[i];
                break;
            }
        }

        InventoryScript = InventoryObject.GetComponent<InventoryScript>();

        parentObject = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryObject == null)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Grabbable")
        {
            return;
        }

        for (int i = 0; i < StaticItemList.itemList.Length; i++)
        {
            if (other.gameObject.name == StaticItemList.itemList[i] ||
                other.gameObject.name == StaticItemList.itemList[i] + "(Clone)")
            {
                if (InventoryScript.addItem(StaticItemList.itemIconList[i]))
                {
                    other.gameObject.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
                }
            }
        }

        for (int i = 0; i < StaticItemList.blockList.Length; i++)
        {
            if (other.gameObject.name == StaticItemList.blockList[i] ||
                other.gameObject.name == StaticItemList.blockList[i] + "(Clone)")
            {
                if (InventoryScript.addBlock(StaticItemList.blockList[i]))
                {
                    other.gameObject.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
                }
            }
        }
    }
}
