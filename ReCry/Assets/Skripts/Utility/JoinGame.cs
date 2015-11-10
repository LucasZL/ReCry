using UnityEngine;
using System.Collections;

public class JoinGame : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Utility.joinRoom = true;
			PhotonNetwork.Destroy(other.gameObject);
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel(1);
		}
	}
}
