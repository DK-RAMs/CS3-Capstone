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

namespace src.CitizenLibrary
{
    public class Town  // Still need to work on this
    {
        #region Class Properties
        public static int townNum = 1;
        private static int HEALTH_EVENT_LOW = 10, HEALTH_EVENT_HIGH = 45;
        private string id, mayor;
        private volatile int day, time, totalInfected, nextNewsEvent, totalDead, totalRebels;
        private static int CITIZEN_START_COUNT = 1000, CITIZEN_START_COUNT_DEBUG = 1000;

        public static volatile bool happinessUpdated;
// For now, the length of a day will be 1 minute (60 seconds). The updateTickRate will be

        //private HashSet<Building> buildings;
        public Collection<Building> recreational, residential;
        public Collection<Supermarket> essentials;
        public Collection<Hospital> emergency;
        private Dictionary<int, bool> policyImplementation;
        public bool[] policyImplemented;
        private HashSet<Policy> allPolicies, availablePolicies;
        private CitizenWorkerThread[] citizenWorkerThreads;
        public static Thread[] buildingThreads;
        public static Thread[] citizenThreads;

        private double baseDetalHappiness,
            baseCitisenRisk,
            baseMortalityRate,
            baseRecoveryRate,
            averageHappiness,
            averageHealth;

        private long money;
        private double favoriteModifier;
        private Collection<Policy> policies;
        public static Stopwatch timer = new Stopwatch();
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
            this.baseDetalHappiness = baseDetalHappiness;
            this.baseCitisenRisk = baseCitisenRisk;
            this.baseMortalityRate = baseMortalityRate;
            policyImplementation = new Dictionary<int, bool>();
            recreational = new Collection<Building>();
            essentials = new Collection<Supermarket>();
            emergency = new Collection<Hospital>();
            residential = new Collection<Building>();
            townNum++;
            money = 250;
            totalInfected = 0;
            favoriteModifier = 1.2;
            averageHappiness = 0;
        }

        public Town(string mayor, double baseDetalHappiness, double baseCitisenRisk, long money) // Town default constructor
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

        public Town(string mayor) // Generates a new town using specified mayor name
        {
            townNum++;
            this.mayor = mayor;
            baseDetalHappiness = 1;
            baseCitisenRisk = Game.BASE_CITIZEN_RISK_FACTOR;
            baseMortalityRate = Game.BASE_CITIZEN_MORTALITY_RATE;
            policyImplementation = new Dictionary<int, bool>();
            policyImplemented = new bool[5];
            recreational = new Collection<Building>();
            essentials = new Collection<Supermarket>();
            emergency = new Collection<Hospital>();
            residential = new Collection<Building>();
            timer = new Stopwatch();
            time = 6;
            money = 63000;
            totalInfected = 0;
            favoriteModifier = 1.2;
            averageHappiness = 50;
        }

        public Town(TownData T) 
        {
            id = T.ID;
            mayor = T.Mayor;
            day = T.Day;
            time = T.Time;
            totalInfected = T.TotalInfected;
            money = T.Money;
            baseDetalHappiness = T.BaseDeltaHappiness;
            favoriteModifier = T.FavoriteMaidifier;
            
            timer.Start();
            while (timer.ElapsedMilliseconds < T.ElapsedTime) continue; // Catches the timer back up to what it should be in game
            timer.Stop();
            policyImplemented = T.PolicyImplementations;
            
            averageHappiness = 0;
            policyImplementation = new Dictionary<int, bool>();
            essentials = new Collection<Supermarket>();
            recreational = new Collection<Building>();
            emergency = new Collection<Hospital>();
            FileManagerSystem.LoadBuildings(this);
            
        }
        #endregion
        
        #region Game State Methods
        public void Start(Game.GameVersion version, int numCitizenThreads)
        {
            Debug.Log("Initializing town...");
            loadBuildings(version);
            initializeCitizens(version);
            initializeBuildingThreads(); // Initializes building threads
            day = 1;
            CreateCitizenThreads(numCitizenThreads);
            
            // Need to add multithreading for buildings
            townReady = true;
            timer.Start();
        }

        private void initializePolicies()
        {
            
        }

        private void CreateCitizenThreads(int numCitizenThreads)
        {
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
            citizenWorkerThreads = new CitizenWorkerThread[numCitizenThreads];
            citizenThreads = new Thread[numCitizenThreads];
            int divider = CitizenWorkerThread.citizens.Count / numCitizenThreads;
            for (int i = 0; i < numCitizenThreads; i++) // Multithreaded implementation. Just be sure to comment out the Start() method to test it. But also note that the threads don't terminate unless the unity environment itself is terminated, or the player actively quits the game.
            {
                citizenWorkerThreads[i] = new CitizenWorkerThread(divider * i, divider * (i + 1), false);
                citizenThreads[i] = new Thread(citizenWorkerThreads[i].Update);
            }
        }

