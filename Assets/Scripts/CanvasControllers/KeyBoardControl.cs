using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyBoardControl : MonoBehaviour
{
     private List<Button> buttons;
     private bool isInMenu = false;
     private int index = 0;
     private int lastIndex = 0;
     private Button selectedButton = null;

     // Use this for initialization
     void Start()
     {
          buttons = new List<Button>(gameObject.GetComponentsInChildren<Button>());
          for (int i = 0; i < buttons.Count;i++)
          {
               Debug.Log(buttons[i].gameObject.name);
          }
          GetCurrentButton();
     }

     // Update is called once per frame
     void Update()
     {
          lastIndex = index;
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
               if(index != lastIndex)
               {
                    GetCurrentButton();
               }      
          }
     }

     private void GetCurrentButton()
     {
          selectedButton = buttons[index];
          Debug.Log(index);
          selectedButton.Select();

     }

     public void SetIsInMenu(bool value)
     {
          isInMenu = value;
     }

}
