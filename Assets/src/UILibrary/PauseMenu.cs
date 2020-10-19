<<<<<<< Updated upstream
﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.UILibrary
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused; // game is currently not paused

        public static bool GameQuit = false;

        public GameObject pauseMenuUI;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                    Resume(); // resume while game is not paused
                else
                    Pause(); // call pause when game is paused
            }
        }

        // pause game
        public void Pause()
        {
            GameIsPaused = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Navigation.isClicked = true;
        }

        // resume game
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Navigation.isClicked = false;
            GameIsPaused = false;
        }

        // load main menu
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
=======
﻿using System.Collections;
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
>>>>>>> Stashed changes
}