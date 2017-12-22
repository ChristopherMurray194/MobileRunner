using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    Camera cam;

	protected virtual void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
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

    protected virtual void onTriggerEnter(Collider other)
    {
        if (other.tag == "Placeable")
        {
            // Do something
        }
    }
}
