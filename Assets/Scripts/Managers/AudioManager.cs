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
     private const int NumberOfAudioSources = 24;
     private static float lastVolumeLevel;
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
          GameManager.Notifications.AddListener(this, "OnHealthOrPickup");
          GameManager.Notifications.AddListener(this, "OnKeyPickup");
          GameManager.Notifications.AddListener(this, "OnLightingAttack");
          GameManager.Notifications.AddListener(this, "OnCyclopsShoot");
          GameManager.Notifications.AddListener(this, "OnCyclopsTeleport");
          GameManager.Notifications.AddListener(this, "OnEnemySpark");
          GameManager.Notifications.AddListener(this, "OnRoomCleared");
          GameManager.Notifications.AddListener(this, "OnBombOrBatteryPickup");
          GameManager.Notifications.AddListener(this, "OnButtonClick");
          GameManager.Notifications.AddListener(this, "OnOtherProjectileFire");
          GameManager.Notifications.AddListener(this, "OnLightningShot");
          GameManager.Notifications.AddListener(this, "OnLightningStrike");
          GameManager.Notifications.AddListener(this, "OnSnakeCry");
          GameManager.Notifications.AddListener(this, "OnfireProjectile");
          GameManager.Notifications.AddListener(this, "OnFireLaser");
          GameManager.Notifications.AddListener(this, "OnSnakePounce");
          GameManager.Notifications.AddListener(this, "OnAlienCry");

     }

     public void MuteSound(bool muteSound)
     {
          if (muteSound == true)
          {
               lastVolumeLevel = AudioListener.volume;
               ChangeGameVolume(0);
          }
          else
          {
               ChangeGameVolume(lastVolumeLevel);
          }
     }

     public void ChangeGameVolume(float volumeLevel)
     {
          if (volumeLevel < 0 || volumeLevel > 1.0)
          {
               volumeLevel = AudioListener.volume;
          }
          AudioListener.volume = volumeLevel;
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

     public void OnHealthOrPickup()
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

     public void OnRoomCleared()
     {
          AudioSources[13].Play();
     }

     public void OnBombOrBatteryPickup()
     {
          AudioSources[14].Play();
     }

     public void OnButtonClick()
     {
          AudioSources[15].Play();
     }
     public void OnOtherProjectileFire()
     {
          AudioSources[16].Play();
     }

     //Boss sounds
     //Lightning Boss
     public void OnLightningShot()
     {
          AudioSources[17].Play();
     }
     public void OnLightningStrike()
     {
          AudioSources[18].Play();
     }
     public void OnSnakeCry()
     {
          AudioSources[19].Play();
     }

     //Snake Boss
     public void OnfireProjectile()
     {
          AudioSources[20].Play();
     }
     public void OnFireLaser()
     {
          AudioSources[21].Play();
     }
     public void OnSnakePounce()
     {
          AudioSources[22].Play();
     }
     public void OnAlienCry()
     {
          AudioSources[23].Play();
     }
}
