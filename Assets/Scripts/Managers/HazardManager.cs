using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public ObjectPool[] hazardPools;

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

    /// <summary> The delay in seconds between hazard spawns </summary>
    public float spawnDelay = 2f;
    /// <summary> The number of hazards to spawn each update call </summary>
    public int spawnNum = 2;

    TerrainManager terrainMgr;
    /// <summary> The most recently spawned object </summary>
    GameObject _spawnedObject;
    public GameObject spawnedObject
    {
        get { return _spawnedObject; }
    }

    GameObject lastTile;
    
    float timer = 0f;

	void Start ()
    {
        terrainMgr = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();
        _zIncrement = tileSize / zIncrements;
	}
	
	void Update ()
    {
        // Always keep the last tile updated
        lastTile = terrainMgr.GetLastTile();
        
        if(timer > spawnDelay)
        {
            for(int i = 0; i < spawnNum; i++)
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
        // Get a random hazard pool
        int hazardPoolIndex = Random.Range(0, hazardPools.Length);
        ObjectPool tempPool = hazardPools[hazardPoolIndex];
        // Get a random hazard from the pool
        int hazardIndex = Random.Range(0, tempPool.GetPooledObjects().Count);

        GameObject hazard = tempPool.GetPooledObjects()[hazardIndex];
        // If the object is not active it is behind the camera and can be moved
        if (!hazard.activeSelf)
        {
            // Reactivate the object
            hazard.SetActive(true);
            Vector3 pos = hazard.transform.position;
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

            // Add the X and Z values to the currently used positions list
            usedPositions.Add(new Vector2(pos.x, pos.z));
            hazard.transform.position = pos;

            // Once we have moved the hazard, enable the collider so we can check for collisions
            hazard.GetComponent<Collider>().enabled = true;

            _spawnedObject = hazard;
        }
    }
}
