using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevelCanvasController : MonoBehaviour
{
     public Button Level1Button;
     public Button Level2Button;
     public Button Level3Button;
     public Button Level4Button;
     public Button Level5Button;
     public Button Level6Button;
     public Button Level7Button;
     public Button Level8Button;
     public Button Level9Button;

     public Text LevelName;
     public Text GoldTimeScoreText;
     public Text PlayerTimeScoreText;
     public Text GoldcombatScoreText;
     public Text PlayerCombatScoreText;


     // Use this for initialization
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {

     }


     public void ButtonSelected(Button selectedButton)
     {
          int numberOfButtonSelected = 0; 
          Text buttonText = selectedButton.GetComponentInChildren<Text>();
          string buttonString = buttonText.text.ToString();
          numberOfButtonSelected = System.Int32.Parse(buttonString);
          DisplayCurrentLevelInfo(numberOfButtonSelected);
          DisplayScores(numberOfButtonSelected);
     }

     public void DisplayCurrentLevelInfo(int levelnum)
     {
          LevelName.text = "Level " + levelnum;
     }

     public void DisplayScores(int levelnum)
     {
          GoldTimeScoreText.text = "Gold Score : " + levelnum;
          PlayerTimeScoreText.text = "High Score : " + levelnum;
          GoldcombatScoreText.text = "Gold Score : " + levelnum;
          PlayerCombatScoreText.text = "High Score : " + levelnum;
     }

}
