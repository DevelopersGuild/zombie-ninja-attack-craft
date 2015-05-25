using UnityEngine;
using System.Collections;

public class TrailProj : MonoBehaviour
{

    public Projectile trailObj, trail;
    public float t;

    // Update is called once per frame
    void Update()
    {
        if (t > 0.5)
        {
            t = 0;
            trail = Instantiate(trailObj, transform.position, transform.rotation) as Projectile;
        }

        t += Time.deltaTime;
    }

    public void setDir(Vector2 dir)
    {
        Projectile proj = GetComponent<Projectile>();
        proj.Shoot(0, dir);
    }

    
}
