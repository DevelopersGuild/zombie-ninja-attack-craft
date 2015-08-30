using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyBoardControl : MonoBehaviour
{
     private List<Button> buttons;
     private bool isInMenu = true;
     private int index = 0;
     private int lastIndex = 0;
     private Button selectedButton = null;

     // Use this for initialization
     void Start()
     {
          buttons = new List<Button>(gameObject.GetComponentsInChildren<Button>());
     }

     // Update is called once per frame
     void Update()
     {
          if(isInMenu == true)
          {
               if(Input.GetButtonDown("Down"))
               {
                    index++;
                    if(index > buttons.Count-1)
                    {
                         index = 0;
                    }
               }
               if(Input.GetButtonDown("Up"))
               {
                    index--;
                    if(index < 0)
                    {
                         index = buttons.Count-1;
                    }
               }
               GetCurrentButton();
          }
     }

     private void GetCurrentButton()
     {
          selectedButton = buttons[index];
          selectedButton.Select();
     }
}
