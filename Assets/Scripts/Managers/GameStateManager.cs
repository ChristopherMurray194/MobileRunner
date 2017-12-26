using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public Player player;
    float restatTimer = 0f;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (player.IsDead)
        {
            DisableScene();

            restatTimer += Time.deltaTime;

            if(restatTimer >= 5f)
                SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
	}

    void DisableScene()
    {
        try
        {
            // Stop spawning placeable objects
            PlaceableManager placeableManager = GameObject.Find("PlaceableManager").GetComponent<PlaceableManager>();
            placeableManager.enabled = false;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("You are missing one or more managers. Alternatively check the spelling of the game object name.\n" + e);
        }
    }
}
