using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : BaseCharacter
{
    Player player;
    Vector3 toPlayer;
    /// <summary> The minimum distance the wolf should maintain from the player </summary>
    float minimumDistance;

	protected override void Start()
    {
        base.Start();

        player = GameObject.Find("Player").GetComponent<Player>();
        toPlayer = player.transform.position - transform.position;
        minimumDistance = toPlayer.magnitude;
	}
	
	protected override void Update ()
    {
        base.Update();
        
        /*
         * If the player is moving faster than the wolf,
         * then maintain the minimum distance from the player.
         * This is to stop the wolf falling off the map
         * because it is going too slow.
         */
        if(player.movementSpeed > this.movementSpeed)
        {
            Vector3 tempPos = transform.position;
            tempPos.z = Mathf.Abs(player.transform.position.z) - 12f;
            transform.position = tempPos;
        }
	}
}
