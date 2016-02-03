//
//  NetworkManager.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans, Kevin Holst on 14.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour
{
    public bool AutoConnect = true;
    public Text NumberOfPlayers;
    public Text NumberOfRooms;
    public Text ConnectedText;
    public Image ConnectedImage;
    public byte Version = 1;
    
    private bool ConnectInUpdate = true;

    public virtual void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(Utility.Version);
        PhotonNetwork.autoJoinLobby = true;
        Cursor.visible = true;
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
        }
        this.NumberOfPlayers.text = PhotonNetwork.countOfPlayers.ToString();
        this.NumberOfRooms.text = PhotonNetwork.countOfRooms.ToString();

    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public virtual void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("JoinGame");
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("Yes im joined a Lobby");
        ConnectedImage.color = Color.green;
        ConnectedText.text = "Connected";
    }

    public virtual void OnReceivedRoomListUpdate()
    {
        Debug.Log(PhotonNetwork.GetRoomList().Length);
    }

    public virtual void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.LogFormat("Failed: {0},{1}",codeAndMsg);
    }
}
