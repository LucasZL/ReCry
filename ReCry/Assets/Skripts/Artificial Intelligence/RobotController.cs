//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Max Mulert on 20.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour
{
    public bool walking = false;
    public WayPoint[] WayPoints;
    public Vector3 target;
    public GameObject Target;
    public GameObject g;
    public int speed = 1;
    bool crossedBridge = true;


    void Start ()
    {
	
	}
	
	void Update ()
    {
        if (walking)
        {
            //walk
            Vector3 moveDirection = new Vector3();
            moveDirection = GetMoveDirection();
            gameObject.transform.Translate(moveDirection.normalized * Time.deltaTime * speed);
            Debug.Log("Move Direction:" + moveDirection);

            if (Arrived())
            {
                walking = false;
            }
        }
        else
        {
            target = FindNewTarget();
        }
    }

    Vector3 FindNewTarget()
    {
        if (crossedBridge)
        {
            crossedBridge = false;
            return FindNewIslandTarget();
        }
        else
        {
            int rnd = Random.Range(0, 2);
            
            if (rnd == 0)
            {
                if (Target.GetComponent<WayPoint>().otherBridgePoint != null)
                {
                    crossedBridge = true;
                    return Target.GetComponent<WayPoint>().otherBridgePoint.transform.position;
                }
                else
                {
                    crossedBridge = false;
                    return FindNewIslandTarget();
                }
            }
            else
            {
                crossedBridge = false;
                return FindNewIslandTarget();
            }
        }
    }

    Vector3 FindNewIslandTarget()
    {
        RaycastHit ray;
        Vector3 startPoint = this.gameObject.transform.position;

        if (Physics.Raycast(startPoint, Vector3.down, out ray, 5))
        {
            Debug.Log("RayCastHit");
            WayPoints = ray.transform.GetComponentsInChildren<WayPoint>();
        }
        walking = true;
        return GetWayPoint();
    }

    Vector3 GetWayPoint()
    {
        int rnd = Random.Range(0, WayPoints.GetLength(0) - 1);

        if ((WayPoints[rnd].bridge == true && WayPoints[rnd].bridgeSpwaned == true) || WayPoints[rnd].bridge == false)
        {
            return WayPoints[rnd].gameObject.transform.position;
            g = WayPoints[rnd].gameObject;
        }
        else
        {
            return GetWayPoint();
        }
    }

    Vector3 GetMoveDirection()
    {
        Vector3 direction = target - this.gameObject.transform.position;
        return direction;
    }

    bool Arrived()
    {
        if (this.transform.position.x - 1 < target.x)
        {
            if (this.transform.position.z - 1 < target.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (this.transform.position.x + 1 > target.x)
        {
            if (this.transform.position.z + 1 > target.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
