using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /// <summary> The pooled object </summary>
    public GameObject obj;
    /// <summary> The total number of this object that can be pooled </summary>
    public int total = 2;
    /// <summary> A list of the instantiated objects </summary>
    List<GameObject> pooledObjects = new List<GameObject>();

	void Start ()
    {
		for(int i = 0; i < total; i++)
        {
            GameObject temp = GameObject.Instantiate(obj);
            // Move the objects off camera to the scene origin
            Vector3 tempPos = temp.transform.position;
            tempPos.x = 0f;
            tempPos.z = 0f;
            temp.transform.position = tempPos;

            pooledObjects.Add(temp);
        }
	}
	
	void Update ()
    {
		
	}

    public List<GameObject> GetPooledObjects()
    {
        return pooledObjects;
    }
}
