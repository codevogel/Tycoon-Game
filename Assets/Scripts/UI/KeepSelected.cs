 using System;
 using UnityEngine;
 using UnityEngine.UI;

 /// <summary>
 /// This class is made to keep a button opaque instead of transparent to make it clear this is the selected building
 /// </summary>
 public class KeepSelected : MonoBehaviour
 {
     //private Button ToKeepActive;

     private void Update()
     {
         if (ToKeepActive != null) ToKeepActive.Select();
     }

     //public void ButtonToKeepActive(Button thisButton)
     //{
     //    ToKeepActive = thisButton;
     //}
 }
