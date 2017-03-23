using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public GameObject PauseMenu;
	public void Go2MainMenu ()
    {
        Application.LoadLevel(0);
	}
    public void Go2LevelSelect()
    {
        Application.LoadLevel(1);
    }
    public void Go2Game()
    {
        Application.LoadLevel(2);
    }
    public void Go2Game2()
    {
        Application.LoadLevel(3);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.GetComponent<Canvas>().enabled = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.GetComponent<Canvas>().enabled = false;
    }    
}
