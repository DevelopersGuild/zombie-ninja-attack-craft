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
    private FireChain laser;

    public TrailProj trailObj;
    private TrailProj trail;

    public Projectile ballObj;
    private Projectile ball;

    public Enemy snakeObj;
    private Enemy snake;

    public Projectile acidballObj;
    private Projectile acidball;

    public Projectile acidFieldObj;
    private Projectile acidField;

    public Vector2 targetPos;

    [HideInInspector]
    public EnemyMoveController moveController;
    [HideInInspector]
    public Health health;

    [HideInInspector]
    public bool isAgro;
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
    public float currentX, currentY, playerX, playerY, angle;

    [HideInInspector]
    public float bite_CD, spawn_CD, acid_CD, fireBall_CD, iceBall_CD, fireTrail_CD, iceTrail_CD, laser_CD, cooldown_CD;

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

        distance = new Vector2(0, 0);
        speed = new Vector2(0, 0);
        isAgro = false;

    }


    public int currentHp()
    {
        return health.currentHealth;
    }

    public void prep()
    {
        isInvincible = false;
        //playAnimation
        isInvincible = true;

    }

    public void biteAttack()
    {
        //After prep
        //Either
        //Fire Projectile that looks like snake + z+1
        //or
        //Create a variable collider during animation
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
        trail = Instantiate(trailObj, transform.position, transform.rotation) as TrailProj;
        trail.setDir(angle, direction);
        //if fire, shoot fire in an arc/cone shape on ground
        //if ice, shoot ice in a rectangle on ground
        //trails^
    }

    public void ballAttack()
    {
        ball = Instantiate(ballObj, transform.position, transform.rotation) as Projectile;
        ball.Shoot(0, direction * 0.7f);
    }

    public void spawnAttack()
    {
        Vector2 newPos = (Vector2)transform.position + new Vector2(0, -0.8f);
        snake = Instantiate(snakeObj, newPos, transform.rotation) as Enemy;

    }

    public void acidAttack()
    {
        acidField = Instantiate(acidFieldObj, transform.position, transform.rotation) as Projectile;
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




}
