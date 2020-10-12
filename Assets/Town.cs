using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using CitizenLibrary;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class Town
{
    public static int townNum;
    private string id, mayor;
    private volatile int day, time, totalInfected;
    private static long updateTickRate, dayLength; // For now, the length of a day will be 1 minute (60 seconds). The updateTickRate will be
    private HashSet<Building> recreational, residential;
    private HashSet<Supermarket> essentials;
    private HashSet<Hospital> emergency;
    private Dictionary<int, bool> policyImplementation;
    private HashSet<Policy> allPolicies, availablePolicies;
    private CitizenWorkerThread[] workerThreads;
    public static Thread[] Threads;
    private double baseDetalHappiness, baseCitisenRisk, baseMortalityRate, baseRecoveryRate, averageHappiness, averageHealth;
    private double money, deltaMoney, favoriteModifier;
    private Collection<Policy> policies;
    private static Stopwatch timer;
    public static volatile bool townReady;
    
    public enum GameVersion {Debug = 0, Release = 1}
        
    

    public Town(string id, int day, int time, long dayLength, long updateTickRate, double baseDetalHappiness, double baseCitisenRisk, double baseMortalityRate, double baseRecoveryRate)
    {
        this.id = id;
        this.day = day;
        this.time = time;
        Town.dayLength = dayLength;
        Town.updateTickRate = updateTickRate;
        this.baseDetalHappiness = baseDetalHappiness;
        this.baseCitisenRisk = baseCitisenRisk;
        this.baseMortalityRate = baseMortalityRate;
        this.baseRecoveryRate = baseRecoveryRate;
        policyImplementation = new Dictionary<int, bool>();
        recreational = new HashSet<Building>();
        essentials = new HashSet<Supermarket>();
        emergency = new HashSet<Hospital>();
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
        
    }

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
        for (int i = 0; i < 4; i++) // Multithreaded implementation. Just be sure to comment out the Start() method to test it. But also note that the threads don't terminate unless the unity environment itself is terminated, or the player actively quits the game.
        {
            workerThreads[i] = new CitizenWorkerThread(divider*i, divider*(i+1), false);
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

        if (timer.ElapsedMilliseconds >= updateTickRate / 4) // Every 30 minutes, a spread check is made bu buildings
        {
            Collection<Building> b = getBuildings();
            foreach (Building building in b)
            {
                building.Update();
            }
        }
        if (timer.ElapsedMilliseconds >= updateTickRate)
        {
            timer.Reset();
            incrementTime();
            for (int i = 0; i < CitizenWorkerThread.citizens.Count; i++) // CitizenWorkerThread has a static collection of Citizens. This is a single threaded implementation
            {
                CitizenWorkerThread.citizens[i].Update();
            }
            int totalRebels = 0;
            double happinessavg = 0;
            for(int i = 0; i < 4; i++)
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
                recreational.Add(new Building(buildingID, 2, 5, 100, 0, 0));
            }
            else if (buildingGenerator == 1)
            {
                essentials.Add(new Supermarket(buildingID, 2, 5, 100, 0, 20));
            }
            else if (buildingGenerator == 2)
            {
                emergency.Add(new Hospital(buildingID, 2, 5, 100, 0, 30, false));
            }
            else if (buildingGenerator == 3)
            {
                residential.Add(new Building(buildingID, 2, 5, 200, 0, 3));
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
    public static long UpdateTickRate
    {
        get => updateTickRate;
        set => updateTickRate = value;
    }

    public static Stopwatch Timer => timer;

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

    public HashSet<Supermarket> Essentials => essentials;

    public HashSet<Hospital> Emergency => emergency;

    public HashSet<Building> Recreational => recreational;

    #endregion
}
