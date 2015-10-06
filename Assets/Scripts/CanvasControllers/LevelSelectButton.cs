using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectButton : Button, ISelectHandler
{

     public LoadLevelCanvasController CanvasController;

     void Start()
     {
          GameObject temp = GameObject.Find("LoadLevelCanvas");
          CanvasController = temp.GetComponent<LoadLevelCanvasController>();
     }

     public override void OnSelect(BaseEventData eventData)
     {
          base.OnSelect(eventData);
          CanvasController.ButtonSelected(this);
     }

}
