using System;
using UnityEngine;

/* Robo, 2 heads, do not move
 * Has a preparation animation before each attack, only continue attack if not hit during preparation.
 * Preparation goes faster as health lowers
 * 5hp each?
 * gap between player and boss, only can hit with arrows
 * attacks:
 * 1) Acid Ball - shoots(lobs) ball of acid that leaves a pool of acid for 3 seconds. Slows and damages player
 * 2) Summon Snakes - Spawn 2(3?) snake enemies with basic hugger AI and a short leap(?) attack
 *      -use only if the player has no more arrows, each snake drops an arrow.
 * 3) Bite - Extends neck and bites the player
 * 4) Laser - Arc laser, does not hit corners or small area in center.
 * 
 * One snake fire, one ice
 * Fire Only:
 * 1) Fireball - 2dmg, small, med spd projectile.
 * 2) Fire trail - Shoots a line of fire that stays. 1dmg, 3s duration
 * 
 * Ice Only:
 * 1) Iceball - 1dmg, slows (stuns?), med spd projectile
 * 2) Ice trail - Shoots a line of ice that stays. 1dmg, slows, 5s duration
 * 
 * If fireball and iceball hit each other, explode into larger explosion of fire and ice. 
 * 2dmg, slows(stuns?), leaves a damaging area for 2s.
 * 
 * on death, spawn bridge to cross gap, door behind boss
 */
public class SnakeBoss : Boss
{
     public Player player;
     public float AgroRange;

     public FireChain laserObj;
     public FireChain laser;

     public TrailProj trailObj;
     private TrailProj trail;

     public Projectile ballObj;
     private Projectile ball;

     public Enemy snakeObj;
     private Enemy snake;

     public ProjectileTerrain acidballObj;
     private ProjectileTerrain acidball;

     [HideInInspector]
     public Animator animator;

     public SnakeBall ball1, ball2, ball3, ball4;
     [HideInInspector]
     public SnakeBall b1, b2, b3, b4;

     public Vector2 targetPos;
     public Vector3 biteDir;
      
     [HideInInspector]
     public float snakeFactor, mDeg, biteTime, attackDelay;

     [HideInInspector]
     public EnemyMoveController moveController;
     [HideInInspector]
     public Health health;

     [HideInInspector]
     public bool isAgro,isBiting, isLasering;
     [HideInInspector]
     public System.Random rnd;

     [HideInInspector]
     public Vector2 distance, speed, direction;
     //Boss should have high knockback
     //Move order?
     //Fire Snake - Bite -> Spawn Snakes -> -> Laser -> Acid Ball -> fire trail -> fireball
     //Ice Snake - Acid Ball -> Spawn Snakes -> Laser -> Bite -> ice trail -> iceball

     //????Bite -> Spawn Snakes -> Acid Ball -> Fire/Ice trail -> Fire/Ice ball -> Laser
     //small room

     //private Animator animator;

    

     [HideInInspector]
     public Vector2 initialPos;

     [HideInInspector]
     public float currentX, currentY, playerX, playerY, angle, mirrorSpawn;

     [HideInInspector]
     public float bite_CD, spawn_CD, acid_CD, fireBall_CD, iceBall_CD, fireTrail_CD, iceTrail_CD, laser_CD, cooldown_CD, count;

     public void Start()
     {
          //animator = GetComponent<Animator>();
          player = FindObjectOfType<Player>();
          moveController = GetComponent<EnemyMoveController>();
          health = GetComponent<Health>();

          isInvincible = true;
          bite_CD = 6;
          spawn_CD = 5;
          acid_CD = 8;
          fireBall_CD = 6;
          fireTrail_CD = 10;
          iceBall_CD = 6;
          iceTrail_CD = 10;
          laser_CD = 13;
          cooldown_CD = 0.8f;
          biteTime = 0;
          count = 1;

          distance = new Vector2(0, 0);
          speed = new Vector2(0, 0);
          isAgro = false;

     }

     public void Awake()
     {

          Vector3 scale = new Vector3(0.5f,0.5f,0);
          b1 = Instantiate(ball1, transform.position + new Vector3(0, 0.4f, 0), transform.rotation) as SnakeBall;
          b2 = Instantiate(ball2, transform.position + new Vector3(0, 0.5f, 0), transform.rotation) as SnakeBall;
          b3 = Instantiate(ball3, transform.position + new Vector3(0, 0.6f, 0), transform.rotation) as SnakeBall;
          b4 = Instantiate(ball4, transform.position + new Vector3(0, 0.7f, 0), transform.rotation) as SnakeBall;

          //1, 1.3, 1.5, 1.35
          b1.setPong(0.5f);
          b2.setPong(0.8f);
          b3.setPong(1.05f);
          b4.setPong(0.85f);

          initialPos = transform.position;

     }


