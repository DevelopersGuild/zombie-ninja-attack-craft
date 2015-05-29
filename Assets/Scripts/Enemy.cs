using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public Player player;
    public float AgroRange;
    public EnemyMoveController moveController;
    public bool isInvincible, blink;
    public float timeSpentInvincible, stunTimer;

    [HideInInspector]
    public float currentX, currentY, playerX, playerY, angle;

    [HideInInspector]
    public Vector2 direction;

    private double t;
    System.Random rnd;

    void Awake() {
        blink = false;
        isInvincible = false;
        timeSpentInvincible = 0;
    }

    void Start() {

    }

    void Update()
    {

    }

    public void checkInvincibility()
    {
        if (isInvincible)
        {
            timeSpentInvincible += Time.deltaTime;

            if (timeSpentInvincible > 0.2f)
            {
                setStun(0.2f);
            }

            if (timeSpentInvincible <= 0.4f)
            {
                blink = !blink;
                GetComponent<Renderer>().enabled = blink;
            }
            
            else
            {
                isInvincible = false;
                timeSpentInvincible = 0;
                GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void idle(double someDub)
    {
        t = someDub;
        rnd = new System.Random();
        if (t < 1)
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude != 0)
            {
                //speed = new Vector2 (0, 0);
                moveController.Move(0, 0);
                t = 3;
            }

        }
        else if (t < 2 && t > 1.3)
        {
            int rand = rnd.Next(1, 5);
            if (rand == 1)
            {
                //speed = new Vector2 (2, 0);
                moveController.Move(1, 0, 5);

                t = 1.3;
            }
            else if (rand == 2)
            {
                //speed = new Vector2 (-2, 0);
                moveController.Move(-1, 0, 5);
                t = 1.3;
            }
            else if (rand == 3)
            {
                //speed = new Vector2 (0, 2);
                moveController.Move(0, 1, 5);
                t = 1.3;
            }
            else
            {
                //speed = new Vector2 (0, -2);
                moveController.Move(0, -1, 5);
                t = 1.3;
            }
        }
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

    public bool checkStun()
    {
        return (stunTimer > 0);
    }

    public void setStun(float st)
    {
        stunTimer = st;
    }


    public void onDeath()
    {
        //death stuff for sub classes
    }
}

