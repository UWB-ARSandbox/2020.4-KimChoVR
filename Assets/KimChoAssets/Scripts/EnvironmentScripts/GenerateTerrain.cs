using ASL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public static List<GameObject> m_generatedBlocks = new List<GameObject>();
    public static Dictionary<Vector3, GameObject> blockDictionary = new Dictionary<Vector3, GameObject>();

    public GameObject environmentParent;

    public int amp = 5;
    public int freq = 5;

    public int rows = 10;
    public int height = 10;
    public int cols = 10;

    public int limitFactor = 30;

    public bool generateTerrain = false;

    public GameObject playerCamera;

    public static int viewDistance = 20;

    private Vector3 previousPosition;

    private float spawnTime;
    private float timer;

    private int prevChildCount;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = this.transform.position;

        spawnTime = 1.0f;
        timer = 0.0f;

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (GameLiftManager.GetInstance().m_PeerId == 1)
        {
            //generateNewTerrainASL();
        }

        environmentParent = GameObject.FindGameObjectWithTag("Environment");
        prevChildCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(BlockStaticScript.blockScaleX * Mathf.FloorToInt(playerCamera.transform.position.x / BlockStaticScript.blockScaleX), 
                                              this.transform.position.y,
                                              BlockStaticScript.blockScaleZ * Mathf.FloorToInt(playerCamera.transform.position.z / BlockStaticScript.blockScaleZ));

        timer += Time.fixedDeltaTime;

        if (this.transform.position != previousPosition && timer > spawnTime)
        {
            generateTerrain = true;
            renderTerrain();
            previousPosition = this.transform.position;
        }

        if (generateTerrain)
        {
            generateNewTerrainASL();
            generateTerrain = false;
            timer = 0.0f;
        }
    }

    void renderTerrain()
    {
        for (; prevChildCount < environmentParent.transform.childCount; prevChildCount++)
        {
            if (blockDictionary.ContainsKey(environmentParent.transform.GetChild(prevChildCount).transform.position))
            {
                environmentParent.transform.GetChild(prevChildCount).GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
                continue;
            }

            blockDictionary.Add(environmentParent.transform.GetChild(prevChildCount).transform.position, environmentParent.transform.GetChild(prevChildCount).gameObject);
            environmentParent.transform.GetChild(prevChildCount).gameObject.SetActive(false);
        }

        float xViewDistance = BlockStaticScript.blockScaleX * viewDistance;
        float yViewDistance = BlockStaticScript.blockScaleY * viewDistance;
        float zViewDistance = BlockStaticScript.blockScaleZ * viewDistance;

        GameObject outBlock;

        for (float x = previousPosition.x - xViewDistance; x < previousPosition.x + xViewDistance; x += BlockStaticScript.blockScaleX)
        {
            for (float y = previousPosition.y - yViewDistance; y < previousPosition.y + yViewDistance; y += BlockStaticScript.blockScaleY)
            {
                for (float z = previousPosition.z - zViewDistance; z < previousPosition.z + zViewDistance; z += BlockStaticScript.blockScaleZ)
                {
                    blockDictionary.TryGetValue(new Vector3(x, y, z), out outBlock);

                    if (outBlock != null)
                    {
                        outBlock.SetActive(false);
                    }
                }
            }
        }


        for (float x = this.transform.position.x - xViewDistance; x < this.transform.position.x + xViewDistance; x += BlockStaticScript.blockScaleX)
        {
            for (float y = this.transform.position.y - yViewDistance; y < this.transform.position.y + yViewDistance; y += BlockStaticScript.blockScaleY)
            {
                for (float z = this.transform.position.z - zViewDistance; z < this.transform.position.z + zViewDistance; z += BlockStaticScript.blockScaleZ)
                {
                    blockDictionary.TryGetValue(new Vector3(x, y, z), out outBlock);

                    if (outBlock != null)
                    {
                        outBlock.SetActive(true);
                    }
                }
            }
        }
    }

    // Limit values to prevent crashes
    void limitValues()
    {
        if (rows > limitFactor)
        {
            rows = limitFactor;
        }

        if (cols > limitFactor)
        {
            cols = limitFactor;
        }

        if (amp > limitFactor)
        {
            amp = limitFactor;
        }
    }

    void deleteTerrainASL()
    {
        foreach (GameObject block in m_generatedBlocks)
        {
            block.GetComponent<SimpleDemos.DeleteObject_Example>().m_Delete = true;
        }

        m_generatedBlocks.Clear();
    }

    bool checkForCollisions(Vector3 spawnPos, float radius)
    {
        Collider[] blockColliders = Physics.OverlapSphere(spawnPos, radius);

        bool overlap = false;

        for (int i = 0; i < blockColliders.Length; i++)
        {
            if (blockColliders[i].gameObject.tag == "Block")
            {
                overlap = true;
            }
        }

        if (blockDictionary.ContainsKey(spawnPos))
        {
            overlap = true;
        }

        return overlap;
    }

    void spawnBlock(int x, int y, int z, SimpleDemos.CreateObject_Example.BlockToCreate blockType)
    {
        string blockName = SimpleDemos.CreateObject_Example.blockList[(int) blockType];

        ASL.ASLHelper.InstanitateASLObject("BlockPrefabs/" + blockName,
            new Vector3(x * BlockStaticScript.blockScaleX + this.transform.position.x,
                        y * BlockStaticScript.blockScaleY + this.transform.position.y,
                        z * BlockStaticScript.blockScaleZ + this.transform.position.z), Quaternion.identity, "Environment", "",
            addObjectToList,
            null,
            null);
    }

    void generateNewTerrainASL()
    {
        limitValues();

        for (int x = -rows; x < rows; x++)
        {
            for (int z = -cols; z < cols; z++)
            {
                int landStartPoint = Mathf.FloorToInt(Mathf.PerlinNoise((x + this.transform.position.x + Random.Range(-1f, 1f)) / (freq * 1.0f),
                    (z + this.transform.position.z + Random.Range(-1f, 1f)) / (freq * 1.0f))
                    * (amp * 1.0f));

                for (int y = height; y >= -height; y--)
                {
                    bool overlap = checkForCollisions(new Vector3(x * BlockStaticScript.blockScaleX + this.transform.position.x,
                                                                  y * BlockStaticScript.blockScaleY + this.transform.position.y,
                                                                  z * BlockStaticScript.blockScaleZ + this.transform.position.z), 0.15f);

                    if (!overlap)
                    {
                        if (y == landStartPoint)
                        {
                            spawnBlock(x, y, z, SimpleDemos.CreateObject_Example.BlockToCreate.Grass);
                        } else if (y < landStartPoint)
                        {
                            spawnBlock(x, y, z, SimpleDemos.CreateObject_Example.BlockToCreate.Dirt);
                        } else if (y == -height)
                        {
                            spawnBlock(x, y, z, SimpleDemos.CreateObject_Example.BlockToCreate.BedRock);
                        } else
                        {
                            spawnBlock(x, y, z, SimpleDemos.CreateObject_Example.BlockToCreate.Air);
                        }
                    }
                }
            }
        }
    }

    public static void addObjectToList(GameObject _myGameObject)
    {
        //_myGameObject.SetActive(false);
        m_generatedBlocks.Add(_myGameObject);
    }
}


