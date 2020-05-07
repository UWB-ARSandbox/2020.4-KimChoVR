using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketScript : MonoBehaviour
{
    public GameObject InventoryObject;
    public InventoryScript InventoryScript;

    // Start is called before the first frame update
    void Start()
    {
        InventoryObject = GameObject.FindGameObjectWithTag("InventoryObject");
        InventoryScript = InventoryObject.GetComponent<InventoryScript>();
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
        Debug.Log("TOUCH: " + other.gameObject.name);

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
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ASSSSSSSSSSSSSSSSSSSS");
    }
}
