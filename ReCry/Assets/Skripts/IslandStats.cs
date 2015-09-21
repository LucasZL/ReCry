using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandStats : MonoBehaviour 
{

    int x, y, z;
    int islandWidth, islandLenght;
    int toBeFilled;
    List<IslandStats> neighbours;
    WayPoint[,] wayPointMesh;
    WayPoint wayPoint;
    GameObject island;

    public IslandStats(GameObject Island)
    {
        this.island = Island;
    }

	void Start () 
    {
        neighbours = new List<IslandStats>();
	}
	
	void Update () 
    {
	
	}

    public void GetPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void GetStats(int lenght, int width)
    {

    }

    void GetNeighbours(IslandStats[,] map)
    {
        if (map[x - 1, z - 1] != null)
            neighbours.Add(map[x - 1, z - 1]);
        if (map[x - 1, z] != null)
            neighbours.Add(map[x - 1, z]);
        if (map[x + 1, z - 1] != null)
            neighbours.Add(map[x + 1, z - 1]);
        if (map[x + 1, z] != null)
            neighbours.Add(map[x + 1, z]);
        if (map[x, z + 1] != null)
            neighbours.Add(map[x, z + 1]);
        if (map[x, z - 1] != null)
            neighbours.Add(map[x, z - 1]);
    }

    void GetWayPointMesh()
    {
        wayPointMesh = new WayPoint[islandWidth - 1, islandLenght - 1];

        for (int x = 0; x < islandWidth; x++)
        {
            for (int z = 0; z < islandLenght; z++)
            {
                //wayPointMesh[x, z] = GameObject.Instantiate(WayPoint , new Vector3(x - islandWidth / 2, toBeFilled, z - islandLenght / 2), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
                wayPointMesh[x, z] = new WayPoint();
            }
        }
    }
}
