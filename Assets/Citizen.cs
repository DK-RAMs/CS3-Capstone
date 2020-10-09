using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace CitizenLibrary {
    public class Citizen
    {

        string id, name;
        int age, startWork, endWork, xPos, yPos, homeX, homeY;
        double happiness, riskofDeath, deltaHappiness;
        private bool infected, wearingMask, rebel, hospitalized, dead;

        public Building workLocation;

        public Building citizenHome;
        
        public enum Occupation { Unemployed = 0, Student = 1, Employed = 2, Retired = 3 };
        enum HealthRisk { Diabetic, Respiratory, Cardial };

        private Stopwatch happinessUpdateTimer;

        private static Random random;

        public static double beginRebelVal, stopRebelVal, baseChance;

        private CitizenTask favoriteTask;

        private Occupation citizenOccupation = Occupation.Employed;
        private CitizenTask currentTask;
        private Collection<HealthRisk> healthRisks;

        public static Town town;

        #region Constructors
        
        
        public Citizen(string id, string name, double happiness, int age, bool infected, bool wearingMask, bool rebel, int xPos, int yPos, int homeX, int homeY, bool hospitalized, bool dead){
            this.id = id;
            this.name = name;
            this.age = age;
            this.infected = infected;
            this.rebel = rebel;
            this.xPos = xPos;
            this.yPos = yPos;
            this.homeX = homeX;
            this.homeY = homeY;
            this.happiness = happiness;
            this.hospitalized = hospitalized;
            this.dead = dead;
        }

        public Citizen(string id)
        {
            this.id = id;
            happiness = 50;
        }
        #endregion

        #region Initialization Methods
        public void generateCitizenRisk(bool diabetic, bool respiratory, bool cardial, double modifier)
        {
            healthRisks = new Collection<HealthRisk>();
            riskofDeath = town.BaseCitisenRisk;
            if (diabetic)
            {
                healthRisks.Add(HealthRisk.Diabetic);
                riskofDeath += 10*modifier;
            }
            if (respiratory)
            {
                healthRisks.Add(HealthRisk.Respiratory);
                riskofDeath += 15*modifier;
            }
            if (cardial)
            {
                healthRisks.Add(HealthRisk.Cardial);
                riskofDeath += 5 * modifier;
            }
        }

        public void loadPreviousTask(int taskID, int startTime, int endTime, int completed)
        {
            currentTask = new CitizenTask(taskID, startTime, endTime, completed);
        }

        public static void initializeRandomizer()
        {
            random = new Random();
        }

        public void InitializeCitizen()
        {
            happinessUpdateTimer = Stopwatch.StartNew();
        }
        #endregion

        #region Citizen Updating Methods
        public void Update()
        {
            if (!hospitalized) // Checks if citizen is hospitalized (i.e. in hospital)
            {
                updateTask();

                if (currentTask.Completed)
                {
                    while (true)
                    {
                        generateTask();
                        if (currentTask.Available)
                        {
                            break;
                        }
                        deltaHappiness -= 2; // The citizen's happiness takes a hit if they aren't able to complete a specific task
                    }
                }

                if (happinessUpdateTimer.ElapsedMilliseconds >= Town.UpdateTickRate
                ) // Update tick rate is longer than a second. (Maybe 1 hour in game is 1 second)
                {
                    Debug.Log(currentTask.Completed);
                    happinessUpdateTimer.Stop();
                    if (happiness >= 100 || happiness <= 0)
                    {
                        deltaHappiness = 0;
                    }

                    double noDelta = 0;
                    if (deltaHappiness != noDelta)
                    {
                        happiness +=
                            deltaHappiness; // Adds the total change to the citizen's happiness to citizen's current happiness
                    }

                    deltaHappiness =
                        town.BaseDetalHappiness; // Resets citizen delta happiness after processing all changes
                    generateRebelFactor();
                    happinessUpdateTimer.Reset();
                    happinessUpdateTimer.Stop();
                    happinessUpdateTimer.Start();
                }
            }
            else
            {
                if (Town.Time % 3 == 0)
                {
                    int deathRoll = rollDice();
                    if (deathRoll <= riskofDeath)
                    {
                        dead = true;
                    }
                }
            }
        }

        private void generateRebelFactor()
        {
            if (rebel)
            {
                if (happiness > stopRebelVal)
                {
                    int roll = rollDice();
                    int chance = Convert.ToInt16(baseChance + happiness - stopRebelVal); // As a citizen's happiness increases, the chance of them staying a rebel decreases (stopRebelVal is specified at the beginning of the game.) This roll occurs 
                    if (roll <= chance)
                    {
                        this.rebel = false;
                    }
                }
            }
            if (!rebel)
            {
                if (happiness < beginRebelVal)
                {
                    int roll = rollDice();
                    int chance = Convert.ToInt16(baseChance + beginRebelVal - happiness); // As a citizen's happiness decreases, the chance of them rebelling increases.
                    if (roll <= chance)
                    {
                        this.rebel = true; // Citizen becomes a rebel after being upset for awhile
                    }
                }
            }
        }

        private void updateTask()
        {
            currentTask.update(this, town);
            if (currentTask.Completed){
                while (true)
                {
                    generateTask();
                    if (currentTask.Available)
                    {
                        break;
                    }
                    deltaHappiness -= 2; // The citizen's happiness takes a hit if they aren't able to complete a specific task
                }
            }
        }
        public void generateTask()
        {
            currentTask.generateNewTask(random, this);
            Debug.Log("Citizen " + id + " decided to " + currentTask.TaskName + ". Task will be completed at " + currentTask.EndTime);
        }

        private int rollDice()
        {
            return random.Next(1, 100);
        }

        #endregion
        
        #region Getters & Setters

        public bool Hospitalized => hospitalized;

        public bool Dead => dead;
        public static Town Town
        {
            get => town;
        }
        
        public Occupation CitizenOccupation
        {
            get => citizenOccupation;
        }

        public bool Infected
        {
            get
            {
                return infected;
            }
            set
            {
                infected = value;
            }
        }

        public string ID
        {
            get
            {
                return id;
            }
        }

        public bool Rebel
        {
            get => rebel;
        }

        public double Happiness
        {
            get => happiness;
        }

        #endregion
        
        #region Property Methods

        public bool Equals(Citizen citizen)
        {
            return this.id == citizen.id;
        }
        #endregion

    }
}