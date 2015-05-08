using UnityEngine;
using System.Collections;

public class FakeAngel : MonoBehaviour
{
    private bool isActive;

    private bool isReal;
   // public Projectile angelProjObject;
    public float blinkTime;

    private float currentTime;

    // Use this for initialization
    void Start()
    {
        isReal = false;
        isActive = true;
        currentTime = blinkTime + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
          
        currentTime -= Time.deltaTime;
      
        //The sprite blinks once the player has stepped in its range and explodes afterwards
        if (currentTime > blinkTime)
        {

        }
        else if (currentTime > 0)
        {
            float remainder = currentTime % .1f;
            GetComponent<Renderer>().enabled = remainder > .05f;
        }
        else if (isReal)
        {
         //   Debug.Log("Hey I'm real!");
         //   Projectile x = Instantiate(angelProjObject, transform.position, transform.rotation) as Projectile;
            Destroy(gameObject);
        }
        else
        {
        //    Debug.Log("Am I real? " + isReal);
            Destroy(gameObject);
        }
    }

    public void real()
    {
        isReal = true;
    }
}
