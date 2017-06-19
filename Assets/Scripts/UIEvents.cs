using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvents : MonoBehaviour
{
    public GameObject PauseMenu;
    void Start()
    {
        Time.timeScale = 1;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }
	public void GoToMainMenu ()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void GoToLevel(int index)
    {
        SceneManager.LoadSceneAsync(index + 1);
    }       
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);        
        foreach (var audio in Camera.main.GetComponents<AudioSource>())
        {
            audio.Pause();
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        foreach (var audio in Camera.main.GetComponents<AudioSource>())
        {           
            audio.Play();         
        }
    }    
}
