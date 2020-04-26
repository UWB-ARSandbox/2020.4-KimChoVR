using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerConnectButton : MonoBehaviour
{
    public Button connectButton;

    // Start is called before the first frame update
    void Start()
    {
        connectButton = GameObject.Find("ConnectButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            connectButton.onClick.Invoke();
        }
    }
}
