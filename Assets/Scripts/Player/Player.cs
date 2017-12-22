using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary> Movement speed of the player </summary>
    public float movementSpeed = 15f;
    /// <summary> The minimum speed the player can move at </summary>
    public float minimumSpeed = 5f;
    /// <summary> The max speed the player can move at</summary>
    public float maximumSpeed = 40f;
    float initialSpeed = 1f;

    /// <summary> The increment at which the player moves left and right </summary>
    float moveIncrement = 7.5f;

    /// <summary> Point on screen where the player initially touches </summary>
    Vector2 touchOrigin = -Vector2.one;
    
	void Start ()
    {
        initialSpeed = movementSpeed;
	}
	
	void Update ()
    {
        MoveForward();

        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

            if(Input.GetKeyDown(KeyCode.A))
                MoveLeft();

            if (Input.GetKeyDown(KeyCode.D))
                MoveRight();

        #elif UNITY_ANDROID
            if(Input.touchCount > 0)
            {
                // Store first touch detected
                Touch touch = Input.touches[0];
                // If this is the beginning of a touch on the screen
                if(touch.phase == TouchPhase.Began)
                {
                    // Store as the intial touch
                    touchOrigin = touch.position;
                }
                // If the touch has ended and is inside the bounds of the screen
                else if(touch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    Vector2 touchEnd = touch.position;
                    // Get the X direction of the touch
                    float x = touchEnd.x - touchOrigin.x;
                    // Revert the touch origin X to offscreen
                    touchOrigin.x = -1;
                
                    if(x > 0)
                        MoveRight();
                    else if(x < 0)
                        MoveLeft();
                }
            }
        #endif
    }

    void MoveForward()
    {
        Vector3 pos = transform.position;
        pos.z += movementSpeed * Time.deltaTime;
        transform.position = pos;
    }

    /// <summary>
    /// Move the player to the next lane on the left
    /// </summary>
    void MoveLeft()
    {
        Vector3 pos = transform.position;
        // Ensure we do not go off the track
        if(!(pos.x - moveIncrement < -26.25f))
        {
            pos.x -= moveIncrement;
        }
        else
        {
            // Notify player they cannot move any further left
        }

        transform.position = pos;
    }

    /// <summary>
    /// Move the player to the next lane on the right
    /// </summary>
    void MoveRight()
    {
        Vector3 pos = transform.position;
        // Ensure we do not go off the track
        if (!(pos.x + moveIncrement > -3.25f))
        {
            pos.x += moveIncrement;
        }
        else
        {
            // Notify player they cannot move any further right
        }

        transform.position = pos;
    }

    /// <summary>
    /// Decrease the player's speed by the given delta.
    /// </summary>
    /// <param name="deltaSpeed"></param>
    public void DecreaseSpeed(float deltaSpeed)
    {
        if(!(movementSpeed - deltaSpeed < minimumSpeed))
            movementSpeed -= deltaSpeed;
    }

    /// <summary>
    /// Increase the player's speed by the given delta.
    /// </summary>
    /// <param name="deltaSpeed"></param>
    public void IncreaseSpeed(float deltaSpeed)
    {
        if (!(movementSpeed + deltaSpeed > maximumSpeed))
            movementSpeed += deltaSpeed;
    }

    /// <summary>
    /// Reset the player's speed to what it was at the beginning of the game.
    /// </summary>
    public void ResetSpeed()
    {
        movementSpeed = initialSpeed;
    }
}
