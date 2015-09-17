using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class PhotonLobby : Photon.MonoBehaviour {

    public Button CreateRoom;
    public Button JoinRoom;
    public Button JoinRandom;
    public InputField Room;
    private string RoomName = "Room Name";

	// Use this for initialization
	void Start ()
    {
        Room.text = RoomName;
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
}
