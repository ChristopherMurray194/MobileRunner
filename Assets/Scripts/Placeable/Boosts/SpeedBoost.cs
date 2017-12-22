using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Boost
{
    /// <summary> The change in the player speed </summary>
    public float speedDelta = 2f;

    protected virtual void Awake()
    {
        base.Awake();
    }

    protected virtual void Update()
    {
        base.Update();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "Player")
        {
            Player playerScript = other.GetComponent<Player>();
            playerScript.IncreaseSpeed(speedDelta);
        }
    }
}
