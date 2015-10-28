using UnityEngine;
using System.Collections;

public class IslandOwner : MonoBehaviour 
{
    public int owner = 0;
    public int respawnTickets;

	void Start ()
    {
        respawnTickets = 5;
	}

	void Update ()
    {
	
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(owner);
		}
		else
		{
			owner = (int)stream.ReceiveNext();
		}
	}
}
