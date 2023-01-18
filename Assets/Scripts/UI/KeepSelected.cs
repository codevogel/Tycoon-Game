 using System;
 using UnityEngine;
 using UnityEngine.UI;

 public class KeepSelected : MonoBehaviour
 {
     private Button ToKeepActive;

     private void Update()
     {
         ToKeepActive.Select();
     }

     public void ButtonToKeepActive(Button thisButton)
     {
         ToKeepActive = thisButton;
     }
 }
