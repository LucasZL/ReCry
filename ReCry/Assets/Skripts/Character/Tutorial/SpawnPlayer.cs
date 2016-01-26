using UnityEngine;
using System.Collections;

public class SpawnPlayer : Photon.MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PhotonNetwork.Instantiate("PlayerPrefab_Tutorial", new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
