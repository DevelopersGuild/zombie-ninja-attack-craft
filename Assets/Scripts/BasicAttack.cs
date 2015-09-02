using UnityEngine;
using System.Collections;

public class BasicAttack : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
         //t = 0.7;
    }

    // Update is called once per frame

    void DestroySelf()
    {
        //animation
        Destroy(gameObject);
    }

}
