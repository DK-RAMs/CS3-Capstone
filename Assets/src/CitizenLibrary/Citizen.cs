using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using System.Threading.Tasks;
using src.SaveLoadLibrary;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace src.CitizenLibrary {
    public class Citizen // Still need to finish
    {

        string id, name;
        int age, startWork, endWork;
        private double happiness, riskofDeath;
        private bool rebel, hospitalized, dead, infected, wearingMask;

        public Building workLocation, homeLocation;
        
        public enum HealthRisk { Diabetic, Respiratory, Cardial, Old };


        private static Random random = new Random();

        public static double beginRebelVal, stopRebelVal, baseChance, favoriteModifier;

        private CitizenTask favoriteTask;

        private CitizenTask currentTask;
        private Collection<HealthRisk> healthRisks;

        private static Collection<string> names;

        public static readonly string IDPREFIX = "citizen";
        public static int citizenNum = 0;


        #region Constructors
        
        
        public Citizen(string id, string name, double happiness, int age, bool infected, bool wearingMask, bool rebel, bool hospitalized, bool dead)
        {
            citizenNum++;
            this.id = id;
            this.name = name;
            this.age = age;
            this.infected = infected;
            this.rebel = rebel;
            this.happiness = happiness;
            this.hospitalized = hospitalized;
            this.dead = dead;
            this.wearingMask = wearingMask;
            
        }

        public Citizen(CitizenData c)
        {

            happiness = 50;
            name = c.Name;
            age = random.Next(18, 45);
            
            loadFavoriteTask(c.CurrentTaskId, Game.town.Buildings.ElementAt(0)); // Need to figure out how we're gonna pull out the building here
            
        }
        public Citizen()
        {
            id = IDPREFIX + citizenNum;
            citizenNum++;
            int nameSelection = random.Next(1);
            if (nameSelection > 0)
            {
                name = "Jane";
            }
            else
            {
                name = "John";
            }
            age = random.Next(18, 65);
            citizenNum++;
            infected = false;
            rebel = false;
            happiness = 50;
            hospitalized = false;
            dead = false;
            wearingMask = false;
            homeLocation = Game.town.Recreational[random.Next(Game.town.Recreational.Count-1)];
            workLocation = Game.town.Buildings.ElementAt(random.Next(Game.town.Buildings.Count - 1));
            favoriteTask = new CitizenTask(random, this, true);
            currentTask = new CitizenTask(random, this, false);
        }
        #endregion

        #region Initialization Methods
        public void generateCitizenRisk(int diabetic, int respiratory, int cardial, double modifier)
        {
            healthRisks = new Collection<HealthRisk>();
            riskofDeath = Game.town.BaseCitisenRisk*modifier;
            if (diabetic == 1)
            {
                healthRisks.Add(HealthRisk.Diabetic);
                riskofDeath += 10*modifier;
            }
            if (respiratory == 1)
            {
                healthRisks.Add(HealthRisk.Respiratory);
                riskofDeath += 15*modifier;
            }
            if (cardial == 1)
            {
                healthRisks.Add(HealthRisk.Cardial);
                riskofDeath += 5 * modifier;
            }

            if (age > 45)
            {
                healthRisks.Add(HealthRisk.Old);
                riskofDeath += 10 * modifier;
            }
        }

        public static void initializeNames()
        {
            names = new Collection<string>();
            
        }

        public void loadPreviousTask(int taskID, int startTime, int endTime, int startDay, int endDay, int completed, Building building)
        {
            
            currentTask = new CitizenTask(taskID, startTime, endTime, startDay, endDay, completed, building);
        }

        private void loadFavoriteTask(int taskID, Building building)
        {
            favoriteTask = new CitizenTask(taskID, 0, 0, 0, 0, 0, building); // Instantiated with Zeros since that data isn't important for loading the citizen's favorite task
        }

        public void initiateTask()
        {
            currentTask.taskLocation.enterBuilding(this);
        }

        #endregion

        #region Citizen Updating Methods
        public void Update()
        {
            if (!hospitalized) // Checks if citizen is hospitalized (i.e. in hospital)
            {
                if (Game.town.Time == 6)
                {
                    if (!rebel && Game.town.policyImplemented[1]) // PolicyImplementation[1] - Citizens must wear face masks at all times
                    {
                        wearingMask = true;
                    }
                    else
                    {
                        wearingMask = false;
                    }
                    if (infected)
                    {
                        int hospitalizeRoll = rollDice();
                        if (hospitalizeRoll <= (riskofDeath+15))
                        {
                            Debug.Log("Citizen with id" + id + " has collapsed and has been sent to the hospital");
                            hospitalized = true;
                        }
                        else if (currentTask.TaskID == 6 && currentTask.EndTime >= Game.town.Time && currentTask.EndDay >= Game.town.Day) // Citizen is cured once they managed to get through 15 days of self quarantine
                        {
                            infected = false;
                        }
                    }
                }
                updateTask(); // Task must update here. The hospitalization roll needs to be committed before update (since citizen is hospitalized IN the method)
                if (Game.town.Timer.ElapsedMilliseconds >= Game.UPDATETICKRATE) // Update tick rate is longer than a second. (Maybe 1 hour in game is 1 second) // This is not required since tasks
                {
                    // Unnecessary check here
                    happiness +=
                        Game.town.BaseDetalHappiness; // Adds the total change to the citizen's happiness to citizen's current happiness

                    if (happiness > 100)
                    {
                        happiness = 100;
                    }
                    else if (happiness < 0)
                    {
                        happiness = 0;
                    }
                    generateRebelFactor();
                }
            }
            else
            {
                if (Game.town.Time % 12 == 0) // Every 12 hours, a death roll happens, if the citizen gets hit by it, they die
                {
                    int deathRoll = rollDice();
                    if (deathRoll <= riskofDeath)
                    {
                        dead = true;
                    }
                }

                if (Game.town.Time >= currentTask.EndTime && Game.town.Day >= currentTask.EndDay) // Citizen managed to recover from the disease
                {
                    infected = false;
                    hospitalized = false;
                    riskofDeath++;
                    rebel = false;
                    happiness = 85;
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
                        rebel = false;
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
                        rebel = true; // Citizen becomes a rebel after being upset for awhile
                    }
                }
            }
        }

        private void updateTask()
        {
            currentTask.Update(random, this, Game.town);
            if (currentTask.Completed)
            {
                if (currentTask.Equals(favoriteTask))
                {
                    happiness += currentTask.calculateTaskHappiness(random, rebel, favoriteModifier);
                }
                else
                {
                    happiness += currentTask.calculateTaskHappiness(random, rebel, 1);
                }
                currentTask = new CitizenTask(random, this, false); // Generates a new task that isn't a favorite
            }
        }
        
        private int rollDice()
        {
            return random.Next(1, 100);
        }

        #endregion
        
        #region Event Methods

        public void rollHealthEvent(int chance)
        {
            int infectRoll = rollDice();
            if (infectRoll <= chance)
            {
                infected = true;
            }
        }

        public void applyHappiness(double happiness)
        {
            this.happiness += happiness;
            if (this.happiness > 100)
            {
                this.happiness = 100;
            }
        }

        public void reset()
        {
            rebel = false;
            happiness = 55;
        }
        
        #endregion
        
        
        #region Getters & Setters

        public bool WearingMask => wearingMask;
        public bool Hospitalized => hospitalized;

        public double RiskofDeath => riskofDeath;

        public bool Dead => dead;

        public bool Infected => infected;

        public string ID => id;

        public string Name => name;

        public bool Rebel => rebel;

        public double Happiness => happiness;

        public int Age => age;

        public CitizenTask CurrentTask => currentTask;

        public CitizenTask FavoriteTask => favoriteTask;

        #endregion
        
        #region Property Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Citizen))
            {
                return false;
            }
            Citizen c = (Citizen) obj;
            return c.id.Equals(id);
        }

        public override int GetHashCode()
        {
            int hash = 11;
            int hashID = hash * 17 + ID.GetHashCode();
            return hashID;
        }

        #endregion

    }
}