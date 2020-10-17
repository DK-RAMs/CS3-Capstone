using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using src.SaveLoadLibrary;
using src.NewspaperLibrary;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;
using src.UILibrary;
using UnityEditor;

namespace src.CitizenLibrary
{
    public class Town 
    {
        #region Class Properties
        public static int townNum = 1;
        private string id, mayor;
        private volatile int day, time, totalInfected, nextNewsEvent;
        private static int CITIZEN_START_COUNT = 800, CITIZEN_START_COUNT_DEBUG = 1200;

        public static volatile bool happinessUpdated;

        private static long
            updateTickRate,
            dayLength; // For now, the length of a day will be 1 minute (60 seconds). The updateTickRate will be

        private HashSet<Building> buildings;
        private Collection<Building> recreational, residential;
        private Collection<Supermarket> essentials;
        private Collection<Hospital> emergency;
        private Dictionary<int, bool> policyImplementation;
        private bool[] policyImplemented;
        private HashSet<Policy> allPolicies, availablePolicies;
        private CitizenWorkerThread[] citizenThreads;
        public static Thread[] Threads;

        private double baseDetalHappiness,
            baseCitisenRisk,
            baseMortalityRate,
            baseRecoveryRate,
            averageHappiness,
            averageHealth;

        private double money, favoriteModifier;
        private Collection<Policy> policies;
        private Stopwatch timer;
        public static volatile bool townReady;
        private int numCitizens;
        // public static Newspaper newspaper;

        #endregion

        #region Constructors
        public Town(string id, string mayor, int day, int time, long dayLength, long updateTickRate,
            double baseDetalHappiness, double baseCitisenRisk, double baseMortalityRate, double baseRecoveryRate)
        {
            this.id = id;
            this.mayor = mayor;
            this.day = day;
            this.time = time;
            Town.dayLength = dayLength;
            Town.updateTickRate = updateTickRate;
            this.baseDetalHappiness = baseDetalHappiness;
            this.baseCitisenRisk = baseCitisenRisk;
            this.baseMortalityRate = baseMortalityRate;
            this.baseRecoveryRate = baseRecoveryRate;
            policyImplementation = new Dictionary<int, bool>();
            buildings = new HashSet<Building>();
            recreational = new Collection<Building>();
            essentials = new Collection<Supermarket>();
            emergency = new Collection<Hospital>();
            residential = new Collection<Building>();
            timer = new Stopwatch();
            townNum++;
            money = 250;
            totalInfected = 0;
            favoriteModifier = 1.2;
            averageHappiness = 0;
        }

        public Town(string mayor, double baseDetalHappiness, double baseCitisenRisk, double money) // Town default constructor
        {
            this.mayor = mayor;
            this.baseDetalHappiness = baseDetalHappiness;
            this.money = money;
            id = mayor + "Town " + townNum;
            day = 1;
            time = 6;
            this.baseCitisenRisk = baseCitisenRisk;
            averageHappiness = 0;
            baseMortalityRate = 5;
            townNum++;
        }

        public Town(string mayor)
        {
            townNum++;
            this.mayor = mayor;
            baseDetalHappiness = 1;
            
        }

        public Town(TownData T)
        {
            id = T.ID;
            mayor = T.Mayor;

            averageHappiness = 0;
            policyImplementation = new Dictionary<int, bool>();
            essentials = new Collection<Supermarket>();
            recreational = new Collection<Building>();
            buildings = new HashSet<Building>();
            emergency = new Collection<Hospital>();
        }
        #endregion
        
        #region Game State Methods
        public void Start(Game.GameVersion version, int numCitizenThreads)
        {
            Debug.Log("Initializing town...");
            timer.Reset();
            loadBuildings(version);
            initializeCitizens(version);
            day = 1;
            if (CITIZEN_START_COUNT % numCitizenThreads != 0)
            {
                Debug.Log("An odd number of threads have been selected. Finding best minimal number of threads to generate... (Note that more than 1 thread is generated)");
                for (int i = 1; i < CITIZEN_START_COUNT + 1; i++)
                {
                    if (CITIZEN_START_COUNT % i == 0 && i > 1)
                    {
                        Debug.Log("Ideal number of Threads found. Total citizen threads: " + i);
                        numCitizenThreads = i;
                        break;
                    }
                }
            }

            citizenThreads = new CitizenWorkerThread[numCitizenThreads];
            Threads = new Thread[numCitizenThreads];
            int divider = CitizenWorkerThread.citizens.Count / numCitizenThreads;
            for (int i = 0; i < numCitizenThreads; i++) // Multithreaded implementation. Just be sure to comment out the Start() method to test it. But also note that the threads don't terminate unless the unity environment itself is terminated, or the player actively quits the game.
            {
                citizenThreads[i] = new CitizenWorkerThread(divider * i, divider * (i + 1), false);
                Debug.Log(divider*i + " " + divider*(i+1));
                Debug.Log("Creating Citizen Thread " + i);
                Threads[i] = new Thread(citizenThreads[i].Update);
                //Threads[i].Start(); 
            }
            
            // Need to add multithreading for buildings
            townReady = true;
        }

