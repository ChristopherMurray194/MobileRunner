using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    /// <summary> The health of the player </summary>
    int _currentHealth = 100;
    public int currentHealth
    {
        get { return _currentHealth; }
    }
    bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    /// <summary> The increment at which the player moves left and right </summary>
    float moveIncrement = 7.5f;
    Rigidbody rb;
    BoxCollider boxCollder;
    int floorMask;
    public float fallMultiplier = 4f;
    public float jumpHeight = 3f;

    /// <summary> Point on screen where the player initially touches </summary>
    Vector2 touchOrigin = -Vector2.one;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        boxCollder = GetComponent<BoxCollider>();
        floorMask = LayerMask.GetMask("Floor");
    }

    protected override void Update()
    {
        base.Update();

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

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

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
    /// Reduce the player's health by the passed amount.
    /// </summary>
    /// <param name="damageDelta"></param>
    public void DamagePlayer(int damageDelta)
    {
        if (_currentHealth - damageDelta > 0)
            _currentHealth -= damageDelta;
        else
            _currentHealth = 0;

        // Notify the player visually that damage has been dealt

        if (_currentHealth <= 0)
            Death();
    }

    /// <summary>
    /// Handles what should happen when to the player when it dies.
    /// </summary>
    void Death()
    {
        isDead = true;
        // Stop animating
        gameObject.GetComponent<Animator>().enabled = false;
        this.enabled = false;
    }
}
