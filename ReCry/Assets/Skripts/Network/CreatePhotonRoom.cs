//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst, Lucas Zacharias-Langhans on 17.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreatePhotonRoom : Photon.MonoBehaviour {

    public  InputField ServerName;
    public InputField MaxPlayers;
    public Toggle IsVisible;
    public Toggle IsPrivate;

    public void CreateNewRoom()
    {
        RoomOptions options = new RoomOptions();
        if (IsVisible.enabled)
        {
            options.isVisible = true;
        }
        else
        {
            options.isVisible = false;
        }

        if (IsPrivate.enabled)
        {
            options.isOpen = false;
        }
        else
        {
            options.isOpen = true;
        }
        options.maxPlayers = byte.Parse(MaxPlayers.text);

        Utility.roomOptions = options;
        Utility.ServerName = ServerName.text;
        Debug.Log(string.Format("Room created with this options Servername: {0}, MaxPlayers: {1}, IsVisible: {2}, IsPrivate: {3} ", ServerName.text, MaxPlayers.text, IsVisible, IsPrivate));

        Application.LoadLevel("CreateRoom");
    }
}