        public void Update()
        {
            if (Game.GAMEPAUSED)
            {
                timer.Stop();
                while (Game.GAMEPAUSED)
                {
                    
                }

                timer.Start();
            }
            /*
            if (Building.buildingTimer.ElapsedMilliseconds >= Building.buildingUpdateTimer ) // Every 30 minutes, a spread check is made bu buildings
            {
                Building.buildingTimer.Restart();
                foreach (Building b in recreational)
                {
                    b.Update();
                }

                foreach (Building b in residential)
                {
                    b.Update();
                }
                foreach (Supermarket s in essentials)
                {
                    s.Update();
                }
                foreach(Hospital h in emergency)
                {
                    h.Update();
                }
                Debug.Log("Updating of buildings complete");
            }*/
            
            if (timer.ElapsedMilliseconds < updateTickRate) return;
            timer.Restart();
            Debug.Log("Incrementing Time...");
            incrementTime();
            double happinessavg = 0;
            Debug.Log("Updating citizens...");
            int totalRebels = 0;
            totalInfected = 0;
            
            for (int i = 0; i < 4; i++)
            {
                happinessavg += citizenThreads[i].AverageHappiness();
                totalRebels += citizenThreads[i].NumRebels;
                totalInfected += citizenThreads[i].NumInfected;
            }

            happinessavg /= 4;
            averageHappiness = happinessavg;
            Debug.Log("Updating of game state & citizens complete! The process took " + timer.ElapsedMilliseconds);
        }

        private void incrementTime()
        {
            time++;
            if (time > 23)
            {
                time = 0;
                day++;
            }
            if (time == 8) // Newspaper events occur at 8am in game time
            {
                loadNewspaperEvent();
            }

            Debug.Log("Time is now " + time);
        }

        private void loadNewspaperEvent()
        {
            if (day == nextNewsEvent)
            {
                Random r = new Random();
                int eventNum = r.Next(1, NewspaperLibrary.Newspaper.events.Length);
                NewspaperLibrary.Newspaper.triggerNewspaperEvent(eventNum);
                nextNewsEvent = r.Next(5, 7);
            }
        }
        #endregion
        
        #region Initialization Methods

        private void initializeCitizens(Game.GameVersion version)
        {
            switch (version)
            {
                case Game.GameVersion.Debug:
                    // Method that generates test citizens
                    Random r = new Random();
                    
                    Stopwatch s = Stopwatch.StartNew();
                    for (int i = 0; i < CITIZEN_START_COUNT_DEBUG; i++) // Test citizens added to CitizenWorkerThread class
                    {
                        int buildingNum = r.Next(buildings.Count - 1);
                        CitizenWorkerThread.citizens.Add(new Citizen());
                        CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 1, 1, 0, buildings.ElementAt(buildingNum));
                    }
                    int infected = r.Next(CITIZEN_START_COUNT - 1);
                    CitizenWorkerThread.citizens[infected].rollHealthEvent(100);
                    for (int i = 0; i < CITIZEN_START_COUNT_DEBUG; i++)
                    {
                        CitizenWorkerThread.citizens[i].initiateTask();
                    }
                    s.Stop();
                    break;
                case Game.GameVersion.ReleaseLoad:
                    Collection<Citizen> citizens = FileManagerSystem.LoadCitizens(this);
                    
                    break;
                case Game.GameVersion.ReleaseNew:
                    Random ranomizer = new Random();
                    for (int i = 0; i < CITIZEN_START_COUNT; i++)
                    {
                        CitizenWorkerThread.citizens.Add(new Citizen()); // Citizen is generated here. Need to figure out how to generate the citizen's tasks though
                        int diabetic = ranomizer.Next(1);
                        int respiratory = ranomizer.Next(1);
                        int cardial = ranomizer.Next(1);
                        CitizenWorkerThread.citizens[i].generateCitizenRisk(diabetic, respiratory, cardial, Game.GAME_MODIFIER);
                    }
                    break;
            }
        }