/*
 * void generateNewTerrain()
    {
        limitValues();

        for (int x = -rows; x < rows; x++)
        {
            for (int z = -cols; z < cols; z++)
            {
                int landStartPoint = Mathf.FloorToInt(Mathf.PerlinNoise((x + this.transform.position.x) / (freq * 1.0f), (z + this.transform.position.z) / (freq * 1.0f)) * (amp * 1.0f));

                for (int y = height; y >= -height; y--)
                {
                    Vector3 spawnPos = new Vector3((x * BlockStaticScript.blockScaleX) + this.transform.position.x,
                    (y * BlockStaticScript.blockScaleY) + this.transform.position.y,
                    (z * BlockStaticScript.blockScaleZ) + this.transform.position.z);
                    float radius = 0.15f;

                    bool overlap = false;

                    Collider[] colliderList = Physics.OverlapSphere(spawnPos, radius);

                    for (int i = 0; i < colliderList.Length; i++)
                    {
                        if (colliderList[i].gameObject.tag == "Block")
                        {
                            overlap = true;
                        }
                    }

                    GameObject newBlock;

                    if (!overlap)
                    {
                        if (y == landStartPoint)
                        {
                            newBlock = Instantiate(blockTypes[3], parentToGenerateTo.transform);
                        } else if (y < landStartPoint)
                        {
                            newBlock = Instantiate(blockTypes[2], parentToGenerateTo.transform);
                        } else if (y == -height)
                        {
                            newBlock = Instantiate(blockTypes[1], parentToGenerateTo.transform);
                        }
                        else
                        {
                            newBlock = Instantiate(blockTypes[0], parentToGenerateTo.transform);
                        }

                        newBlock.transform.position = new Vector3((x * BlockStaticScript.blockScaleX) + this.transform.position.x,
                            (y * BlockStaticScript.blockScaleY) + this.transform.position.y,
                            (z * BlockStaticScript.blockScaleZ) + this.transform.position.z);
                    }
                }
            }
        }
    }

    // Fills in blocks under the upper layered blocks
    void fillUnderTerrain(int x, int y, int z, int blockType)
    {

        for (; y >= 0; y--)
        {
            GameObject newBlock;

            Vector3 spawnPos = new Vector3((x * BlockStaticScript.blockScaleX) + this.transform.position.x,
                    (y * BlockStaticScript.blockScaleY) + this.transform.position.y,
                    (z * BlockStaticScript.blockScaleZ) + this.transform.position.z);
            float radius = 0.15f;

            bool overlap = false;

            Collider[] colliderList = Physics.OverlapSphere(spawnPos, radius);

            for (int i = 0; i < colliderList.Length; i++)
            {
                if (colliderList[i].gameObject.tag == "Block")
                {
                    overlap = true;
                }
            }

            if (overlap)
            {
                Debug.Log("UNDER FOUND");
                continue;
            }

            if (y == 0)
            {
                newBlock = Instantiate(blockTypes[0], parentToGenerateTo.transform);
            }
            else
            {
                newBlock = Instantiate(blockTypes[blockType], parentToGenerateTo.transform);
            }

            newBlock.transform.position = new Vector3((x * BlockStaticScript.blockScaleX) + this.transform.position.x,
                        (y * BlockStaticScript.blockScaleY) + this.transform.position.y,
                        (z * BlockStaticScript.blockScaleZ) + this.transform.position.z);

            newBlock.transform.localScale = new Vector3(BlockStaticScript.blockScaleX,
                BlockStaticScript.blockScaleY,
                BlockStaticScript.blockScaleZ);

            generatedBlocks.Add(newBlock);
        }
    }
 */
