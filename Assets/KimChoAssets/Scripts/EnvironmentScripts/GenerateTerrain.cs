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

    public int limitFactor = 50;

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

    // Delete All Blocks That Were Generated
    void deleteTerrain()
    {
        foreach (GameObject block in generatedBlocks)
        {
            Destroy(block);
        }

        generatedBlocks.Clear();
    }

    // Generate a New Terrain
    void generateNewTerrain()
    {
        limitValues();

        for (int x = -rows; x < rows; x++)
        {
            for (int z = -cols; z < cols; z++)
            {
                int y = Mathf.FloorToInt(Mathf.PerlinNoise(x / (freq * 1.0f), z / (freq * 1.0f)) * (amp * 1.0f));

                if (y < 1)
                {
                    y = 1;
                }

                GameObject newBlock = Instantiate(blockTypes[2], this.transform);
                newBlock.transform.position = new Vector3(x * BlockStaticScript.blockScaleX,
                    y * BlockStaticScript.blockScaleY,
                    z * BlockStaticScript.blockScaleZ);

                newBlock.transform.localScale = new Vector3(BlockStaticScript.blockScaleX,
                    BlockStaticScript.blockScaleY,
                    BlockStaticScript.blockScaleZ);

                generatedBlocks.Add(newBlock);

                fillUnderTerrain(x, y, z, 1);
            }
        }
    }

    // Fills in blocks under the upper layered blocks
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

            newBlock.transform.position = new Vector3(x * BlockStaticScript.blockScaleX,
                    y * BlockStaticScript.blockScaleY,
                    z * BlockStaticScript.blockScaleZ);

            newBlock.transform.localScale = new Vector3(BlockStaticScript.blockScaleX,
                BlockStaticScript.blockScaleY,
                BlockStaticScript.blockScaleZ);

            generatedBlocks.Add(newBlock);
        }
    }
}
