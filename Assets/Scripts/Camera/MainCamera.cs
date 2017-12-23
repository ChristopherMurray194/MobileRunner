using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /// <summary> Camera follow movement speed </summary>
    public float followSpeed = 5f;
    /// <summary> The target for the camera to follow </summary>
    public Transform target;
    /// <summary> Distance vector to maintain from the target </summary>
    Vector3 distance;

    void Start()
    {
        distance = transform.position - target.position;
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Move with the player
        Vector3 posToTarget = target.position + distance;
        transform.position = Vector3.Lerp(transform.position, posToTarget, followSpeed * Time.deltaTime);
    }
}
