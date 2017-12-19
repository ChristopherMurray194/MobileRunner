using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public GameObject[] hazards;

    /// <summary> The X axis values for each lane - from left to right </summary>
    float[] lanes = { -26.25f, -18.75f, -11.25f, -3.75f };

    TerrainManager terrainMgr;

    float timer = 0f;

	void Start ()
    {
        terrainMgr = GameObject.Find("TerrainManager").GetComponent<TerrainManager>();
	}
	
	void Update ()
    {
        if(timer > 5f)
        {
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
        
        GameObject hazard = GameObject.Instantiate(hazards[hazardIndex]);
        Vector3 pos = hazard.transform.position;
        // Set the lane
        pos.x = lanes[laneIndex];
        // Set the Z index
        pos.z = terrainMgr.GetLastTile().transform.position.z;
        hazard.transform.position = pos;
    }
}
