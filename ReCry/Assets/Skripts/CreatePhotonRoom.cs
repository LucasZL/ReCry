using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreatePhotonRoom : Photon.MonoBehaviour {

    public  InputField ServerName;
    public InputField MaxPlayers;
    public Toggle IsVisible;
    public Toggle IsPrivate;
	
	void Start ()
    {
        
	}

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

        PhotonNetwork.CreateRoom(ServerName.text, options, TypedLobby.Default);
        Debug.Log(string.Format("Room created with this options Servername: {0}, MaxPlayers: {1}, IsVisible: {2}, IsPrivate: {3} ", ServerName.text, MaxPlayers.text, IsVisible, IsPrivate));
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.JoinRoom(ServerName.text);
        
    }

    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        CreateNewRoom();
    }

}
