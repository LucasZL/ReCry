//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans, Kevin Holst on 17.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerRandom : Photon.MonoBehaviour
{
	/// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
	public bool AutoConnect = true;

	public byte Version = 1;
	
	/// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
	private bool ConnectInUpdate = true;

	[Range(1, 31)]
	public int mapSize = 3;
	[Range(1, 100)]
	public int mapHightDifference = 3;
	public float islandSize = 10;
	public int mapSideLength;

    public string[] IslandsToPlace;
    public string[] SmallEnvirementsToPlace;
    public string[] BigEnvirementsToPlace;

    //IslandStats[,] map;
    //IslandStats islandStats;

    GameObject[] smallEnvirement;
    GameObject[] bigEnvirement;
    GameObject[,] Map;


    bool mapNeedsCorrection = false;

    public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
        mapHightDifference = (int)(mapHightDifference * (islandSize / 10));
    }

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
	
	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(Version + "."+Application.loadedLevel);
		}
	}
	
	// to react to events "connected" and (expected) error "failed to join random room", we implement some methods. PhotonNetworkingMessage lists all available methods!
	
	public virtual void OnConnectedToMaster()
	{
		if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}
	
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4 }, null);
	}
	
	// the following methods are implemented to give you some context. re-implement them as needed.
	
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}
	
	public void OnJoinedRoom()
	{
		if (PhotonNetwork.isMasterClient) 
		{
			calculateMapDimentions();
			placeIslands();
			placeSmallEnvirement();
            placeBigEnvirement();
        }

        PhotonNetwork.Instantiate("Playerprefab_Multi", new Vector3(45, 20, 38), Quaternion.identity, 0);
    }

    public void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
	}

	void calculateMapDimentions()
	{
		if(!isOdd(mapSize))
		{
			mapSize++;
		}
		if(mapSize % 4 == 1)
		{
			mapNeedsCorrection = true;
		}
		mapSideLength = (mapSize + 1) / 2;
	}
	
	void placeIslands()
	{
		Vector3 position = new Vector3(0, 0, 0);
		bool xIsOdd = false;
		
		for (int x = 0; x < mapSize; x++)
		{
			for (int z = 0; z < mapSize; z++)
			{
				int yPosition = Random.Range(0, mapHightDifference);
				if (!isOdd(x))
				{
					xIsOdd = false;
					position = new Vector3((islandSize - (islandSize / 8)) * x, yPosition, (islandSize * z) - (islandSize / 2));
				}
				else
				{
					xIsOdd = true;
					position = new Vector3((islandSize - (islandSize / 8)) * x, yPosition, (islandSize * z));
				}
				
				if (x <= (mapSize / 2) - 1)
				{
					int toSpawnCountInLine = mapSideLength + x;
					int zSpawnLineMinimum = (int)Mathf.Round((mapSize / 2) - (toSpawnCountInLine / 2));
					int zSpawnLineMaximum = zSpawnLineMinimum;
					if (xIsOdd && !mapNeedsCorrection)
					{
						zSpawnLineMinimum -= 1;
					}
					if(xIsOdd && mapNeedsCorrection)
					{
						zSpawnLineMaximum += 1;
					}
					if(mapNeedsCorrection)
					{
						zSpawnLineMinimum -= 1;
					}
					if (z > zSpawnLineMinimum && z < mapSize - zSpawnLineMaximum)
					{
                        placeIsland(position);
                    }
				}
				
				else if (x == (mapSize - 1) / 2)
				{
                    placeIsland(position);
                }
				
				else if (x >= (mapSize / 2) - 1)
				{
					int toSpawnCountInLine = mapSideLength + (mapSize - x);
					int zSpawnLineMinimum = (int) Mathf.Round((mapSize / 2) - (toSpawnCountInLine / 2));
					int zSpawnLineMaximum = zSpawnLineMinimum;
					if (xIsOdd)
					{
						zSpawnLineMaximum += 1;
					}
					if (xIsOdd && mapNeedsCorrection)
					{
						zSpawnLineMinimum -= 1;
					}
					if(!xIsOdd && mapNeedsCorrection)
					{
						zSpawnLineMaximum += 1;
					}
					if (z > zSpawnLineMinimum && z < mapSize - zSpawnLineMaximum)
					{
                        placeIsland(position);
                    }
				}
			}
		}
        CreateMapArray();
	}

    void placeSmallEnvirement()
    {
        if (smallEnvirement == null)
            smallEnvirement = GameObject.FindGameObjectsWithTag("EnvSmall");

        foreach (GameObject emptyGameObject in smallEnvirement)
        {
            int random = Random.Range(0, 7);
            int randomEnvirement = Random.Range(0, SmallEnvirementsToPlace.Length);
            if (random != 0)
            {
                PhotonNetwork.Instantiate(SmallEnvirementsToPlace[randomEnvirement], new Vector3(emptyGameObject.transform.position.x, emptyGameObject.transform.position.y, emptyGameObject.transform.position.z), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), 0).transform.parent = emptyGameObject.transform.parent;
            }
            foreach(GameObject smallEnv in GameObject.FindGameObjectsWithTag("EnvSmall"))
            {
                if(smallEnv.GetComponent<PhotonView>())
                {
                    Destroy(smallEnv.GetComponent<PhotonView>());
                }
            }
        }
    }

    void placeBigEnvirement()
    {
        if (bigEnvirement == null)
            bigEnvirement = GameObject.FindGameObjectsWithTag("EnvBig");

        foreach (GameObject emptyGameObject in bigEnvirement)
        {
            int random = Random.Range(0, 7);
            int randomEnvirement = Random.Range(0, BigEnvirementsToPlace.Length);
            if (random != 0)
            {
                PhotonNetwork.Instantiate(BigEnvirementsToPlace[randomEnvirement], new Vector3(emptyGameObject.transform.position.x, emptyGameObject.transform.position.y, emptyGameObject.transform.position.z), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), 0).transform.parent = emptyGameObject.transform.parent;
            }
        }
        foreach (GameObject smallEnv in GameObject.FindGameObjectsWithTag("EnvBig"))
        {
            if (smallEnv.GetComponent<PhotonView>())
            {
                Destroy(smallEnv.GetComponent<PhotonView>());
            }
        }
    }

    void placeIsland(Vector3 position)
    {
        int random = Random.Range(0, IslandsToPlace.Length);
        int rotation = Random.Range(0, 5);
        PhotonNetwork.Instantiate(IslandsToPlace[random], position, Quaternion.Euler(0.0f, rotation * 60, 0.0f), 0);
    }

    bool isOdd(int value)
	{
		return value % 2 != 0;
	}

    void RenameBridgePoints(int random, PhotonView name)
    {

        //WayPoint[] wP = photonView.transform.gameObject.GetComponents<WayPoint>();

        switch (random)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    void CreateMapArray()
    {
        //map = new IslandStats[mapSize, mapSize];
        Map = new GameObject[mapSize, mapSize];

        List<GameObject> islands = new List<GameObject>();
        GameObject[] islandArray = GameObject.FindGameObjectsWithTag("Env");
        GameObject toCopyIsland = islandArray[0];
        int z = 0;
        int x = 0;
        int currentMapWidth = (mapSize / 2) +1;

        foreach (var island in islandArray)
        {
            islands.Add(island);
        }

        for (int i = 0; i < islands.Count; i++)
        {
            foreach (var island in islands)
            {
                if (island.transform.position.x <= toCopyIsland.transform.position.x)
                {
                    if (island.transform.position.z <= toCopyIsland.transform.position.z)
                    {
                        toCopyIsland = island;
                    }
                }
            }

            if (z < mapSize / 2)
            {
                if (x == currentMapWidth)
                {
                    z++;
                    currentMapWidth++;
                    x = 0;
                }
            }
            else if (z >= mapSize / 2)
            {
                if (x == currentMapWidth)
                {
                    z++;
                    currentMapWidth--;
                    x = 0;
                }
            }
            //map[x, z] = new IslandStats(toCopyIsland, x, z);
            //map[x, z] = GameObject.Instantiate(islandStats);
            //map[x, z].GetSomeStats(toCopyIsland, x, z);
            Map[x, z] = toCopyIsland;
            Map[x, z].AddComponent<IslandStats>();
            Map[x, z].GetComponent<IslandStats>().GetSomeStats(toCopyIsland, x, z);
            x++;
        }

        //foreach (var island in Map)
        //{
        //    island.GetComponent<IslandStats>().GetNeighbours(Map, mapSize);
        //}
    }

    void GenerateBridges()
    {
        Map[0, 0].GetComponentInChildren<WayPoint>();
    }
}
