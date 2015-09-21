using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGeneratorSinglePlayer : MonoBehaviour
{
    [Range(1, 31)]
    public int mapSize = 3;
    [Range(1, 10)]
    public int mapHightDifference = 3;
    public float islandSize = 10;
    public int mapSideLength;
    public List<GameObject> toSpawnIslands;

    GameObject[] smallEnvirement;

    bool mapNeedsCorrection = false;

    void Start()
    {
        calculateMapDimentions();
        placeIslands();
        placeSmallEnvirement();
    }

    void calculateMapDimentions()
    {
        if(!isOdd(mapSize))
        {
            mapSize++;
        }
        if(mapSize % 4 == 1)
        {
            mapNeedsCorrection = true;
        }
        mapSideLength = (mapSize + 1) / 2;
    }

    void placeIslands()
    {
        Vector3 position = new Vector3(0, 0, 0);
        bool xIsOdd = false;

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                GameObject nextTile = toSpawnIslands[Random.Range(0, toSpawnIslands.Count)];
                int yPosition = Random.Range(0, mapHightDifference);
                if (!isOdd(x))
                {
                    xIsOdd = false;
                    position = new Vector3((islandSize - (islandSize / 8)) * x, yPosition, (islandSize * z) - (islandSize / 2));
                }
                else
                {
                    xIsOdd = true;
                    position = new Vector3((islandSize - (islandSize / 8)) * x, yPosition, (islandSize * z));
                }
                
                if (x <= (mapSize / 2) - 1)
                {
                    int toSpawnCountInLine = mapSideLength + x;
                    int zSpawnLineMinimum = (int)Mathf.Round((mapSize / 2) - (toSpawnCountInLine / 2));
                    int zSpawnLineMaximum = zSpawnLineMinimum;
                    if (xIsOdd && !mapNeedsCorrection)
                    {
                        zSpawnLineMinimum -= 1;
                    }
                    if(xIsOdd && mapNeedsCorrection)
                    {
                        zSpawnLineMaximum += 1;
                    }
                    if(mapNeedsCorrection)
                    {
                        zSpawnLineMinimum -= 1;
                    }
                    if (z > zSpawnLineMinimum && z < mapSize - zSpawnLineMaximum)
                    {
                        Instantiate(nextTile, position, Quaternion.identity);
                    }
                }

                else if (x == (mapSize - 1) / 2)
                {
                    Instantiate(toSpawnIslands[0], position, Quaternion.identity);
                }
               
                else if (x >= (mapSize / 2) - 1)
                {
                    int toSpawnCountInLine = mapSideLength + (mapSize - x);
                    int zSpawnLineMinimum = (int) Mathf.Round((mapSize / 2) - (toSpawnCountInLine / 2));
                    int zSpawnLineMaximum = zSpawnLineMinimum;
                    if (xIsOdd)
                    {
                        zSpawnLineMaximum += 1;
                    }
                    if (xIsOdd && mapNeedsCorrection)
                    {
                        zSpawnLineMinimum -= 1;
                    }
                    if(!xIsOdd && mapNeedsCorrection)
                    {
                        zSpawnLineMaximum += 1;
                    }
                    if (z > zSpawnLineMinimum && z < mapSize - zSpawnLineMaximum)
                    {
                        Instantiate(nextTile, position, Quaternion.identity);
                    }
                }
            }
        }
    }

    void placeSmallEnvirement()
    {
        if (smallEnvirement == null)
            smallEnvirement = GameObject.FindGameObjectsWithTag("EnvSmall");

        foreach (GameObject emptyGameObject in smallEnvirement)
        {
            int random = Random.Range(0, 3);
            if(random == 0)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<BoxCollider>();
                cube.transform.position = new Vector3(emptyGameObject.transform.position.x, emptyGameObject.transform.position.y + (cube.transform.localScale.y / 2), emptyGameObject.transform.position.z);
            }
        }
    }

    bool isOdd(int value)
    {
        return value % 2 != 0;
    }
}
