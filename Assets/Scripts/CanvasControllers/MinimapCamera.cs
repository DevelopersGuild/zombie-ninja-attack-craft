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
        camera.enabled = isActive;
    }

    // Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Map"))
	    {
            Debug.Log("enabling!");
            isActive = !isActive;
            camera.enabled = isActive;
	    }
	}
}
