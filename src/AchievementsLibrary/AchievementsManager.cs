using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    // displays for acheivement 1
    public TextMeshProUGUI t1;
    public TextMeshProUGUI d1;
    // displays for acheivement 2
    public TextMeshProUGUI t2;
    public TextMeshProUGUI d2;
    // displays for acheivement 3
    public TextMeshProUGUI t3;
    public TextMeshProUGUI d3;
    // displays for acheivement 4
    public TextMeshProUGUI t4;
    public TextMeshProUGUI d4;
    // displays for acheivement 5
    public TextMeshProUGUI t5;
    public TextMeshProUGUI d5;

    // Start is called before the first frame update
    void Start()
    {
        // achievement names and descriptions
        if (GlobalAchievements.ac1Count == 1)
        {
            t1.text = "Hello World";
            d1.text = "Complete the Tutorial.";
        }

        if (GlobalAchievements.ac2Count == 1)
        {
            t2.text = "Ease of Access";
            d2.text = "Reduce the lockdown level for the first time.";
        }
        if (GlobalAchievements.ac3Count == 1)
        {
            t3.text = "Tough Times";
            d3.text = "Raise the lockdown level for the first time.";
        }
        if (GlobalAchievements.ac4Count == 1)
        {
            t4.text = "Hello World";
            d4.text = "Complete the Tutorial.";
        }
        if (GlobalAchievements.ac5Count == 1)
        {
            t5.text = "Hello World";
            d5.text = "Complete the Tutorial.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // achievement names and descriptions
        if (GlobalAchievements.ac1Count == 1)
		{
            t1.text = "Hello World";
            d1.text = "Complete the Tutorial.";
		}

        if (GlobalAchievements.ac2Count == 1)
        {
            t2.text = "Ease of Access";
            d2.text = "Reduce the lockdown level for the first time.";
        }
        if (GlobalAchievements.ac3Count == 1)
        {
            t3.text = "Tough Times";
            d3.text = "Raise the lockdown level for the first time.";
        }
        if (GlobalAchievements.ac4Count == 1)
        {
            t4.text = "Hello World";
            d4.text = "Complete the Tutorial.";
        }
        if (GlobalAchievements.ac5Count == 1)
        {
            t5.text = "Hello World";
            d5.text = "Complete the Tutorial.";
        }
    }
}
