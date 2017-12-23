using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary> Movement speed of the player </summary>
    public float movementSpeed = 15f;
    /// <summary> The minimum speed the player can move at </summary>
    public float minimumSpeed = 10f;
    /// <summary> The max speed the player can move at</summary>
    public float maximumSpeed = 40f;
    float initialSpeed = 1f;

    /// <summary> The increment at which the player moves left and right </summary>
    float moveIncrement = 7.5f;
    Rigidbody rb;
    BoxCollider boxCollder;
    int floorMask;
    public float fallMultiplier = 4f;
    public float jumpHeight = 3f;

    /// <summary> Point on screen where the player initially touches </summary>
    Vector2 touchOrigin = -Vector2.one;
    
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        boxCollder = GetComponent<BoxCollider>();
        floorMask = LayerMask.GetMask("Floor");
        initialSpeed = movementSpeed;
    }
	
	void Update ()
    {
        MoveForward();

        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

            // Can only change lanes if the player is touching the floor
            if (isGrounded())
            {
                if (Input.GetKeyDown(KeyCode.A))
                    MoveLeft();

                if (Input.GetKeyDown(KeyCode.D))
                    MoveRight();
            }

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
                
                    if(isGrounded())
                    {   
                        if(x > 0)
                            MoveRight();
                        else if(x < 0)
                            MoveLeft();
                    }
                }
            }
        #endif
    }

    void FixedUpdate()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();

            // If the player is falling
            if (rb.velocity.y < 0)
            {
                // Fall faster
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
            }
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
                    else if(touch.phase == TouchPhase.Ended && touchOrigin.y >= 0)
                    {
                        Vector2 touchEnd = touch.position;

                        float y = touchEnd.y - touchOrigin.y;
                        touchOrigin.y = -1;

                        // If the swipe is up
                        if(y < 0)
                            Jump();
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
    /// Jump but only if the player is touching the floor
    /// </summary>
    void Jump()
    {
        // Only jump if touching the ground
        if (isGrounded())
        {
            rb.velocity = new Vector3(0f, Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight), 0f);
        }
    }

    /// <summary>
    /// Returns true if the player is touching the floor, false otherwise
    /// </summary>
    /// <returns></returns>
    bool isGrounded()
    {
        // If the player is touching the floor
        if (Physics.CheckBox(boxCollder.bounds.center, boxCollder.bounds.extents, boxCollder.transform.rotation, floorMask))
            return true;
        return false;
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
