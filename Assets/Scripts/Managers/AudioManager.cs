using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

     public static AudioManager audioManager
     {
          get
          {
               if (audioController == null)
               {
                    audioController = GameManager.Instance.GetComponent<AudioManager>();
               }
               return audioController;
          }
     }

     private static AudioManager audioController = null;
     private const int NumberOfAudioSources = 13;
     public AudioSource LevelMusic;
     public AudioSource[] AudioSources = new AudioSource[NumberOfAudioSources];

     // Use this for initialization
     void Start()
     {
          LevelMusic.Play();
          GameManager.Notifications.AddListener(this, "CoinPickedUp");
          GameManager.Notifications.AddListener(this, "PlayerWhipAttack");
          GameManager.Notifications.AddListener(this, "PlayerProjectileAttack");
          GameManager.Notifications.AddListener(this, "OnExplosion");
          GameManager.Notifications.AddListener(this, "OnHit");
          GameManager.Notifications.AddListener(this, "OnPlayerDeath");
          GameManager.Notifications.AddListener(this, "OnPlayerDash");
          GameManager.Notifications.AddListener(this, "OnHealthOrBatteryPickup");
          GameManager.Notifications.AddListener(this, "OnKeyPickup");
          GameManager.Notifications.AddListener(this, "OnLightingAttack");
          GameManager.Notifications.AddListener(this, "OnCyclopsShoot");
          GameManager.Notifications.AddListener(this, "OnCyclopsTeleport");
          GameManager.Notifications.AddListener(this, "OnEnemySpark");
     }

     public void CoinPickedUp()
     {
          AudioSources[0].Play();
     }

     public void PlayerWhipAttack()
     {
          AudioSources[1].Play();
     }

     public void PlayerProjectileAttack()
     {
          AudioSources[2].Play();
     }

     public void OnExplosion()
     {
          AudioSources[3].Play();
     }

     public void OnHit()
     {
          AudioSources[4].Play();
     }
     public void OnPlayerDeath()
     {
          AudioSources[5].Play();
     }
     public void OnPlayerDash()
     {
          AudioSources[6].Play();
     }
     public void OnHealthOrBatteryPickup()
     {
          AudioSources[7].Play();
     }
     public void OnKeyPickup()
     {
          AudioSources[8].Play();
     }
     public void OnLightingAttack()
     {
          AudioSources[9].Play();
     }
     public void OnCyclopsShoot()
     {
          AudioSources[10].Play();
     }
     public void OnCyclopsTeleport()
     {
          AudioSources[11].Play();
     }
     public void OnEnemySpark()
     {
          AudioSources[12].Play();
     }
}
