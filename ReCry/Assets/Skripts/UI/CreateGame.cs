using UnityEngine;
using UnityEngine.UI;
using Photon;
using System.Collections;

public class CreateGame : Photon.MonoBehaviour {

    public InputField Servername;
    public InputField Player;

    public void CreatePhotonGame()
    {
        RoomOptions options = new RoomOptions();
        options.maxPlayers = byte.Parse(Player.text);
        PhotonNetwork.CreateRoom(Servername.text, options, TypedLobby.Default);
        Debug.Log(string.Format("Room created with this options Servername: {0}, MaxPlayers: {1}, IsVisible: {2}, IsPrivate: {3} ", Servername.text, Player.text));

        PhotonNetwork.LoadLevel("JoinGame");
    }
}
