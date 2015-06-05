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
     public AudioSource[] AudioSources = new AudioSource[NumberOfAudioSources];

     // Use this for initialization
     void Start()
     {
          GameManager.Notifications.AddListener(this, "CoinPickedUp");
     }

     public void CoinPickedUp()
     {
          Debug.Log("Successful Load of Audio Manager");
     }
}
