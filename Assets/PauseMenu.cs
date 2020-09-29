using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool GameQuit = false;
    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu(){
            Time.timeScale = 1f;
            SceneManager.LoadScene("main");
    }

    public void Quit(){
        Debug.Log("Quitting game...");
        GameQuit = true;
        // Save state stuff happens here. Need to wait for all the relevant data to be written and *maybe* compressed? (Could use huffman compression :thinking:)
        Application.Quit();
    }
}
