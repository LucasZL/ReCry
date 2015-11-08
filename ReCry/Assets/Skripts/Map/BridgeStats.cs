//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Maximilian Mulert on 08.11.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class BridgeStats : MonoBehaviour
{
    GameObject startIsland;
    GameObject endIsland;
    GameObject plank;
    GameObject[] planks;
    float lenght;
    float xAngle;
    float yAngle;
    float plankWidth;
    float gapWidth;

    void Start ()
    {
	
	}

    public void GetStats(GameObject plank, GameObject startIsland, GameObject endIsland, float lenght, float xAngle, float yAngle, float plankWidth, float gapWidth)
    {
        this.plank = plank;
        this.startIsland = startIsland;
        this.endIsland = endIsland;
        this.lenght = lenght;
        this.xAngle = xAngle;
        this.yAngle = yAngle;
        this.plankWidth = plankWidth;
        this.gapWidth = gapWidth;
    }

    public void SpawnBridge()
    {
        int plankNumber;
        float effectiveGapWidth;
        float xGrowth;
        float yGrowth;
        float zGrowth;

        plankNumber = GetPlankNumber();
        effectiveGapWidth = CorrectGapWidth(plankNumber);

        planks = new GameObject[plankNumber];

        xGrowth = GetXGrowth(GetXDiff(startIsland.transform.position.x, endIsland.transform.position.x), plankNumber);
        yGrowth = GetYGrowth(GetYDiff(startIsland.transform.position.y, endIsland.transform.position.y), plankNumber);
        zGrowth = GetZGrowth(GetZDiff(startIsland.transform.position.z, endIsland.transform.position.z), plankNumber);
    }

    int GetPlankNumber()
    {
        float effectiveLenght = lenght - plankWidth;
        return (int)(effectiveLenght / (plankWidth + gapWidth));
    }

    float CorrectGapWidth(int plankNumber)
    {
        float missingDiff = lenght - ((plankWidth + gapWidth) * plankNumber);
        return (missingDiff / plankNumber) + gapWidth; ;
    }

    float GetXDiff(float firstX, float secondX)
    {
        return firstX - secondX - plankWidth;
    }

    float GetYDiff(float firstY, float secondY)
    {
        return firstY - secondY - plankWidth;
    }

    float GetZDiff(float firstZ, float secondZ)
    {
        return firstZ - secondZ - plankWidth;
    }

    float GetXGrowth(float xDiff, int plankNumber)
    {
        return 0;
    }

    float GetYGrowth(float yDiff, int plankNumber)
    {
        return 0;
    }

    float GetZGrowth(float zDiff, int plankNumber)
    {
        return 0;
    }
}
