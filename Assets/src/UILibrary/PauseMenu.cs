using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.UILibrary
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused; // game is currently not paused

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
}