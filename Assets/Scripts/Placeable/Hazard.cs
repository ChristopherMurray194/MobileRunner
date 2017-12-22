using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : Placeable
{
    HazardManager hazardMgr;

	protected virtual void Awake ()
    {
        base.Awake();
        hazardMgr = GameObject.Find("HazardManager").GetComponent<HazardManager>();
	}
	
	protected void Update ()
    {
        base.Update();
	}

    protected virtual void OnTriggerEnter(Collider other)
    {
        base.onTriggerEnter(other);
        
        /* If this object collides with another hazard &&
          other is the most recently spawned object */
        if (other.tag == "Hazard" && other.gameObject.Equals(hazardMgr.spawnedObject))
        {
            // Move the other hazard up one increment on the Z axis
            Vector3 temp = other.gameObject.transform.position;
            temp.z += hazardMgr.zIncrement;
            other.gameObject.transform.position = temp;
        }
    }
}
