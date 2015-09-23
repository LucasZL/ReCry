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

public class WayPoint : MonoBehaviour
{
    int x;
    int z;
    int islandWidth, islandLenght;
    List<WayPoint> neighbours;
    public bool bridge = false;
    public int bridgeNumber;

    public WayPoint(int islandWidth, int islandLenght, int x, int z)
    {
        neighbours = new List<WayPoint>();

        this.islandLenght = islandLenght;
        this.islandWidth = islandWidth;
        this.x = x;
        this.z = z;
    }

    void Start()
    {
        neighbours = new List<WayPoint>();
    }

    void Update()
    {

    }

    public void GetNeighbours(WayPoint[,] wayPointMesh)
    {
        if (z - 1 >= 0)
            neighbours.Add(wayPointMesh[x, z - 1]);
        if (x + 1 < islandWidth)
            neighbours.Add(wayPointMesh[x + 1, z]);
        if (z + 1 < islandLenght)
            neighbours.Add(wayPointMesh[x, z + 1]);
        if (x - 1 >= 0)
            neighbours.Add(wayPointMesh[x - 1, z]);

        if (x - 1 >= 0 && z - 1 >= 0)
            neighbours.Add(wayPointMesh[x - 1, z - 1]);
        if (x + 1 < islandWidth && z - 1 >= 0)
            neighbours.Add(wayPointMesh[x + 1, z - 1]);
        if (x + 1 < islandWidth && z + 1 < islandLenght)
            neighbours.Add(wayPointMesh[x + 1, z + 1]);
        if (x - 1 >= 0 && z + 1 < islandLenght)
            neighbours.Add(wayPointMesh[x - 1, z + 1]);
    }
}
