using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class playerUI : MonoBehaviour
    {
        public static int maxHealth = 100; //replresents the population of the town
        public int currentHealth; //replesents the population without covid
        public float revWait = 60f; // time till revenue stats update
        public float revWaitCounter;
        public float waitTime = 5f; // time till infection stats update
        public float waitCounter;
        public long money;
        public static int dead = 0;
        public static int score = 0; //player score

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
            healthBar.setMaxHealth(maxHealth);
            revenue.text = "R " + 0;
            money = 0;
            levelIndicator.text = "LEVEL 5";
        }

        // Update is called once per frame
        void Update()
        {

            levelIndicator.text = "LEVEL " + PolicyUI.level;
            waitCounter -= Time.deltaTime;
            revWaitCounter -= Time.deltaTime;

            money = Game.town.Money;

            revenue.text = "R" + money;
            
        }

        //reduce the number of people without the virus
        void takeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.setHealth(currentHealth);
            confirmed.text = "Confirmed Cases: " + Game.town.TotalInfected;
        }

        //count the number of people who have recovered from the virus
    }
}