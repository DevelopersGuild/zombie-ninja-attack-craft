using UnityEngine;
using System.Collections;

public class EnableInRange : MonoBehaviour {

    private Player player;
    public float maxDistance;
    private Light light;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>();
        light = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = transform.position - player.transform.position;
        float distance = direction.magnitude;
        Debug.Log(distance);
	    if(distance < maxDistance)
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
	}
}
