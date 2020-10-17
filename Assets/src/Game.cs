using System;
using System.Diagnostics;
using System.Threading;
using src.CitizenLibrary;
using src.SaveLoadLibrary;
using src.NewspaperLibrary;
using src.AchievementsLibrary;
using src.UILibrary;
using UnityEngine;
//using UnityEngine.iOS;
using UnityEngine.iOS;
using Debug = UnityEngine.Debug;

namespace src
{
    public class Game : MonoBehaviour // Game script is currently linked to trees
    {
        public static Town town;
        public static bool GAMEQUIT = false, GAMEPAUSED = false, GAMESTART = false, GAMECLOSED;
        public static bool ISNEWGAME = true, TOWNREADY = false;
        public static int BASE_CITIZEN_RISK_FACTOR = 25;
        public static int BASE_CITIZEN_MORTALITY_RATE = 5;
        public static int BASE_BUILDING_EXPOSURE_FACTOR = 35;
        public static double GAME_MODIFIER = 1;
        public static long UPDATETICKRATE = 2500;
        
        public enum GameVersion
        {
            Debug = 0,
            ReleaseNew = 1,
            ReleaseLoad = 2
        }
        private static GameVersion Version = GameVersion.ReleaseNew;
        private static int numCitizenThreads = 4;
        
        public void Start()
        {
            checkGameRules();
            
            TOWNREADY = false;
            loadCitizenTasks();
            string townID = "mainTown";
            town = FileManagerSystem.LoadTown(townID);
            if (town == null || ISNEWGAME)
            { 
                Debug.Log("Save file does not exist. Generating new save files...");
            
                town = new Town(townID); // If the save file doesn't exist, a new Town is constructed
            }
            
            town.Start(Version, numCitizenThreads); // Town generation happens here (Stuff like loading town's 
            TOWNREADY = true;
            GAMESTART = true;
            Building.buildingTimer = Stopwatch.StartNew();
            Debug.Log(Building.buildingTimer.IsRunning);
        }

        public void Update()
        {
            town.Update();
            //Debug.Log(Town.timer.ElapsedMilliseconds);
        }
        
        #region Event Methods
        public void OnApplicationPause(bool pauseStatus)
        {
            GAMEPAUSED = pauseStatus;
        }

        public void OnApplicationQuit()
        {
            GAMEQUIT = true;
            Debug.Log("yes");
            //FileManagerSystem.SaveCitizens(town, CitizenWorkerThread.citizens);
            //FileManagerSystem.SaveTown(town);
            GAMECLOSED = true;
            for (int i = 0; i < Town.Threads.Length; i++)
            {
                Debug.Log("Closing thread " + i);
                Town.Threads[i].Join();
            }
            Debug.Log("Yes, the game closed");
        }
        
        #endregion
        
        #region Game Initialization Methods
        /**
         * Causes the thread that called it to wait for the Game.TOWNREADY property to be true before proceeding. Used to ensure that scripts wait for
         * the Game's town object to be instantiated before continuing with completing tasks
         */
        public static void waitTownInitialization() 
        {
            while (!TOWNREADY)
            {
                
            }
        }
        /***
         * 
         */
        public static void waitStart()
        {
            while (!GAMESTART)
            {
                
            }
        }
        
        /**
         * Loads data regarding the different tasks that citizens can do
         */
        private static void loadCitizenTasks() // For now, task keys and descriptions are hard-coded, will need to figure out how to load .asset files and stuff
        {
            CitizenTask.taskKeys.Add(0, ("Go Drinking", true));
            CitizenTask.taskKeys.Add(1, ("Sleep", true));
            CitizenTask.taskKeys.Add(2, ("Work", true));
            CitizenTask.taskKeys.Add(3, ("go shopping", true));
            CitizenTask.taskKeys.Add(4, ("Visit friend", true));
            CitizenTask.taskKeys.Add(5, ("Self Quarantine", true));
            CitizenTask.taskKeys.Add(6, ("Recover in hospital", true));
        }
        /**
         * Checks specified game rules before starting the game. Mainly here in case someone decides to fiddle with the values. Preventative
         * measure in case something breaks
         */
        private static void checkGameRules()
        {
            if (BASE_CITIZEN_RISK_FACTOR >= 100)
            {
                BASE_CITIZEN_RISK_FACTOR = 100;
            }

            if (BASE_CITIZEN_RISK_FACTOR <= 0)
            {
                BASE_CITIZEN_RISK_FACTOR = 0;
            }

            if (BASE_CITIZEN_MORTALITY_RATE >= 100)
            {
                Debug.Log("Geez, you really just want people to die huh?");
                BASE_CITIZEN_MORTALITY_RATE = 100;
            }
            else if (BASE_CITIZEN_MORTALITY_RATE <= 0)
            {
                Debug.Log("Come on! Where's the danger in doing that?");
                BASE_CITIZEN_MORTALITY_RATE = 0;
            }
            Debug.Log("Initializing game with:\nBase Exposure Factor = " + BASE_CITIZEN_RISK_FACTOR + "\nBase mortality rate = " + BASE_CITIZEN_MORTALITY_RATE);
        }

        public static void startWatches()
        {
            Thread.Sleep(10);
            if (town.Timer.ElapsedMilliseconds > 0)
            {
                town.Timer.Restart();
                Building.buildingTimer.Restart();
            }
            else
            {
            }
        }

        private static void waitRestofInitialization()
        {
            while (true)
            {
                // Check to see if every script finished all the procedures that needed to be undertaken on Start() [i.e. wait for every script to load before executing]
                if (CitizenMovement.CitizenMovementReady) // Wait for other scripts to finish loading
                {
                    break;
                }
            }
        }
        #endregion
        
        #region Save/Load Protocols
        public static void RunSaveProtocol()
        {
            FileManagerSystem.SaveCitizens(town, CitizenWorkerThread.citizens);
            FileManagerSystem.SaveTown(town);
            
        }
        #endregion
    }
}