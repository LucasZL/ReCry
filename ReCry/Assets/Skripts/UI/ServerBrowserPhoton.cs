//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 12.01.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using Photon;
using System.Collections;

public class ServerBrowserPhoton : Photon.MonoBehaviour {

    private GameObject server;
    public GameObject ServerPrefab;
    public Text Servername;
    public Text Player;
    public Text Ping;
    public Text NoServers;
    public Transform Serverbrowser;


	// Use this for initialization
	void Start ()
    {
        LoadServers();
	}

    public void LoadServers()
    {
        foreach (Transform item in Serverbrowser)
        {
            Destroy(item.gameObject);
        }
        if (PhotonNetwork.insideLobby)
        {
            foreach (var room in PhotonNetwork.GetRoomList())
            {
                if (room.visible)
                {
                    NoServers.text = "";
                    Servername.text = room.name;
                    Player.text = string.Format("{0} / {1}", room.playerCount, room.maxPlayers);
                    Ping.text = PhotonNetwork.GetPing().ToString();
                    this.server = Instantiate(ServerPrefab);
                    this.server.transform.SetParent(Serverbrowser, false);
                    var script = this.server.GetComponent<JoinGame>();
                    script.Name = room.name;
                }
            }
            if (PhotonNetwork.GetRoomList() == null)
            {
                NoServers.text = "No Server avaiable, create a new room";
            }

        }
    }
}
