using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* PlayerScript - Handle Input from a player */

class ArrowKeys
{
     private static string[] buttonNames = { "Up", "Right", "Down", "Left" };
     private static Vector2[] directionVector = { Vector2.up,
                                                  Vector2.right,
                                                  Vector2.down,
                                                  Vector2.left }; 
     private static float tapSpeed = .25f;
     private List<int> keysPressed = new List<int>();
     private float[] lastTapTimes = new float[4];
     private bool[] dashing = new bool[4];

     public void Update()
     {
          for (int i = 0; i < 4; i++)
          {
               string name = buttonNames[i];

               //Regular movement
               if (Input.GetButton(name))
               {
                    if (!keysPressed.Contains(i))
                    {
                         keysPressed.Add(i);
                    }
               }
               else
               {
                    keysPressed.Remove(i);
               }

               //Dashing
               if (Input.GetButtonDown(name))
               {
                    dashing[i] = Time.time - lastTapTimes[i] < tapSpeed;
                    lastTapTimes[i] = Time.time;
               }
               else
               {
                    dashing[i] = false;
               }
          }
     }
     
     public Vector2 GetMovementDirection()
     {
          if (IsDashing())
          {
               return directionVector[DashingDirection()];
          }
          else
          {
               Vector2 v = Vector2.zero;
               foreach (int direction in keysPressed)
               {
                    v += directionVector[direction];
               }
               return v.normalized;
          }
     }

     //Only call if getMovementDirection() != Vector2.zero
     public Vector2 GetFacing()
     {
          if (IsDashing ())
          {
               return directionVector[DashingDirection()];
          }
          else
          {
               int lastKeyPressed = keysPressed[keysPressed.Count-1];
               return directionVector[lastKeyPressed];
          }
     }

     public bool IsDashing()
     {
          return dashing[0] || dashing[1] ||
                 dashing[2] || dashing[3];
     }
     
     private int DashingDirection()
     {
          return dashing[0] ? 0 :
                 dashing[1] ? 1 :
                 dashing[2] ? 2 :
                              3;
     }
}

public class Player : MonoBehaviour
{
     //Components
     private PlayerMoveController playerMoveController;
     private AttackController attackController;
     public Health health;
     public bool gotAttacked;

     //Player progression
     public bool IsBowUnlocked;
     public bool UpgradedBow;
     public bool IsOtherWeaponsUnlocked;
     LoadAndSaveManager.GameStateData.PlayerData DataAboutPlayer;
     public enum SecondaryWeapons { Projectile, Mine };
     public SecondaryWeapons ChosenWeapon;

     //Double tap flags
     private float ButtonCooler;
     private int ButtonCount;
     private float lastTapTimeW;
     private float lastTapTimeS;
     private float lastTapTimeA;
     private float lastTapTimeD;

     private ArrowKeys arrowKeys = new ArrowKeys();

     //Timers
     public bool isInvincible;
     public bool PlayerToGodMode;
     public float timeSpentInvincible;
     public float attackedTimer;
     public float TimeBowCharging;
     public float BaseTime;
     private float stun_Timer, slow, slow_Timer;


     void Awake()
     {
          PlayerToGodMode = false;
          stun_Timer = 0;
          isInvincible = false;
          timeSpentInvincible = 1;
          playerMoveController = GetComponent<PlayerMoveController>();
          attackController = GetComponent<AttackController>();
          GameManager.Notifications.AddListener(this, "LevelLoaded");
          GameManager.Notifications.AddListener(this, "PrepareToSave");
          GameManager.Notifications.AddListener(this, "UnlockBow");
          GameManager.Notifications.AddListener(this, "UnlockPowerShot");
          GameManager.Notifications.AddListener(this, "UnlockDash");
          GameManager.Notifications.AddListener(this, "UnlockGrenade");
          GameManager.Notifications.AddListener(this, "UpgradeDashSpeed");
          ChosenWeapon = SecondaryWeapons.Projectile;
     }

     public void LevelLoaded()
     {
          DataAboutPlayer = GameManager.StateManager.GameState.Player;
          IsBowUnlocked = DataAboutPlayer.IsBowUnlocked;
          UpgradedBow = DataAboutPlayer.IsBowHoldDownUnlocked;
          playerMoveController.SetDashLockState(DataAboutPlayer.IsDashUnlocked);
          IsOtherWeaponsUnlocked = DataAboutPlayer.IsLandMineUnlocked;
          playerMoveController.setDashSpeed(DataAboutPlayer.DashSpeed);


     }

