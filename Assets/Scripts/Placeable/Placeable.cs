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
            placeableMgr.ReleasePosition(new Vector2(transform.position.x, transform.position.z));
            // Detach from the current parent tile
            transform.parent = null;
            gameObject.SetActive(false);
        }
    }
}
