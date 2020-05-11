using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public List<GameObject> blockTypes;
    public List<GameObject> generatedBlocks;

    public int amp = 5;
    public int freq = 5;

    public int rows = 20;
    public int cols = 20;

    public bool generateTerrain = false;

    // Start is called before the first frame update
    void Start()
    {
        generateNewTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (generateTerrain)
        {
            deleteTerrain();
            generateNewTerrain();
            generateTerrain = false;
        }
    }

    void limitValues()
    {
        if (rows > 100)
        {
            rows = 100;
        }

        if (cols > 100)
        {
            cols = 100;
        }
    }

    void deleteTerrain()
    {
        foreach (GameObject block in generatedBlocks)
        {
            Destroy(block);
        }

        generatedBlocks.Clear();
    }

    void generateNewTerrain()
    {
        limitValues();

        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < cols; z++)
            {
                int y = Mathf.FloorToInt(Mathf.PerlinNoise(x / (freq * 1.0f), z / (freq * 1.0f)) * (amp * 1.0f));

                if (y < 1)
                {
                    y = 1;
                }

                GameObject newBlock = Instantiate(blockTypes[2], this.transform);
                newBlock.transform.position = new Vector3(x, y, z);

                generatedBlocks.Add(newBlock);

                fillUnderTerrain(x, y, z, 1);
            }
        }
    }

    void fillUnderTerrain(int x, int y, int z, int blockType)
    {

        for (; y >= 0; y--)
        {
            GameObject newBlock;

            if (y == 0)
            {
                newBlock = Instantiate(blockTypes[0], this.transform);
            } else
            {
                newBlock = Instantiate(blockTypes[blockType], this.transform);
            }

            newBlock.transform.position = new Vector3(x, y, z);

            generatedBlocks.Add(newBlock);
        }
    }
}
