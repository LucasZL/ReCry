//
//  SpawnEnemies.cs
//  ReCry
//  
//  Created by Kevin Holst on 26.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class SpawnEnemies : Photon.MonoBehaviour {
    
	void Start ()
    {
        for (int i = 0; i < 3; i++)
        {
            PhotonNetwork.Instantiate("Dummy", new Vector3(this.transform.position.x + (i * 20), this.transform.position.y, this.transform.position.z), Quaternion.identity, 0);
        }
	}
}
