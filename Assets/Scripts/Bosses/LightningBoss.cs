using System;
using UnityEngine;

//Should the boss have an invincibility timer?
//Lightning boss looks like what? Electrified robot? an electric entity? Some dude doping on an electric drug?
//Lightning boss, has 9 attacks:
//At 50% hp, The Lightning boss supercharges himself and his moves gain extra abilities 
//and he gains the ability to use the move Thunder. He also moves at an increased speed.
//Extra abilities are indicated by a star
//1) Overload - Creates an electric field around himself that deals damage and knocks back the player, close range
//   *Larger electric field is created*
//2) Normal Attack - Punches in front if the player is there and knocks back, 
//   close range and slow so the player can hit first 
//   *Knocks back more*
//3) Lightning storm - Field effect that makes lightning strike at random areas in the room, 
//   indicates where before striking, whole room range 
//   *Less cooldown in between lightning strikes*
//4) Lightning Bolt - A very fast bolt of electricity that's shot at the player. Does not deal damage or knockback.
//   stuns the player which makes the next move the Boss uses harder to dodge. long range
//   *Bolt stuns for a bit longer*
//5) *Thunder* - Indicates a large (radius 1) area that after 2 seconds gets struck by
//   a flash of white light that covers the screen. After that, the area is electrified for 3 seconds, dealing damage
//   if the player steps in the area. long range.
//6) Electric Ball - A ball of electricity that is shot at the player. Medium speed, long range, 
//   deals damage and knockback.
//   *Slightly bends toward the player, is a bit faster*
//7) Electric Field - A hook that is impossible to escape is shot to the right of the player and gets lodged into the wall.
//   Then another hook is readied, then thrown at a dodgeable speed to the left of the player. A field of electricity is
//   created in between the two hooks. 
//   *Hooks go faster, so harder to dodge*
//8?) Lightning Speed - The Boss electrifies himself, then instantly appears in melee range of the player (random angle).
//   The boss the takes 0.5s(?) to hit you and then appears back at his starting point. If the player hits him, he is
//   knocked out of this state and does not attack, but instead immediately appears at his starting point.
//   *Boss takes less time to hit the player*
//9) Lightning Charge - The Boss electrifies himself, then charges at the player at an extreme, but dodgeable, speed.
//   If the player is hit, he is stunned for 1.5s. The Boss is stunned for 0.5s after collision with anything, and then
//   gains a 3s speed boost of 2x speed to be able to escape trouble. Can collide with poles.
//   *Cooldown increases, 3x speed boost, and the boss has a small electric field around him when he charges*

//Boss hovers slowly away from player when not doing anything else
//If there is a wall (<0.5unit) behind the boss, he will instead move toward another corner of the map that is
//either closest in delta y or delta x 
//(so depending on where the wall is, if it's to his left he will go up or down, if it's below him he will go right or left)
//always mvoes away from the player (when deciding up or down, left or right)
// 4 invis objects without colliders placed at each corner, so the boss knows where the corners are
public class LightningBoss : Boss
{
    public Player player;
    public float AgroRange;
    // public Chain chainObj, chainObjBlock;
    public Landmine overloadObject;
    private Landmine overload;

    public BasicAttack normalAttackObject;
    private BasicAttack normalAttack;

    public Projectile boltObj, shotObj;
    private Projectile bolt, shot;

    public ZigZagProj sparkObj;
    private ZigZagProj sparkC, sparkU, sparkD;

    public LightningField fieldObj;
    private LightningField field;

    public Indicator indicObj;
    private Indicator indic;

    public GameObject pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9;
    private Vector2[] posArr;
    private Vector2 targetPos;


    private EnemyMoveController moveController;
    private Health health;

    private bool isAgro, supercharged;

    System.Random rnd;

    private Vector2[] rotArr;

    private Vector2 distance, speed, direction;
    private double norm_CD, temp, cd_Reduction;
    private double shot_CD, overload_CD, spark_CD, speed_CD, thunder_CD, cooldown_CD, bolt_CD, lightning_CD;
    //Boss should have high knockback
    //slow normal attack, player can hit first
    //more hp though, so player can't just fist fight to death
    private double overload_Range;
    //Move order?
    //overload -> normal attack if in range -> lightning storm (effect) -> lightning bolt projectile(to set up other moves)
    //-> thunder (put bolt here instead? depends on thunder activation speed)-> shot -> field -> speed -> charge
    //close-range -> keep at range -> melee
    //Should charge leave an electrified trail?
    //electric gates all over the room? don't affect boss, only player
    //electric gate- no rigid body, yes box collider, no hp, yes deal dmg to player
    //^poles same thing, just no deal dmg to player^
    //0.5s (1.1s?) cd between moves
    //Room size? 5x5? make camera bigger please

    //private Animator animator;

    // private double shot_CD, charge_CD, overload_CD, field_CD, speed_CD, thunder_CD, cooldown_CD, more;
    private float bolt_Stun, hook_Speed, knockback, speedBoost, storm_CD;
    private float currentX, currentY, playerX, playerY, angle;


    public Player player;
    public float AgroRange;
    public Projectile chainObj, chainObjBlock;

