using ASL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION DETECTED ON PLANE");
        if (collision.gameObject.GetComponent<ASL.ASLObject>() != null)
        {
            if (collision.gameObject.GetComponent<ASL.ASLObject>().m_Mine)
            {
                if (GameLiftManager.GetInstance().m_PeerId == 1)
                {
                    Debug.Log("RESPAWN PLAYER AT POSITION 1");
                    playspace.transform.position = new Vector3(-10, 5, -80);
                    collision.gameObject.transform.position = new Vector3(-10, 5, -80);
                }

                if (GameLiftManager.GetInstance().m_PeerId == 2)
                {
                    Debug.Log("RESPAWN PLAYER AT POSITION 2");
                    playspace.transform.position = new Vector3(-10, 5, -80);
                    collision.gameObject.transform.position = new Vector3(-10, 5, -80);
                }
            }
        }
    }
}
