using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour
{
	/// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
	public bool AutoConnect = true;

	public byte Version = 1;
	
	/// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
	private bool ConnectInUpdate = true;

	[Range(1, 31)]
	public int mapSize = 3;
	[Range(1, 10)]
	public int mapHightDifference = 3;
	public float islandSize = 10;
	public int mapSideLength;
	
	GameObject[] smallEnvirement;
	
	bool mapNeedsCorrection = false;


	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
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
						print ("spawned");
						PhotonNetwork.Instantiate("Hexagon_Sand", position, Quaternion.identity, 0);
					}
				}
				
				else if (x == (mapSize - 1) / 2)
				{
					print ("spawned");
					PhotonNetwork.Instantiate("Hexagon_Sand", position, Quaternion.identity, 0);
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
						print ("spawned");
						PhotonNetwork.Instantiate("Hexagon_Sand", position, Quaternion.identity, 0);
					}
				}
			}
		}
	}
	
	void placeSmallEnvirement()
	{
		if (smallEnvirement == null)
			smallEnvirement = GameObject.FindGameObjectsWithTag("EnvSmall");
		
		foreach (GameObject emptyGameObject in smallEnvirement)
		{
			int random = Random.Range(0, 3);
			if(random == 0)
			{
				PhotonNetwork.Instantiate("Small_Cube", new Vector3(emptyGameObject.transform.position.x, emptyGameObject.transform.position.y, emptyGameObject.transform.position.z), Quaternion.identity, 0);
			}
		}
	}
	
	bool isOdd(int value)
	{
		return value % 2 != 0;
	}
}
