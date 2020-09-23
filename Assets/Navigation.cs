using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void GoBack(){
            SceneManager.LoadScene("Scene4");
        }

    public void OpenNoticeboard() {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

    public void OpenNews() {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Quit(){
        Debug.Log("Quit!");
        Application.Quit();
    }


}
