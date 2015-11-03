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
using System;

public class NetworkManagerRandom : Photon.MonoBehaviour
{
    public bool AutoConnect = true;
    public string Version = "1.01";
    private bool ConnectInUpdate = true;

    [Range(1, 31)]
    public int mapSize = 3;
    [Range(1, 100)]
    public int mapHightDifference = 3;
    public float islandSize = 10.0f;
    public float BridgeTileWidth;
    public int mapSideLength;

    public bool gameStarted = false;

    public string[] IslandsToPlace;
    public string[] SmallEnvirementsToPlace;
    public string[] BigEnvirementsToPlace;

    GameObject[] smallEnvirement;
    GameObject[] bigEnvirement;
    GameObject[,] Map;
    GameObject[] respawnPoints;
    public List<GameObject> minimapIslands;
    public List<GameObject> mapIslands;
    public GameObject bridgeCube;
    bool playerSpawned = false;
    bool mapNeedsCorrection = false;
    bool envFixed = false;
    bool mapFixed = false;

    GameObject firstTeamSpawn;
    GameObject seccTeamSpawn;
    GameObject thirdTeamSpawn;
    GameObject fourthTeamSpawn;

    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
        mapHightDifference = (int)(mapHightDifference * (islandSize / 10));
        minimapIslands = new List<GameObject>();
        mapIslands = new List<GameObject>();
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
            PhotonNetwork.ConnectUsingSettings(Version);
        }

        if (GameObject.FindGameObjectWithTag("Respawn") && PhotonNetwork.connectionStateDetailed.ToString() == "Joined")
        {
            if (!playerSpawned)
            {
                getSpawnPoints();
                int spawn = UnityEngine.Random.Range(0, respawnPoints.Length);
                PhotonNetwork.Instantiate("Playerprefab_Multi", respawnPoints[spawn].transform.position, Quaternion.identity, 0);
                playerSpawned = true;
            }
        }

        if (!mapFixed)
        {
            minimapIslands.AddRange(GameObject.FindGameObjectsWithTag("minimapIsland"));
            mapIslands.AddRange(GameObject.FindGameObjectsWithTag("Env"));
        }

        if (minimapIslands.Count != 0 && mapIslands.Count != 0 && !mapFixed)
        {
            for (int i = 0; i < mapIslands.Count; i++)
            {
                int owner = mapIslands[i].GetComponent<IslandOwner>().owner;
                minimapIslands[i].GetComponent<MinimapIslandStats>().owner = owner;
            }

            foreach (var island in minimapIslands)
            {
                island.transform.localScale = new Vector3(islandSize / 30 * 0.25f, 0.1f * 0.25f, islandSize / 30 * 0.25f);
                island.transform.parent = GameObject.Find("Minimap").transform;
            }

            GameObject minimap = GameObject.Find("Minimap");
            minimap.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            minimap.transform.parent = GameObject.Find("MiniMapParent").transform;
            GameObject minimapparent = GameObject.Find("MiniMapParent");
            minimapparent.transform.parent = GameObject.Find("MiniMapParentParent").transform;
            GameObject minimapparentparent = GameObject.Find("MiniMapParentParent");
            minimapparentparent.transform.position = new Vector3(((islandSize * mapSize) / 2 - (islandSize * mapSize) / 8) * 1.11f, 60, ((islandSize * mapSize) / 2 - (((islandSize * mapSize) / 2) * 0.25f) - (islandSize * mapSize) / 8) * 1.05f);
            mapFixed = true;
        }

        if (minimapIslands.Count != 0 && mapIslands.Count != 0)
        {
            for (int i = 0; i < mapIslands.Count; i++)
            {
                int owner = mapIslands[i].GetComponent<IslandOwner>().owner;
                minimapIslands[i].GetComponent<MinimapIslandStats>().owner = owner;
            }
        }
    }

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

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        calculateMapDimentions();

        if (PhotonNetwork.isMasterClient)
        {
            placeIslands(IslandsToPlace, 0, true, 1);
            placeIslands(IslandsToPlace, -550, false, 0.25f);
        }
        placeSmallEnvirement();
        placeBigEnvirement();

        SetPivotPoint();
        fillMapList();
        setFirstSpawn();
        Cursor.visible = false;
    }

    private void setFirstSpawn()
    {
        foreach (GameObject island in mapIslands)
        {
            if (island.transform.position.x == 0 && island.transform.position.z == (((mapSize + 1) * islandSize) / 2) - (islandSize * 1.5f))
            {
                Debug.Log("erster");
                firstTeamSpawn = island;
                island.GetComponent<IslandOwner>().owner = 1;
            }
            else if (island.transform.position.x == ((((mapSize - 1) * islandSize) * 0.875) / 2) && island.transform.position.z == 0)
            {
                Debug.Log("zweiter");
                seccTeamSpawn = island;
                island.GetComponent<IslandOwner>().owner = 2;
            }
            else if (island.transform.position.x == (((mapSize - 1) * islandSize) * 0.875) && island.transform.position.z == ((float)mapSize / 2) * islandSize)
            {
                Debug.Log("dritter");
                thirdTeamSpawn = island;
                island.GetComponent<IslandOwner>().owner = 3;
            }
            else if (island.transform.position.x == ((((mapSize - 1) * islandSize) * 0.875) / 2) && island.transform.position.z == (mapSize - 1) * islandSize)
            {
                Debug.Log("vierter");
                fourthTeamSpawn = island;
                island.GetComponent<IslandOwner>().owner = 4;
            }
        }
    }

    private void SetPivotPoint()
    {
        GameObject point = GameObject.Find("PivotPoint");
        point.transform.position = new Vector3(((islandSize * mapSize) / 2 - (islandSize * mapSize) / 8) * 1.11f, 0, ((islandSize * mapSize) / 2 - (islandSize * mapSize) / 8) * 1.01f);
        Debug.Log((islandSize * mapSize) / 4);
    }

    public void OnJoinedLobby()
    {
        Utility.roomInfo = PhotonNetwork.GetRoomList();
    }

    void calculateMapDimentions()
    {
        if (!isOdd(mapSize))
        {
            mapSize++;
        }
        if (mapSize % 4 == 1)
        {
            mapNeedsCorrection = true;
        }
        mapSideLength = (mapSize + 1) / 2;
    }

    void placeIslands(string[] islands, int yLevel, bool yRandom, float scale)
    {
        Vector3 position = new Vector3(0, 0, 0);
        bool xIsOdd = false;
        int yPosition;
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                if (yRandom)
                {
                    yPosition = UnityEngine.Random.Range(0, mapHightDifference);
                }
                else
                {
                    yPosition = 0;
                }
                if (!isOdd(x))
                {
                    xIsOdd = false;
                    position = new Vector3((islandSize - (islandSize / 8)) * x, yPosition - yLevel, (islandSize * z) - (islandSize / 2));
                }
                else
                {
                    xIsOdd = true;
                    position = new Vector3((islandSize - (islandSize / 8)) * x, yPosition - yLevel, (islandSize * z));
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
                    if (xIsOdd && mapNeedsCorrection)
                    {
                        zSpawnLineMaximum += 1;
                    }
                    if (mapNeedsCorrection)
                    {
                        zSpawnLineMinimum -= 1;
                    }
                    if (z > zSpawnLineMinimum && z < mapSize - zSpawnLineMaximum)
                    {
                        if (yRandom)
                        {
                            placeIsland(position, islands);
                        }
                        else
                        {
                            GameObject cube = PhotonNetwork.Instantiate("Hexagon_Sand", new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
                            cube.transform.parent = GameObject.Find("Minimap").transform;
                            cube.transform.position = new Vector3(position.x, 0, position.z) * scale;
                            cube.transform.localScale = new Vector3(islandSize / 30 * scale, 0.1f * scale, islandSize / 30 * scale);
                            minimapIslands.Add(cube);
                        }
                    }
                }

                else if (x == (mapSize - 1) / 2)
                {
                    if (yRandom)
                    {
                        placeIsland(position, islands);
                    }
                    else
                    {
                        GameObject cube = PhotonNetwork.Instantiate("Hexagon_Sand", new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
                        cube.transform.parent = GameObject.Find("Minimap").transform;
                        cube.transform.position = new Vector3(position.x, 0, position.z) * scale;
                        cube.transform.localScale = new Vector3(islandSize / 30 * scale, 0.1f * scale, islandSize / 30 * scale);
                        minimapIslands.Add(cube);
                    }
                }

                else if (x >= (mapSize / 2) - 1)
                {
                    int toSpawnCountInLine = mapSideLength + (mapSize - x);
                    int zSpawnLineMinimum = (int)Mathf.Round((mapSize / 2) - (toSpawnCountInLine / 2));
                    int zSpawnLineMaximum = zSpawnLineMinimum;
                    if (xIsOdd)
                    {
                        zSpawnLineMaximum += 1;
                    }
                    if (xIsOdd && mapNeedsCorrection)
                    {
                        zSpawnLineMinimum -= 1;
                    }
                    if (!xIsOdd && mapNeedsCorrection)
                    {
                        zSpawnLineMaximum += 1;
                    }
                    if (z > zSpawnLineMinimum && z < mapSize - zSpawnLineMaximum)
                    {
                        if (yRandom)
                        {
                            placeIsland(position, islands);
                        }
                        else
                        {
                            GameObject cube = PhotonNetwork.Instantiate("Hexagon_Sand", new Vector3(0, 0, 0), Quaternion.Euler(0.0f, 0.0f, 0.0f), 0);
                            cube.transform.parent = GameObject.Find("Minimap").transform;
                            cube.transform.position = new Vector3(position.x, 0, position.z) * scale;
                            cube.transform.localScale = new Vector3(islandSize / 30 * scale, 0.1f * scale, islandSize / 30 * scale);
                            minimapIslands.Add(cube);
                        }
                    }
                }
            }
        }
        if (yRandom)
        {
            createMapArray();
            GetOtherBridgePoint();
            //GenerateBridges();
        }
    }


    void placeSmallEnvirement()
    {
        if (smallEnvirement == null)
            smallEnvirement = GameObject.FindGameObjectsWithTag("EnvSmall");

        foreach (GameObject emptyGameObject in smallEnvirement)
        {
            int random = UnityEngine.Random.Range(0, 7);
            int randomEnvirement = UnityEngine.Random.Range(0, SmallEnvirementsToPlace.Length);
            if (random != 0)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    GameObject prefab = PhotonNetwork.Instantiate(SmallEnvirementsToPlace[randomEnvirement], new Vector3(emptyGameObject.transform.position.x, emptyGameObject.transform.position.y, emptyGameObject.transform.position.z), Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f), 0);
                }
            }
        }


        foreach (GameObject smallEnv in GameObject.FindGameObjectsWithTag("EnvSmall"))
        {
            Destroy(smallEnv);
        }
    }

    void placeBigEnvirement()
    {
        if (bigEnvirement == null)
            bigEnvirement = GameObject.FindGameObjectsWithTag("EnvBig");

        List<GameObject> envirmts = new List<GameObject>();

        foreach (GameObject emptyGameObject in bigEnvirement)
        {
            int random = UnityEngine.Random.Range(0, 7);
            int randomEnvirement = UnityEngine.Random.Range(0, BigEnvirementsToPlace.Length);
            if (random != 0)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    GameObject prefab = PhotonNetwork.Instantiate(BigEnvirementsToPlace[randomEnvirement], new Vector3(emptyGameObject.transform.position.x, emptyGameObject.transform.position.y, emptyGameObject.transform.position.z), Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f), 0);
                    envirmts.Add(prefab);
                }
            }
        }

        foreach (GameObject smallEnv in GameObject.FindGameObjectsWithTag("EnvBig"))
        {
            Destroy(smallEnv);
        }
    }

    void placeIsland(Vector3 position, string[] islands)
    {
        int random = UnityEngine.Random.Range(0, islands.Length);
        int rotation = UnityEngine.Random.Range(0, 5);
        PhotonNetwork.Instantiate(islands[random], position, Quaternion.Euler(0.0f, rotation * 60, 0.0f), 0);
    }

    void fillMapList()
    {
        GameObject[] islands = GameObject.FindGameObjectsWithTag("Env");
        mapIslands.AddRange(islands);
    }

    bool isOdd(int value)
    {
        return value % 2 != 0;
    }

    void renameBridgePoints(List<GameObject> points, GameObject island)
    {
        int sw = (int)island.transform.eulerAngles.y / 60;

        switch (sw)
        {
            case 1:
                foreach (var point in points)
                {
                    WayPoint wp = point.GetComponent<WayPoint>();

                    switch (wp.bridgeNumber)
                    {
                        case 1:
                            wp.bridgeNumber++;
                            point.name = "BridgePoint2";
                            break;
                        case 2:
                            wp.bridgeNumber++;
                            point.name = "BridgePoint3";
                            break;
                        case 3:
                            wp.bridgeNumber++;
                            point.name = "BridgePoint4";
                            break;
                        case 4:
                            wp.bridgeNumber++;
                            point.name = "BridgePoint5";
                            break;
                        case 5:
                            wp.bridgeNumber++;
                            point.name = "BridgePoint6";
                            break;
                        case 6:
                            wp.bridgeNumber = 1;
                            point.name = "BridgePoint1";
                            break;
                    }
                }
                break;
            case 2:
                foreach (var point in points)
                {
                    WayPoint wp = point.GetComponent<WayPoint>();

                    switch (wp.bridgeNumber)
                    {
                        case 1:
                            wp.bridgeNumber = 3;
                            point.name = "BridgePoint3";
                            break;
                        case 2:
                            wp.bridgeNumber = 4;
                            point.name = "BridgePoint4";
                            break;
                        case 3:
                            wp.bridgeNumber = 5;
                            point.name = "BridgePoint5";
                            break;
                        case 4:
                            wp.bridgeNumber = 6;
                            point.name = "BridgePoint6";
                            break;
                        case 5:
                            wp.bridgeNumber = 1;
                            point.name = "BridgePoint1";
                            break;
                        case 6:
                            wp.bridgeNumber = 2;
                            point.name = "BridgePoint2";
                            break;
                    }
                }
                break;
            case 3:
                foreach (var point in points)
                {
                    WayPoint wp = point.GetComponent<WayPoint>();

                    switch (wp.bridgeNumber)
                    {
                        case 1:
                            wp.bridgeNumber = 4;
                            point.name = "BridgePoint4";
                            break;
                        case 2:
                            wp.bridgeNumber = 5;
                            point.name = "BridgePoint5";
                            break;
                        case 3:
                            wp.bridgeNumber = 6;
                            point.name = "BridgePoint6";
                            break;
                        case 4:
                            wp.bridgeNumber = 1;
                            point.name = "BridgePoint1";
                            break;
                        case 5:
                            wp.bridgeNumber = 2;
                            point.name = "BridgePoint2";
                            break;
                        case 6:
                            wp.bridgeNumber = 3;
                            point.name = "BridgePoint3";
                            break;
                    }
                }
                break;
            case 4:
                foreach (var point in points)
                {
                    WayPoint wp = point.GetComponent<WayPoint>();

                    switch (wp.bridgeNumber)
                    {
                        case 1:
                            wp.bridgeNumber = 5;
                            point.name = "BridgePoint5";
                            break;
                        case 2:
                            wp.bridgeNumber = 6;
                            point.name = "BridgePoint6";
                            break;
                        case 3:
                            wp.bridgeNumber = 1;
                            point.name = "BridgePoint1";
                            break;
                        case 4:
                            wp.bridgeNumber = 2;
                            point.name = "BridgePoint2";
                            break;
                        case 5:
                            wp.bridgeNumber = 3;
                            point.name = "BridgePoint3";
                            break;
                        case 6:
                            wp.bridgeNumber = 4;
                            point.name = "BridgePoint4";
                            break;
                    }
                }
                break;
            case 5:
                foreach (var point in points)
                {
                    WayPoint wp = point.GetComponent<WayPoint>();

                    switch (wp.bridgeNumber)
                    {
                        case 1:
                            wp.bridgeNumber = 6;
                            point.name = "BridgePoint6";
                            break;
                        case 2:
                            wp.bridgeNumber = 1;
                            point.name = "BridgePoint1";
                            break;
                        case 3:
                            wp.bridgeNumber = 2;
                            point.name = "BridgePoint2";
                            break;
                        case 4:
                            wp.bridgeNumber = 3;
                            point.name = "BridgePoint3";
                            break;
                        case 5:
                            wp.bridgeNumber = 4;
                            point.name = "BridgePoint4";
                            break;
                        case 6:
                            wp.bridgeNumber = 5;
                            point.name = "BridgePoint5";
                            break;
                    }
                }
                break;

            default:
                break;

        }
    }

    void createMapArray()
    {
        //map = new IslandStats[mapSize, mapSize];
        Map = new GameObject[mapSize, mapSize];

        List<GameObject> islands = new List<GameObject>();
        List<GameObject> islandList = new List<GameObject>();
        List<float> lines = new List<float>();
        List<GameObject> clearList = new List<GameObject>();
        GameObject[] islandArray = GameObject.FindGameObjectsWithTag("Env");
        GameObject toCopyIsland = islandArray[0];
        int z = 0;
        int x = 0;
        int counter;
        float firstLine;
        float secondLine;
        float copyIslandX;
        int islandCount;
        int currentMapWidth = (mapSize / 2) + 1;

        foreach (var island in islandArray)
        {
            islands.Add(island);
        }

        foreach (var island in islands)
        {
            counter = 0;
            if (lines.Count == 0)
            {
                lines.Add(island.transform.position.z);
            }

            foreach (var line in lines)
            {
                if (island.transform.position.z == line)
                {
                    break;
                }
                else
                {
                    counter++;
                }
            }

            if (counter == lines.Count)
            {
                lines.Add(island.transform.position.z);
            }
        }

        for (int i = 0; i < ((lines.Count / 2) + 1); i++)
        {
            firstLine = 2000;
            secondLine = 2000;

            foreach (var island in islands)
            {
                if (island.transform.position.z <= firstLine)
                {
                    if (island.transform.position.z == firstLine)
                    {
                        islandList.Add(island);
                    }
                    else
                    {
                        islandList.Add(island);
                        firstLine = island.transform.position.z;
                    }
                }
                else if (island.transform.position.z <= secondLine)
                {
                    if (island.transform.position.z == secondLine)
                    {
                        islandList.Add(island);
                    }
                    else
                    {
                        islandList.Add(island);
                        secondLine = island.transform.position.z;
                    }
                }
            }


            foreach (var island in islandList)
            {
                if (island.transform.position.z != firstLine && island.transform.position.z != secondLine)
                {
                    clearList.Add(island);
                }
            }

            foreach (var island in clearList)
            {
                islandList.Remove(island);
            }

            foreach (var island in islandList)
            {
                islands.Remove(island);
            }

            islandCount = islandList.Count;
            clearList.Clear();

            for (int j = 0; j < islandCount; j++)
            {
                copyIslandX = 2000;

                foreach (var island in islandList)
                {
                    if (island.transform.position.x < copyIslandX)
                    {
                        toCopyIsland = island;
                        copyIslandX = island.transform.position.x;
                    }
                }

                if (islandList.Count != (lines.Count / 2) + 1 && x == 0)
                {
                    x += (((lines.Count / 2) + 1) - islandList.Count) / 2;
                }

                Map[x, z] = toCopyIsland;
                Map[x, z].AddComponent<IslandStats>();
                Map[x, z].GetComponent<IslandStats>().GetSomeStats(toCopyIsland, x, z);
                islandList.Remove(toCopyIsland);
                x++;
            }

            z++;
            x = 0;
        }

        foreach (var island in Map)
        {
            if (island != null)
            {
                island.GetComponent<IslandStats>().GetNeighbours(Map, islandSize);
            }
        }
    }

    void getSpawnPoints()
    {
        if (respawnPoints == null)
            respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    void GetOtherBridgePoint()
    {
        IslandStats IslandStats;
        WayPoint wp;
        List<GameObject> pointList = new List<GameObject>();

        foreach (var island in Map)
        {
            if (island != null)
            {
                island.GetComponent<IslandStats>().GetBridgePoints();

                renameBridgePoints(island.GetComponent<IslandStats>().bridgePoints, island);
            }
        }

        foreach (var island in Map)
        {
            if (island != null)
            {
                IslandStats = island.GetComponent<IslandStats>();

                foreach (var point in IslandStats.bridgePoints)
                {
                    wp = point.GetComponent<WayPoint>();

                    switch (wp.bridgeNumber)
                    {
                        case 1:
                            if (!IslandStats.Odd)
                            {
                                foreach (var neighbourIsland in IslandStats.neighbours)
                                {
                                    if (neighbourIsland.GetComponent<IslandStats>().z == IslandStats.z + 1 && neighbourIsland.GetComponent<IslandStats>().x == IslandStats.x)
                                    {
                                        wp.otherBridgePoint = neighbourIsland.transform.FindChild("BridgePoint4").gameObject;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var neighbourIsland in IslandStats.neighbours)
                                {
                                    if (neighbourIsland.GetComponent<IslandStats>().z == IslandStats.z + 1 && neighbourIsland.GetComponent<IslandStats>().x == IslandStats.x)
                                    {
                                        wp.otherBridgePoint = neighbourIsland.transform.FindChild("BridgePoint4").gameObject;
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (!IslandStats.Odd)
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z + 1 && neighbour.GetComponent<IslandStats>().x == IslandStats.x + 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint5").gameObject;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z && neighbour.GetComponent<IslandStats>().x == IslandStats.x + 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint5").gameObject;
                                    }
                                }
                            }
                            break;
                        case 3:
                            if (!IslandStats.Odd)
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z && neighbour.GetComponent<IslandStats>().x == IslandStats.x + 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint6").gameObject;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z - 1 && neighbour.GetComponent<IslandStats>().x == IslandStats.x + 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint6").gameObject;
                                    }
                                }
                            }
                            break;
                        case 4:
                            if (!IslandStats.Odd)
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z - 1 && neighbour.GetComponent<IslandStats>().x == IslandStats.x)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint1").gameObject;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z - 1 && neighbour.GetComponent<IslandStats>().x == IslandStats.x)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint1").gameObject;
                                    }
                                }
                            }
                            break;
                        case 5:
                            if (!IslandStats.Odd)
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z && neighbour.GetComponent<IslandStats>().x == IslandStats.x - 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint2").gameObject;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z - 1 && neighbour.GetComponent<IslandStats>().x == IslandStats.x - 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint2").gameObject;
                                    }
                                }
                            }
                            break;
                        case 6:
                            if (!IslandStats.Odd)
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z + 1 && neighbour.GetComponent<IslandStats>().x == IslandStats.x - 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint3").gameObject;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var neighbour in IslandStats.neighbours)
                                {
                                    if (neighbour.GetComponent<IslandStats>().z == IslandStats.z && neighbour.GetComponent<IslandStats>().x == IslandStats.x - 1)
                                    {
                                        wp.otherBridgePoint = neighbour.transform.FindChild("BridgePoint3").gameObject;
                                    }
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }

    void GenerateBridges()
    {
        WayPoint wp;
        Vector3 position;
        int id;
        float scaling;
        float bridgeLenght;
        float rotation;

        foreach (var island in Map)
        {
            if (island != null)
            {
                foreach (var point in island.GetComponent<IslandStats>().bridgePoints)
                {
                    wp = point.GetComponent<WayPoint>();
                    //JustForDebugging
                    IslandStats IS = island.GetComponent<IslandStats>();

                    if (wp.otherBridgePoint != null && !wp.bridgeSpwaned && !wp.otherBridgePoint.GetComponent<WayPoint>().bridgeSpwaned)
                    {
                        //SpawnBridge
                        bridgeLenght = GetBridgeLenght(point.transform, wp.otherBridgePoint.gameObject.transform);
                        bridgeCube.transform.localScale = new Vector3(1, 1, bridgeLenght);
                        position = GetSpawnPosition(point.transform, wp.otherBridgePoint.gameObject.transform);
                        Instantiate(bridgeCube, position, Quaternion.Euler(0, GetYAngle(point.transform, wp.otherBridgePoint.gameObject.transform), 0));
                        bridgeCube.transform.localScale = new Vector3(1, 1, 1);
                        wp.bridgeSpwaned = true;
                        wp.otherBridgePoint.GetComponent<WayPoint>().bridgeSpwaned = true;
                    }
                }
            }
        }
    }


    Vector3 GetSpawnPosition(Transform point, Transform otherPoint)
    {
        Vector3 pos = new Vector3();
        float diffX = 0;
        float diffY = 0;
        float diffZ = 0;

        if (point.position.x >= 0 && otherPoint.position.x >= 0)
        {
            if (point.position.x < otherPoint.position.x)
            {
                diffX = otherPoint.position.x - point.position.x;
            }
            else
            {
                diffX = point.position.x - otherPoint.position.x;
            }
        }
        else if (point.position.x < 0 && otherPoint.position.x >= 0)
        {
            diffX = otherPoint.position.x + Mathf.Sqrt(point.position.x * 2);
        }
        else if (point.position.x < 0 && otherPoint.position.x < 0)
        {
            if (point.position.x < otherPoint.position.x)
            {
                diffX = otherPoint.position.x - point.position.x;
            }
            else
            {
                diffX = point.position.x - otherPoint.position.x;
            }
        }
        else if (point.position.x >= 0 && otherPoint.position.x < 0)
        {
            diffX = point.position.x + Mathf.Sqrt(otherPoint.position.x * 2);
        }

        if (point.position.x > otherPoint.position.x)
        {
            pos.x = otherPoint.position.x + (diffX / 2);
        }
        else
        {
            pos.x = point.position.x + (diffX / 2);
        }

        if (point.position.y >= 0 && otherPoint.position.y >= 0)
        {
            if (point.position.y < otherPoint.position.y)
            {
                diffY = otherPoint.position.y - point.position.y;
            }
            else
            {
                diffY = point.position.y - otherPoint.position.y;
            }
        }
        else if (point.position.y < 0 && otherPoint.position.y >= 0)
        {
            diffY = otherPoint.position.y + Mathf.Sqrt(point.position.y * 2);
        }
        else if (point.position.y < 0 && otherPoint.position.y < 0)
        {
            if (point.position.y < otherPoint.position.y)
            {
                diffY = otherPoint.position.y - point.position.y;
            }
            else
            {
                diffY = point.position.y - otherPoint.position.y;
            }
        }
        else if (point.position.y >= 0 && otherPoint.position.y < 0)
        {
            diffY = point.position.y + Mathf.Sqrt(otherPoint.position.y * 2);
        }

        if (point.position.y > otherPoint.position.y)
        {
            pos.y = otherPoint.position.y + (diffY / 2);
        }
        else
        {
            pos.y = point.position.y + (diffY / 2);
        }

        if (point.position.z >= 0 && otherPoint.position.z >= 0)
        {
            if (point.position.z < otherPoint.position.z)
            {
                diffZ = otherPoint.position.z - point.position.z;
            }
            else
            {
                diffZ = point.position.z - otherPoint.position.z;
            }
        }
        else if (point.position.z < 0 && otherPoint.position.z >= 0)
        {
            diffZ = otherPoint.position.z + Mathf.Sqrt(point.position.z * 2);
        }
        else if (point.position.z < 0 && otherPoint.position.z < 0)
        {
            if (point.position.z < otherPoint.position.z)
            {
                diffZ = otherPoint.position.z - point.position.z;
            }
            else
            {
                diffZ = point.position.z - otherPoint.position.z;
            }
        }
        else if (point.position.z >= 0 && otherPoint.position.z < 0)
        {
            diffZ = point.position.z + Mathf.Sqrt(otherPoint.position.z * 2);
        }

        if (point.position.z > otherPoint.position.z)
        {
            pos.z = otherPoint.position.z + (diffZ / 2);
        }
        else
        {
            pos.z = point.position.z + (diffZ / 2);
        }

        return pos;
    }

    float GetBridgeLenght(Transform point, Transform otherPoint)
    {
        return Vector3.Distance(point.position, otherPoint.position);
    }

    float GetYAngle(Transform point, Transform otherPoint)
    {
        Vector3 thirdPoint = new Vector3(point.position.x, 0, point.position.z + 5);
        float angleAlpha;

        if (otherPoint.position.x > point.position.x)
        {
            angleAlpha = Vector3.Angle(thirdPoint - point.position, otherPoint.position - point.position);
            return angleAlpha;
        }
        else if (otherPoint.position.x < point.position.x)
        {
            angleAlpha = Vector3.Angle(thirdPoint - point.position, otherPoint.position - point.position);
            return 360 - angleAlpha;

        }
        else if (otherPoint.position.x == point.position.x && otherPoint.position.z > point.position.z)
        {
            return 0;
        }
        else if (otherPoint.position.x > point.position.x && otherPoint.position.z == point.position.z)
        {
            return 90;
        }
        else if (otherPoint.position.x == point.position.x && otherPoint.position.z < point.position.z)
        {
            return 180;
        }
        else if (otherPoint.position.x < point.position.x && otherPoint.position.z == point.position.z)
        {
            return 270;
        }
        return 0;
    }

    float GetXAngle(Transform point, Transform otherPoint)
    {
        float angle;

        Vector3 thirdPoint = new Vector3(point.position.x, otherPoint.position.y, point.position.z);

        angle = Vector3.Angle(thirdPoint - point.position, otherPoint.position - point.position);

        return angle;
    }

    public void RespawnPlayer(GameObject Player, int Index)
    {
        GameObject island = mapIslands[Index];
        if (island.GetComponent<IslandOwner>().respawnTickets > 0)
        {
            Player.transform.position = island.transform.Find("Respawn").transform.position;
            Player.GetComponent<CharacterStats>().Life = 100;
            island.GetComponent<IslandOwner>().respawnTickets--;
            return;
        }
     
    }


    public void SpawnPlayer(GameObject Player)
    {
        int team = Player.GetComponent<CharacterStats>().team;
        if (team == 1)
            Player.transform.position = firstTeamSpawn.transform.Find("Respawn").transform.position;
        else if (team == 2)
            Player.transform.position = seccTeamSpawn.transform.Find("Respawn").transform.position;
        else if (team == 3)
            Player.transform.position = thirdTeamSpawn.transform.Find("Respawn").transform.position;
        else if (team == 4)
            Player.transform.position = fourthTeamSpawn.transform.Find("Respawn").transform.position;
    }
}
