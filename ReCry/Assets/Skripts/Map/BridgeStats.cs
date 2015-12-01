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
    Vector3 startPos;
    Vector3 endPos;
    GameObject plank;
    public GameObject[] planks;
    float lenght;
    float xAngle;
    float yAngle;
    float plankWidth;
    float gapWidth;
    int lifePoints;

    void Start()
    {

    }

    public void GetStats(GameObject plank, Vector3 startPos, Vector3 endPos, float lenght, float xAngle, float yAngle, float plankWidth, float gapWidth)
    {
        this.plank = plank;
        this.startPos = startPos;
        this.endPos = endPos;
        this.lenght = lenght;
        this.xAngle = xAngle;
        this.yAngle = yAngle;
        this.plankWidth = plankWidth;
        this.gapWidth = gapWidth;
    }

    public void SpawnBridge()
    {
        int plankNumber;
        int counter = 0;
        int lenght;
        float effectiveGapWidth;
        float xGrowth;
        float yGrowth;
        float zGrowth;
        HingeJoint hingeJ;
        Rigidbody rig;
        GameObject prevPlank;

        plankNumber = GetPlankNumber();
        effectiveGapWidth = CorrectGapWidth(plankNumber);

        planks = new GameObject[plankNumber + 1];

        xGrowth = GetXGrowth(GetXDiff(startPos.x, endPos.x), plankNumber);
        yGrowth = GetYGrowth(GetYDiff(startPos.y, endPos.y), plankNumber);
        zGrowth = GetZGrowth(GetZDiff(startPos.z, endPos.z), plankNumber);

        foreach (var items in planks)
        {
            planks[counter] = (GameObject)Instantiate(plank, new Vector3(startPos.x + xGrowth * counter, startPos.y + yGrowth * counter, startPos.z + zGrowth * counter), Quaternion.Euler(xAngle, yAngle, 0));
            planks[counter].AddComponent<BridgePlankScript>();
            planks[counter].GetComponent<BridgePlankScript>().GetBridge(this.gameObject);
            counter++;
        }

        // Rigidbody etc. 

        //counter = 0;
        //rig = planks[0].GetComponent<Rigidbody>();

        //foreach (var plank in planks)
        //{
        //    plank.AddComponent<HingeJoint>();
        //    hingeJ = plank.GetComponent<HingeJoint>();
        //    prevPlank = plank;

        //    if (counter - 1 >= 0)
        //    {
        //        hingeJ.connectedBody = planks[counter - 1].GetComponent<Rigidbody>();
        //    }
        //    //plank.GetComponent<Rigidbody>().useGravity = false;
        //    counter++;
        //    rig = plank.GetComponent<Rigidbody>();
        //}

        //rig.isKinematic = true;
        //rig.useGravity = false;
        //rig = planks[0].GetComponent<Rigidbody>();
        //rig.isKinematic = true;
        //rig.useGravity = false;

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
        return secondX - firstX - plankWidth;
    }

    float GetYDiff(float firstY, float secondY)
    {
        return secondY - firstY - plankWidth;
    }

    float GetZDiff(float firstZ, float secondZ)
    {
        return secondZ - firstZ - plankWidth;
    }

    float GetXGrowth(float xDiff, int plankNumber)
    {
        return xDiff / plankNumber;
    }

    float GetYGrowth(float yDiff, int plankNumber)
    {
        return yDiff / plankNumber;
    }

    float GetZGrowth(float zDiff, int plankNumber)
    {
        return zDiff / plankNumber;
    }

    public void GetDamage(int damage)
    {
        lifePoints -= damage;

        if (lifePoints <= 0)
        {
            DestroyBridge();
        }
    }

    void DestroyBridge()
    {
        foreach (var plank in planks)
        {
            plank.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