    private EnemyMoveController moveController;
    private Health health;

    private bool isAgro;

    System.Random rnd;

    private Vector2 distance, speed, facing;
    private double t, temp;
    private double shot_CD, charge_CD, overload_CD, field_CD, speed_CD, thunder_CD, cooldown_CD, more;
    //supercharged at below 50% health, abilities gain more range and sometimes new abilities or speed;
    //Below 50%:
    //shot slowly follows (bends) towards player 
    //Uses thunder
    //overload has increased field
    //field move goes faster (proj speed?)
    //speed move harder to dodge
    //charge goes faster
    //base speed increases?
    //Boss should have high knockback
    //slow normal attack, player can hit first
    //more hp though, so player can't just fist fight to death
    //runs away at some Coins?
    private double overload_Range, charge_Spd, field_Spd, speed_Return;
    //Move order?
    //overload -> normal attack if in range -> lightning storm (effect) -> lightning bolt projectile(to set up other moves) -> thunder (put bolt here instead? depends on thunder activation speed)-> shot -> field -> speed -> charge
    //close-range -> keep at range -> melee
    //Should charge leave an electrified trail?
    //electric gates all over the room? don't affect boss, only player
    //electric gate- no rigid body, yes box collider, no hp, yes deal dmg to player
    //^poles same thing, just no deal dmg to player^
    //lightning storm - mini lightnings that don't leave electrified areas?
    //super fast projectile, lightning bolt, stuns if hit, deals no damage (makes it easy to hit other stuff)
    //0.5s (1.1s?) cd between moves
    //Room size? 5x5? make camera bigger please

    //private Animator animator;


    public void Start()
    {

        //animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        moveController = GetComponent<EnemyMoveController>();
        health = GetComponent<Health>();
        isInvincible = false;

        posArr = new Vector2[9];
        posArr[0] = pos1.transform.position;
        posArr[1] = pos2.transform.position;
        posArr[2] = pos3.transform.position;
        posArr[3] = pos4.transform.position;
        posArr[4] = pos5.transform.position;
        posArr[5] = pos6.transform.position;
        posArr[6] = pos7.transform.position;
        posArr[7] = pos8.transform.position;
        posArr[8] = pos9.transform.position;

        rotArr = new Vector2[9];
        rotArr[0] = new Vector2(0, -1);
        rotArr[1] = new Vector2(1, 0);
        rotArr[2] = new Vector2(0, 1);
        rotArr[3] = new Vector2(-1, 0);
        rotArr[4] = new Vector2(1, -1);
        rotArr[5] = new Vector2(1, 1);
        rotArr[6] = new Vector2(-1, 1);
        rotArr[7] = new Vector2(-1, -1);
        rotArr[8] = new Vector2(0, -1);

        shot_CD = 6;
        overload_CD = 6;
        spark_CD = 2;
        thunder_CD = 12;
        cooldown_CD = 1.5;
        bolt_CD = 6;
        lightning_CD = 10;
        storm_CD = 1;

        overload_Range = 0.5;
        bolt_Stun = 1.5f;
        hook_Speed = 8;
        knockback = 2;
        speedBoost = 2;
        cd_Reduction = 0;

        distance = new Vector2(0, 0);
        speed = new Vector2(0, 0);
        isAgro = false;
        norm_CD = 1.5;

    }

