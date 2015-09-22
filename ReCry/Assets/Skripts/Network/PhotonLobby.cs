//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst, Lucas Zacharias-Langhans on 17.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class PhotonLobby : Photon.MonoBehaviour {

    public Button CreateRoomBtn;
    public Button JoinRoom;
    public Button JoinRandom;
    public Button RefreshList;
    public Text Servername;
    public Text PlayerNumber;
    public GameObject ServerPanel;
    public Transform ServerPanelTransform;

    private RoomInfo[] roomInfo;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void Update()
    {
        if (PhotonNetwork.connected)
        {
            Debug.Log("Connected to photon");
        }
        else
        {
            Debug.Log("Not connected to Photon");
        }
    }

    public void ShowServerInBrowser()
    {
        foreach (var room in Utility.roomInfo)
        {
            GameObject serverpanel = Instantiate(ServerPanel) as GameObject;
            serverpanel.transform.SetParent(ServerPanelTransform);
            Debug.Log(room);
        }
        Debug.Log("Nicht geklappt");
    }

    public void joinRoomRandom()
    {
        Application.LoadLevel("JoinRandomRoom");
    }

    public void CreateRoom()
    {
        Application.LoadLevel("CreateRoomSetup");
    }
}
