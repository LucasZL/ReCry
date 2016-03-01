//
//  Login.cs
//  ReCry
//  
//  Created by Kevin Holst on 10.11.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

[XmlRoot]
public class Login : MonoBehaviour
{
    private InputField username;
    private InputField password;
    private Toggle saveUserName;
    private string url = "http://recry.de/Login.php";

    void Start()
    {
        this.username = GameObject.Find("Username").GetComponent<InputField>() as InputField;
        this.password = GameObject.Find("Password").GetComponent<InputField>() as InputField;
        this.saveUserName = GameObject.Find("SaveUser").GetComponent<Toggle>() as Toggle;
        this.saveUserName.isOn = false;
    }

    public void LoginButton()
    {
        StartCoroutine(CheckLogin());
    }

    IEnumerator CheckLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("Username", this.username.text);
        form.AddField("Password", this.password.text);

        Utility.Username = this.username.text;

        WWW check = new WWW(url, form);

        yield return check;

        var json = JSON.Parse(check.text);

        if (!string.IsNullOrEmpty(check.error))
        {
            Debug.Log(check.error);
        }
        else
        {
            int adminNumber = int.Parse(json["IsAdmin"]);
            if (adminNumber == 1)
            {
                Debug.Log("Admin");
                Application.LoadLevel("Menu");
            }
            else
            {
                Application.LoadLevel("Menu");
            }
        }
    }
}
