using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class PhotonLobby : Photon.MonoBehaviour {

    public Button CreateRoom;
    public Button JoinRoom;
    public Button JoinRandom;
    public Button RefreshList;
    public Text Servername;
    public Text PlayerNumber;
    public GameObject ServerPanel;
    public Transform ServerPanelTransform;

    private RoomInfo[] roomInfo;
	// Use this for initialization
	void Start ()
    {
        
	}
	
    bool CheckPhotonRoom(string roomName)
    {
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        foreach (var room in roomList)
        {
            if(room.name == roomName)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowServerInBrowser()
    {
        foreach (var room in roomInfo)
        {
            GameObject serverpanel = Instantiate(ServerPanel) as GameObject;
            serverpanel.transform.SetParent(ServerPanelTransform);
            Debug.Log(room);
        }
        Debug.Log("Nicht geklappt");
    }

    public void joinRoomByName()
    {

    }

    public void joinRoomRandom()
    {
        Application.LoadLevel("JoinRandomRoom");
    }

    public void createRoomByName()
    {

    }

     void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate");
    }
}
