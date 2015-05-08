using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class LightningBoss : MonoBehaviour
    {
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
        //runs away at some points?
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
            moveController = GetComponent<EnemyMoveController>();
            health = GetComponent<Health>();

            distance = new Vector2(0, 0);
            speed = new Vector2(0, 0);
            isAgro = false;
            t = 3;

        }

        public void Update()
        {
            rnd = new System.Random();

            if (player != null)
            {
                //basic aggression range formula
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

                    float xSp = player.transform.position.x - transform.position.x;
                    float ySp = player.transform.position.y - transform.position.y;

                    moveController.Move(xSp / 15, ySp / 15);

                }

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


    }
}

