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
    float speedTimer = 0f;
    /// <summary> The time in seconds to have passed since the last speed boost or hazard has been hit before the speed returns to normal </summary>
    public float timerDelay = 5f;
    /// <summary> The timer used for reverting the speed back to its original value every x seconds </summary>
    float speedRevertTimer = 0f;

    /// <summary> The increment at which the player moves left and right </summary>
    float moveIncrement = 7.5f;
    Rigidbody rb;
    BoxCollider boxCollder;
    int floorMask;
    public float fallMultiplier = 4f;
    public float jumpHeight = 3f;

    /// <summary> Point on screen where the player initially touches </summary>
    Vector2 touchOrigin = -Vector2.one;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollder = GetComponent<BoxCollider>();
        floorMask = LayerMask.GetMask("Floor");
        initialSpeed = movementSpeed;
    }

    void Update()
    {
        MoveForward();
        
        if(speedTimer > timerDelay)
        {
            revertSpeed();
        }
        else
        {
            speedTimer += Time.deltaTime;
        }

#if UNITY_STANDALONE || UNITY_WEBPLAYER

            // Can only change lanes if the player is touching the floor
            if (isGrounded())
            {
                if (Input.GetKeyDown(KeyCode.A))
                    MoveLeft();

                if (Input.GetKeyDown(KeyCode.D))
                    MoveRight();
            }

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            // Store first touch detected
            Touch touch = Input.touches[0];
            // If this is the beginning of a touch on the screen
            if (touch.phase == TouchPhase.Began)
            {
                // Store as the intial touch
                touchOrigin = touch.position;
            }
            // If the touch has ended
            else if (touch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                Vector2 touchEnd = touch.position;
                // Get the X direction of the touch
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                // Revert the touch origin X to offscreen
                touchOrigin.x = -1;
                touchOrigin.y = -1;

                if (isGrounded() && Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x > 0)
                        MoveRight();
                    else if (x < 0)
                        MoveLeft();
                }
            }
        }
#endif
    }

    void FixedUpdate()
    {
        // If the player is falling
        if (rb.velocity.y < 0)
        {
            // Fall faster
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }

#if UNITY_STANDALONE || UNITY_WEBPLAYER

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                touchOrigin = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && touchOrigin.y >= 0)
            {
                Vector2 touchEnd = touch.position;

                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                //touchOrigin.x = -1;
                //touchOrigin.y = -1;

                // If the swipe is up
                if (Mathf.Abs(y) > Mathf.Abs(x) && y > 0)
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
        if (!(pos.x - moveIncrement < -26.25f))
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
}
