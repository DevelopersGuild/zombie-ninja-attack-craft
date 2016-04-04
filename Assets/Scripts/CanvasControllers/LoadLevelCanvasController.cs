using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevelCanvasController : MonoBehaviour
{

	public Button[] levelButtons = new Button[9];
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

	void Start()
	{
		foreach(Button button in levelButtons)
		{
			foreach(Image image in button.GetComponentsInChildren<Image>())
			{
				if(image.name == "SwordImage")
				{
					Debug.Log("Sword Image");
				}
				else if(image.name == "TimeImage")
				{
					Debug.Log ("Time Image");
				}
			}
		}
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
          GoldcombatScoreText.text = "Gold Score : " + GameManager.Instance.GetComponent<LevelScores>().LevelScoreCombat[levelnum-1];
          PlayerCombatScoreText.text = "Combat Score : " + GameManager.Instance.GetPlayerCombatScore(levelnum);
          GoldTimeScoreText.text = "Gold Score : " + GameManager.Instance.GetComponent<LevelScores>().LevelScoreTime[levelnum-1];
          PlayerTimeScoreText.text = "Time Score : " + GameManager.Instance.GetPlayerTimeScore(levelnum);
     }

}
