using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class MainMenu : MonoBehaviour
    {

        public static TextMeshProUGUI currentSave;

        // starts the game
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //opens game achievements
        public void OpenAchievements()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

        //exits game achievements
        public void ExitAchievements()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        }

        //go to main menu
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
        }

        //exits the game
        public void Quit()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }


    }
}