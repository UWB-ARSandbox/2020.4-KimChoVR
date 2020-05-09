using ASL;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoScript : MonoBehaviour
{
    public GameObject playerToLookAt;
    public GameObject objectToSyncWith;
    public TextMeshProUGUI nameText;

    // Start is called before the first frame update
    void Start()
    {
        playerToLookAt = GameObject.FindGameObjectWithTag("MainCamera");
        nameText = this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToLookAt == null)
        {
            playerToLookAt = GameObject.FindGameObjectWithTag("MainCamera");
            return;
        }

        if (objectToSyncWith == null)
        {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("PlayerHead")) {
                if (obj.GetComponent<ASL.ASLObject>() != null)
                {
                    if (obj.GetComponent<ASL.ASLObject>().m_Mine)
                    {
                        objectToSyncWith = obj;
                    }
                }
            }
            return;
        }

        this.transform.position = objectToSyncWith.transform.position + new Vector3(0, 0.5f, 0);
        this.transform.LookAt(playerToLookAt.transform);

        nameText.text = GameLiftManager.GetInstance().m_Username;
    }
}
