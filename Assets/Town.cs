using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using CitizenLibrary;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Town
{
    public static int townNum;
    private string id, mayor;
    private volatile int day, time;
    private static long updateTickRate, dayLength; // For now, the length of a day will be 1 minute (60 seconds). The updateTickRate will be
    private Collection<Building> buildings, bars;
    private Collection<Supermarket> shops;
    private Collection<Hospital> hospitals;
    private Dictionary<int, bool> policyImplementation;
    private HashSet<Policy> allPolicies, availablePolicies;
    private CitizenWorkerThread[] workerThreads;
    public static Thread[] Threads;
    private double baseDetalHappiness, baseCitisenRisk, baseMortalityRate, baseRecoveryRate, averageHappiness, averageHealth;
    private double money, deltaMoney;
    private Collection<Policy> policies;
    private Stopwatch timer;
    public static volatile bool townReady;
    //public static Newspaper newspaper; // newspaper object so can update it here every x days ect., public so it can be accessed from UpdateNewsText class - Z

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
    }

    public Town(double baseDetalHappiness, double baseCitisenRisk) // Town default constructor
    {
        this.baseDetalHappiness = baseDetalHappiness;
        this.id = mayor + "Town " + townNum;
        this.day = 1;
        this.time = 6;
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
        for (int i = 0; i < 2; i++) // Test citizens added to CitizenWorkerThread class
        {
            CitizenWorkerThread.citizens.Add(new Citizen(i.ToString()));
            CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 0);
            CitizenWorkerThread.citizens[i].InitializeCitizen();
        }
        
        Citizen.town = this; // Sets the citizen town to this
        // Number of Citizen Worker Threads
        
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
        timer.Start();
        //Newspaper = new Newspaper(); // - initialise a new newspaper object to be used throughout the program, I think this is probably a good place to put it - Z
        Newspaper.readArticles();
        //newspaper.incrementID(); // testing to see if it shows more than the first article - Z
        townReady = true;
    }
    
    public void Update()
    {
        for (int i = 0; i < CitizenWorkerThread.citizens.Count; i++) // CitizenWorkerThread has a static collection of Citizens. This is a single threaded implementation
        {
            CitizenWorkerThread.citizens[i].Update();
        }
        if (timer.ElapsedMilliseconds >= updateTickRate)
        {
            foreach (Building b in buildings)
            {
                b.Update(); // Updates the building. If there are citizens that are infected
            }
            int totalRebels = 0;
            timer.Reset();
            incrementTime();
            double happinessavg = 0;
            for(int i = 0; i < 4; i++)
            {
                happinessavg += workerThreads[i].AverageHappiness();
                totalRebels += workerThreads[i].NumRebels;
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
    
    #region Setters & Getters
    public static long UpdateTickRate
    {
        get => updateTickRate;
        set => Town.updateTickRate = value;
    }

    public int Time
    {
        get => time;
    }
    
    private void loadBuildings(Collection<Building> buildings){
        foreach(Building building in buildings)
        {
            switch (building.getBuildingType)
            {
                case Building.BuildingType.Emergency:
                    
                    break;
                case Building.BuildingType.Recreational:
                    break;
                case Building.BuildingType.Supermarket:
                    break;
                default:
                    break;
                    
            }
        }
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

    public Collection<Supermarket> Shops => shops;

    public Collection<Hospital> Hospitals => hospitals;

    public Collection<Building> Bars => bars;

    #endregion
}
