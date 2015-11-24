using UnityEngine;
using UnityEngine.UI;
using Photon;
using System.Collections;

public class ServerBrowserButton : Photon.MonoBehaviour {

    public InputField Servername;

	public void CreateGame()
    {
        Utility.ServerName = this.Servername.text;
        Utility.joinRoom = false;
        Utility.isInGame = true;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("JoinRandomRoom");
    }

    public void JoinGame()
    {
        Utility.ServerName = this.Servername.text;
        Utility.joinRoom = true;
        Utility.isInGame = true;
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel("JoinRandomRoom");
    }
}
