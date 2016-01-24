using UnityEngine;
using System.Collections;
using System;

public class ButtonManagerInGame : Photon.MonoBehaviour {

    public GameObject MenuPanel;
    public GameObject OptionPanel;

    void Start()
    {
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
            Cursor.visible = true;
            MenuPanel.SetActive(true);
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
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        MenuPanel.SetActive(false);
    }

}
