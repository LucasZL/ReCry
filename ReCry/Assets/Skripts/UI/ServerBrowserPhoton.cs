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
    public Transform Serverbrowser;


	// Use this for initialization
	void Start ()
    {
        LoadServers();
	}

    public void LoadServers()
    {
        foreach (var room in PhotonNetwork.GetRoomList())
        {
            Servername.text = room.name;
            Player.text = string.Format("{0} / {1}", room.playerCount, room.maxPlayers);
            Ping.text = PhotonNetwork.GetPing().ToString();
            this.server = Instantiate(ServerPrefab);
            this.server.transform.SetParent(Serverbrowser);
        }
    }
}
