using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.UILibrary
{
    public class Navigation : MonoBehaviour
    {
        public static bool isClicked = false; // navigational button is not clicked 

        // potential in game minigames
        public void OpenChallenge1()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
            isClicked = true;
        }

        public void OpenChallenge2()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
            isClicked = true;
        }

        public void OpenChallenge3()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
            isClicked = true;
        }

        public void OpenChallenge4()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
            isClicked = true;
        }

        public void OpenChallenge5()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 8);
            isClicked = true;
        }

        public void OpenChallenge6()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 9);
            isClicked = true;
        }

        // navigational button is clicked
        public void ButtonIsClicked()
        {
            isClicked = true;
        }

        // cancel button
        public void CancelButton()
        {
            isClicked = false;
        }

    }
}