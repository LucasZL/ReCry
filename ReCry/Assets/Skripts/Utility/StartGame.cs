using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Utility.joinRoom = false;
            Utility.isInGame = true;
			PhotonNetwork.Destroy(other.gameObject);
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel("JoinRandomRoom");
		}
	}
}
