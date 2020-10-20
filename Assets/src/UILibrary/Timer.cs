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

    void Update() {
        while (Game.GAMEPAUSED)
		{
            
		}

        if (Game.town.Time < 10)
        {
            textBox.text = "0" + Game.town.Time + ":00";
        }
        else
        {
            textBox.text = Game.town.Time + ":00";
        }
        dayBox.text = "Day " + Game.town.Day;
        
    }
}
