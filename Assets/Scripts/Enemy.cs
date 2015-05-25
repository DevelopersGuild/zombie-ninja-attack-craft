using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public Player player;
    public float AgroRange;
    public EnemyMoveController moveController;

    private double t;
    System.Random rnd;

    void Awake() {

    }

    void Start() {

    }

    void Update()
    {

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

    public void onDeath()
    {
        //death stuff for sub classes
    }
}