        public void startCitizenThreads()
        {
            for (int i = 0; i < citizenThreads.Length; i++)
            {
                citizenThreads[i].Start();
            }
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
            if (Building.buildingTimer.ElapsedMilliseconds >= Building.buildingUpdateTimer ) // Every 15 in-game minutes, a spread check is made bu buildings
            {
                Building.buildingTimer.Restart();
                for (int i = 0; i < recreational.Count; i++)
                {
                    recreational[i].Update();
                }

                for (int i = 0; i < residential.Count; i++)
                {
                    residential[i].Update();
                }

                for (int i = 0; i < essentials.Count; i++)
                {
                    essentials[i].Update();
                }

                for (int i = 0; i < emergency.Count; i++)
                {
                    emergency[i].Update();
                }
            }
            
            if (timer.ElapsedMilliseconds < Game.UPDATETICKRATE) return;
            timer.Restart();
            incrementTime();
            
            double happinessavg = 0;
            int totalRebels = 0;
            int numInfected = 0;
            int numDead = 0;
            
        
            for (int i = 0; i < citizenWorkerThreads.Length; i++)
            {
                happinessavg += citizenWorkerThreads[i].AverageHappiness();
                totalRebels += citizenWorkerThreads[i].NumRebels;
                numInfected += citizenWorkerThreads[i].NumInfected;
                totalRebels += citizenWorkerThreads[i].NumRebels;
            }
            happinessavg /= (double)citizenWorkerThreads.Length;
            averageHappiness = happinessavg;
        }

        private void initializeBuildingThreads()
        {
            buildingThreads = new Thread[4];
            buildingThreads[0] = new Thread(updateEssentials);
            buildingThreads[1] = new Thread(updateHospitals);
            buildingThreads[2] = new Thread(updateRecreational);
            buildingThreads[3] = new Thread(updateResidential);
        }

        private void updateRecreational()
        {
            for(int i = 0; i < recreational.Count; i++)
            {
                recreational[i].Update();
            }
        }

        private void updateHospitals()
        {
            for(int i = 0; i < emergency.Count; i++)
            {
                emergency[i].Update();
            }
        }

        private void updateResidential()
        {
            for(int i = 0; i < residential.Count; i++)
            {
                residential[i].Update();
            }
        }

