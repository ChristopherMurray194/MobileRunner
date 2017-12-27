using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    /// <summary> Movement speed of the player </summary>
    public float movementSpeed = 15f;
    /// <summary> The minimum speed the player can move at </summary>
    public float minimumSpeed = 10f;
    /// <summary> The max speed the player can move at</summary>
    public float maximumSpeed = 40f;
    float initialSpeed = 1f;
    float speedTimer = 0f;
    /// <summary> The time in seconds to have passed since the last speed boost or hazard has been hit before the speed returns to normal </summary>
    public float timerDelay = 5f;
    /// <summary> The timer used for reverting the speed back to its original value every x seconds </summary>
    float speedRevertTimer = 0f;

    bool displayInfo = false;

    protected virtual void Start()
    {
        initialSpeed = movementSpeed;
    }
	
	protected virtual void Update()
    {
        MoveForward();

        if (speedTimer > timerDelay)
        {
            revertSpeed();
        }
        else
        {
            speedTimer += Time.deltaTime;
        }
    }

    void MoveForward()
    {
        Vector3 pos = transform.position;
        pos.z += movementSpeed * Time.deltaTime;
        transform.position = pos;
    }

    /// <summary>
    /// Decrease the player's speed by the given delta.
    /// </summary>
    /// <param name="deltaSpeed"></param>
    public void DecreaseSpeed(float deltaSpeed)
    {
        if (!(movementSpeed - deltaSpeed < minimumSpeed))
            movementSpeed -= deltaSpeed;
        // A speed hazard has been hit reset the speed timer
        speedTimer = 0f;
    }

    /// <summary>
    /// Increase the player's speed by the given delta.
    /// </summary>
    /// <param name="deltaSpeed"></param>
    public void IncreaseSpeed(float deltaSpeed)
    {
        if (!(movementSpeed + deltaSpeed > maximumSpeed))
            movementSpeed += deltaSpeed;
        // A speed boost has been hit reset the speed timer
        speedTimer = 0f;
    }

    /// <summary>
    /// If after a certain number of seconds the player has not hit a speed boost or hazard,
    /// revert to the original speed
    /// </summary>
    void revertSpeed()
    {
        // Every second
        if (speedRevertTimer > 1f)
        {
            // If the player is moving faster
            if (movementSpeed > initialSpeed)
                // Reduce speed
                movementSpeed--;
            // If the player is moving slower
            else if (movementSpeed < initialSpeed)
                // Increase speed
                movementSpeed++;
            // Reset the timer
            speedRevertTimer = 0f;
        }
        else
        {
            speedRevertTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// USED FOR DEBUGGING PURPOSES
    /// </summary>
    private void OnGUI()
    {
        displayDebuggingInfo();
    }

    void displayDebuggingInfo()
    {
        if (displayInfo)
        {
            Handles.Label(transform.position + Vector3.up * 2,
                "Z position is: " + transform.position.z.ToString() + "\n" + "Speed is: " + movementSpeed);
        }
    }

    public void toggleDebugging()
    {
        if (!displayInfo) displayInfo = true;
        else displayInfo = false;
    }
}
