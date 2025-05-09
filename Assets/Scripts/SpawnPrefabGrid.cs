using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPrefabGrid : MonoBehaviour
{
     public int initialbaseRoom1Count = 1;
    public int initialbaseRoom2Count = 1;
    public int initialbaseRoom3Count = 0;
    public int initiallootRoomCount = 0;
    public int initialbarracksRoomCount = 0;
    public int initialarmoryRoomCount = 1;
    public Transform shopkeeperTransform;
    public Transform dungeonTransform;
    public Transform lossDungeonTransform;
    public Transform bossDungeonTransform;
    
    public RuntimeNavMeshBaker runtimeNavMeshBaker;
    public enum TileType { Empty, Room, Hallway, Door, Wall }
    private TileType[,] gridMap;
    private GameObject[,] spawnedTiles;

    public GameObject[] GridPrefabs = new GameObject[4]; // 0 = Room, 1 = Hallway, 2 = Door
    private List<Vector2Int> roomCenters = new List<Vector2Int>();
    private List<RoomData> roomPrefabs = new List<RoomData>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    List<GameObject> spawnedWalls = new List<GameObject>();
    List<GameObject> spawnedDoors = new List<GameObject>();
    List<GameObject> spawnedHalls = new List<GameObject>();
    public GameObject waypointPrefab;
    public GameObject lootPrefab;

    public GameObject enemyMelee;
    public GameObject enemyRanged;
    public GameObject enemyMage;

    public int baseRoom1Count = 3;
    public int baseRoom2Count = 2;
    public int baseRoom3Count = 1;
    public int lootRoomCount = 1;
    public int barracksRoomCount = 1;
    public int armoryRoomCount = 1;

    

    public enum RoomSize { BaseRoom1, BaseRoom2, BaseRoom3, LootRoom, ArmoryRoom, BarracksRoom, None }

    public class RoomData
    {
        public RoomSize size;
        public int width, height;
        public int x, y; // Bottom-left corner
        public List<GameObject> enemyPrefabs = new List<GameObject>();
        public List<GameObject> lootPrefabs = new List<GameObject>();
        public List<Vector3> enemyOffsets = new List<Vector3>();
        public List<Vector3> lootOffsets = new List<Vector3>();
        public List<Transform> roomWaypoints = new List<Transform>();
        
        

        public RoomData(RoomSize size, int x, int y)
        {
            this.size = size;
            this.x = x;
            this.y = y;
            switch (size)
            {
                case RoomSize.BaseRoom1: width = 3; height = 5; break;
                case RoomSize.BaseRoom2: width =  height = 5; break;
                case RoomSize.BaseRoom3: width = height = 7; break;
                case RoomSize.LootRoom: width = 5; height = 9; break;
                case RoomSize.ArmoryRoom: width = 7; height = 5; break;
                case RoomSize.BarracksRoom: width = 11; height = 7; break;
            }
        }

        public RoomData Clone()
        {
            RoomData clone = new RoomData(this.size, this.x, this.y)
            {
                width = this.width,
                height = this.height,
                enemyPrefabs = new List<GameObject>(this.enemyPrefabs),
                lootPrefabs = new List<GameObject>(this.lootPrefabs),
                enemyOffsets = new List<Vector3>(this.enemyOffsets),
                lootOffsets = new List<Vector3>(this.lootOffsets),
                roomWaypoints = new List<Transform>(this.roomWaypoints)
            };
            return clone;
        }

        public Vector2Int Center => new Vector2Int(x + width / 2, y + height / 2);
    }

    public float GridX = 30f;
    public float GridZ = 30f;
    public float gridSpacingOffset = 1f;
    public Vector3 GridOrigin = Vector3.zero;

    void Start()
    {
        
        // Vector3 testSpawn = new Vector3(38, 0, 38); // somewhere known
        // Instantiate(enemyMelee, testSpawn, Quaternion.identity);
        // Debug.Log("Forced enemy spawn at (0,5,0)");

        gridMap = new TileType[(int)GridX, (int)GridZ];
        spawnedTiles = new GameObject[(int)GridX, (int)GridZ];
        SpawnGrid();
    }
    private bool canCheckEnemies = false;
    private bool canSpawnDungeon = false;
    private bool hasStartedCoroutine = false;

void Update()
{
    if (!hasStartedCoroutine)
    {
        StartCoroutine(HoldTeleporter());
        hasStartedCoroutine = true;
        return;
    }

    // Check for key press before returning
    if (Input.GetKeyDown(KeyCode.Alpha0))
    {
        Debug.Log("Pressed 0");
        Debug.Log("Teleport player back");
        StartCoroutine(RespawnDungeon());
        return;
    }
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        Debug.Log("Pressed 0");
        Debug.Log("Teleport player back");
        StartCoroutine(BossTeleporter());
        return;
    }

    // Only run this coroutine if not already checking
    if (canCheckEnemies && !checkingEnemies)
    {
        StartCoroutine(RemoveDestroyedEnemies());
    }
}
private bool checkingEnemies = false;
    IEnumerator RemoveDestroyedEnemies()
{
    checkingEnemies = true;

    yield return null; // Optional frame delay

    spawnedEnemies.RemoveAll(e => e == null);

    if (spawnedEnemies.Count <= 0)
    {
        Debug.Log("Teleport player");
        TeleportPlayer();
        canCheckEnemies = false;
    }

    checkingEnemies = false;
}
    IEnumerator BossTeleporter()
    {
        yield return new WaitForSeconds(2f);
        TeleportPlayerToBoss();
    }
    void TeleportPlayerToBoss() //boss
    {
        if (PlayerController.Instance != null)
        {
            CharacterController controller = PlayerController.Instance.GetComponent<CharacterController>();
            //CharacterInput controller2 = PlayerController.Instance.GetComponent<CharacterInput>();

            if (controller != null)
            {
                controller.enabled = false;
                //controller2.enabled = false;
                PlayerController.Instance.transform.position = bossDungeonTransform.position;
                controller.enabled = true;
            }
            else
            {
                PlayerController.Instance.transform.position = bossDungeonTransform.position;
            }

            Debug.Log("Player teleported to " + bossDungeonTransform.position);
        }
        else
        {
            Debug.LogWarning("PlayerController.Instance is null");
        }
    }
    
    void TeleportPlayer() //shop
    {
        if (PlayerController.Instance != null)
        {
            CharacterController controller = PlayerController.Instance.GetComponent<CharacterController>();
            //CharacterInput controller2 = PlayerController.Instance.GetComponent<CharacterInput>();

            if (controller != null)
            {
                controller.enabled = false;
                //controller2.enabled = false;
                PlayerController.Instance.transform.position = shopkeeperTransform.position;
                controller.enabled = true;
            }
            else
            {
                PlayerController.Instance.transform.position = shopkeeperTransform.position;
            }

            Debug.Log("Player teleported to " + shopkeeperTransform.position);
        }
        else
        {
            Debug.LogWarning("PlayerController.Instance is null");
        }
    }
    void TeleportBackPlayer() //dungeon redo
    {
        if (PlayerController.Instance != null)
        {
            CharacterController controller = PlayerController.Instance.GetComponent<CharacterController>();
            //CharacterInput controller2 = PlayerController.Instance.GetComponent<CharacterInput>();

            if (controller != null)
            {
                controller.enabled = false;
                //controller2.enabled = false;
                PlayerController.Instance.transform.position = dungeonTransform.position;
                controller.enabled = true;
            }
            else
            {
                PlayerController.Instance.transform.position = dungeonTransform.position;
            }

            Debug.Log("Player teleported to " + dungeonTransform.position);
        }
        else
        {
            Debug.LogWarning("PlayerController.Instance is null");
        }
    }
