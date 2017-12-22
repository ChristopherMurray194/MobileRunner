using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Placeable
{

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
        
    }
}
