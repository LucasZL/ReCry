//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 01.02.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//


using UnityEngine;
using System.Collections;

public class GetVolume : MonoBehaviour {
    public AudioSource Audiosource;

	// Use this for initialization
	void Start () {
        Audiosource.volume = PlayerPrefs.GetFloat("Volume");
	}
}