IEnumerator HoldTeleporter()
{
    yield return new WaitForSeconds(5f);
    canCheckEnemies = true;
}



IEnumerator RespawnDungeon()
{
    yield return new WaitForSeconds(3f);
    
    // Clear all lists
    roomCenters.Clear();
    roomPrefabs.Clear();
    
    // Clear existing tiles and grid
    for (int x = 0; x < GridX; x++)
    {
        for (int z = 0; z < GridZ; z++)
        {
            if (spawnedTiles[x, z] != null)
            {
                Destroy(spawnedTiles[x, z]);
                spawnedTiles[x, z] = null;
            }
            gridMap[x, z] = TileType.Empty;
        }
    }
    
    // Clear all spawned objects
    foreach (GameObject hall in spawnedHalls) { if (hall != null) Destroy(hall); }
    foreach (GameObject enemy in spawnedEnemies) { if (enemy != null) Destroy(enemy); }
    foreach (GameObject wall in spawnedWalls) { if (wall != null) Destroy(wall); }
    foreach (GameObject door in spawnedDoors) { if (door != null) Destroy(door); }
    
    spawnedHalls.Clear();
    spawnedEnemies.Clear();
    spawnedWalls.Clear();
    spawnedDoors.Clear();
    
    // Reset the grid
    gridMap = new TileType[(int)GridX, (int)GridZ];
    
    // Reset room counts to initial values
    // Add these variables at the class level if you haven't already
    baseRoom1Count = initialbaseRoom1Count;
    baseRoom2Count = initialbaseRoom2Count;
    baseRoom3Count = initialbaseRoom3Count;
    lootRoomCount = initiallootRoomCount;
    barracksRoomCount = initialbarracksRoomCount;
    armoryRoomCount = initialarmoryRoomCount;
    
    SpawnGrid();
    TeleportBackPlayer();
    canCheckEnemies = true;
}
    private void PlaceWallIfNeeded(int x, int z, Quaternion rotation)
    {
        if (x < 0 || x >= GridX || z < 0 || z >= GridZ)
        return;

        if (gridMap[x, z] != TileType.Empty) // place only on empty tiles
        return;

    Vector3 pos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + GridOrigin;
    Vector3 offset = Vector3.zero;

        if (rotation == Quaternion.Euler(0, 180, 0)) offset = new Vector3(0, 0, -gridSpacingOffset / 2f);
        else if (rotation == Quaternion.Euler(0, 0, 0)) offset = new Vector3(0, 0, gridSpacingOffset / 2f);
        else if (rotation == Quaternion.Euler(0, -90, 0)) offset = new Vector3(-gridSpacingOffset / 2f, 0, 0);
        else if (rotation == Quaternion.Euler(0, 90, 0)) offset = new Vector3(gridSpacingOffset / 2f, 0, 0);

    Vector3 newpos = pos + offset;
    GameObject wall = Instantiate(GridPrefabs[3], newpos, rotation);
    spawnedWalls.Add(wall);
    wall.transform.parent = transform;


    }

    private void SpawnGrid()
    {
        
        GenerateMapData();

        // Spawn Room/Hallway
        for (int x = 0; x < GridX; x++)
        {
            for (int z = 0; z < GridZ; z++)
            {
                Vector3 pos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + GridOrigin;
                GameObject go = null;

                switch (gridMap[x, z])
                {
                    case TileType.Empty: break;
                    case TileType.Room: go = Instantiate(GridPrefabs[0], pos, Quaternion.identity); 
                    gridMap[x, z] = TileType.Room;
                    go.layer = LayerMask.NameToLayer("Walkable");break;
                    case TileType.Hallway: go = Instantiate(GridPrefabs[1], pos, Quaternion.identity); 
                    gridMap[x, z] = TileType.Hallway;
                    go.layer = LayerMask.NameToLayer("Terrain");break; // different layer to prevent wall clipping. 
                    case TileType.Door: go = Instantiate(GridPrefabs[2], pos, Quaternion.identity); 
                    gridMap[x, z] = TileType.Door;
                    go.layer = LayerMask.NameToLayer("Walkable");break;
                }

                if (go != null) spawnedTiles[x, z] = go;

            }
        }

    

        foreach (RoomData room in roomPrefabs)
        {
            Debug.Log($"[EnemyGenerator] Room at ({room.x}, {room.y}) has {room.enemyPrefabs.Count} enemies.");

            List<Vector2Int> edgeCenters = new List<Vector2Int>
            {
                new Vector2Int(room.x + room.width / 2, room.y),                         // bottom edge
                new Vector2Int(room.x + room.width / 2, room.y + room.height - 1),      // top edge
                new Vector2Int(room.x, room.y + room.height / 2),                       // left edge
                new Vector2Int(room.x + room.width - 1, room.y + room.height / 2)       // right edge
            };

           
            
            foreach (var edge in edgeCenters)
            {
                int x = edge.x;
                int z = edge.y;

                if (gridMap[x, z] != TileType.Room) continue;
                if (!IsAdjacentToHallway(x, z)) continue;

                Destroy(spawnedTiles[x, z]);
                Vector3 pos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + GridOrigin;


                Quaternion rotation = Quaternion.identity;
                Vector3 offset = Vector3.zero;

                if (IsHallway(x + 1, z)) rotation = Quaternion.Euler(0, 90, 0); // right
                else if (IsHallway(x - 1, z)) rotation = Quaternion.Euler(0, -90, 0); // left
                else if (IsHallway(x, z + 1)) rotation = Quaternion.Euler(0, 0, 0); // up
                else if (IsHallway(x, z - 1)) rotation = Quaternion.Euler(0, 180, 0); // down

                // Offset to make doors align properly
                if (rotation == Quaternion.Euler(0, 180, 0)) offset = new Vector3(0, 5, -gridSpacingOffset / 2f);
                else if (rotation == Quaternion.Euler(0, 0, 0)) offset = new Vector3(0, 5, gridSpacingOffset / 2f);
                else if (rotation == Quaternion.Euler(0, -90, 0)) offset = new Vector3(-gridSpacingOffset / 2f, 5, 0);
                else if (rotation == Quaternion.Euler(0, 90, 0)) offset = new Vector3(gridSpacingOffset / 2f, 5, 0);

                Vector3 newpos = pos + offset;

                GameObject door = Instantiate(GridPrefabs[4], newpos, rotation);
                spawnedDoors.Add(door);
                gridMap[x, z] = TileType.Door;
                spawnedTiles[x, z] = door;

                GameObject doorTile = Instantiate(GridPrefabs[2], pos, rotation);
                spawnedTiles[x, z] = doorTile;
                // gridMap[x, z] = TileType.Door;

            }

            
             // Top and bottom edges
            for (int x = room.x; x < room.x + room.width; x++)
            {
                PlaceWallIfNeeded(x, room.y - 1, Quaternion.Euler(0, 0, 0)); // bottom
                PlaceWallIfNeeded(x, room.y + room.height, Quaternion.Euler(0, 180, 0)); // top
            }

            // Left and right edges
            for (int z = room.y; z < room.y + room.height; z++)
            {
                PlaceWallIfNeeded(room.x - 1, z, Quaternion.Euler(0, 90, 0)); // left
                PlaceWallIfNeeded(room.x + room.width, z, Quaternion.Euler(0, -90, 0)); // right
            }
            
        }
        for (int x = 0; x < GridX; x++)
        {
            for (int z = 0; z < GridZ; z++)
            {
                if (gridMap[x, z] != TileType.Hallway)
                    continue;

                // Check all 8 directions around the tile
                PlaceWallIfNeeded(x + 1, z, Quaternion.Euler(0, -90, 0)); // right
                PlaceWallIfNeeded(x - 1, z, Quaternion.Euler(0, 90, 0));  // left
                PlaceWallIfNeeded(x, z + 1, Quaternion.Euler(0, 180, 0)); // up
                PlaceWallIfNeeded(x, z - 1, Quaternion.Euler(0, 0, 0));   // down

  
            }
        }

                // Second pass: Add walls between Room and Hallway tiles
        for (int x = 0; x < GridX; x++)
        {
            for (int z = 0; z < GridZ; z++)
            {
                TileType current = gridMap[x, z];

                // Check right neighbor
                if (x + 1 < GridX)
                {
                    TileType right = gridMap[x + 1, z];
                    if (AreRoomAndHallway(current, right))
                        PlaceWallBetween(x, z, x + 1, z);
                }

                // Check top neighbor
                if (z + 1 < GridZ)
                {
                    TileType top = gridMap[x, z + 1];
                    if (AreRoomAndHallway(current, top))
                        PlaceWallBetween(x, z, x, z + 1);
                }
            }
        }

        
    }

    bool IsHallway(int x, int z)
    {
        if (x < 0 || x >= gridMap.GetLength(0) || z < 0 || z >= gridMap.GetLength(1)) return false;
        return gridMap[x, z] == TileType.Hallway;
    }
    
    IEnumerator LootGenerator()
    {
        yield return new WaitForSeconds(2f);

        foreach (RoomData room in roomPrefabs)
            {
                Vector3 basePos = new Vector3(room.x * gridSpacingOffset, 0, room.y * gridSpacingOffset) + GridOrigin;
                for (int i = 0; i < room.lootPrefabs.Count; i++)
                {
                    Vector3 spawnPos = basePos + room.lootOffsets[i];
                    GameObject enemy = Instantiate(room.lootPrefabs[i], spawnPos, Quaternion.identity);
                    //spawnedLoot.Add(enemy);
                    enemy.transform.parent = transform;

                }
            }
    }
    IEnumerator EnemyGenerator()
    {
        // Wait for NavMesh baking to complete
     yield return new WaitUntil(() => runtimeNavMeshBaker.BakeComplete());

        foreach (RoomData room in roomPrefabs)
            {
                Vector3 basePos = new Vector3(room.x * gridSpacingOffset, 0, room.y * gridSpacingOffset) + GridOrigin;
                for (int i = 0; i < room.enemyPrefabs.Count; i++)
                {
                    Vector3 spawnPos = basePos + room.enemyOffsets[i];
                    GameObject enemy = Instantiate(room.enemyPrefabs[i], spawnPos, Quaternion.identity);
                    spawnedEnemies.Add(enemy);
                    enemy.transform.parent = transform;

                    List<Transform> enemyWaypoints = new List<Transform>();

                    for (int w = 0; w < 4; w++) // generate waypoints around enemy spawn
                    {
                        Vector3 offset = Random.insideUnitSphere * 3f;
                        offset.y = 0;
                        Vector3 wpPos = spawnPos + offset;

                        if (NavMesh.SamplePosition(wpPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                        {
                            GameObject wp = Instantiate(waypointPrefab, hit.position, Quaternion.identity);
                            wp.name = $"Waypoint_{room.x}_{room.y}_{i}_{w}";
                            wp.transform.parent = transform;
                            enemyWaypoints.Add(wp.transform);
                        }
                    }

                    EnemyRangedAI ai = enemy.GetComponent<EnemyRangedAI>();
                    if (ai != null)
                    {
                        ai.waypoints = enemyWaypoints.ToArray();
                    }
                    EnemyMeleeAI ai2 = enemy.GetComponent<EnemyMeleeAI>();
                    if (ai2 != null)
                    {
                        ai2.waypoints = enemyWaypoints.ToArray();
                    }
                    EnemyMageAI ai3 = enemy.GetComponent<EnemyMageAI>();
                    if (ai3 != null)
                    {
                        ai3.waypoints = enemyWaypoints.ToArray();
                    }
                }
            }
    }


    private bool AreRoomAndHallway(TileType a, TileType b)
    {
        return (a == TileType.Room && b == TileType.Hallway) || 
            (a == TileType.Hallway && b == TileType.Room);
    }

    private void PlaceWallBetween(int x1, int z1, int x2, int z2)
    {
        Vector3 center1 = new Vector3(x1 * gridSpacingOffset, 0, z1 * gridSpacingOffset) + GridOrigin;
        Vector3 center2 = new Vector3(x2 * gridSpacingOffset, 0, z2 * gridSpacingOffset) + GridOrigin;

        Vector3 midPoint = (center1 + center2) / 2f;
        Quaternion rotation;

        if (x1 != x2) // horizontal wall
            rotation = Quaternion.Euler(0, 90, 0);
        else // vertical wall
            rotation = Quaternion.Euler(0, 0, 0);

        GameObject wall = Instantiate(GridPrefabs[3], midPoint, rotation);
        spawnedWalls.Add(wall);
        wall.transform.parent = transform;
    }

    public bool canPlace = false;
    public bool IsGenerationComplete {get; private set;} = false;
    private void GenerateMapData()
    {
        
        int startX = (int)GridX / 2;
        int startZ = (int)GridZ / 2;
        RoomData startingRoom = new RoomData(RoomSize.BaseRoom1, startX, startZ);

        // Ensure the grid space is empty
        canPlace = true;
        for (int x = startingRoom.x; x < startingRoom.x + startingRoom.width && canPlace; x++)
        {
            for (int z = startingRoom.y; z < startingRoom.y + startingRoom.height && canPlace; z++)
            {
                if (gridMap[x, z] != TileType.Empty) canPlace = false;
            }
        }

        if (canPlace)
        {
            for (int x = startingRoom.x; x < startingRoom.x + startingRoom.width; x++)
                for (int z = startingRoom.y; z < startingRoom.y + startingRoom.height; z++)
                    gridMap[x, z] = TileType.Room;

            roomPrefabs.Add(startingRoom);
            roomCenters.Add(startingRoom.Center);

            baseRoom1Count--;
        }

        int totalNeeded = baseRoom1Count + baseRoom2Count + baseRoom3Count +
                        barracksRoomCount + lootRoomCount + armoryRoomCount;

        int placed = 0, attempts = 0, maxAttempts = 200;

        while (placed < totalNeeded && attempts++ < maxAttempts)
        {
            RoomSize size = ChooseRandomAvailableRoomType();
            if (size == RoomSize.None) continue;

            // Decrement the room count
            RoomData room = TryPlaceRoom(size);
            
            if (room != null)
            {
                RoomData newRoom = room.Clone();
                roomPrefabs.Add(newRoom);
                roomCenters.Add(newRoom.Center);
                placed++;
            
                for (int x = newRoom.x; x < newRoom.x + newRoom.width; x++)
                for (int z = newRoom.y; z < newRoom.y + newRoom.height; z++)
                    gridMap[x, z] = TileType.Room;

            switch (size)
            {
                case RoomSize.BaseRoom1: baseRoom1Count--; 
                // room.enemyPrefabs.Add(enemyMelee);
                // room.enemyOffsets.Add(new Vector3(1, 0, 1));
                // room.enemyPrefabs.Add(enemyMelee);
                // room.enemyOffsets.Add(new Vector3(2, 0, 2));
                break;
                case RoomSize.BaseRoom2: baseRoom2Count--; 
                newRoom.enemyPrefabs.Add(enemyRanged);
                newRoom.enemyOffsets.Add(new Vector3(0, 0, 0));
                newRoom.enemyPrefabs.Add(enemyRanged);
                newRoom.enemyOffsets.Add(new Vector3(0, 0, 0));
                break;
                case RoomSize.BaseRoom3: baseRoom3Count--; 
                newRoom.enemyPrefabs.Add(enemyMelee);
                newRoom.enemyOffsets.Add(new Vector3(1, 0, 1));
                newRoom.enemyPrefabs.Add(enemyMelee);
                newRoom.enemyOffsets.Add(new Vector3(2, 0, 2));
                break;
                case RoomSize.BarracksRoom: barracksRoomCount--; 
                newRoom.enemyPrefabs.Add(enemyMage);
                newRoom.enemyOffsets.Add(new Vector3(1, 0, 1));
                newRoom.enemyPrefabs.Add(enemyRanged);
                newRoom.enemyOffsets.Add(new Vector3(2, 0, 2));
                break;
                case RoomSize.LootRoom: lootRoomCount--; 
                newRoom.enemyPrefabs.Add(enemyMage);
                newRoom.enemyOffsets.Add(new Vector3(1, 0, 1));
                newRoom.enemyPrefabs.Add(enemyRanged);
                newRoom.enemyOffsets.Add(new Vector3(2, 0, 2));
                break;
                case RoomSize.ArmoryRoom: armoryRoomCount--; 
                newRoom.enemyPrefabs.Add(enemyMage);
                newRoom.enemyOffsets.Add(new Vector3(1, 1, 1));
                newRoom.enemyPrefabs.Add(enemyRanged);
                newRoom.enemyOffsets.Add(new Vector3(2, 1, 2));
                newRoom.lootPrefabs.Add(lootPrefab);
                newRoom.lootOffsets.Add(new Vector3(3,1,3));
                break;
            }
                
        }
        }
        

        ConnectRoomsWithHallways();

        IsGenerationComplete = true;

        StartCoroutine(EnemyGenerator()); 
        StartCoroutine(LootGenerator());
        
    }

    private RoomSize ChooseRandomAvailableRoomType()
{
    List<RoomSize> availableTypes = new List<RoomSize>();

    if (baseRoom1Count > 0) availableTypes.Add(RoomSize.BaseRoom1);
    if (baseRoom2Count > 0) availableTypes.Add(RoomSize.BaseRoom2);
    if (baseRoom3Count > 0) availableTypes.Add(RoomSize.BaseRoom3);
    if (barracksRoomCount > 0) availableTypes.Add(RoomSize.BarracksRoom);
    if (lootRoomCount > 0) availableTypes.Add(RoomSize.LootRoom);
    if (armoryRoomCount > 0) availableTypes.Add(RoomSize.ArmoryRoom);

    if (availableTypes.Count == 0) return RoomSize.None; //  fallback enum

    return availableTypes[Random.Range(0, availableTypes.Count)];
}

    private RoomData TryPlaceRoom(RoomSize size)
    {
        RoomData test = new RoomData(size, 0, 0);

        for (int i = 0; i < 100; i++)
        {
            int rx = Random.Range(1, (int)(GridX - test.width - 1));
            int rz = Random.Range(1, (int)(GridZ - test.height - 1));
            test = new RoomData(size, rx, rz);

            bool canPlace = true;

            for (int x = test.x; x < test.x + test.width && canPlace; x++)
            {
                for (int z = test.y; z < test.y + test.height && canPlace; z++)
                {
                    if (gridMap[x, z] != TileType.Empty) canPlace = false;
                }
            }

            if (!canPlace) continue;

            for (int x = test.x; x < test.x + test.width; x++)
                for (int z = test.y; z < test.y + test.height; z++)
                    gridMap[x, z] = TileType.Room;

            return test;
        }

        return null;
    }

    private void ConnectRoomsWithHallways()
    {
        for (int i = 0; i < roomCenters.Count - 1; i++)
        {
            Vector2Int a = roomCenters[i];
            Vector2Int b = roomCenters[i + 1];

            // First horizontal
            for (int x = Mathf.Min(a.x, b.x); x <= Mathf.Max(a.x, b.x); x++)
            {
                // Only place hallway if not cutting through a room
                if (gridMap[x, a.y] != TileType.Room)
                {
                    SetIfEmpty(x, a.y, TileType.Hallway);
                }
            }

            // Then vertical
            for (int z = Mathf.Min(a.y, b.y); z <= Mathf.Max(a.y, b.y); z++)
            {
                // Only place hallway if not cutting through a room
                if (gridMap[b.x, z] != TileType.Room)
                {
                    SetIfEmpty(b.x, z, TileType.Hallway);
                }
            }
        }
    }

    private void SetIfEmpty(int x, int z, TileType type)
    {
        if (x >= 0 && x < GridX && z >= 0 && z < GridZ)
        {
            if (gridMap[x, z] == TileType.Empty || gridMap[x, z] == TileType.Hallway)
            {
                gridMap[x, z] = type;
                
                // If this is a hallway, add it to the spawned halls list
                if (type == TileType.Hallway)
                {
                    Vector3 pos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + GridOrigin;
                    GameObject hall = Instantiate(GridPrefabs[1], pos, Quaternion.identity);
                    spawnedHalls.Add(hall);
                    hall.transform.parent = transform;
                }
            }
        }
    }

    private bool IsAdjacentToHallway(int x, int z)
    {
        return (x > 0 && gridMap[x - 1, z] == TileType.Hallway) ||
               (x < GridX - 1 && gridMap[x + 1, z] == TileType.Hallway) ||
               (z > 0 && gridMap[x, z - 1] == TileType.Hallway) ||
               (z < GridZ - 1 && gridMap[x, z + 1] == TileType.Hallway);
    }


    // enemy functionality in rooms
    // loot spawns, ignore doors
    // prefabs walls, doors, floors, other.
}
