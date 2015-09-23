//
//  CharacterMovementMultiplayer.cs
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

    int x, y, z;
    int islandWidth, islandLenght;
    int toBeFilled;
    List<GameObject> neighbours;
    WayPoint[,] wayPointMesh;
    WayPoint wayPoint;
    GameObject island;

    public IslandStats(GameObject Island, int x, int z)
    {
        neighbours = new List<GameObject>();
        this.island = Island;
        this.x = x;
        this.z = z;
    }

    void Start()
    {
        neighbours = new List<GameObject>();
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

    public void GetNeighbours(GameObject[,] map, int mapLenght)
    {
        int mapWidth = GetMapWidth(mapLenght);

        if (x < (mapLenght / 2) + 1)
        {
            if (x - 1 >= 0 && z - 1 >= 0)
                neighbours.Add(map[x - 1, z - 1]);
            if (z - 1 >= 0)
                neighbours.Add(map[x, z - 1]);
            if (x + 1 < mapWidth)
                neighbours.Add(map[x + 1, z]);
            if (x + 1 < mapLenght && z + 1 < mapWidth)
                neighbours.Add(map[x + 1, z + 1]);
            if (z + 1 < mapLenght)
                neighbours.Add(map[x, z + 1]);
            if (x - 1 >= 0)
                neighbours.Add(map[x - 1, z]);
        }

        if (x == (mapLenght / 2) + 1)
        {
            if (x - 1 >= 0 && z - 1 >= 0)
                neighbours.Add(map[x - 1, z - 1]);
            if (z - 1 >= 0)
                neighbours.Add(map[x, z - 1]);
            if (x + 1 < islandWidth)
                neighbours.Add(map[x + 1, z]);
            if (z + 1 < islandWidth)
                neighbours.Add(map[x, z + 1]);
            if (z + 1 < islandLenght && x - 1 >= 0)
                neighbours.Add(map[x - 1, z + 1]);
            if (x - 1 >= 0)
                neighbours.Add(map[x - 1, z]);
        }

        if (x > (mapLenght / 2) + 1)
        {
            if (z - 1 >= 0)
                neighbours.Add(map[x, z - 1]);
            if (z - 1 >= 0 && x + 1 < mapWidth)
                neighbours.Add(map[x + 1, z - 1]);
            if (x + 1 < mapWidth)
                neighbours.Add(map[x + 1, z]);
            if (z + 1 < mapLenght)
                neighbours.Add(map[x, z + 1]);
            if (z + 1 < mapLenght && x - 1 >= 0)
                neighbours.Add(map[x - 1, z + 1]);
            if (x - 1 >= 0)
                neighbours.Add(map[x - 1, z]);
        }
    }

    void GetWayPointMesh()
    {
        wayPointMesh = new WayPoint[islandWidth - 1, islandLenght - 1];

        for (int x = 0; x < islandWidth; x++)
        {
            for (int z = 0; z < islandLenght; z++)
            {
                //wayPointMesh[x, z] = GameObject.Instantiate(WayPoint , new Vector3(x - islandWidth / 2, toBeFilled, z - islandLenght / 2), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
                wayPointMesh[x, z] = new WayPoint(islandWidth, islandLenght, x, z);
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
