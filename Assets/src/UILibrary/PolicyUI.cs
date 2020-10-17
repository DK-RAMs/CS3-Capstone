using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace src.UILibrary
{
    public class PolicyUI : MonoBehaviour
    {
        public TextMeshProUGUI currentLevel;
        public TextMeshProUGUI policy;
        public TextMeshProUGUI stats;

        public static int level = 5;
        public int max = 5;
        public int min = 1;

        // Start is called before the first frame update
        void Start()
        {
            //set the initial level to 5
            level = 5;
        }

        // Update is called once per frame
        void Update()
        {
            //update the current level field 
            currentLevel.text = "Current Level: " + level;
        }

        //display policy text based on the selected level
        public void policy1()
        {
            policy.text = "Business as almost usual";
            // most normal activity can remuse, with precautions and health guidelines followed at all times. Population prepared for an increase in alert levels if necessary.
        }

        public void policy2() // level 2
        {
            policy.text = "60% freedom";
            // physical distancing and restrictions on leisure and social activities to prevent a resurgence of the virus.
        }

        public void policy3() // level 3
        {
            policy.text = "40% freedom";
            // restrictions on many activities, including at workplaces and socially, to address a high risk of transmission.
        }

        public void policy4() // level 4
        {
            policy.text = "20% freedom";
            // extreme precautions to limit community transmissions and outbreaks, while allowing some activity to resume.
        }

        public void policy5() // level 5
        {
            policy.text = "Community Policies\n" +
                          "1.all non - essential business closed\n" +
                          "2.all non - essential businesses limited capacity opening" +
                          "\nIndividual Policies\n" +
                          "1.mask wearing required at all times in public\n" +
                          "2. no gatherings of any sort allowed";
            // drastic measures to contain the spread of the virus and save lives.
        }

        //display the various stats of the town to the player
        public void showstats()
        {
            stats.text = "Population: " + playerUI.maxHealth + "\nConfirmed cases:" + playerUI.cases + "\nRecovered: " +
                         playerUI.nowImmune + "\nDead: " + playerUI.dead;
        }

        //reduce the current level
        public void levelReduce()
        {
            if (level != min)
            {
                PolicyUI.level -= 1;
            }
        }

        //raise the current level
        public void levelRaise()
        {
            if (level != max)
            {
                PolicyUI.level += 1;
            }
        }

        //implement a specific policy 
        public void implementPolicy()
        {

        }
    }
}