using UnityEngine;
using System.Collections;

public class shield : MonoBehaviour {

     public ShieldBoss boss;
     public Player player;

     private Vector2 distBetween, lookPos;
     private Quaternion quat;
     private Transform lookDir;
     
	// Use this for initialization
	void Start () {
          player = FindObjectOfType<Player>();

	}
	
	// Update is called once per frame
	void Update () {
          distBetween = player.transform.position - boss.transform.position;
          lookPos = distBetween;
          quat = transform.rotation;
          quat.z = Vector2.Angle(boss.transform.position, player.transform.position);
          transform.position = (Vector2)boss.transform.position + distBetween.normalized * 0.3f;
          //lookDir = transform.LookAt(player.transform.position);


	}

     public void setRot(Quaternion q)
     {
          
          transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
          Debug.Log(transform.rotation + "vs" + Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f));
     }
}
