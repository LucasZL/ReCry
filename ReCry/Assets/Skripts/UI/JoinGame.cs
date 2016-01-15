using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinGame : Photon.MonoBehaviour
{

    Button JoinButton;

    public string Name { get; set; }


    public void Click()
    {
        PhotonNetwork.JoinRoom(Name);
        Debug.Log("Joined Room" + Name);
    }
}
