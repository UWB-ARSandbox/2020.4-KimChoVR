using ASL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimPlayerAvatarScript : MonoBehaviour
{
    public GameObject playerView;
    public GameObject terrainGenerator;


    // Start is called before the first frame update
    void Start()
    {
        playerView = GameObject.FindGameObjectWithTag("MainCamera");
        terrainGenerator = GameObject.Find("TerrainGenerator");

        if (GameLiftManager.GetInstance().m_PeerId % 2 != 0)
        {
            playerView.transform.position = new Vector3(-10, 5, -80);

            terrainGenerator.GetComponent<GenerateTerrain>().generateTerrain = true;
        }

        if (GameLiftManager.GetInstance().m_PeerId % 2 == 0)
        {
            playerView.transform.position = new Vector3(10, 5, -80);

            terrainGenerator.GetComponent<GenerateTerrain>().generateTerrain = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject gameObject in SimpleDemos.CreateObject_Example.m_HandleToFreshObjects)
        {
            if (!gameObject.GetComponent<ASL.ASLObject>().m_Mine)
            {
                gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    Debug.Log("Claimed Avatar!!!");
                });
            }
        }
    }
}
