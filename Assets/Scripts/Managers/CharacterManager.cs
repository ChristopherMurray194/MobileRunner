using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Player player;
    public BaseCharacter[] otherCharacters;
    
    public void IncreasePlayerSpeed(float deltaSpeed)
    {
        player.IncreaseSpeed(deltaSpeed);
        /*
         * The other characters also need to increase their speed 
         * to keep up with the player otherwise they will fall off 
         * the track as it is moving faster than them.
         */ 
        foreach(BaseCharacter other in otherCharacters)
        {
            // If the character is not moving faster than the player
            if(!(other.movementSpeed + deltaSpeed > player.movementSpeed))
                other.IncreaseSpeed(deltaSpeed);
        }
    }

    public void IncreaseOtherCharacterSpeed(float deltaSpeed, Collider other)
    {
        BaseCharacter otherCharacter = other.GetComponent<BaseCharacter>();
        // Make sure the character does not overtake the player
        if(!(otherCharacter.movementSpeed + deltaSpeed > player.movementSpeed))
            otherCharacter.IncreaseSpeed(deltaSpeed);
    }

    public void DecreaseOtherCharacterSpeed(float deltaSped, Collider other)
    {
        BaseCharacter otherCharacter = other.GetComponent<BaseCharacter>();
        /*
         * If the player is moving faster than its initial speed,
         * other characters cannot slow down otherwise they may 
         * eventually fall off the track because they are going to slow.
         */
        if (!(player.movementSpeed > player.IntialSpeed))
            otherCharacter.DecreaseSpeed(deltaSped);
    }
}
