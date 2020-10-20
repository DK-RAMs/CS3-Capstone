using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.UILibrary
{
    public class PauseMenu : MonoBehaviour
    {

        public GameObject pauseMenuUI;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (Game.GAMEPAUSED)
                {
                    Resume(); // resume while game is not paused

                }
                else
                {
                    Pause(); // call pause when game is paused
                }
            }
        }

        // pause game
        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Game.GAMEPAUSED = true;
            Navigation.isClicked = true;
        }

        // resume game
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Game.GAMEPAUSED = false;
            Navigation.isClicked = false;
        }

        // load main menu
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}