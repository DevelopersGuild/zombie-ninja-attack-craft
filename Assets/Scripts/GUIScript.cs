using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour
{

     public Player player;
     public UILabel healthLabel;
     public UILabel scoreLabel;
     public UILabel ammoLabel;

     public UISprite bow;
     public UISprite bomb;
     private bool usingArrow;


     Health playerHealth;
     AttackController playerAttackController;

     // Use this for initialization
     void Start()
     {
          usingArrow = true;
          player = FindObjectOfType<Player>();
          playerHealth = player.GetComponent<Health>();
          playerAttackController = player.GetComponent<AttackController>();
          bow.enabled = false;
          bomb.enabled = false;
          GameManager.Notifications.AddListener(this, "BombSelected");
          GameManager.Notifications.AddListener(this, "Projectileselected");
     }

     // Update is called once per frame
     void Update()
     {
          healthLabel.text = playerHealth.currentHealth.ToString();
          scoreLabel.text = GameManager.getCoins().ToString();

          if(player.IsBowUnlocked == false && player.IsOtherWeaponsUnlocked == false)
          {
               bow.enabled = false;
               ammoLabel.enabled = false;
          }
          else
          {
               bow.enabled = true;
               ammoLabel.enabled = true;
          }

          if(usingArrow && player.IsBowUnlocked == true)
          {
               ammoLabel.enabled = true;
               ammoLabel.text = playerAttackController.Ammo.ToString();
               bow.enabled = true;
               bomb.enabled = false;
          }
          else if (player.IsOtherWeaponsUnlocked)
          {
               ammoLabel.enabled = true;
               ammoLabel.text = playerAttackController.Grenades.ToString();
               bow.enabled = false;
               bomb.enabled = true;
          }
          
     }

     public void BombSelected()
     {
          usingArrow = false;
     }

     public void Projectileselected()
     {
          usingArrow = true;
     }
}
