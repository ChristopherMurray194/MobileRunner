using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary> Movement speed of the player </summary>
    public float movementSpeed = 15f;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        Move();
	}

    void Move()
    {
        Vector3 pos = transform.position;
        pos.z += movementSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
