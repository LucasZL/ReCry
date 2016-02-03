//
//  Menu.cs
//  ReCry
//  
//  Created by Kevin Holst on 12.01.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//


using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public Animator Animator;

    public bool Animate
    {
        get { return Animator.GetBool("IsOpen"); }
        set { Animator.SetBool("IsOpen",value); }
    }
    
    void Awake ()
    {
        Animator = GetComponent<Animator>();
	}
}
