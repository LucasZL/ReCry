using UnityEngine;
using System.Collections;

public class JoinGame : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Utility.joinRoom = true;
            Utility.isInGame = true;
            PhotonNetwork.Destroy(other.gameObject);
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel("JoinRandomRoom");
		}
	}
}