        private void updateEssentials()
        {
            for(int i = 0; i < essentials.Count; i++)
            {
                essentials[i].Update();
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
            if (time == 8) // Newspaper events occur at 8am in game time
            {
                loadNewspaperEvent();
            }

            if (time % 4 == 0)
            {
                generateFunds();
            }

            if (day == 32)
            {
                Debug.Log("The mayor's term has ended. Ending game and generating score");
                endGame();
            }
        }

        private void endGame()
        {
            Game.GAMEPAUSED = true;
            Game.GAMEQUIT = true;
            for (int i = 0; i < citizenThreads.Length; i++)
            {
                citizenThreads[i].Join();
            }
            Game.PLAYER_SCORE = 23 * (CitizenCount - totalInfected) + 7 * (long)averageHappiness - 29*totalDead;
            if (Game.PLAYER_SCORE < 0)
            {
                Game.PLAYER_SCORE = 0;
            }
        }

        private void loadNewspaperEvent()
        {
            if (day == nextNewsEvent)
            {
                Random r = new Random();
                if (Newspaper.events.Length == 1)
                {
                    return;
                }
                int eventNum = r.Next(1, Newspaper.events.Length - 1);
                Newspaper.triggerNewspaperEvent(eventNum);
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
                        int buildingType = r.Next(3);
                        CitizenWorkerThread.citizens.Add(new Citizen());
                        switch (buildingType)
                        {
                            case 0:
                                int recNum = r.Next(recreational.Count-1);
                                CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 1, 1, 0, recreational[recNum]);
                                break;
                            case 1:
                                int essNum = r.Next(essentials.Count-1);
                                CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 1, 1, 0, essentials[essNum]);
                                break;
                            case 2:
                                int emerNum = r.Next(emergency.Count -1);
                                CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 1, 1, 0, emergency[emerNum]);
                                break;
                            case 3:
                                int resNum = r.Next(residential.Count-1);
                                CitizenWorkerThread.citizens[i].loadPreviousTask(1, 6, 13, 1, 1, 0, residential[resNum]);
                                break;
                        }
                    }
                    int infected = r.Next(CITIZEN_START_COUNT - 1);
                    CitizenWorkerThread.citizens[infected].rollHealthEvent(100);
                    for (int i = 0; i < CITIZEN_START_COUNT_DEBUG; i++)
                    {
                        CitizenWorkerThread.citizens[i].initiateTask();
                    }
                    s.Stop();
                    break;
                
                // Case where we're loading an old save
                case Game.GameVersion.ReleaseLoad:
                    Collection<Citizen> citizens = FileManagerSystem.LoadCitizens(this);
                    if (citizens == null)
                    {
                        Debug.Log("This game is trash. A town manager without any civilians? HAH! Get me out");
                        Application.Quit();
                        return;
                    }
                    CitizenWorkerThread.citizens = citizens;
                    for (int i = 0; i < CITIZEN_START_COUNT; i++)
                    {
                        CitizenWorkerThread.citizens[i].initiateTask();
                    }
                    break;
                
                // Case where we're creating a new save
                case Game.GameVersion.ReleaseNew:
                    for (int i = 0; i < CITIZEN_START_COUNT; i++)
                    {
                        CitizenWorkerThread.citizens.Add(new Citizen());// Citizen is generated here. Need to figure out how to generate the citizen's tasks though
                    }
                    Random random = new Random();
                    int infectedNew = random.Next(CitizenCount-1);
                    CitizenWorkerThread.citizens[infectedNew].rollHealthEvent(100);
                    for (int i = 0; i < CITIZEN_START_COUNT; i++)
                    {
                        CitizenWorkerThread.citizens[i].initiateTask();
                    }
                    break;
            }
        }

        private void loadBuildings(Game.GameVersion version)
        {
            switch (version)
            {
                case Game.GameVersion.ReleaseLoad:
                    HashSet<Building> buildings = FileManagerSystem.LoadBuildings(this);
                    for (int i = 0; i < buildings.Count; i++)
                    {
                        if (buildings.ElementAt(i).ID.Contains("recreational"))
                        {
                            recreational.Add(buildings.ElementAt(i)); // Have to pass the reference to the object
                        }
                        else if (buildings.ElementAt(i).ID.Contains("residential"))
                        {
                            residential.Add(buildings.ElementAt(i));
                        }
                    }
                    break;
                case Game.GameVersion.ReleaseNew:
                    int numRecreational = 8;
                    int numShops = 17;
                    int numHospitals = 4;
                    int numResidentials = 9;

                    string id;
                    for (int i = 0; i < numRecreational; i++)
                    {
                        id = "recreational" + i;
                        Building b = new Building(id, Game.BASE_BUILDING_EXPOSURE_FACTOR+5, 5000, 0, 0);
                        recreational.Add(b);
                    }

                    for (int i = 0; i < numShops; i++)
                    {
                        id = "shop" + i;
                        Supermarket s = new Supermarket(id, Game.BASE_BUILDING_EXPOSURE_FACTOR, 100, 0, 20);
                        essentials.Add(s);
                    }

                    for (int i = 0; i < numHospitals; i++)
                    {
                        id = "hospital" + i;
                        Hospital h = new Hospital(id, Game.BASE_BUILDING_EXPOSURE_FACTOR - 10, 5000, 0, 20, false);
                        emergency.Add(h);
                    }

                    for (int i = 0; i < numResidentials; i++)
                    {
                        id = "residential" + i;
                        Building b = new Building(id, 0, 5000, 0, 3);
                        residential.Add(b);
                    }
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
                    break;
            }
        }
        
        #endregion
        
        #region Town Update Methods
        public void applyEvent(int deltaHappiness, int deltaHealth, int deltaMoney, int eventID)
        {
            if (eventID == 69) // This is an event that occurs only when shit gets real in the game
            {
                try
                {
                    Thread t = new Thread(resetTown);
                    t.Start();
                    return;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            if (deltaHappiness > 0)
            {
                try
                {
                    Thread t = new Thread(() => applyHappiness(deltaHappiness)); // Instantiate a thread that applies the deltaHappiness to all the citizens (Should not get in the way of game performance in theory
                    t.Start();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
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

        public void processDeltaMoney(long delta)
        {
            money += delta;
        }

        private void generateFunds()
        {
            long happinessFactor = (long)averageHappiness * 2000;
            long healthFactor = (CitizenCount - totalInfected - totalDead) * 400;
            money += happinessFactor + healthFactor;
        }

        private void applyHappiness(int deltaHappiness)
        {
            for (int i = 0; i < CitizenWorkerThread.citizens.Count; i++)
            {
                try
                {
                    new Thread(() => CitizenWorkerThread.citizens[i].applyHappiness(deltaHappiness)).Start();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public void incrementDead()
        {
            totalDead++;
        }

        public void incrementInfected()
        {
            totalInfected++;
        }

        public void decrementInfected()
        {
            totalInfected--;
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

        
        #endregion

        #region Setters & Getters
        
        public string ID => id;

        public string Mayor => mayor;

        public int TotalInfected => totalInfected;

        public long Money => money;


        public double FavoriteModifier => favoriteModifier;


        public Stopwatch Timer => timer;

        public int Time => time;

        public int Day => day;

        public double BaseDetalHappiness => baseDetalHappiness;

        public double BaseCitisenRisk => baseCitisenRisk;

        public double AverageHappiness => averageHappiness;

        public Dictionary<int, bool> PolicyImplementation
        {
            get => policyImplementation;
        }

        public static int CitizenCount => CITIZEN_START_COUNT;

        public static int CitizenCountDebug => CITIZEN_START_COUNT_DEBUG;

        public int TotalDead => totalDead;

        public Collection<Supermarket> Essentials => essentials;

        public Collection<Hospital> Emergency => emergency;

        public Collection<Building> Recreational => recreational;

        public Collection<Building> Residential => residential;

        #endregion
    }
}