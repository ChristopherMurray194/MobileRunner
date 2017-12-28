using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHazard : Hazard
{
    /// <summary> The change in the player speed </summary>
    public float speedDelta = 2f;

    CharacterManager characterMgr;

    protected override void Awake()
    {
        base.Awake();
        characterMgr = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
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
            playerScript.DecreaseSpeed(speedDelta);
        }
        else if (other.tag == "Enemy")
        {
            characterMgr.DecreaseOtherCharacterSpeed(.2f, other);
        }
    }
}
