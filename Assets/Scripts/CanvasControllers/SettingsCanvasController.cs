using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsCanvasController : MonoBehaviour
{
     private Slider volumeSlider;
     private Toggle soundToggle;
     private AudioManager audioManager;
     private Button backButton;
     private GUIManager guiManager;
     // Use this for initialization
     void Start()
     {
          audioManager = GameManager.Instance.GetComponent<AudioManager>();
          volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();
          volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
          soundToggle = GameObject.Find("Mute").GetComponent<Toggle>();
          soundToggle.onValueChanged.AddListener(delegate { BoolValueChangeCheck(); });
          backButton = GameObject.Find("Back").GetComponent<Button>();
          backButton.onClick.AddListener(delegate { OnBackButtonClicked(); });
          guiManager = GameObject.Find("GUIEventSystem").GetComponent<GUIManager>();
     }

     public void ValueChangeCheck()
     {
          audioManager.ChangeGameVolume(volumeSlider.value);
     }

     public void BoolValueChangeCheck()
     {
          audioManager.MuteSound(soundToggle.isOn);
     }

     public void OnBackButtonClicked()
     {
          guiManager.HideSettingScreen();
     }


}
