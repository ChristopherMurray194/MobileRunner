using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Boost
{
    /// <summary> The change in the player speed </summary>
    public float speedDelta = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player playerScript = other.GetComponent<Player>();
            playerScript.IncreaseSpeed(speedDelta);
        }
    }
}
