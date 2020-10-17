using System.Collections;
using System.Collections.Generic;
using src;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextMeshProUGUI dayBox;
    private float startTime;
    private int day;
    void Start()
    {
        startTime = Time.time;
        day = Game.town.Day;
    }

    void Update() {
        while (Game.GAMEPAUSED)
		{
            
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

        if (Game.town.Time < 10)
        {
            textBox.text = "0" + Game.town.Time + ":00";
        }
        else
        {
            textBox.text = Game.town.Time + ":00";
        }
        
        /*
        if (seconds.Contains(","))
		{
            textBox.text = minutes + ":" + seconds.Remove(1);
        }
		else
		{
            textBox.text = minutes + ":" + seconds;
        }*/
       
        dayBox.text = "Day " + Game.town.Day;
        
    }
}
