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
using Random = System.Random;

namespace CitizenLibrary {
    public class Citizen
    {

        string id, name;
        int age, startWork, endWork, xPos, yPos, homeX, homeY;
        double happiness, riskofInfection, deltaHappiness;
        private bool infected, wearingMask, rebel;

        Collection<Citizen> familyMembers; // I don't think this is that important for now
        public enum Occupation { Unemployed = 0, Student = 1, Employed = 2, Retired = 3 };
        enum HealthRisk { Diabetic, Respiratory, Cardial };

        private Stopwatch happinessUpdateTimer;

        private static Random random;

        public static double beginRebelVal, stopRebelVal, baseChance;

        private CitizenTask favoriteTask;

        private Occupation citizenOccupation;
        private CitizenTask currentTask;
        private Collection<HealthRisk> healthRisks;

        public static Town town;

        #region Constructors
        
        
        public Citizen(string id, string name, int age, bool infected, bool wearingMask, bool rebel, int xPos, int yPos, int homeX, int homeY){
            this.id = id;
            this.name = name;
            this.age = age;
            this.infected = infected;
            this.rebel = rebel;
            this.xPos = xPos;
            this.yPos = yPos;
            this.homeX = homeX;
            this.homeY = homeY;
        }
        #endregion

        #region Initialization Methods
        public bool addFamilyMember(Citizen citizen)
        {
            foreach(Citizen familyMember in familyMembers)
            {
                if (familyMember.Equals(citizen))
                {
                    return false;
                }
            }
            familyMembers.Add(citizen);
            return true;
        }

        public void generateCitizenRisk(bool diabetic, bool respiratory, bool cardial, double modifier)
        {
            riskofInfection = Town.BaseCitisenRisk;
            if (diabetic)
            {
                healthRisks.Add(HealthRisk.Diabetic);
                riskofInfection += 10*modifier;
            }
            if (respiratory)
            {
                healthRisks.Add(HealthRisk.Respiratory);
                riskofInfection += 15*modifier;
            }
            if (cardial)
            {
                healthRisks.Add(HealthRisk.Cardial);
                riskofInfection += 5 * modifier;
            }
        }

        public void loadPreviousTask(int taskID, int startTime, int endTime, int completed)
        {
            currentTask = new CitizenTask(taskID, startTime, endTime, completed);
        }

        public static void initializeRandomizer()
        {
            
        }

        public void initializeCitizen()
        {
            happinessUpdateTimer = Stopwatch.StartNew();
        }
        #endregion

        #region Citizen Updating Methods
        public void update()
        {
            
            //if()
            
            // 1.) If a citizen is a rebel, they won't wear their mask

            // 2.) If a citizen is a rebel, they won't listen to law implemetations (i.e not social distance, just enter the store and affect other citizens' happiness

            

            if (currentTask.Completed)
            {
                generateTask();
            }

            if (happinessUpdateTimer.ElapsedMilliseconds >= town.UpdateTickRate) // Update tick rate is longer than a second. (Maybe 1 hour in game is 1 second)
            {
                happinessUpdateTimer.Stop();
                
                if (happiness >= 100 || happiness <= 0)
                {
                    deltaHappiness = 0;
                }
                deltaHappiness += currentTask.calculateTaskHappiness(1);
                if (deltaHappiness != 0)
                {
                    happiness += deltaHappiness; // Adds the total change to the citizen's happiness to citizen's current happiness
                }
                deltaHappiness = Town.BaseDetalHappiness; // Resets citizen delta happiness after processing all changes
                generateRebelFactor();
                happinessUpdateTimer.Reset();
                happinessUpdateTimer.Stop();
                happinessUpdateTimer.Start();
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
        }
        public void generateTask()
        {
            while (!currentTask.Available)
            {
                currentTask = new CitizenTask(random, citizenOccupation); // REMEMBER TO MAKE THE CITIZENTASK CLASS!!! NBNBNBNBNB
            }
        }

        private int rollDice()
        {
            return random.Next(1, 100);
        }

        #endregion
        
        #region Getters & Setters

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

        public bool Rebel
        {
            get => rebel;
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