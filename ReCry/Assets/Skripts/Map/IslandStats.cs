//
//  IslandStats.cs
//  ReCry
//  
//  Created by Max Mulert on 21.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandStats : MonoBehaviour
{

    public int x, y, z;
    public bool Odd;
    int islandWidth, islandLenght;
    int toBeFilled;
    public List<GameObject> neighbours;
    public List<GameObject> bridgePoints;
    WayPoint[,] wayPointMesh;
    WayPoint wayPoint;
    GameObject island;

    public IslandStats(GameObject Island, int x, int z)
    {
        this.island = Island;
        this.x = x;
        this.z = z;
    }

    void Start()
    {
    }

    public void GetSomeStats(GameObject Island, int x, int z)
    {
        this.island = Island;
        this.x = x;
        this.z = z;
    }

    void Update()
    {

    }


    public void GetStats(int lenght, int width)
    {

    }

    public void GetNeighbours(GameObject[,] map, float islandSize)
    {
        neighbours = new List<GameObject>();

        if (gameObject.transform.position.z % islandSize == 0)
        {
            foreach (var island in map)
            {
                if (island != null)
                {
                    if (island.GetComponent<IslandStats>().x == x - 1 && island.GetComponent<IslandStats>().z == z)
                    {
                        neighbours.Add(map[x - 1, z]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z - 1 && island.GetComponent<IslandStats>().x == x)
                    {
                        neighbours.Add(map[x, z - 1]);
                    }
                    else if (island.GetComponent<IslandStats>().x == x + 1 && island.GetComponent<IslandStats>().z == z)
                    {
                        neighbours.Add(map[x + 1, z]);
                    }
                    else if (island.GetComponent<IslandStats>().x == x + 1 && island.GetComponent<IslandStats>().z == z + 1)
                    {
                        neighbours.Add(map[x + 1, z + 1]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z + 1 && island.GetComponent<IslandStats>().x == x)
                    {
                        neighbours.Add(map[x, z + 1]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z + 1 && island.GetComponent<IslandStats>().x == x - 1)
                    {
                        neighbours.Add(map[x - 1, z + 1]);
                    }
                }
            }
        }

        else
        {
            Odd = true;
            foreach (var island in map)
            {
                if (island != null)
                {
                    if (island.GetComponent<IslandStats>().x == x - 1 && island.GetComponent<IslandStats>().z == z)
                    {
                        neighbours.Add(map[x - 1, z]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z - 1 && island.GetComponent<IslandStats>().x == x + 1)
                    {
                        neighbours.Add(map[x + 1, z - 1]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z - 1 && island.GetComponent<IslandStats>().x == x)
                    {
                        neighbours.Add(map[x, z - 1]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z - 1 && island.GetComponent<IslandStats>().x == x - 1)
                    {
                        neighbours.Add(map[x - 1, z - 1]);
                    }
                    else if (island.GetComponent<IslandStats>().x == x + 1 && island.GetComponent<IslandStats>().z == z)
                    {
                        neighbours.Add(map[x + 1, z]);
                    }
                    else if (island.GetComponent<IslandStats>().z == z + 1 && island.GetComponent<IslandStats>().x == x)
                    {
                        neighbours.Add(map[x, z + 1]);
                    }
                }
            }
        }
    }

    public void GetBridgePoints()
    {
        Transform bridgeTrans;
        bridgePoints = new List<GameObject>();

        bridgeTrans = island.transform.FindChild("BridgePoint1");
        bridgePoints.Add(bridgeTrans.gameObject);

        bridgeTrans = island.transform.FindChild("BridgePoint2");
        bridgePoints.Add(bridgeTrans.gameObject);

        bridgeTrans = island.transform.FindChild("BridgePoint3");
        bridgePoints.Add(bridgeTrans.gameObject);

        bridgeTrans = island.transform.FindChild("BridgePoint4");
        bridgePoints.Add(bridgeTrans.gameObject);

        bridgeTrans = island.transform.FindChild("BridgePoint5");
        bridgePoints.Add(bridgeTrans.gameObject);

        bridgeTrans = island.transform.FindChild("BridgePoint6");
        bridgePoints.Add(bridgeTrans.gameObject);
    }

    void GetWayPointMesh()
    {
        wayPointMesh = new WayPoint[islandWidth - 1, islandLenght - 1];

        for (int x = 0; x < islandWidth; x++)
        {
            for (int z = 0; z < islandLenght; z++)
            {
                //wayPointMesh[x, z] = GameObject.Instantiate(WayPoint , new Vector3(x - islandWidth / 2, toBeFilled, z - islandLenght / 2), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
                //wayPointMesh[x, z] = new WayPoint(islandWidth, islandLenght, x, z);
            }
        }

        foreach (var wayPoint in wayPointMesh)
        {
            wayPoint.GetNeighbours(wayPointMesh);
        }
    }

    int GetMapWidth(int mapSize)
    {
        float mapMiddle = (mapSize / 2) + 1;

        if (z + 1 == mapMiddle)
            return mapSize;
        else
            return (int)(mapSize - (Mathf.Sqrt(((z + 1) - mapMiddle) * ((z + 1) - mapMiddle))));
    }
}
