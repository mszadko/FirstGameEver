using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BoardManager : MonoBehaviour
{

    public int Size;
    public GameObject[] floorPrefabs;
    public GameObject[] wallPrefabs;
    public GameObject[] pickablePrefabs;
    public GameObject[] obstaclePrefabs;
    private bool[,] PlaceWithObstacle;//when we spawn pickables we dont want to spawn it on obstacle;
    private int pickableSpawnDelay = 15;
    private Transform board;
    GameObject obstacles;


    void CreateFloor()
    {
        System.Random r = new System.Random();
        int indexOfaPrefab;
        //columns are x and rows are y
        for (int col = 1; col < Size; col++)//on 0 and size i will spawn walls
        {
            for (int row = 1; row < Size; row++)
            {
                indexOfaPrefab = r.Next(0, floorPrefabs.Length);
                Instantiate(floorPrefabs[indexOfaPrefab], new Vector3(col, row), Quaternion.identity, board.transform);
            }
        }
    }
    void CreateWalls()
    {
        for (int col = 1; col < Size; col++)//spawn lower and upper horizontal wall;
        {
            Instantiate(wallPrefabs[6], new Vector3(col, 0), Quaternion.identity, board.transform);//lower
            Instantiate(wallPrefabs[1], new Vector3(col, Size), Quaternion.identity, board.transform);//upper
        }
        for (int row = 1; row < Size; row++)//spawn left and right vertical wall;
        {
            Instantiate(wallPrefabs[7], new Vector3(0, row), Quaternion.identity, board.transform);//left
            Instantiate(wallPrefabs[3], new Vector3(Size, row), Quaternion.identity, board.transform);//right
        }
        //now time for corners
        Instantiate(wallPrefabs[5], new Vector3(0, 0), Quaternion.identity, board.transform);//down left
        Instantiate(wallPrefabs[4], new Vector3(Size, 0), Quaternion.identity, board.transform);//down right
        Instantiate(wallPrefabs[0], new Vector3(0, Size), Quaternion.identity, board.transform);//upper left
        Instantiate(wallPrefabs[2], new Vector3(Size, Size), Quaternion.identity, board.transform);//upper right
    }
    void CreateObstacles()
    {
        obstacles = new GameObject("obstacle");
        int spawnAttempts = (int)(System.Math.Pow(Size / 2, 2) * 2 / 3);
        PlaceWithObstacle = new bool[Size, Size];
        System.Random r = new System.Random();
        int indexOfAnObstacle, col, row;
        for (int i = 0; i < spawnAttempts; i++)
        {
            col = r.Next(2, Size - 1);//we dont want to spawn it on a wall and we want to leave free alley
            row = r.Next(2, Size - 1);
            if (!PlaceWithObstacle[col, row]) //if there is not an obstacle there we can instantiate it
            {
                indexOfAnObstacle = r.Next(0, obstaclePrefabs.Length);
                Instantiate(obstaclePrefabs[indexOfAnObstacle], new Vector3(col, row), Quaternion.identity, obstacles.transform);
                PlaceWithObstacle[col, row] = true;
            }
        }
    }
    void SpawnPickable()
    {
        bool ammoSpawned = false;
        System.Random r = new System.Random();
        int row, col, choice;
        while (!ammoSpawned)
        {
            row = r.Next(2, Size - 2);
            col = r.Next(2, Size - 2);
            choice = r.Next(0, pickablePrefabs.Length);
            if (!PlaceWithObstacle[col, row])
            {
                Instantiate(pickablePrefabs[choice], new Vector3(col, row), Quaternion.identity, obstacles.transform);
                ammoSpawned = true;
            }
        }
    }
    public void SetGameArea()
    {
        board = new GameObject("board").transform;
        CreateWalls();
        CreateFloor();
        CreateObstacles();
       
        InvokeRepeating("SpawnPickable", pickableSpawnDelay, pickableSpawnDelay);
    }
    public List<Vector3> ReturnSpawnPositions(int n)
    {

        List<Vector3> spawnpoints = new List<Vector3>();
        spawnpoints.Add(new Vector3(1, 1));
        spawnpoints.Add(new Vector3(Size - 1, Size - 1));

        if (n >= 3)
        {
            spawnpoints.Add(new Vector3(1, Size - 1));
        }
        if (n == 4)
        {
            spawnpoints.Add(new Vector3(Size - 1, 1));
        }
        return spawnpoints;
    }
    public List<int> ReturnSpawnRotations(int n)
    {
        List<int> spawnrotations = new List<int>();
        spawnrotations.Add(-45);
        spawnrotations.Add(135);

        if (n >= 3)
        {
            spawnrotations.Add(-135);
        }
        if (n == 4)
        {
            spawnrotations.Add(45);
        }
        return spawnrotations;
    }

    public void DeleteObstacles()
    {
        DestroyImmediate(obstacles);
    }
    public void ResetMap()
    {
        DeleteObstacles();
        CreateObstacles();
    }
    
}
