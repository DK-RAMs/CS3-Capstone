﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class playerUI : MonoBehaviour
{
    public static int maxHealth = 100; //replresents the population of the town
    public static int maxHappy = 100;
    public int currentHealth; //replesents the population without covid
    public int currentHappy;
    public float revWait = 60f; // time till revenue stats update
    public float revWaitCounter;
    public float waitTime = 5f; // time till infection stats update
    public float waitCounter;
    public static int cases = 1; 
    public static int money = 500000; 
    public static int nowImmune = 0; // number of recovered people
    public static int dead = 0;
    public static int score = 0; //player score

    public happinessBar happinessBar;
    public HealthBar healthBar;
    public TextMeshProUGUI revenue;
    public TextMeshProUGUI confirmed;
    public TextMeshProUGUI recovered;
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI levelIndicator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentHappy = maxHappy;
        healthBar.setMaxHealth(maxHealth);
        happinessBar.setMaxHappy(maxHappy);
        revenue.text = "R 500000";
        waitCounter = waitTime;
        revWaitCounter = revWait;
        levelIndicator.text = "LEVEL 5";
    }

    // Update is called once per frame
    void Update()
    {

        levelIndicator.text = "LEVEL " + PolicyUI.level;
        waitCounter -= Time.deltaTime;
        revWaitCounter -= Time.deltaTime;

        //check the implemented policy and its rate of infection
        if (waitCounter < 0)
        {
            switch (PolicyUI.level)
			{
                case 1:
                    takeDamage(3);
                    break;
                case 2:
                    takeDamage(2);
                    break;
                case 3:
                    takeDamage(1);
                    break;
                case 4:
                    takeDamage(1);
                    break;
                case 5:
                    takeDamage(1);
                    break;
            }
            switch (PolicyUI.isRaised)
            {
                case true:
                    getSad(1);
                    break;
            }
            switch (PolicyUI.isReduced)
            {   
                case true:
                    getHappy(5);
                    break;
            }


            waitCounter = waitTime;
        }

        if (UpgradeUI.bought == true)
		{
            money -= UpgradeUI.cost;
            revenue.text = "R " + (money);
            
		}

        if (revWaitCounter < 0)
		{
            getMoney();
            revWaitCounter = revWait;
		}
    }

    //reduce the number of people without the virus
    void takeDamage(int damage){
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        cases += damage;
        confirmed.text = "Confirmed Cases: " + cases;
    }

    void getHappy(int amount)
	{
        currentHappy += amount / PolicyUI.level;
        if (currentHappy > maxHappy)
        {
            happinessBar.setHappy(maxHappy);
        }
        else
		{
            happinessBar.setHappy(currentHappy);
        }
    }

    void getSad(int amount)
	{
        currentHappy -= amount * PolicyUI.level;
        if (currentHappy < 0)
        {
            happinessBar.setHappy(0);
        }
        else
        {
            happinessBar.setHappy(currentHappy);
        }
    }


    //count the number of people who have recovered from the virus
    void recoveredCount(int people)
	{
        recovered.text = "Recovered: " + people;
	}

    //increase revenue based on the current policy level
    void getMoney()
	{
        money = money + (100000 / PolicyUI.level);
        revenue.text = "R " + money;
	}
}
