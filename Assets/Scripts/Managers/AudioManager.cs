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
     private const int NumberOfAudioSources = 10;
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
}
