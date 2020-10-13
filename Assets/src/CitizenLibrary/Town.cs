using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using CitizenLibrary;
using SaveLoadLibrary;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;
using SaveSystem = src.SaveLoadLibrary.SaveSystem;
using TownData = src.SaveLoadLibrary.TownData;
using src.UILibrary;

namespace src.CitizenLibrary
{
    #region constructors
    public class Town
    {
        public static int townNum = 1;
        private string id, mayor;
        private volatile int day, time, totalInfected;

        private static long
            updateTickRate,
            dayLength; // For now, the length of a day will be 1 minute (60 seconds). The updateTickRate will be

        private HashSet<Building> buildings;
        private Collection<Building> recreational, residential;
        private Collection<Supermarket> essentials;
        private Collection<Hospital> emergency;
        private Dictionary<int, bool> policyImplementation;
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

        public enum GameVersion
        {
            Debug = 0,
            Release = 1
        }



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
            townNum++;
        }

        public Town(string mayor, double baseDetalHappiness, double baseCitisenRisk) // Town default constructor
        {
            this.mayor = mayor;
            this.baseDetalHappiness = baseDetalHappiness;
            id = mayor + "Town " + townNum;
            day = 1;
            time = 6;
            this.baseCitisenRisk = baseCitisenRisk;
            baseMortalityRate = 5;
            townNum++;

        }

        public Town(TownData T)
        {
            
            policyImplementation = new Dictionary<int, bool>();
            essentials = new Collection<Supermarket>();
            recreational = new Collection<Building>();
            buildings = new HashSet<Building>();
            emergency = new Collection<Hospital>();
        }
        #endregion
        public void Start()
        {
            HealthBar.setMaxHealth(100);
            happinessBar.setMaxHappy(100);
            Debug.Log("Initializing town...");
            townReady = false;
            timer = Stopwatch.StartNew();
            timer.Reset();
            initializeCitizens(GameVersion.Debug);
            loadBuildings("buildingSaveLocation");

            Citizen.town = this; // Sets the citizen town to this

            workerThreads = new CitizenWorkerThread[4];
            Threads = new Thread[4];
            int divider = CitizenWorkerThread.citizens.Count / 4;
            for
            (int i = 0;
                i < 4;
                i++) // Multithreaded implementation. Just be sure to comment out the Start() method to test it. But also note that the threads don't terminate unless the unity environment itself is terminated, or the player actively quits the game.
            {
                workerThreads[i] = new CitizenWorkerThread(divider * i, divider * (i + 1), false);
                Debug.Log("Creating Citizen Thread " + i);
                Threads[i] = new Thread(new ThreadStart(workerThreads[i].update));
                //Threads[i].Start(); 
            }

            // Need to add multithreading for buildings

            townReady = true;
            timer.Start();
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

            if (timer.ElapsedMilliseconds >= updateTickRate / 4
            ) // Every 30 minutes, a spread check is made bu buildings
            {
                foreach (Building b in recreational)
                {
                    b.Update();
                }

                foreach (Supermarket s in essentials)
                {
                    s.Update();
                }
            }

            if (timer.ElapsedMilliseconds >= updateTickRate)
            {
                timer.Reset();
                incrementTime();
                for (int i = 0;
                    i < CitizenWorkerThread.citizens.Count;
                    i++) // CitizenWorkerThread has a static collection of Citizens. This is a single threaded implementation
                {
                    CitizenWorkerThread.citizens[i].Update();
                }

                int totalRebels = 0;
                double happinessavg = 0;
                for (int i = 0; i < 4; i++)
                {
                    happinessavg += workerThreads[i].AverageHappiness();
                    totalRebels += workerThreads[i].NumRebels;
                    totalInfected += workerThreads[i].NumInfected;
                }

                happinessavg /= 4;
                happinessBar.setHealth(Convert.ToInt16(Math.Floor(happinessavg)));
                Debug.Log(happinessavg);
                timer.Start();

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

        #region Initialization Methods

        private void initializeCitizens(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.Debug:
                    for (int i = 0; i < 2; i++) // Test citizens added to CitizenWorkerThread class
                    {
                        CitizenWorkerThread.citizens.Add(new Citizen(i.ToString()));
                        CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 0);
                        CitizenWorkerThread.citizens[i].InitializeCitizen();
                    }

                    break;
                case GameVersion.Release:

                    break;
            }
        }

        private void loadBuildings(string fileName)
        {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                int buildingGenerator = r.Next(0, 3);
                string buildingID = "building" + 0;
                //Recreational = 0,
                //Supermarket = 1,
                //Emergency = 2,
                //Residential = 3
                if (buildingGenerator == 0) // 
                {
                    Building b = new Building(buildingID, 2, 5, 100, 0, 0);
                    recreational.Add(b);
                    buildings.Add(b);
                }
                else if (buildingGenerator == 1)
                {
                    Supermarket s = new Supermarket(buildingID, 2, 5, 100, 0, 20);
                    essentials.Add(s);
                    buildings.Add(s);
                }
                else if (buildingGenerator == 2)
                {
                    Hospital h = new Hospital(buildingID, 2, 5, 100, 0, 30, false);
                    emergency.Add(h);
                    buildings.Add(h);
                }
                else if (buildingGenerator == 3)
                {
                    Building b = new Building(buildingID, 2, 5, 200, 0, 3);
                    residential.Add(b);
                }
            }
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

        #endregion

        #region Setters & Getters

        public string ID => id;

        public static long UpdateTickRate
        {
            get => updateTickRate;
            set => updateTickRate = value;
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

        public Dictionary<int, bool> PolicyImplementation
        {
            get => policyImplementation;
        }

        public Collection<Supermarket> Essentials => essentials;

        public Collection<Hospital> Emergency => emergency;

        public Collection<Building> Recreational => recreational;

        #endregion
    }
}