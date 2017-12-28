using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    ///<summary> List of floor tiles to iterate over </summary>
    public List<GameObject> floorTiles;
    /// <summary> Size of each tile </summary>
    public float tileSize = 30f;

    /// <summary> The tile furthest from the camera </summary>
    GameObject lastTile;

    Camera cam;
    Player player;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Player>();
        lastTile = floorTiles[floorTiles.Count - 1];
    }

    void Update()
    {
        RotateTiles();

        MoveForward();
    }

    /// <summary>
    /// Move the tiles back, giving the illusion the character,
    /// is moving forward.
    /// </summary>
    void MoveForward()
    {
        foreach (GameObject tile in floorTiles)
        {
            Vector3 pos = tile.transform.position;
            pos.z -= player.movementSpeed * Time.deltaTime;
            tile.transform.position = pos;
        }
    }

    void RotateTiles()
    {
        if (floorTiles.Count != 0)
        {
            Vector3 camFwd = cam.transform.forward;
            Vector3 camToTile = (floorTiles[0].transform.position - cam.transform.position);
            // If the tile is behind the camera AND the distance from the tile and the camera is greater than the width of a tile
            if ((Vector3.Dot(camFwd, camToTile) < 0) &&
                (camToTile.magnitude > tileSize * 2))
            {
                // Move the tile infront of the last tile in the array
                float newZ = floorTiles[floorTiles.Count - 1].transform.position.z + tileSize;
                Vector3 temp = floorTiles[0].transform.position;
                temp.z = newZ;
                floorTiles[0].transform.position = temp;
                // Store the tile as the last tile in the list
                lastTile = floorTiles[0];
                // Move the tile to the end of the list
                floorTiles.Add(floorTiles[0]);
                // Remove the tile from the beginning of the list
                floorTiles.RemoveAt(0);
            }
        }
    }

    public GameObject GetLastTile() { return lastTile; }
}
