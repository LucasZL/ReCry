//
//  SpawnPlayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 26.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class SpawnPlayer : Photon.MonoBehaviour {

	void Start ()
    {
        PhotonNetwork.Instantiate("PlayerPrefab_Tutorial", new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, 0);
	}
}
