using ASL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ClaimPlayerAvatarScript
 * -----------------------
 * This script gives ownership of avatar body parts
 * to a connected user.
 */
public class ClaimPlayerAvatarScript : MonoBehaviour
{
    public GameObject playerView;
    public GameObject playspace;
    public GameObject terrainGenerator;


    // Start is called before the first frame update
    void Start()
    {
        playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playspace = GameObject.FindGameObjectWithTag("Player");
        terrainGenerator = GameObject.Find("TerrainGenerator");

        Debug.Log("MY PEER ID: " + GameLiftManager.GetInstance().m_PeerId);

        if (GameLiftManager.GetInstance().m_PeerId == 1)
        {
            playspace.transform.position = new Vector3(-10, 5, -80);

            terrainGenerator.GetComponent<GenerateTerrain>().generateTerrain = true;
        }

        if (GameLiftManager.GetInstance().m_PeerId == 2)
        {
            playspace.transform.position = new Vector3(10, 5, -80);

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
