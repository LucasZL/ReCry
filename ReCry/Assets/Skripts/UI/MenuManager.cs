﻿//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 12.01.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : Photon.MonoBehaviour {

    public Menu CurrentMenu;

	// Use this for initialization
	void Start ()
    {
        StartAnimation(CurrentMenu);
	}
	
    public void StartAnimation(Menu menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.Animate = false;
        }
        CurrentMenu = menu;
        CurrentMenu.Animate = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadTutorial()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("TutorialRoom");
        Application.LoadLevel("Tutorial");
    }
}