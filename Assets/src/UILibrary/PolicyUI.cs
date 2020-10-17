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
        }

        public void policy2()
        {
            policy.text = "60% freedom";
        }

        public void policy3()
        {
            policy.text = "40% freedom";
        }

        public void policy4()
        {
            policy.text = "20% freedom";
        }

        public void policy5()
        {
            policy.text = "Community Policies\n" +
                          "1.all non - essential business closed\n" +
                          "2.all non - essential businesses limited capacity opening" +
                          "\nIndividual Policies\n" +
                          "1.mask wearing required at all times in public\n" +
                          "2. no gatherings of any sort allowed";
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