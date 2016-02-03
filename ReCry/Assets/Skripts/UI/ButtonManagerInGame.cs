//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst on 24.01.2016
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//


using UnityEngine;
using System.Collections;
using System;

public class ButtonManagerInGame : Photon.MonoBehaviour {

    public GameObject MenuPanel;
    public GameObject OptionPanel;
    private bool IsInMenu;

    void Start()
    {
        IsInMenu = false;
        MenuPanel.SetActive(false);
        OptionPanel.SetActive(false);
    }


    void Update()
    {
        CheckIfESCPressed();
    }

    private void CheckIfESCPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsInMenu)
            {
                Cursor.visible = true;
                Utility.IsInGame = true;
                MenuPanel.SetActive(true);
                IsInMenu = true;
            }
        }
    }

    public void OptionButton()
    {
        MenuPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel("Menu");
    }

    public void ReturntoMenu()
    {
        OptionPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Utility.IsInGame = false;
        MenuPanel.SetActive(false);
        IsInMenu = false;
    }

}
