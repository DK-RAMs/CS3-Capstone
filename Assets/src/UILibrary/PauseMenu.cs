using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // game is currently not paused
    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            
            if(GameIsPaused){
                Resume(); // resume while game is not paused
                
            }
            else{
                Pause(); // call pause when game is paused
            }
        }
    }
    // pause game
    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Navigation.isClicked = true;
    }
    // resume game
    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Navigation.isClicked = false;
    }
    // load main menu
    public void LoadMenu(){
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
