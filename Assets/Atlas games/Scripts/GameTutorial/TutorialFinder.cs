using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFinder : MonoBehaviour
{
   // this script only prevents probable issues GameTutorial.cs might cause for finding UI parts in the tutorial tips
   public string uiPartName;
   private bool _isClicked = false;

   public void InitiateTutorialClick()
   {
      if (!_isClicked)
      {
         _isClicked = true;
         FindObjectOfType<GameTutorial>().NextTip();
      }
   }
}
