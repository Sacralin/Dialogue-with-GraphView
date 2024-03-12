using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class MapGeneration : MonoBehaviour
{
    // map spawn position settings
    static float currentSpawnPositionX = 0;
    static float currentSpawnPositionY = 0;
    static float currentSpawnPositionZ = 0;
    public float spawnPositionX = 0;
    public float spawnPositionY = 0;
    public float spawnPositionZ = 0;

    // map size settings
    static int currentMapX = 25; //define map x
    static int currentMapY = 25; //define map y
    public int mapX = 25;
    public int mapY = 25;

    // scale settings
    static Vector3 currentScale = new Vector3(1,1,1);
    public Vector3 scale = new Vector3(1,1,1);

    // set number of path start points
    //public int pathsXMin = 0;
    //public int pathsXMax = 0;
    //public int pathsYMin = 0;
    //public int pathsYMax = 0;

    // tile weight and prefab settings
    public GameObject grassCubePrefab;
    static int currentGrassCubeWeight = 1;
    public int grassCubeWeight = 1;
    public GameObject dirtCubePrefab;
    static int currentDirtCubeWeight = 1;
    public int dirtCubeWeight = 1;
    public GameObject pathCubePrefab;
    static int currentPathCubeWeight = 1;
    public int pathCubeWeight = 1;

    // weights and data structures
    static int weightsTotal = 0;
    Dictionary<GameObject, int> weights;
    static GameObject[,] tileMap;

    // navmesh baking
    public Transform tileParent;
    private NavMeshSurface surface;



    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        weights = new Dictionary<GameObject, int> // stores prefabs and associated weights 
        {
            { grassCubePrefab, currentGrassCubeWeight },
            { dirtCubePrefab, currentDirtCubeWeight },
            { pathCubePrefab, currentPathCubeWeight }
        };

        tileMap = new GameObject[currentMapX, currentMapY]; 

        GenerateMap();
        
    }

    // Update is called once per frame
    void Update() // check if any input has been entered
    {
        // check if weights are altered
        if(dirtCubeWeight != currentDirtCubeWeight || grassCubeWeight != currentGrassCubeWeight || pathCubeWeight != currentPathCubeWeight)
        {
            currentDirtCubeWeight = dirtCubeWeight;
            currentGrassCubeWeight = grassCubeWeight;
            currentPathCubeWeight = pathCubeWeight;
            weights[grassCubePrefab] = currentGrassCubeWeight;
            weights[dirtCubePrefab] = currentDirtCubeWeight;
            weights[pathCubePrefab] = currentPathCubeWeight;
            GenerateMap();
        }
        // check if map size is altered
        if(mapX != currentMapX || mapY != currentMapY)
        {
            if (mapX < 3) mapX = 3;
            if (mapY < 3) mapY = 3;
            currentMapX = mapX;
            currentMapY = mapY;
            
            tileMap = new GameObject[currentMapX, currentMapY];
            GenerateMap();
        }
        //check is cube scale is altered
        if(scale != currentScale)
        {
            currentScale = scale;
            foreach (KeyValuePair<GameObject, int> kvp in weights) 
            {
                kvp.Key.transform.localScale = currentScale;
            }
            GenerateMap();

        }
        //check if spawn position is altered
        if(currentSpawnPositionX != spawnPositionX || currentSpawnPositionY != spawnPositionY || currentSpawnPositionZ != spawnPositionZ)
        {
            currentSpawnPositionX = spawnPositionX;
            currentSpawnPositionY = spawnPositionY;
            currentSpawnPositionZ = spawnPositionZ;

            GenerateMap();
        }

    }

    private void GenerateMap() // generates the map randomly and stores the generated map in the tileMap array.
    {
        DestroyTiles(); // removes all tiles in preperation of creating a new random map
        
        weightsTotal = 0;
        foreach (KeyValuePair<GameObject, int> kvp in weights)  
        {
            weightsTotal += kvp.Value; //calculates the total weight values from the dictionary
        }

        for (int i = 0; i < tileMap.GetLength(0); i++)
        {
            for (int j = 0; j < tileMap.GetLength(1); j++) 
            {
                System.Random random = new System.Random();
                int randomSelector = random.Next(0, weightsTotal);
                int getWeight = 0;
                int checkTileChance = 0;
                foreach (KeyValuePair<GameObject, int> kvp in weights)
                {
                    getWeight += kvp.Value;
                    checkTileChance = weightsTotal - getWeight;
                    if (randomSelector >= checkTileChance)
                    {
                        Vector3 position = new Vector3(spawnPositionX + currentScale.x * i, spawnPositionY + currentScale.y * 0, spawnPositionZ + currentScale.z * j);
                        GameObject tile = Instantiate(kvp.Key, position, Quaternion.identity);
                        tile.transform.SetParent(tileParent);
                        tileMap[i, j] = kvp.Key;
                        break;
                    }
                }
            }
        }

        
        surface.BuildNavMesh();
        

    }

    public void DestroyTiles() // removes all game tiles from the scene
    {
        GameObject[] mapCubes = GameObject.FindGameObjectsWithTag("MapCube");
        foreach(GameObject go in mapCubes)
        {
            Destroy(go);
        }

    }

    public void ConnectPaths()
    {
        for (int i = 0; i < tileMap.GetLength(0); i++)
        {

        }
    }

}
