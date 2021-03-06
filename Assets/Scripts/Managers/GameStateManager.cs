﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public Player player;
    public BaseCharacter[] enemies;
    float restatTimer = 0f;

    /// <summary> Is the game paused? </summary>
    bool isPaused = false;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
                PauseGame();
            else if (isPaused)
                ResumeGame();
        }
        */

        if (player.IsDead)
        {
            PauseGame();

            restatTimer += Time.deltaTime;

            // If 5 seconds has passed since the player has died, restart the scene."
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

            // Disable all other characters
            if (enemies != null)
            {
                foreach (BaseCharacter b in enemies)
                {
                    b.enabled = false;
                    b.gameObject.GetComponent<Rigidbody>().Sleep();
                    b.GetComponent<Animator>().enabled = false;
                }
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log("You are missing one or more managers. Alternatively check the spelling of the game object name.\n" + e);
        }
    }

    /// <summary>
    /// Handels what should happen when the game is paused.
    /// </summary>
    void PauseGame()
    {
        isPaused = true;
        //======== FREEZE THE PLAYER ============
        // Stop the player from moving
        player.enabled = false;
        // Freeze player's rigid body
        player.gameObject.GetComponent<Rigidbody>().Sleep();
        // Stop the player from animating
        player.GetComponent<Animator>().enabled = false;
        //========================================

        DisableScene();
    }

    /// <summary>
    /// Handles what should happen when the game is resumed.
    /// </summary>
    void ResumeGame()
    {
        isPaused = false;
        //========== UNFREEZE THE PLAYER =================
        // Allow the player to continue moving
        player.enabled = true;
        // Unfreeze the player's rigidbody
        player.gameObject.GetComponent<Rigidbody>().WakeUp();
        // Allow the player to resume animating
        player.GetComponent<Animator>().enabled = true;
        //===============================================

        //======= UNFREEZE ALL OTHER CHARACTERS ==========
        // Disable all other characters
        if (enemies != null)
        {
            foreach (BaseCharacter b in enemies)
            {
                b.enabled = true;
                b.gameObject.GetComponent<Rigidbody>().WakeUp();
                b.GetComponent<Animator>().enabled = true;
            }
        }
        //================================================

        //======== ENABLE MANAGERS======================
        try
        {
            // Stop spawning placeable objects
            PlaceableManager placeableManager = GameObject.Find("PlaceableManager").GetComponent<PlaceableManager>();
            placeableManager.enabled = true;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("You are missing one or more managers. Alternatively check the spelling of the game object name.\n" + e);
        }
        //=============================================
    }

    /// <summary>
    /// The function to be invoked by the UI toggle to pause and unpause the game.
    /// </summary>
    public void PauseToggle()
    {
        if (!isPaused)
            PauseGame();
        else if (isPaused)
            ResumeGame();
    }
}
