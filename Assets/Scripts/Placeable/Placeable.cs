using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    PlaceableManager placeableMgr;
    Camera cam;

	protected virtual void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        placeableMgr = GameObject.Find("PlaceableManager").GetComponent<PlaceableManager>();
        gameObject.SetActive(false);
	}
	
	protected virtual void Update()
    {
        DeactivateSelf();
    }

    void DeactivateSelf()
    {
        Vector3 camFwd = cam.transform.forward;
        Vector3 camToObj = (transform.position - cam.transform.position);
        // If the object is behind the camera
        if (Vector3.Dot(camFwd, camToObj) < 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        /* If this object collides with another placeable object &&
          other is the most recently spawned object */
        if (other.tag == "Placeable" && other.gameObject.Equals(placeableMgr.spawnedObject))
        {
            // Move the other object up one increment on the Z axis
            Vector3 temp = other.gameObject.transform.position;
            temp.z += placeableMgr.zIncrement;
            other.gameObject.transform.position = temp;
        }
    }
}