        private void loadBuildings(Game.GameVersion version)
        {
            switch (version)
            {
                case Game.GameVersion.ReleaseLoad:
                    // Code that will load previous buildings in
                    break;
                case Game.GameVersion.ReleaseNew:
                    // Code that will generate the buildings that exist in the instance of the game
                    break;
                case Game.GameVersion.Debug:
                    Random r = new Random();
                        
                    Building b1 = new Building("firstRes", 0, 35, 5000, 3);
                    Hospital h1 = new Hospital("firstHos", 35, 5000, 0, 30, false);
                    Supermarket s1 = new Supermarket("firstSupwe", 35, 5000, 0, 20);
                    Building b2 = new Building("firstRec", 35, 35, 5000, 0);
                    residential.Add(b1);
                    emergency.Add(h1);
                    essentials.Add(s1);
                    recreational.Add(b2);
                    for (int i = 0; i < 10; i++)
                    {
                        int buildingGenerator = r.Next(0, 3);
                        string buildingID = "building" + 0;
                        if (buildingGenerator == 0)
                        {
                            Building b = new Building(buildingID, 35, 5000, 0, 0);
                            recreational.Add(b);
                            
                        }
                        else if (buildingGenerator == 1)
                        {
                            Supermarket s = new Supermarket(buildingID, 35, 5000, 0, 20);
                            essentials.Add(s);
                        }
                        else if (buildingGenerator == 2)
                        {
                            Hospital h = new Hospital(buildingID, 35, 5000, 0, 30, false);
                            emergency.Add(h);
                        }
                        else if (buildingGenerator == 3)
                        {
                            Building b = new Building(buildingID, 0, 200, 0, 3);
                            residential.Add(b);
                        }
                    }

                    foreach (Building b in recreational)
                    {
                        buildings.Add(b);
                    }
                    foreach (Building b in residential)
                    {
                        buildings.Add(b);
                    }
                    foreach (Supermarket b in essentials)
                    {
                        buildings.Add(b);
                    }
                    foreach (Hospital b in emergency)
                    {
                        buildings.Add(b);
                    }
                    break;
            }
        }
        

        public void applyEvent(int deltaHappiness, int deltaHealth, int deltaMoney, int eventID)
        {
            if (eventID == 69) // This is an event that occurs only when shit gets real in the game
            {
                resetTown();
                return;
            }
            if (deltaHappiness > 0)
            {
                new Thread(() => applyHappiness(deltaHappiness))
                    .Start(); // Instantiate a thread that applies the deltaHappiness to all the citizens (Should not get in the way of game performance in theory
            }
            Random r = new Random();
            int numAffected = r.Next(15, 25);
            for (int i = 0; i < numAffected; i++)
            {
                int citizenPos = r.Next(CitizenWorkerThread.citizens.Count - 1);
                CitizenWorkerThread.citizens[citizenPos].rollHealthEvent(deltaHealth);
                
            }
            money += deltaMoney;
        }
        

        private void applyHappiness(int deltaHappiness)
        {
            for (int i = 0; i < CitizenWorkerThread.citizens.Count; i++)
            {
                new Thread(() => CitizenWorkerThread.citizens[i].applyHappiness(deltaHappiness)).Start();
            }
        }

        public void resetTown()
        {
            foreach (Citizen c in CitizenWorkerThread.citizens)
            {
                c.reset();
            }
        }
        #endregion
        
        #region Policy Methods

        public void addGamePolicies(int numPolicies)
        {
            for (int i = 0; i < numPolicies; i++)
            {
                policyImplementation.Add(i, false);
            }
        }
        
        #endregion

        #region Setters & Getters
        
        public string ID => id;

        public string Mayor => mayor;

        public int TotalInfected => totalInfected;

        public double Money => money;


        public double FavoriteModifier => favoriteModifier;

        public static long UpdateTickRate
        {
            get => updateTickRate;
        }

        public Stopwatch Timer => timer;

        public int Time
        {
            get => time;
        }

        public int Day
        {
            get => day;
        }

        public double BaseDetalHappiness
        {
            get => baseDetalHappiness;
        }

        public double BaseCitisenRisk
        {
            get => baseCitisenRisk;
        }

        public double AverageHappiness => averageHappiness;

        public void updateDeltaHappiness(double factor)
        {
            baseDetalHappiness += factor;
        }
        
        private Collection<Building> getBuildings()
        {
            Collection<Building> buildings = new Collection<Building>();
            foreach (Hospital h in emergency)
            {
                buildings.Add(h);
            }

            foreach (Supermarket s in essentials)
            {
                buildings.Add(s);
            }

            foreach (Building b in recreational)
            {
                buildings.Add(b);
            }

            foreach (Building b in residential)
            {
                buildings.Add(b);
            }

            return buildings;
        }

        public Dictionary<int, bool> PolicyImplementation
        {
            get => policyImplementation;
        }

        public Collection<Supermarket> Essentials => essentials;

        public Collection<Hospital> Emergency => emergency;

        public Collection<Building> Recreational => recreational;

        public HashSet<Building> Buildings => buildings;

        public Collection<Building> Residential => residential;

        #endregion
    }
}