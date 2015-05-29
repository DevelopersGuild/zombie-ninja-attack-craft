using UnityEngine;
using System.Collections;

public class TrailProj : Projectile
{

    public Projectile trailObj, trail;
    public float t,trailTime;

    public void start()
    {
        t = 0;
        trailTime = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 0.25)
        {
            t = 0;
            trail = Instantiate(trailObj, transform.position, transform.rotation) as Projectile;
            trail.TimeToLive = trailTime;
            trailTime -= 0.25f;
        }

        t += Time.deltaTime;
    }

    public void setDir(float ang, Vector2 dir)
    {
        Shoot(ang, dir);
    }



    
}