     public void PrepareToSave()
     {
          DataAboutPlayer.IsBowUnlocked = IsBowUnlocked;
          DataAboutPlayer.IsBowHoldDownUnlocked = UpgradedBow;
          DataAboutPlayer.IsDashUnlocked = playerMoveController.GetDashLockState();
          DataAboutPlayer.IsLandMineUnlocked = IsOtherWeaponsUnlocked;
          DataAboutPlayer.DashSpeed = playerMoveController.getDashSpeed();
          GameManager.StateManager.GameState.Player = DataAboutPlayer;
     }


     // Update is called once per frame
     void Update()
     {
          if (Time.deltaTime == 0)
          {
               //Game is paused.
               return;
          }

          if(PlayerToGodMode == false)
          {
               if (isInvincible)
               {
                    timeSpentInvincible += Time.deltaTime;

                    if (timeSpentInvincible <= 3f)
                    {
                         float remainder = timeSpentInvincible % .3f;
                         GetComponent<Renderer>().enabled = remainder > .15f;
                    }
                    else
                    {
                         isInvincible = false;
                         timeSpentInvincible = 0;
                         GetComponent<Renderer>().enabled = true;
                    }
               }
          }
          //Check for the invincibility timer. If the player is invincible, add to the timer. When the timers over, reset the flags


          //The player acts according to input
          if (slow_Timer > 0)
          {
               stun_Timer -= Time.deltaTime;
          }
          else if (slow != 0)
          {
               playerMoveController.speed = 3.1f;
               slow = 0;
          }

          if (stun_Timer > 0)
          {
               playerMoveController.Move(0, 0);
               stun_Timer -= Time.deltaTime;
          }
          else
          {
               HandlePlayerActions();
          }
     }

     private void HandlePlayerActions()
     {
          /* Player Input */
          if (Input.GetButtonDown("Switch") && IsOtherWeaponsUnlocked == true)
          {
               if (ChosenWeapon == SecondaryWeapons.Projectile)
               {
                    ChosenWeapon = SecondaryWeapons.Mine;
                    GameManager.Notifications.PostNotification(this, "BombSelected");
               }
               else if (ChosenWeapon == SecondaryWeapons.Mine)
               {
                    ChosenWeapon = SecondaryWeapons.Projectile;
                    GameManager.Notifications.PostNotification(this, "Projectileselected");
               }
          }

          //Check for attack input
          if (Input.GetButtonDown("Fire1") && attackController.CanAttack())
          {
               attackController.Attack();
          }

          if (Input.GetButtonDown("Fire2"))
          {
               if (ChosenWeapon == SecondaryWeapons.Projectile)
               {
                    if (BaseTime == 0)
                    {
                         BaseTime = Time.time;
                    }
               }
               if (ChosenWeapon == SecondaryWeapons.Mine)
               {
                    attackController.ThrowBomb();
               }

          }

          if (Input.GetButtonUp("Fire2") && attackController.CanAttack() && IsBowUnlocked == true)
          {
               if (ChosenWeapon == SecondaryWeapons.Projectile)
               {
                    TimeBowCharging = Time.time;
                    double timeDifference = TimeBowCharging - BaseTime;
                    if (timeDifference < 1.0f)
                    {
                         attackController.ShootProjectile();
                    }
                    else
                    {
                         attackController.ShootProjectile(3);
                    }
                    BaseTime = 0;
               }
          }

          arrowKeys.Update();
          Vector2 direction = arrowKeys.GetMovementDirection();
          playerMoveController.Move(direction);
          if (direction != Vector2.zero)
          {
               //Handle double taps for dashing
               playerMoveController.Face(arrowKeys.GetFacing());
               if (arrowKeys.IsDashing())
               {
                    playerMoveController.Dash();
               }
          }
          
          if (Input.GetButtonDown("Jump"))
          {
               playerMoveController.Dash();
          }
     }

     public void setStun(float st)
     {
          stun_Timer = st;
     }

     public void setSlow(float newSpeed, float slowTime)
     {
          if (slow == 0)
          {
               slow_Timer = slowTime;
               slow = newSpeed;
               playerMoveController.speed *= slow;
          }
     }

     //Player Progression
     public void UnlockBow()
     {
          IsBowUnlocked = true;
     }

     public bool GetIsBowUnlocked()
     {
          return IsBowUnlocked;
     }
     public void UnlockPowerShot()
     {
          UpgradedBow = true;
     }

     public void UnlockDash()
     {
          playerMoveController.SetDashLockState(true);
     }
     public void UnlockGrenade()
     {
          IsOtherWeaponsUnlocked = true;
     }

     public bool GetIsOtherWeaponsUnlocked()
     {
          return IsOtherWeaponsUnlocked;
     }
     public void UpgradeDashSpeed()
     {
          playerMoveController.setDashSpeed(1);
     }
}

