//
//  IslandOwnerCollider.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans on 04.10.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class IslandOwnerCollider : MonoBehaviour 
{
    IslandOwner islandOwner;
	public int owner;
	
	void Start ()
    {
        islandOwner = transform.parent.GetComponent<IslandOwner>();
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CharacterStats stats = other.GetComponent<CharacterStats>();
            islandOwner.owner = stats.team;
			this.owner = stats.team;
        }
    }
}