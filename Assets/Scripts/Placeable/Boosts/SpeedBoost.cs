using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Boost
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
            // Increase the speed of all characters including the player
            characterMgr.IncreasePlayerSpeed(speedDelta);
        }
        else if(other.tag == "Enemy")
        {
            characterMgr.IncreaseOtherCharacterSpeed(.2f, other);
        }
    }
}
