using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevelCanvasController : MonoBehaviour
{
     public float PercentForSilverScore;
     public Button[] levelButtons = new Button[9];
     public Text LevelName;
     public Text GoldTimeScoreText;
     public Text PlayerTimeScoreText;
     public Text GoldcombatScoreText;
     public Text PlayerCombatScoreText;
     public Image LevelInfoScoreImage;
     public Image LevelInfoTimeImage;
     public Sprite[] TimeIcons = new Sprite[3];
     public Sprite[] CombatIcons = new Sprite[3];

     void Start()
     {
          int levelNumber = 1;
          int goldScoreCombat;
          int playerScoreCombat;
          double goldScoreTime;
          double playerScoreTime;
          foreach (Button button in levelButtons)
          {
               goldScoreCombat = GameManager.Instance.GetComponent<LevelScores>().LevelScoreCombat[levelNumber - 1];
               playerScoreCombat = GameManager.Instance.GetPlayerCombatScore(levelNumber);
               goldScoreTime = GameManager.Instance.GetComponent<LevelScores>().LevelScoreTime[levelNumber - 1];
               playerScoreTime = GameManager.Instance.GetPlayerTimeScore(levelNumber);
               foreach (Image image in button.GetComponentsInChildren<Image>())
               {
                    if (image.name == "SwordImage")
                    {
                         if (playerScoreCombat >= goldScoreCombat)
                         {
                              image.sprite = CombatIcons[2];
                         }
                         else if (playerScoreCombat >= (goldScoreCombat * PercentForSilverScore))
                         {
                              image.sprite = CombatIcons[1];
                         }

                    }
                    else if (image.name == "TimeImage")
                    {
                         if (playerScoreTime != 0)
                         {
                              if (playerScoreTime <= goldScoreTime)
                              {
                                   image.sprite = TimeIcons[2];
                              }
                              else if (playerScoreTime <= (goldScoreTime * PercentForSilverScore))
                              {
                                   image.sprite = TimeIcons[1];
                              }
                         }
                    }
               }
               levelNumber++;
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

     public void DisplayScores(int levelNum)
     {
          int goldScoreCombat = GameManager.Instance.GetComponent<LevelScores>().LevelScoreCombat[levelNum - 1];
          int playerScoreCombat = GameManager.Instance.GetPlayerCombatScore(levelNum);
          double goldScoreTime = GameManager.Instance.GetComponent<LevelScores>().LevelScoreTime[levelNum - 1];
          double playerScoreTime = GameManager.Instance.GetPlayerTimeScore(levelNum);
          GoldcombatScoreText.text = "Gold Score : " + goldScoreCombat;
          PlayerCombatScoreText.text = "Combat Score : " + playerScoreCombat;
          GoldTimeScoreText.text = "Gold Score : " + goldScoreTime;
          PlayerTimeScoreText.text = "Time Score : " + playerScoreTime;

          if(playerScoreCombat > 0)
          {
               if (playerScoreCombat >= goldScoreCombat)
               {
                    LevelInfoScoreImage.sprite = CombatIcons[2];
               }
               else if (playerScoreCombat >= (goldScoreCombat * PercentForSilverScore))
               {
                    LevelInfoScoreImage.sprite = CombatIcons[1];

               }
               if (playerScoreTime != 0)
               {
                    if (playerScoreTime <= goldScoreTime)
                    {
                         LevelInfoTimeImage.sprite = TimeIcons[2];
                    }
                    else if (playerScoreTime <= (goldScoreTime * PercentForSilverScore))
                    {
                         LevelInfoTimeImage.sprite = TimeIcons[1];
                    }
               }
          }
          else
          {
               LevelInfoScoreImage.sprite = CombatIcons[0];
               LevelInfoTimeImage.sprite = TimeIcons[0];
          }
          

     }

}