     public int currentHp()
     {
          return health.currentHealth;
     }

     public void prep()
     {
          isInvincible = false;
          //playAnimation, speed based on hp
          isInvincible = true;

     }

     public void biteAttack()
     {

          //After prep
          //Either
          //Fire Projectile that looks like snake + z+1
          //or
          //Create a variable collider during animation
          findPos();
          biteDir = direction/4;
          float diff = initialPos.y - playerY;
          //mDeg = 180;

          b1.setBite(true, biteDir * 4f/5);
          b2.setBite(true, biteDir * 3f/5);
          b3.setBite(true, biteDir * 2f/5);
          b4.setBite(true, biteDir/5);
          isBiting = true;
          bite_CD = 0;


     }

     public void laserAttack()
     {
          //After prep
          //animation

          laser = Instantiate(laserObj, transform.position, transform.rotation) as FireChain;
          laser.setLaserOne(190, 255);
          //create laser
          //after 0.5s, rotate around point from ~190 degrees to ~255 degrees
          //Ice snake mirrors that, from ~350 to ~285
          //laser ends
     }

     public void trailAttack()
     {
          //After prep
           Vector2 tempDir = new Vector2(playerX - currentX, playerY - currentY);
          float angle = Mathf.Atan2(tempDir.y, tempDir.x) * Mathf.Rad2Deg - 90;
          var q = Quaternion.AngleAxis(angle, Vector3.forward);
          trail = Instantiate(trailObj, transform.position, q) as TrailProj;
          trail.setDir(angle, direction, q);
          //if fire, shoot fire in an arc/cone shape on ground
          //if ice, shoot ice in a rectangle on ground
          //trails^
          fireTrail_CD = 0;
          iceTrail_CD = 0;
     }

     public void ballAttack()
     {
          ball = Instantiate(ballObj, transform.position, transform.rotation) as Projectile;
          ball.Shoot(0, direction * 0.7f);
          fireBall_CD = 0;
          iceBall_CD = 0;
     }

     public void spawnAttack()
     {
          spawn_CD = 0;
          Vector2 newPos = transform.position + new Vector3(mirrorSpawn, -2.4f);
          snake = Instantiate(snakeObj, newPos, transform.rotation) as Enemy;
     }

     public void acidAttack()
     {
          
          Vector2 tempDir = new Vector2(playerX - currentX, playerY - currentY);
          float angle = Mathf.Atan2(tempDir.y, tempDir.x) * Mathf.Rad2Deg - 90;
          var q = Quaternion.AngleAxis(angle, Vector3.forward);
          acidball = Instantiate(acidballObj, transform.position, q) as ProjectileTerrain;
          acidball.Shoot(0, tempDir * 0.125f);
          acid_CD = 0;
     }


     public void findPos()
     {
          currentX = transform.position.x;
          currentY = transform.position.y;
          playerX = player.transform.position.x;
          playerY = player.transform.position.y;

          angle = Vector2.Angle(player.transform.position, transform.position);
          direction = new Vector2(playerX - currentX, playerY - currentY);
          direction = direction.normalized;
     }

     public void updatePos()
     {
          
//          transform.position = new Vector3(initialPos.x + (float)Math.Sin(snakeFactor % Math.PI),initialPos.y,0);
          //snakeFactor = Mathf.PingPong(Time.time / 2.2f, 0.7f);
          //transform.position = new Vector3(initialPos.x + snakeFactor, initialPos.y, 0);

          float degreesPerSecond = 180.0f;
          mDeg = Mathf.Repeat(mDeg + (Time.deltaTime * degreesPerSecond), 360.0f);
          float radians = mDeg * Mathf.Deg2Rad + 90;

          Vector3 offset = new Vector3(Mathf.Sin(radians)/110f, 0, 0);
          transform.position = transform.position + offset;
          //snakeFactor += 0f;
     }

     public void laserEnd()
     {
          isLasering = false;
          b1.stopMove(true);
          b2.stopMove(true);
          b3.stopMove(true);
          b4.stopMove(true);
     }

     public void onDeath()
     {
          b1.dead();
          b2.dead();
          b3.dead();
          b4.dead();
          //create bridge
     }

     public void open()
     {
          animator.SetBool("isOpen", true);
          isInvincible = false;
     }

     public void close()
     {
          attackDelay = 2.2f;
          animator.SetBool("isOpen", false);
     }

     public void OnCollisionEnter2D(Collision2D other)
     {
          if (!isInvincible)
          {
               if (other.gameObject.GetComponent<DealDamageToEnemy>() && other.gameObject.GetComponent<Projectile>()) 
               {
                    isInvincible = true;
               }
          }
     }




}
