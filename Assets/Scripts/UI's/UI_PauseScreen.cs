using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseScreen : MonoBehaviour
{
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        //Ask if player want to save the game
        
        Application.Quit();
    }
}
