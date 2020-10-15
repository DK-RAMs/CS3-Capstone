﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.UILibrary
{
    public class ButtonClicks : MonoBehaviour
    {
        public void OpenNoticeboard()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenNews()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}