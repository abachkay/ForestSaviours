using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvents : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }
    public GameObject PauseMenu;
	public void Go2MainMenu ()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void Go2LevelSelect()
    {
        SceneManager.LoadSceneAsync(1);
    }    
    public void Go2Game1()
    {              
        SceneManager.LoadSceneAsync(2);
    }
    public void Go2Game2()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Go2Game3()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void Go2Game4()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.GetComponent<Canvas>().enabled = true;
        foreach (var audio in GetComponents<AudioSource>())
        {
            audio.Pause();
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.GetComponent<Canvas>().enabled = false;
        foreach (var audio in GetComponents<AudioSource>())
        {
            if (audio.loop)
            {
                audio.Play();
            }
        }
    }     
}
