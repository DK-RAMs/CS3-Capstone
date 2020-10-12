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

    public void OpenAchievements()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void OpenChallenge1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }

    public void OpenChallenge2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }
    public void OpenChallenge3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
    }
    public void OpenChallenge4()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
    }
    public void OpenChallenge5()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 8);
    }
    public void OpenChallenge6()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 9);
    }
    public void Quit(){
        Debug.Log("Quit!");
        Application.Quit();
    }


}
