//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Max Mulert on 20.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//
using UnityEngine;
using System.Collections;

public class TriggerController : MonoBehaviour
{
    RobotController rc;

    void Start()
    {
        rc = this.gameObject.transform.parent.GetComponent<RobotController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rc.Enemys.Add(other.gameObject);
        }
        else if (other.gameObject.tag == "Agent")
        {
            rc.Allies.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rc.Enemys.Remove(other.gameObject);
        }
        else if (other.gameObject.tag == "Agent")
        {
            rc.Allies.Add(other.gameObject);
        }
    }
}