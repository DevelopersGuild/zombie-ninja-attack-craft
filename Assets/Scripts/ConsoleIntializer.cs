using UnityEngine;
using System.Collections;

public class ConsoleIntializer : MonoBehaviour
{
     private Player player;
     // Use this for initialization
     void Start()
     {
          
          if(GameObject.FindGameObjectWithTag("Player") != null)
          {
               player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
               var repo = ConsoleCommandsRepository.Instance;
               repo.RegisterCommand("help", Help);
               repo.RegisterCommand("godmode", GodMode);
               repo.RegisterCommand("givemeweapons", GiveMeWeapons);
               repo.RegisterCommand("givemeammo", GiveMeAmmo);
          }

     }

     public string Help(params string[] args)
     {
          return "";//"godmode -- Makes player invincible\n" +
               //"givemeweapons -- Unlocks all of the weapons and upgrades\n" +
               //"givemeammo -- gives the player 100 of each type of ammo";
     }

     public string GodMode(params string[] args)
     {
          if (player.isInvincible == false)
          {
               player.PlayerToGodMode = true;
               player.isInvincible = true;
               return "GodMode on";
          }
          else
          {
               player.PlayerToGodMode = false;
               player.isInvincible = false;
               return "GodMode off";
          }
     }

     public string GiveMeWeapons(params string[] args)
     {
          
          player.UnlockBow();
          player.UnlockPowerShot();
          player.UnlockGrenade();
          player.UnlockDash();
          return "Weapons Unlocked";
     }

     public string GiveMeAmmo(params string[] args)
     {
          player.GetComponent<AttackController>().SetPlayerProjectileAmmo(100);
          player.GetComponent<AttackController>().SetPlayerGrenadeAmmo(100);
          return "Ammo given to player";
     }


}
