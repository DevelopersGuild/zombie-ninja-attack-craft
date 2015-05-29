using UnityEngine;
using DG.Tweening;
using System.Collections;

public class AmmoPickup : Pickup
{

     public int ammoValue;
     private Tween target;
     private bool grabbed;
     private AttackController attackController;


     // Use this for initialization
     void Start()
     {
          ammoValue = Random.Range(1, 4);
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          if (other.gameObject.tag == "Player")
          {
               grabbed = true;
               attackController = other.gameObject.GetComponent<AttackController>();
               attackController.Ammo += ammoValue;

               Tweener tween = transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 100, Camera.main.pixelHeight - 10)), 1, false)
                               .OnStepComplete(() =>
                               {
                                    Destroy(gameObject);
                               });
               //tween.OnUpdate(() => {
               //    tween.ChangeEndValue(new Vector3(Camera.main.pixelWidth - 100, Camera.main.pixelHeight - 10));
               //    Debug.Log("test");
               //});
          }
     }

     public override void AddItemToInventory(Collider2D player, int value)
     {
          attackController = player.gameObject.GetComponent<AttackController>();
          attackController.Ammo += value;
     }
}
