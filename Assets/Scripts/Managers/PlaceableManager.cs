using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableManager : MonoBehaviour
{
    public ObjectPool[] placeablePools;

    /// <summary> The size of each tile </summary>
    public float tileSize = 30f;
    public int zIncrements = 4;
    float _zIncrement = 0f;
    public float zIncrement
    {
        get { return _zIncrement; }
    }

    /// <summary> The X axis values for each lane - from left to right </summary>
    float[] lanes = { -26.25f, -18.75f, -11.25f, -3.75f };

    List<Vector2> usedPositions = new List<Vector2>();

    /// <summary> The delay in seconds between placeable spawns </summary>
    public float spawnDelay = 2f;
    /// <summary> The number of placeables to spawn each update call </summary>
    public int spawnNum = 2;

    TerrainManager terrainMgr;
    /*
    /// <summary> The most recently spawned object </summary>
    GameObject _spawnedObject;
    public GameObject spawnedObject
    {
        get { return _spawnedObject; }
    }
    */
    GameObject lastTile;

    float timer = 0f;

    void Start()
    {
        terrainMgr = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();
        _zIncrement = tileSize / zIncrements;
    }

    void Update()
    {
        // Always keep the last tile updated
        lastTile = terrainMgr.GetLastTile();

        if (timer > spawnDelay)
        {
            for (int i = 0; i < spawnNum; i++)
                PositionHazard();

            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void PositionHazard()
    {
        // Get a random placeable pool
        int placeablePoolIndex = Random.Range(0, placeablePools.Length);
        ObjectPool tempPool = placeablePools[placeablePoolIndex];
        // Get a random placeable from the pool
        int placeableIndex = Random.Range(0, tempPool.GetPooledObjects().Count);

        GameObject placeable = tempPool.GetPooledObjects()[placeableIndex];
        // If the object is not active it is behind the camera and can be moved
        if (!placeable.activeSelf)
        {
            // Reactivate the object
            placeable.SetActive(true);
            Vector3 pos = placeable.transform.position;
            int laneIndex, randomRow;
            float tileZ;

            // Get a random lane
            laneIndex = Random.Range(0, lanes.Length);
            // Get a random row
            randomRow = Random.Range(0, zIncrements);
            // Get the tile's Z position
            tileZ = lastTile.transform.position.z;

            // Get the lane
            pos.x = lanes[laneIndex];
            // Get the Z value
            pos.z = tileZ + (_zIncrement * randomRow);

            // If this position is already being used
            if (usedPositions.Contains(new Vector2(pos.x, pos.z)))
                return; // We cannot place here so exit the function.

            // Add the X and Z values to the currently used positions list
            usedPositions.Add(new Vector2(pos.x, pos.z));
            placeable.transform.position = pos;
        }
    }

    /// <summary>
    /// Remove the passed position form the list of used positions.
    /// </summary>
    /// <param name="pos"></param>
    public void removePosFromList(Vector2 pos)
    {
        usedPositions.Remove(pos);
    }
}
