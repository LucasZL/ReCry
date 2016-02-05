//
//  BridgeStats.cs
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

    public void SpawnMultiBridge()
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
            //planks[counter] = (GameObject)Instantiate(plank, new Vector3(startPos.x + xGrowth * counter, startPos.y + yGrowth * counter, startPos.z + zGrowth * counter), Quaternion.Euler(xAngle, yAngle, 0));
            planks[counter] = (GameObject)PhotonNetwork.Instantiate("BridgeCube", new Vector3(startPos.x + xGrowth * counter, startPos.y + yGrowth * counter, startPos.z + zGrowth * counter), Quaternion.Euler(xAngle, yAngle, 0), 0);
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

    public void SpawnSingleBridge()
    {
        plank.transform.localScale = new Vector3(10, 0.5f, lenght);
        Instantiate(plank, GetSpawnPosition(startPos, endPos), Quaternion.Euler(xAngle, yAngle, 0));
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

    Vector3 GetSpawnPosition(Vector3 start, Vector3 end)
    {
        Vector3 pos = new Vector3();
        float diffX = 0;
        float diffY = 0;
        float diffZ = 0;

        if (start.x >= 0 && end.x >= 0)
        {
            if (start.x < end.x)
            {
                diffX = end.x - start.x;
            }
            else
            {
                diffX = start.x - end.x;
            }
        }
        else if (start.x < 0 && end.x >= 0)
        {
            diffX = end.x + Mathf.Sqrt(start.x * start.x);
        }
        else if (start.x < 0 && end.x < 0)
        {
            if (start.x < end.x)
            {
                diffX = end.x - start.x;
            }
            else
            {
                diffX = start.x - end.x;
            }
        }
        else if (start.x >= 0 && end.x < 0)
        {
            diffX = start.x + Mathf.Sqrt(end.x * end.x);
        }

        if (start.x > end.x)
        {
            pos.x = end.x + (diffX / 2);
        }
        else
        {
            pos.x = start.x + (diffX / 2);
        }

        if (start.y >= 0 && end.y >= 0)
        {
            if (start.y < end.y)
            {
                diffY = end.y - start.y;
            }
            else
            {
                diffY = start.y - end.y;
            }
        }
        else if (start.y < 0 && end.y >= 0)
        {
            diffY = end.y + Mathf.Sqrt(start.y * start.y);
        }
        else if (start.y < 0 && end.y < 0)
        {
            if (start.y < end.y)
            {
                diffY = end.y - start.y;
            }
            else
            {
                diffY = start.y - end.y;
            }
        }
        else if (start.y >= 0 && end.y < 0)
        {
            diffY = start.y + Mathf.Sqrt(end.y * end.y);
        }

        if (start.y > end.y)
        {
            pos.y = end.y + (diffY / 2);
        }
        else
        {
            pos.y = start.y + (diffY / 2);
        }

        if (start.z >= 0 && end.z >= 0)
        {
            if (start.z < end.z)
            {
                diffZ = end.z - start.z;
            }
            else
            {
                diffZ = start.z - end.z;
            }
        }
        else if (start.z < 0 && end.z >= 0)
        {
            diffZ = end.z + Mathf.Sqrt(start.z * start.z);
        }
        else if (start.z < 0 && end.z < 0)
        {
            if (start.z < end.z)
            {
                diffZ = end.z - start.z;
            }
            else
            {
                diffZ = start.z - end.z;
            }
        }
        else if (start.z >= 0 && end.z < 0)
        {
            diffZ = start.z + Mathf.Sqrt(end.z * end.z);
        }

        if (start.z > end.z)
        {
            pos.z = end.z + (diffZ / 2);
        }
        else
        {
            pos.z = start.z + (diffZ / 2);
        }

        return pos;
    }

    void DestroyBridge()
    {
        foreach (var plank in planks)
        {
            plank.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