    public void Update()
    {
        isInvincible = false;
        if (player != null)
        {
            //find position after animation? will it use the position from before the animation starts? be ready to change
            findPos();

            if (!supercharged && currentHp() <= health.startingHealth / 2)
            {
                supercharged = true;
                cd_Reduction = 3;
                overload_Range *= 1.5;
                bolt_Stun += 0.5f;
                hook_Speed *= 1.5f;
                knockback += 1.5f;
                speedBoost++;
                storm_CD = 0.6f;

            }
            rnd = new System.Random();


            //if stunned = true, wait 0.5s, then gain a speedBoost for 3s
            //else

            distance = player.transform.position - transform.position;
            if (distance.magnitude <= AgroRange)
            {
                isAgro = true;
            }
            if (distance.magnitude > AgroRange)
            {
                isAgro = false;
            }

            if (isAgro)
            {
                //targetPos *= 0.8f;
                if (cooldown_CD > 1)
                {
                    cooldown_CD = 0;
                    //faster the closer you are from him, probably better ways to do this but I don't want to look it up
                    float xSp = 2 * (AgroRange - (player.transform.position.x - transform.position.x));
                    float ySp = 2 * (AgroRange - (player.transform.position.y - transform.position.y));
                    //targetPos = new Vector2(0, 0);
                    moveController.Move(0, 0);
                    //basic aggression range formula
                    if (distance.magnitude < overload_Range && overload_CD > 12 - cd_Reduction)
                    {
                        overloadAttack();
                    }
                    else if (distance.magnitude < 0.3 && norm_CD > 1.5)
                    {
                        normAttack();
                    }
                    else if (lightning_CD > 10)
                    {
                        //animation
                        //lightning storm, player parameter or getObject(player)
                        //take in either bool supercharged or cooldown
                        //field = new LightningField();

                        field = Instantiate(fieldObj, player.transform.position, transform.rotation) as LightningField;
                        field.set(0.7f, storm_CD);
                        lightning_CD = 0;
                        //dark shadow below player?
                    }
                    else if (bolt_CD > 10 - cd_Reduction)
                    {
                        boltAttack();
                    }
                    //overload -> normal attack if in range -> lightning storm (effect) -> lightning bolt projectile(to set up other moves)
                    //-> thunder (put bolt here instead? depends on thunder activation speed)-> shot -> field -> speed -> charge
                    else if (supercharged && thunder_CD > 20)
                    {
                        //animation
                        //thunder, parameter player or getobject
                        //in thunder class, create black white black screen around player
                        //indic = new Indicator();

                        //indic.set or indicObj.set?
                        indic = Instantiate(indicObj, player.transform.position, transform.rotation) as Indicator;
                        indic.set(2);
                        thunder_CD = 0;
                    }
                    else if (shot_CD > 10 - cd_Reduction)
                    {
                        shotAttack();
                    }
                    else
                    {
                        //if wall within 0.2 units of one corner, move to another corner clockwise
                        //else
                        //if wall(not pole) to the left or right, disregard x velocity and increase y velocity
                        //if wall(not pole) above or below, disregard y velocity and increase x velocity
                        //else move away from player
                        cooldown_CD = 0.8f;
                    }
                    if (spark_CD > 1 + currentHp() / 10.0)
                    {
                        sparkAttack();
                    }
                }
                //moveController.Move(targetPos);
                // float xSp = player.transform.position.x - transform.position.x;
                // float ySp = player.transform.position.y - transform.position.y;




                overload_CD += Time.deltaTime;
                norm_CD += Time.deltaTime;
                lightning_CD += Time.deltaTime;
                bolt_CD += Time.deltaTime;
                shot_CD += Time.deltaTime;
                thunder_CD += Time.deltaTime;
                cooldown_CD += Time.deltaTime;
                spark_CD += Time.deltaTime;

            }
        }
        //Debug.Log("My pos is " + posArr[9 - currentHp()]);
        findPos();
        transform.position = posArr[9 - currentHp()];
        if (currentHp() > 5)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 * (9 - currentHp()));
        }
        else if (currentHp() > 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45 + 90 * (5 - currentHp()));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public bool getAgro()
    {
        return isAgro;
    }

    public int currentHp()
    {
        return health.currentHealth;
    }

    private void findPos()
    {
        currentX = transform.position.x;
        currentY = transform.position.y;
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;

        angle = Vector2.Angle(player.transform.position, transform.position);
        direction = new Vector2(playerX - currentX, playerY - currentY);
        direction = direction.normalized;
    }

    private void overloadAttack()
    {
        //Animation
        //overload
        overload = Instantiate(overloadObject, transform.position, transform.rotation) as Landmine;
        overload_CD = 0;
    }

    private void normAttack()
    {
        //animation
        //create box collider prefab with deal damage to player in front of boss's facing direction
        //same prefab as charger after charge
        normalAttack = Instantiate(normalAttackObject, transform.position + transform.forward / 5f, transform.rotation) as BasicAttack;
        norm_CD = 0;
    }

    private void boltAttack()
    {
        //animation
        //bolt
        bolt = Instantiate(boltObj, transform.position, transform.rotation) as Projectile;
        bolt.setStun(bolt_Stun);
        //Debug.Log(direction);
        findPos();
        bolt.Shoot(0, direction / 1.5f);
        bolt_CD = 0;
    }

    private void shotAttack()
    {
        //if supercharged, shoot a homer bolt
        //else shoot normal bolt at a direction
        //or one class with a bool
        shot = Instantiate(shotObj, transform.position, transform.rotation) as Projectile;
        shot.home(supercharged);
        findPos();
        //Debug.Log(direction);
        shot.Shoot(0, direction / 4f);
        shot_CD = 0;
    }

    private void sparkAttack()
    {
        spark_CD = 0;
        int numArr = 9 - currentHp();


        Vector3 posC = new Vector3(transform.position.x + rotArr[numArr].x * 0.4f, transform.position.y + rotArr[numArr].y * 0.4f);
        Vector3 posU = new Vector3(transform.position.x + rotArr[numArr].y * -0.3f, transform.position.y + rotArr[numArr].x * -0.3f);
        Vector3 posD = new Vector3(transform.position.x + rotArr[numArr].y * 0.3f, transform.position.y + rotArr[numArr].x * 0.3f);
        sparkC = Instantiate(sparkObj, posC, transform.rotation) as ZigZagProj;
        sparkU = Instantiate(sparkObj, posU, transform.rotation) as ZigZagProj;
        sparkD = Instantiate(sparkObj, posD, transform.rotation) as ZigZagProj;

        sparkU.setRot(-90);
        sparkD.setRot(90);

        sparkC.setDir(rotArr[numArr]);
        sparkU.setDir(rotArr[numArr]);
        sparkD.setDir(rotArr[numArr]);



        Debug.Log("What is going on" + posU + ". It should be " + transform.position);

    }


}

