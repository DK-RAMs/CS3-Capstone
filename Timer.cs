using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextMeshProUGUI dayBox;
    private float startTime;
    private int day;
    public bool paused = false;
    void Start()
    {
        startTime = Time.time;
        day = 1;
    }

    void Update() {
        if (paused)
		{
            return;
		}

        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        

        int m = (int)t / 60;
        if (m == 5)
		{
            startTime = Time.time;
            day += ((int)t / 300);
        }

        seconds = seconds.Remove(2);
        if (seconds.Contains(","))
		{
            textBox.text = minutes + ":" + seconds.Remove(1);
        }
		else
		{
            textBox.text = minutes + ":" + seconds;
        }
       
        dayBox.text = "Day " + day;
        
    }
}
