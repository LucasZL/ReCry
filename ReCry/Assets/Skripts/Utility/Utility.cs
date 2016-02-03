//
//  Utility.cs
//  ReCry
//  
//  Created by Kevin Holst, Lucas Zacharias-Langhans, Max Mulert on 14.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon;

public static class Utility
{
    public static string ServerName;
    public static RoomOptions roomOptions;
    public static RoomInfo[] roomInfo;
    public static bool IsInGame;
    public static string Username;
    public static int Team;
	public static bool joinRoom;
    public static int TutorialID;
    public static float MouseSensitivity;
    public static string Version = "1.01";
}
