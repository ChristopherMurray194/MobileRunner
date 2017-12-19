using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    Camera cam;

	void Awake ()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	void Update ()
    {
        DestroySelf();
    }

    void DestroySelf()
    {
        Vector3 camFwd = cam.transform.forward;
        Vector3 camToObj = (transform.position - cam.transform.position);
        // If the object is behind the camera
        if (Vector3.Dot(camFwd, camToObj) < 0)
        {
            // Destroy THIS object
            Destroy(this.gameObject);
        }
    }
}
