//
//  IslandOwner.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans on 10.11.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class IslandOwner : MonoBehaviour 
{
    public int owner = 0;
    public int respawnTickets;
    private Rigidbody rigidbody;
    private float timer;

    void Start()
    {
        respawnTickets = 5;
        this.rigidbody = this.gameObject.GetComponent<Rigidbody>();
        this.timer = 0;
    }

    void Update()
    {
        if (respawnTickets <= 0)
        {
            this.owner = 5;

            timer += Time.deltaTime;

            if (timer >= 10)
            {
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
            }

            if (this.gameObject.transform.position.y <= -500)
            {
                this.gameObject.active = false;
            }
        }
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
