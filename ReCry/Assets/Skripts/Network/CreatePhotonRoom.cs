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
    public Text failuretext;

    public void CreateNewRoom()
    {
        RoomOptions options = new RoomOptions();
        if (IsVisible.isOn)
        {
            options.isVisible = true;
        }
        else
        {
            options.isVisible = false;
        }

        if (IsPrivate.isOn)
        {
            options.isOpen = false;
        }
        else
        {
            options.isOpen = true;
        }
        options.maxPlayers = byte.Parse(MaxPlayers.text);

        if (ServerName.text != "")
        {
            PhotonNetwork.CreateRoom(ServerName.text,options,TypedLobby.Default);
        }
        else
        {
            failuretext.text = "Please enter a Servername";
        }
        
        Debug.Log(string.Format("Room created with this options Servername: {0}, MaxPlayers: {1}, IsVisible: {2}, IsPrivate: {3} ", ServerName.text, MaxPlayers.text, IsVisible, IsPrivate));
    }

    public void JoinPhotonRoom()
    {
        PhotonNetwork.JoinRoom(ServerName.text);
    }
}
