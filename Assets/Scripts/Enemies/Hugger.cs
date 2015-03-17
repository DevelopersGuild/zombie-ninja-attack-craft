using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (Rigidbody2D))]
public class Hugger : MonoBehaviour {

    public Transform target;
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;
    private EnemyMoveController moveController;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance;
    private int currentWaypoint = 0;

	// Use this for initialization
	void Start () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        moveController = GetComponent<EnemyMoveController>();

        if (target == null) {
            Debug.LogError("NO PLAYER!");
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator UpdatePath() {
        if (target == null) {
            //return;
        } 

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());

    }

	// Update is called once per frame
	void FixedUpdate () {
        if (target == null) {
            return;
        }

        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) {
            if (pathIsEnded) return;

            //Debug.Log("END OF PATH REACHED");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //rb.AddForce(dir, fMode);
        //rb.velocity = dir;
        moveController.Move(dir);


        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(dist < nextWaypointDistance){
            currentWaypoint++;
            return;
        }
	}

}
