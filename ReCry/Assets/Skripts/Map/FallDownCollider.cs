//
//  FallDownCollider.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans on 10.11.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class FallDownCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterStats>().Life = 0;
        }
    }
}
