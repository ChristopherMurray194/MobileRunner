using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHazard : Hazard
{
    /// <summary> The amount of damage applied to the colliding object </summary>
    public int damageDealt = 1;

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
            playerScript.DamagePlayer(damageDealt);
        }
    }
}
