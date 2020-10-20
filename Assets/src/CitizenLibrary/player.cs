using System.Collections;
using System.Collections.Generic;
using System.Threading;
using src.SaveLoadLibrary;
using UnityEngine;
using src.UILibrary;

namespace src.CitizenLibrary
{
    public class player : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;

        public HealthBar healthBar;


        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            healthBar.setMaxHealth(maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            if (Town.happinessUpdated)
            {
                int health = (Town.CitizenCount - Game.town.TotalInfected - Game.town.TotalDead)/Town.CitizenCount*100; // Defines the health
                healthBar.setHealth(health);
            }
        }
        
    }
}