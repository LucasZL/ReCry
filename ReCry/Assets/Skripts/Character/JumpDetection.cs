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

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Env" || other.gameObject.tag == "BigPrefab" || other.gameObject.tag == "SmallPrefab")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}
