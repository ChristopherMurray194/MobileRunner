using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public GameObject[] hazards;

    /// <summary> The X axis values for each lane - from left to right </summary>
    float[] lanes = { -26.25f, -18.75f, -11.25f, -3.75f };
    /// <summary> The size of the tiles </summary>
    public float tileSize = 30f;
    /// <summary> The delay in seconds between hazard spawns </summary>
    public float spawnDelay = 2f;
    /// <summary> The number of hazards to spawn per incremement </summary>
    public int spawnNum = 2;

    TerrainManager terrainMgr;
    GameObject lastTile;

    float timer = 0f;

	void Start ()
    {
        terrainMgr = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();
	}
	
	void Update ()
    {
        // Always keep the last tile updated
        lastTile = terrainMgr.GetLastTile();
        
        if(timer > spawnDelay)
        {
            for(int i = 0; i < spawnNum; i++)
                SpawnHazard();

            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
	}

    void SpawnHazard()
    {
        // Get a random hazard
        int hazardIndex = Random.Range(0, hazards.Length);
        // Get a random lane
        int laneIndex = Random.Range(0, lanes.Length);
        float tileZ = lastTile.transform.position.z;
        // Get a random Z value
        float randZ = Random.Range(tileZ, tileZ + tileSize);
        
        GameObject hazard = GameObject.Instantiate(hazards[hazardIndex]);
        Vector3 pos = hazard.transform.position;
        // Set the lane
        pos.x = lanes[laneIndex];
        // Set the Z index
        pos.z = randZ;
        hazard.transform.position = pos;
    }
}
