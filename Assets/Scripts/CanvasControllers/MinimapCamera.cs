using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour
{
    private bool isActive;
    private Camera camera;

    void Start()
    {
        isActive = false;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Map") && isActive)
	    {
            Debug.Log("enabling!");
	        camera.enabled = false;
	        isActive = false;
	    }
	    else if(Input.GetButtonDown("Map") && !isActive)
	    {
	        camera.enabled = true;
	        isActive = true;
	    }
	}
}
