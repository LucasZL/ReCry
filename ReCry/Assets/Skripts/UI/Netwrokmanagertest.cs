using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Netwrokmanagertest : MonoBehaviour
{
    public bool AutoConnect = true;
    public Text NumberOfPlayers;
    public Text NumberOfRooms;
    public byte Version = 1;

    public virtual void Start()
    {
        PhotonNetwork.ConnectUsingSettings(Utility.Version);
        Debug.Log(Utility.Version);
        this.NumberOfRooms.text = PhotonNetwork.countOfRooms.ToString();
        PhotonNetwork.autoJoinLobby = false; // we join randomly. always. no need to join a lobby to get the list of rooms.
        Cursor.visible = true;
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public virtual void Update()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        CreateNewRoom();
    }

    public void CreateNewRoom()
    {
        PhotonNetwork.CreateRoom(Utility.ServerName, Utility.roomOptions, TypedLobby.Default);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {

        }

        PhotonNetwork.Instantiate("Playerprefab_Multi", new Vector3(0, 1, 0), Quaternion.identity, 0);
    }

    public void OnJoinedLobby()
    {

    }
}
