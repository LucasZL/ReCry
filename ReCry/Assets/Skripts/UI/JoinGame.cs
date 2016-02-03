//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 23.01.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinGame : Photon.MonoBehaviour
{

    Button JoinButton;

    public string Name { get; set; }


    public void Click()
    {
        PhotonNetwork.JoinRoom(Name);
    }
}
