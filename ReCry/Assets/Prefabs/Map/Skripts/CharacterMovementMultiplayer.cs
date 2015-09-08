using UnityEngine;
using System.Collections;

public class CharacterMovementMultiplayer : Photon.MonoBehaviour {

    PhotonView ph;
	// Use this for initialization
	void Start ()
    {
        ph = PhotonView.Get(transform.parent.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ph.isMine)
        {
            MoveCharacter();
        }
	}

    void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(horizontal, 0, vertical);
    }
}
