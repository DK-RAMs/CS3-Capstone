using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using src.SaveLoadLibrary;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;
using src.UILibrary;

namespace src.CitizenLibrary
{
    public class Town
    {
        #region Class Properties
        public static int townNum = 1;
        private string id, mayor;
        private volatile int day, time, totalInfected;
        private static int NUM_TESTCITIZENS = 1;

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
        private CitizenWorkerThread[] workerThreads;
        public static Thread[] Threads;

        private double baseDetalHappiness,
            baseCitisenRisk,
            baseMortalityRate,
            baseRecoveryRate,
            averageHappiness,
            averageHealth;

        private double money, deltaMoney, favoriteModifier;
        private Collection<Policy> policies;
        private Stopwatch timer;
        public static volatile bool townReady;
        private int numCitizens;
        // public static Newspaper newspaper;
        public enum GameVersion
        {
            Debug = 0,
            ReleaseNew = 1,
            ReleaseLoad = 2
        }
        
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
            deltaMoney = 1;
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
            baseMortalityRate = 5;
            townNum++;
        }

        public Town(TownData T)
        {
            id = T.ID;
            policyImplementation = new Dictionary<int, bool>();
            essentials = new Collection<Supermarket>();
            recreational = new Collection<Building>();
            buildings = new HashSet<Building>();
            emergency = new Collection<Hospital>();
        }
        #endregion
        
        #region Game State Methods
        public void Start()
        {
            Debug.Log("Initializing town...");
            townReady = false;
            timer.Reset();
            initializeCitizens(GameVersion.Debug);
            loadBuildings(GameVersion.Debug);

            Citizen.town = this; // Sets the citizen town to this

            workerThreads = new CitizenWorkerThread[4];
            Threads = new Thread[4];
            int divider = CitizenWorkerThread.citizens.Count / 4; 
            /*
            for (int i = 0; i < 4; i++) // Multithreaded implementation. Just be sure to comment out the Start() method to test it. But also note that the threads don't terminate unless the unity environment itself is terminated, or the player actively quits the game.
            {
                workerThreads[i] = new CitizenWorkerThread(divider * i, divider * (i + 1), false);
                Debug.Log("Creating Citizen Thread " + i);
                Threads[i] = new Thread(new ThreadStart(workerThreads[i].update));
                //Threads[i].Start(); 
            }

            // Need to add multithreading for buildings
            */
            timer.Start();
            townReady = true;
            
            Debug.Log(GetHashCode());
        }

        public void Update()
        {
            if (PauseMenu.GameIsPaused)
            {
                timer.Stop();
                while (PauseMenu.GameIsPaused)
                {

                }

                timer.Start();
            }
            if (timer.ElapsedMilliseconds >= updateTickRate / 4) // Every 30 minutes, a spread check is made bu buildings
            {
                /*
                foreach (Building b in recreational)
                {
                    b.Update();
                }

                foreach (Supermarket s in essentials)
                {
                    s.Update();
                }*/
            }

            if (timer.ElapsedMilliseconds >= updateTickRate)
            {
                timer.Reset();
                Stopwatch s = Stopwatch.StartNew();
                double happinessavg = 0;
                incrementTime();
                for (int i = 0; i < CitizenWorkerThread.citizens.Count; i++) // CitizenWorkerThread has a static collection of Citizens. This is a single threaded implementation
                {
                    CitizenWorkerThread.citizens[i].Update();
                    happinessavg += CitizenWorkerThread.citizens[i].Happiness;
                }
                happinessavg /= CitizenWorkerThread.citizens.Count;
                /*
                int totalRebels = 0;
                for (int i = 0; i < 4; i++)
                {
                    happinessavg += workerThreads[i].AverageHappiness();
                    totalRebels += workerThreads[i].NumRebels;
                    totalInfected += workerThreads[i].NumInfected;
                }

                happinessavg /= 4;
                */
                
                Debug.Log(happinessavg);
                timer.Start();
                Debug.Log("Updated all citizens. Time Taken: " + s.ElapsedMilliseconds);
            }
        }

        private void incrementTime()
        {
            time++;
            if (time > 23)
            {
                time = 0;
                day++;
            }

            Debug.Log("Time is now " + time);
        }
        #endregion
        
        #region Initialization Methods

        private void initializeCitizens(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.Debug:
                    Stopwatch s = Stopwatch.StartNew();
                    for (int i = 0; i < NUM_TESTCITIZENS; i++) // Test citizens added to CitizenWorkerThread class
                    {
                        CitizenWorkerThread.citizens.Add(new Citizen());
                        CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 0);
                    }
                    s.Stop();
                    Debug.Log("Citizens have been generated. Time taken: " + s.ElapsedMilliseconds);
                    break;
                case GameVersion.ReleaseNew:
                    Collection<Citizen> citizens = FileManagerSystem.LoadCitizens(this);
                    
                    break;
                case GameVersion.ReleaseLoad:
                    break;
            }
        }

        private void loadBuildings(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.ReleaseLoad:
                    
                    break;
                case GameVersion.Debug:
                    Random r = new Random();
                    for (int i = 0; i < 10; i++)
                    {
                        int buildingGenerator = r.Next(0, 3);
                        string buildingID = "building" + 0;
                        if (buildingGenerator == 0)
                        {
                            Building b = new Building(buildingID, 2, 5, 100, 0, 0);
                            recreational.Add(b);
                        }
                        else if (buildingGenerator == 1)
                        {
                            Supermarket s = new Supermarket(buildingID, 2, 5, 100, 0, 20);
                            essentials.Add(s);
                        }
                        else if (buildingGenerator == 2)
                        {
                            Hospital h = new Hospital(buildingID, 2, 5, 100, 0, 30, false);
                            emergency.Add(h);
                        }
                        else if (buildingGenerator == 3)
                        {
                            Building b = new Building(buildingID, 2, 5, 200, 0, 3);
                            residential.Add(b);
                        }
                    }
                    break;
            }
        }

        public void applyDecisionEvent(double deltaHappiness, double deltaHealth, double deltaMoney)
        {
            for (int i = 0; i < CitizenWorkerThread.citizens.Count; i++)
            {
                CitizenWorkerThread.citizens[i].applyHappiness(deltaHappiness);
            }
            Random r = new Random();
            int numAffected = r.Next(15, 25);
            for (int i = 0; i < numAffected; i++)
            {
                int citizenPos = r.Next(CitizenWorkerThread.citizens.Count - 1);
                
                
            }
            money += deltaMoney;
        }

        public void addCitizenHappiness(double happiness)
        {
            
        }

        public void applyQuizEvent(double deltaHappiness, double deltaHealth, double deltaMoney)
        {
            
        }

        public void applyStateEvent(double deltaHappiness, double deltaHealth, double deltaMoney)
        {
            
        }
        #endregion

        #region Setters & Getters
        
        public string ID => id;

        public string Mayor => mayor;

        public int TotalInfected => totalInfected;

        public double Money => money;

        public double DeltaMoney => deltaMoney;

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

        #endregion
    }
}