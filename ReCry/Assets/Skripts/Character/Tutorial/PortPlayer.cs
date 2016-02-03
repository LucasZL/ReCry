//
//  PortPlayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 26.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class PortPlayer : MonoBehaviour {

    public GameObject Spawn;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = new Vector3(Spawn.transform.position.x, Spawn.transform.position.y, Spawn.transform.position.z);
        }
    }
}
