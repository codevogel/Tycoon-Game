using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseScript : MonoBehaviour
{
    
    public bool PauseGame(bool gamepaused)
    {

        if (gamepaused)
        {
            Time.timeScale = 0f;
            return true; 
        }
        else
        {
            Time.timeScale = 1;
            return false;
        }
    }
}
