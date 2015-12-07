//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 16.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class JumpDetection : MonoBehaviour
{

    public bool isGrounded;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Env" || other.gameObject.tag == "BigPrefab" || other.gameObject.tag == "SmallPrefab")
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Env" || other.gameObject.tag == "BigPrefab" || other.gameObject.tag == "SmallPrefab" || other.gameObject.tag == "Bridge") 
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}
