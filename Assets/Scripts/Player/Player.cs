using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary> Movement speed of the player </summary>
    public float movementSpeed = 15f;

    /// <summary> The increment at which the player moves left and right </summary>
    float moveIncrement = 7.5f;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        MoveForward();
        
        if(Input.GetKeyDown(KeyCode.A))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
    }

    void MoveForward()
    {
        Vector3 pos = transform.position;
        pos.z += movementSpeed * Time.deltaTime;
        transform.position = pos;
    }

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
}
