using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PolicyUI : MonoBehaviour
{
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI policy;
    public TextMeshProUGUI stats;
    public TextMeshProUGUI policyTimer;

    public float startTime = 5f;
    public float timer;
    public bool isImplemented = false;
    public static int level = 5;

    public static bool isRaised = false;
    public static bool isReduced = false;

    public int max = 5;
    public int min = 1;

    // Start is called before the first frame update
    void Start()
    {
        //set the initial level to 5
        level = 5;
        //set the start time of the policy timer
        timer = Time.time;
    

    }

    // Update is called once per frame
    void Update()
    {
        //update the current level field 
        currentLevel.text = "Current Level: " + level;

        if (isImplemented)
		{
            Time.timeScale = 1f;
            float t = startTime - timer;

            string seconds = (t % 60).ToString("f2");
            seconds = seconds.Remove(1);
            policyTimer.text = seconds;
        }
    }

    //display policy text based on the selected level
    public void policy1()
	{
        policy.text = "Most normal activity can resume, with precautions and health guidelines followed at all times. Population prepared for an increase in alert levels if necessary.";
	}

    public void policy2()
    {
        policy.text = "Physical distancing and restrictions on leisure and social activities to prevent a resurgence of the virus.";
    }
    public void policy3()
    {
        policy.text = "Restrictions on many actitivies, including at workplaces and socially, to address a high risk of transmission.";
    }
    public void policy4()
    {
        policy.text = "Extreme precautions to limit community transmission and outbreaks, while allowing some activity to resume.";
    }
    public void policy5()
    {
        policy.text = "Drastic measures to contain the spread of the virus and save lives.";
    }

    //display the various stats of the town to the player
    public void showstats()
	{
        stats.text = "Population: " +playerUI.maxHealth +"\nConfirmed cases:" +playerUI.cases+ "\nRecovered: " + playerUI.nowImmune+"\nDead: " + playerUI.dead;
	}

    //reduce the current level
    public void levelReduce()
	{
        if (level != min)
        {
            PolicyUI.level -= 1;
            isReduced = true;
            isRaised = false;
        }
    }

    //raise the current level
    public void levelRaise()
    {
        if (level != max)
        {
            PolicyUI.level += 1;
            isReduced = false;
            isRaised = true;
        }
    }
    //implement a specific policy 
    public void implementPolicy()
	{

	}

    public void PolicyMade()
	{
        isImplemented = true;
	}
}
