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
//using UnityEngine.iOS;
using Debug = UnityEngine.Debug;

namespace src
{
    public class Game : MonoBehaviour // Game script is currently linked to trees
    {
        public static Town town;
        public static bool GAMEQUIT = false, GAMEPAUSED = false, GAMESTART = false, GAMECLOSED;
        public static bool ISNEWGAME = true, TOWNREADY = false;
        public static int BASE_CITIZEN_RISK_FACTOR = 25;
        public static int BASE_CITIZEN_MORTALITY_RATE = 2;
        public static int BASE_BUILDING_EXPOSURE_FACTOR = 50;
        public static double GAME_MODIFIER = 1;
        public static long UPDATETICKRATE = 2500;
        public static long PLAYER_SCORE;

        public enum GameVersion
        {
            Debug = 0,
            ReleaseNew = 1,
            ReleaseLoad = 2
        }

        private static GameVersion Version = GameVersion.ReleaseNew;
        private static int numCitizenThreads = 2;

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
            }
            town = new Town(townID); // If the save file doesn't exist, a new Town is constructed
            
            Debug.Log("Game is working");

            //town.Start(Version, numCitizenThreads); // Town generation happens here (Stuff like loading town's 
            TOWNREADY = true;
            GAMESTART = true;
            town.Start(Version, numCitizenThreads);
            Building.buildingTimer.Start();
            //town.startCitizenThreads();
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
            GAMEPAUSED = true;
            Debug.Log("yes");
            RunSaveProtocol();
            //FileManagerSystem.SaveCitizens(town, CitizenWorkerThread.citizens);
            //FileManagerSystem.SaveTown(town);
            GAMEQUIT = true;
            GAMECLOSED = true;
            for (int i = 0; i < Town.citizenThreads.Length; i++)
            {
                Debug.Log("Closing thread " + i);
                Town.citizenThreads[i].Join();
            }

            Debug.Log("Yes, the game closed");
        }

        #endregion

        #region Game Initialization Methods

        /**
         * Loads data regarding the different tasks that citizens can do
         */
        private static void
            loadCitizenTasks() // For now, task keys and descriptions are hard-coded, will need to figure out how to load .asset files and stuff
        {
            CitizenTask.taskKeys[0] = ("Go have some fun!", true);
            CitizenTask.taskKeys[1] = ("Sleep", true);
            CitizenTask.taskKeys[2] = ("Work", true);
            CitizenTask.taskKeys[3] = ("go shopping", true);
            CitizenTask.taskKeys[4] = ("Visit friend", true);
            CitizenTask.taskKeys[5] = ("Self Quarantine", true);
            CitizenTask.taskKeys[6] = ("Recover in hospital", true);
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

            Debug.Log("Initializing game with:\nBase Exposure Factor = " + BASE_CITIZEN_RISK_FACTOR +
                      "\nBase mortality rate = " + BASE_CITIZEN_MORTALITY_RATE);
        }

        #endregion

        #region Save/Load Protocols

        public static void RunSaveProtocol()
        {
            for (int i = 0; i < Town.citizenThreads.Length; i++) // Closing citizen threads
            {
                Town.citizenThreads[i].Join();
            }
            
            for (int i = 0; i < Town.buildingThreads.Length; i++) // Closing building update threads
            {
                Town.buildingThreads[i].Join();
            }

            // Note that all threads need to be closed before saves can be made

            FileManagerSystem.SaveCitizens(town, CitizenWorkerThread.citizens);
            FileManagerSystem.SaveTown(town);
            FileManagerSystem.SaveBuildings(town);
        }

        #endregion
    }
}