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

public class CreatePhotonRoom : Photon.MonoBehaviour
{

    public InputField ServerName;
    public InputField MaxPlayers;
    public Toggle IsVisible;
    public Toggle IsPrivate;
    public Text failuretext;

    private int maxplayer;
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
        this.maxplayer = int.Parse(MaxPlayers.text);


        if (ServerName.text != "")
        {
            if (maxplayer > 32)
            {
                failuretext.text = "Max Player cannot be higher than 32";
                StartCoroutine(DeleteTextAfterFewSeconds(4));
            }
            else
            {
                options.maxPlayers = byte.Parse(MaxPlayers.text);
                PhotonNetwork.CreateRoom(ServerName.text, options, TypedLobby.Default);

            }
        }
        else
        {
            failuretext.text = "Please enter a Servername";
            StartCoroutine(DeleteTextAfterFewSeconds(4));
        }
    }

    public void JoinPhotonRoom()
    {
        PhotonNetwork.JoinRoom(ServerName.text);
    }


    IEnumerator DeleteTextAfterFewSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        failuretext.text = "";
    }
}
