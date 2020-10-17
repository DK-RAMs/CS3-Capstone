using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using src.UILibrary;

namespace src.CitizenLibrary
{
    public class player : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;

        public HealthBar healthBar;

        public Town town;

        // Start is called before the first frame update
        void Start()
        {
            Game.waitTownInitialization();

            currentHealth = maxHealth;
            healthBar.setMaxHealth(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            if (Town.happinessUpdated)
            {
                healthBar.setHealth((int)town.AverageHappiness);
            }
        }
        
    }
}