using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public GameObject newsB;


    public void OpenNoticeboard() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenNews() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }


}
